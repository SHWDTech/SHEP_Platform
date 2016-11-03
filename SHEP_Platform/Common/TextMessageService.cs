using System;
using System.Net;
using System.Text;

namespace SHEP_Platform.Common
{
    public class TextMessageService
    {
        private const string UserName = "api";

        private const string Password = "key-3238b07e280986f5274b907a2af74238";

        private const string Url = "http://sms-api.luosimao.com/v1/send.json";
        public static void Send(string mobile, string statName)
        {
            var message = $"【扬尘超标提醒】尊敬的{statName}管理员，您的堆场已发生扬尘超标，请立即采取响应戳是降低扬尘（今日第一次15分钟均值超过1mg/m³）【上海英凡】";

            var byteArray = Encoding.UTF8.GetBytes("mobile=" + mobile + "&message=" + message);
            var webRequest = (HttpWebRequest)WebRequest.Create(new Uri(Url));
            var auth = "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes(UserName + ":" + Password));
            webRequest.Headers.Add("Authorization", auth);
            webRequest.Method = "POST";
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.ContentLength = byteArray.Length;

            var newStream = webRequest.GetRequestStream();
            newStream.Write(byteArray, 0, byteArray.Length);
            newStream.Close();
        }
    }
}