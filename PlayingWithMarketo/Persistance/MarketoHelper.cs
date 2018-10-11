using log4net;
using Newtonsoft.Json;
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

        public static string GetToken()
        {
            using (var db = new MarketoDbContext())
            {
                var token = db.Tokens.FirstOrDefault(t => t.ExpiresAt > DateTime.UtcNow);

                if (token != null)
                    return token.AccessToken;

                Dictionary<string, string> dict = MarketoAPIHelper.GetTokenRequest();

                token = new Token()
                {
                    AccessToken = dict["access_token"],
                    CreatedAt = DateTime.UtcNow,
                    ExpiresAt = DateTime.UtcNow.AddSeconds(Double.Parse(dict["expires_in"])),
                    UserName = dict["scope"]
                };

                db.Tokens.Add(token);
                db.SaveChanges();

                return token.AccessToken;
            }
        }

        public static string CreateExportJob(DateTime startDate, DateTime endDate)
        {
            var json = MarketoAPIHelper.CreateExportJobRequest(startDate, endDate);
            var createJobResult = JsonConvert.DeserializeObject<RequestResult>(json);

            if (!createJobResult.success)
            {
                var errorRes = JsonConvert.DeserializeObject<RequestResultError>(json);
                ErrorHandling("Create Export Job", errorRes);

                return null;
            }

            using (var db = new MarketoDbContext())
            {
                var exportJob = new ExportJob()
                {
                    ExportId = createJobResult.result.Single().exportId,
                    Format = createJobResult.result.Single().format,
                    Status = createJobResult.result.Single().status,
                    CreatedAt = createJobResult.result.Single().createdAt
                };

                db.ExportJobs.Add(exportJob);
                db.SaveChanges();

                return exportJob.ExportId;
            }
        }

        public static void QueueJob(string exportJobId)
        {
            var json = MarketoAPIHelper.QueueJobRequest(exportJobId);
            var queueJobResult = JsonConvert.DeserializeObject<RequestResult>(json);

            if (!queueJobResult.success)
            {
                var errorRes = JsonConvert.DeserializeObject<RequestResultError>(json);
                ErrorHandling("Queue Job", errorRes);

                return;
            }
            using (var db = new MarketoDbContext())
            {
                var exportJobToQueue = db.ExportJobs.FirstOrDefault(j => j.ExportId == exportJobId);
                exportJobToQueue.Status = queueJobResult.result.Single().status;
                exportJobToQueue.QueuedAt = queueJobResult.result.Single().queuedAt;

                db.SaveChanges();
            }
        }

        public static Status? GetJobStatus(string exportJobId)
        {
            var json = MarketoAPIHelper.GetJobStatusRequest(exportJobId);
            var jobStatusResult = JsonConvert.DeserializeObject<RequestResult>(json);

            if (!jobStatusResult.success)
            {
                var errorRes = JsonConvert.DeserializeObject<RequestResultError>(json);
                ErrorHandling("Get Job Status", errorRes);

                return null;
            }
            using (var db = new MarketoDbContext())
            {
                var exportJob = db.ExportJobs.SingleOrDefault(j => j.ExportId == exportJobId);
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

                db.SaveChanges();

                return (Status)Enum.Parse(typeof(Status), status, true);
            }
        }

        public static bool RetreiveData(string exportJobId)
        {
            var result = MarketoAPIHelper.RetreiveDataRequest(exportJobId);
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


                leadActivitiesList.Add(leadActivity);
            }
            PullMissingLeads(leadIdList);
            using (var db = new MarketoDbContext())
            {
                foreach (var leadActivity in leadActivitiesList)
                {
                    int activityDBId = 0;
                    int leadDbId = 0;

                    activityDBId = db.Activities.Single(a => a.ActivityId == leadActivity.ActivityId).Id;
                    leadDbId = db.Leads.Single(l => l.LeadId == leadActivity.LeadId).Id;

                    leadActivity.LeadId = leadDbId;
                    leadActivity.ActivityId = activityDBId;

                    db.LeadActivities.Add(leadActivity);
                }

                db.SaveChanges();
            }
            return true;
        }

        public static void PullMissingLeads(List<int> leadIdList)
        {
            using (var db = new MarketoDbContext())
            {
                foreach (var leadId in leadIdList)
                {
                    var leadIsInDB = db.Leads.Any(l => l.LeadId == leadId);
                    if (!leadIsInDB)
                    {
                        var json = MarketoAPIHelper.PullMissingLeadRequest(leadId);

                        var leadInfo = JsonConvert.DeserializeObject<RequestResult>(json);
                        if (leadInfo.success)
                        {

                            var lead = new Lead()
                            {
                                LeadId = leadId,
                                SFDCId = leadInfo.result.FirstOrDefault().sfdcLeadId,
                                CampaignId = leadInfo.result.FirstOrDefault().SFDCCampaignID
                            };
                            db.Leads.Add(lead);
                        }
                        else
                            ErrorHandling("Get Lead Info", JsonConvert.DeserializeObject<RequestResultError>(json));

                        Thread.Sleep(2000);
                    }
                }
                db.SaveChanges();
            }
        }

        public static void ErrorHandling(string method, RequestResultError resultError)
        {
            GlobalContext.Properties["Method"] = method;
            log.Error($"Error returned with code: {resultError.errors.Single().code}",
                new Exception(resultError.errors.Single().message));
        }
    }
}
