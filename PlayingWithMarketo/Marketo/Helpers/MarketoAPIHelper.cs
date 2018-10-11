using log4net;
using Newtonsoft.Json;
using PlayingWithMarketo.Marketo.DTO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace PlayingWithMarketo.Marketo.Helpers
{
    public static class MarketoAPIHelper
    {
        private static string host = ConfigurationManager.AppSettings["host"];
        private static string clientId = ConfigurationManager.AppSettings["clientId"];
        private static string clientSecret = ConfigurationManager.AppSettings["clientSecret"];
        private static string queryFields = ConfigurationManager.AppSettings["queryFields"];
        private static string activityTypeIds = ConfigurationManager.AppSettings["actiityTypeIds"];
        private static readonly ILog log = LogManager.GetLogger(typeof(MarketoAPIHelper));


        public static Dictionary<string, string> GetTokenRequest()
        {
            string url = host + "/identity/oauth/token?grant_type=client_credentials&client_id=" + clientId +
                "&client_secret=" + clientSecret;

            var json = APIRequestHelper.SendRequest(url);

            if (json != null)
                return JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            else
                return null;
        }

        public static string CreateExportJobRequest(DateTime startDate, DateTime endDate)
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
            return APIRequestHelper.SendRequest(url, dataToSend, "POST", true);
        }

        public static string QueueJobRequest(string exportJobId)
        {
            string url = $"{host}/bulk/v1/activities/export/{exportJobId}/enqueue.json";
            return APIRequestHelper.SendRequest(url, method: "POST", tokenRequired: true);
        }

        public static string GetJobStatusRequest(string exportJobId)
        {
            string url = $"{host}/bulk/v1/activities/export/{exportJobId}/status.json";
            return APIRequestHelper.SendRequest(url, tokenRequired: true);
        }

        public static string RetreiveDataRequest(string exportJobId)
        {
            string url = $"{host}/bulk/v1/activities/export/{exportJobId}/file.json";
            return APIRequestHelper.SendRequest(url, tokenRequired: true);
        }

        public static string PullMissingLeadRequest(int leadId)
        {
            var url = $"{host}/rest/v1/lead/{leadId}.json?fields=sfdcLeadId,id,SFDCCampaignID";
            return APIRequestHelper.SendRequest(url, tokenRequired: true);
        }
    }
}
