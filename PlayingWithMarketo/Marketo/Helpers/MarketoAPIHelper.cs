using log4net;
using Newtonsoft.Json;
using PlayingWithMarketo.Core;
using PlayingWithMarketo.Marketo.DTO;
using PlayingWithMarketo.Persistance;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace PlayingWithMarketo.Marketo.Helpers
{
    public class MarketoAPIHelper
    {
        private string host = ConfigurationManager.AppSettings["host"];
        private string clientId = ConfigurationManager.AppSettings["clientId"];
        private string clientSecret = ConfigurationManager.AppSettings["clientSecret"];
        private string queryFields = ConfigurationManager.AppSettings["queryFields"];
        private string activityTypeIds = ConfigurationManager.AppSettings["actiityTypeIds"];
        private readonly ILog log = LogManager.GetLogger(typeof(MarketoAPIHelper));
        private readonly APIRequestHelper requestHelper;
        private readonly IUnitOfWork _unitOfWork;

        public MarketoAPIHelper(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            requestHelper = new APIRequestHelper();
        }


        public Dictionary<string, string> GetTokenRequest()
        {
            string url = host + "/identity/oauth/token?grant_type=client_credentials&client_id=" + clientId +
                "&client_secret=" + clientSecret;

            var json = requestHelper.SendRequest(url);

            if (json != null)
                return JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            else
                return null;
        }

        public string CreateExportJobRequest(DateTime startDate, DateTime endDate)
        {
            string url = host + "/bulk/v1/activities/export/create.json";
            var fieldsArray = queryFields.Split(',');
            var activityTypeIdArray = activityTypeIds.Split(',').Select(Int32.Parse);

            var job = new Job()
            {
                format = "CSV",
                filter = new JobFilter()
                {
                    activityTypeIds = activityTypeIdArray.ToList(),
                    createdAt = new CreatedAt()
                    {
                        startAt = startDate.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                        endAt = endDate.ToString("yyyy-MM-ddTHH:mm:sszzz")
                    }
                },
                fields = fieldsArray.ToList()

            };

            var dataToSend = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(job));
            return requestHelper.SendRequest(url, dataToSend, "POST", new MarketoHelper(_unitOfWork).GetToken());
        }

        public string QueueJobRequest(string exportJobId)
        {
            string url = $"{host}/bulk/v1/activities/export/{exportJobId}/enqueue.json";
            return requestHelper.SendRequest(url, method: "POST", token: new MarketoHelper(_unitOfWork).GetToken());
        }

        public string GetJobStatusRequest(string exportJobId)
        {
            string url = $"{host}/bulk/v1/activities/export/{exportJobId}/status.json";
            return requestHelper.SendRequest(url, token: new MarketoHelper(_unitOfWork).GetToken());
        }

        public string RetreiveDataRequest(string exportJobId)
        {
            string url = $"{host}/bulk/v1/activities/export/{exportJobId}/file.json";
            return requestHelper.SendRequest(url, token: new MarketoHelper(_unitOfWork).GetToken());
        }

        public string PullMissingLeadRequest(int leadId)
        {
            var url = $"{host}/rest/v1/lead/{leadId}.json?fields=sfdcLeadId,id,SFDCCampaignID";
            return requestHelper.SendRequest(url, token: new MarketoHelper(_unitOfWork).GetToken());
        }
    }
}
