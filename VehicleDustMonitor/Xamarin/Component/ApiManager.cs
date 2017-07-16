using System;
using System.IO;
using System.Net;
using System.Text;
using VehicleDustMonitor.Xamarin.Model;

namespace VehicleDustMonitor.Xamarin.Component
{
    public class ApiManager
    {
        private const string ApiServer = "http://yun.shweidong.com:10888";

        private static readonly string ApiLogin = $"{ApiServer}/Account/VehicleLogin";

        private static readonly string ApiLastData = $"{ApiServer}/Ajax/DeviceRecentData";

        private static readonly string ApiUploadRecord = $"{ApiServer}/Ajax/UploadVehicleRecord";

        private static readonly string ApiRefreshCordinate = $"{ApiServer}/Ajax/RefreshCordinate";

        public const string HttpMethodPost = "POST";

        public const string HttpMethodGet = "GET";

        private const string LoginParamterNamePassword = "password";

        private const string GetLastDataParamterNameDevId = "devId";

        private const string UploadRecordParamterNameDevId = "DevId";

        private const string UploadRecordParamterNameRecordName = "RecordName";

        private const string UploadRecordParamterNameComment = "Comment";

        private const string UploadRecordParamterNameStartDateTime = "StartDateTime";

        private const string UploadRecordParamterNameEndDateTime = "EndDateTime";

        private const string RefreshCordinateParamterNameDevId = "devId";

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

        public static void UploadRecord(VehicleRecord record, HttpResponseHandler handler)
        {
            var requestParams = new XHttpRequestParamters();
            requestParams.AddBodyParamter(UploadRecordParamterNameDevId, $"{record.DevId}");
            requestParams.AddBodyParamter(UploadRecordParamterNameRecordName, $"{record.RecordName}");
            requestParams.AddBodyParamter(UploadRecordParamterNameComment, $"{record.Comment}");
            requestParams.AddBodyParamter(UploadRecordParamterNameStartDateTime, $"{record.StartDateTime}");
            requestParams.AddBodyParamter(UploadRecordParamterNameEndDateTime, $"{record.EndDateTime}");
            StartRequest(ApiUploadRecord, HttpMethodPost, requestParams, handler);
        }

        public static void RefreshCordinate(int devId, HttpResponseHandler handler)
        {
            var requestParams = new XHttpRequestParamters();
            requestParams.AddBodyParamter(RefreshCordinateParamterNameDevId, $"{devId}");
            StartRequest(ApiRefreshCordinate, HttpMethodPost, requestParams, handler);
        }
    }
}