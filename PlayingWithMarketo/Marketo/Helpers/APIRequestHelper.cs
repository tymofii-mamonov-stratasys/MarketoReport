using log4net;
using PlayingWithMarketo.Persistance;
using System;
using System.IO;
using System.Net;

namespace PlayingWithMarketo.Marketo.Helpers
{
    public static class APIRequestHelper
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(APIRequestHelper));
        public static string SendRequest(string url, byte[] dataToSend = null, string method = "GET", bool tokenRequired = false)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.ContentType = "application/json";
            if (tokenRequired)
            {
                var value = "Bearer " + MarketoHelper.GetToken();
                request.Headers.Add(HttpRequestHeader.Authorization, value);
            }

            switch (method)
            {
                case "POST":
                    request.Method = "POST";
                    if (dataToSend != null)
                    {
                        request.ContentLength = dataToSend.Length;
                        request.GetRequestStream().Write(dataToSend, 0, dataToSend.Length);
                    }
                    break;
                default:
                    break;
            }

            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream resStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(resStream);
                return reader.ReadToEnd();
            }
            catch (Exception e)
            {
                log.Error(e.Message, e);
                return null;
            }

        }
    }
}
