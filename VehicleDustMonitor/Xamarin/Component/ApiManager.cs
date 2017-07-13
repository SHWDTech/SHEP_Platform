using System;
using System.IO;
using System.Net;
using System.Text;

namespace VehicleDustMonitor.Xamarin.Component
{
    public class ApiManager
    {
        private const string ApiServer = "http://yun.shweidong.com:10888";

        private static readonly string ApiLogin = $"{ApiServer}/Account/VehicleLogin";

        private static readonly string ApiLastData = $"{ApiServer}/Ajax/DeviceRecentData";

        public const string HttpMethodPost = "POST";

        public const string HttpMethodGet = "GET";

        private const string LoginParamterNamePassword = "password";

        private const string GetLastDataParamterNameDevId = "devId";

        public static void StartRequest(string api, string method, XHttpRequestParamters paramter, HttpResponseHandler handler)
        {
            var request = (HttpWebRequest)WebRequest.Create(api);
            request.Method = method;
            request.Accept = "application/json";
            request.ContentType = "application/x-www-form-urlencoded";
            foreach (var headerString in paramter.HeaderStrings)
            {
                request.Headers[headerString.Key] = headerString.Value;
            }

            var builder = new StringBuilder();
            foreach (var bodyParamter in paramter.BodyParamters)
            {
                builder.AppendFormat("&{0}={1}", bodyParamter.Key, bodyParamter.Value);
            }
            builder.Remove(0, 1);

            request.BeginGetRequestStream(PostCallBack, new HttpRequestAsyncState(request, builder, handler));
        }

        private static void PostCallBack(IAsyncResult asynchronousResult)
        {
            var asyncResult = (HttpRequestAsyncState)asynchronousResult.AsyncState;
            try
            {
                var postStream = asyncResult.Request.EndGetRequestStream(asynchronousResult);
                var byteArray = Encoding.UTF8.GetBytes(asyncResult.BodyParamters.ToString());
                postStream.Write(byteArray, 0, byteArray.Length);

                asyncResult.Request.BeginGetResponse(ReadCallBack, new HttpResponseAsyncResult(asyncResult.Request, asyncResult.Handler));
            }
            catch (Exception ex)
            {
                asyncResult.Handler.ProcessError(ex);
            }
        }

        private static void ReadCallBack(IAsyncResult asynchronousResult)
        {
            var asyncResult = (HttpResponseAsyncResult)asynchronousResult.AsyncState;
            try
            {
                var reponse = asyncResult.Request.EndGetResponse(asynchronousResult);
                var stream = reponse.GetResponseStream();
                if (stream == null)
                {
                    asyncResult.Handler.ProcessResponse(string.Empty);
                    return;
                }
                using (var reader = new StreamReader(stream))
                {
                    var responseStr = reader.ReadToEnd();
                    asyncResult.Handler.ProcessResponse(responseStr);
                }
            }
            catch (Exception ex)
            {
                asyncResult.Handler.ProcessError(ex);
            }
        }

        public static void Login(string userName, string password, HttpResponseHandler handler)
        {
            var requestParams = new XHttpRequestParamters();
            requestParams.AddBodyParamter("username", userName);
            requestParams.AddBodyParamter(LoginParamterNamePassword, password);
            StartRequest(ApiLogin, HttpMethodPost, requestParams, handler);
        }

        public static void GetLastData(string devId, HttpResponseHandler handler)
        {
            var requestParams = new XHttpRequestParamters();
            requestParams.AddBodyParamter(GetLastDataParamterNameDevId, devId);
            StartRequest(ApiLastData, HttpMethodPost, requestParams, handler);
        }
    }
}