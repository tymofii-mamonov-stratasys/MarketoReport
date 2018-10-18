using log4net;
using Newtonsoft.Json;
using PlayingWithMarketo.Core;
using PlayingWithMarketo.Core.Models;
using PlayingWithMarketo.Marketo.DTO;
using PlayingWithMarketo.Marketo.Enums;
using PlayingWithMarketo.Marketo.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace PlayingWithMarketo.Persistance
{
    public class MarketoHelper
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(MarketoHelper));
        private readonly IUnitOfWork _unitOfWork;
        private readonly MarketoAPIHelper marketoAPIHelper;

        public MarketoHelper(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            marketoAPIHelper = new MarketoAPIHelper(_unitOfWork);
        }

        public string GetToken()
        {
            var token = _unitOfWork.Tokens.GetToken();

            if (token != null)
                return token.AccessToken;

            Dictionary<string, string> dict = marketoAPIHelper.GetTokenRequest();

            if (dict != null)
            {
                token = new Token()
                {
                    AccessToken = dict["access_token"],
                    CreatedAt = DateTime.UtcNow,
                    ExpiresAt = DateTime.UtcNow.AddSeconds(Double.Parse(dict["expires_in"])),
                    UserName = dict["scope"]
                };

                _unitOfWork.Tokens.AddToken(token);
                _unitOfWork.Complete();

                return token.AccessToken;
            }
            else
                return null;
        }

        public string CreateExportJob(DateTime startDate, DateTime endDate)
        {
            var json = marketoAPIHelper.CreateExportJobRequest(startDate, endDate);
            var createJobResult = JsonConvert.DeserializeObject<RequestResult>(json);

            if (!createJobResult.success)
            {
                var errorRes = JsonConvert.DeserializeObject<RequestResultError>(json);
                ErrorHandling("Create Export Job", errorRes);

                return null;
            }

            var exportJob = new ExportJob()
            {
                ExportId = createJobResult.result.Single().exportId,
                Format = createJobResult.result.Single().format,
                Status = createJobResult.result.Single().status,
                CreatedAt = createJobResult.result.Single().createdAt
            };

            _unitOfWork.ExportJobs.AddExportJob(exportJob);
            _unitOfWork.Complete();

            return exportJob.ExportId;
        }

        public void QueueJob(string exportJobId)
        {
            var json = marketoAPIHelper.QueueJobRequest(exportJobId);
            var queueJobResult = JsonConvert.DeserializeObject<RequestResult>(json);

            if (!queueJobResult.success)
            {
                var errorRes = JsonConvert.DeserializeObject<RequestResultError>(json);
                ErrorHandling("Queue Job", errorRes);

                return;
            }
            var exportJobToQueue = _unitOfWork.ExportJobs.GetExportJob(exportJobId);
            exportJobToQueue.Status = queueJobResult.result.Single().status;
            exportJobToQueue.QueuedAt = queueJobResult.result.Single().queuedAt;

            _unitOfWork.Complete();
        }

        public Status? GetJobStatus(string exportJobId)
        {
            var json = marketoAPIHelper.GetJobStatusRequest(exportJobId);
            var jobStatusResult = JsonConvert.DeserializeObject<RequestResult>(json);

            if (!jobStatusResult.success)
            {
                var errorRes = JsonConvert.DeserializeObject<RequestResultError>(json);
                ErrorHandling("Get Job Status", errorRes);

                return null;
            }
            var exportJob = _unitOfWork.ExportJobs.GetExportJob(exportJobId);
            var status = jobStatusResult.result.Single().status;
            exportJob.Status = status;

            if (jobStatusResult.result.Single().finishedAt != DateTime.MinValue)
            {
                exportJob.FinishedAt = jobStatusResult.result.Single().finishedAt;
                exportJob.NumberOfRecords = jobStatusResult.result.Single().numberOfRecords;
                exportJob.FileSize = jobStatusResult.result.Single().fileSize;
            }
            if (jobStatusResult.result.Single().startedAt != DateTime.MinValue)
                exportJob.StartedAt = jobStatusResult.result.Single().startedAt;

            _unitOfWork.Complete();

            return (Status)Enum.Parse(typeof(Status), status, true);
        }

        public bool RetreiveData(string exportJobId)
        {
            var result = marketoAPIHelper.RetreiveDataRequest(exportJobId);
            try
            {
                var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(result);
                if (!bool.Parse(dict["success"]))
                {
                    var errorRes = JsonConvert.DeserializeObject<RequestResultError>(result);
                    ErrorHandling("Get Job Status", errorRes);

                    return false;
                }
            }
            catch (Exception)
            {

            }

            var resultList = result.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();

            var leadIdList = new List<int>();
            var leadActivitiesList = new List<LeadActivity>();
            foreach (var line in resultList)
            {
                var data = line.Split(',');
                var isParsed = int.TryParse(data[0], out var leadId);
                if (!isParsed)
                    continue;

                DateTime.TryParse(data[1], out var activityDate);
                int.TryParse(data[2], out var activityId);
                var attributesList = new List<string>();
                for (int i = 3; i < data.Length; i++)
                    attributesList.Add(data[i]);
                string attributes = string.Join(",", attributesList);

                if (!leadIdList.Any(li => li == leadId))
                    leadIdList.Add(leadId);

                var leadActivity = new LeadActivity()
                {
                    LeadId = leadId,
                    ActivityDate = activityDate,
                    ActivityId = activityId,
                    Attributes = attributes
                };

                if (!_unitOfWork.LeadActivities.IsInDB(leadActivity))
                    leadActivitiesList.Add(leadActivity);
            }
            PullMissingLeads(leadIdList);
            foreach (var leadActivity in leadActivitiesList)
            {
                int activityDBId = 0;
                int leadDbId = 0;

                activityDBId = _unitOfWork.Activities.GetId(leadActivity.ActivityId);

                leadDbId = _unitOfWork.Leads.GetId(leadActivity.LeadId);

                leadActivity.LeadId = leadDbId;
                leadActivity.ActivityId = activityDBId;
                _unitOfWork.LeadActivities.AddLeadActivity(leadActivity);
            }

            _unitOfWork.Complete();
            return true;
        }

        public void PullMissingLeads(List<int> leadIdList)
        {
            foreach (var leadId in leadIdList)
            {
                var leadIsInDB = _unitOfWork.Leads.LeadIsInDb(leadId);
                if (!leadIsInDB)
                {
                    var json = marketoAPIHelper.PullMissingLeadRequest(leadId);
                    var leadInfo = JsonConvert.DeserializeObject<RequestResult>(json);

                    if (leadInfo.result.FirstOrDefault().sfdcLeadId == null)
                    {
                        json = marketoAPIHelper.PullMissingLeadRequest(leadId, "sfdcContactId");
                        leadInfo = JsonConvert.DeserializeObject<RequestResult>(json);
                    }
                    if (leadInfo.success)
                    {

                        var lead = new Lead()
                        {
                            LeadId = leadId,
                            SFDCId = leadInfo.result.FirstOrDefault().sfdcLeadId,
                            CampaignId = leadInfo.result.FirstOrDefault().SFDCCampaignID
                        };
                        _unitOfWork.Leads.AddLead(lead);
                    }
                    else
                        ErrorHandling("Get Lead Info", JsonConvert.DeserializeObject<RequestResultError>(json));

                    Thread.Sleep(2000);
                }
            }
            _unitOfWork.Complete();
        }

        public void ErrorHandling(string method, RequestResultError resultError)
        {
            GlobalContext.Properties["Method"] = method;
            log.Error($"Error returned with code: {resultError.errors.Single().code}",
                new Exception(resultError.errors.Single().message));
        }
    }
}
