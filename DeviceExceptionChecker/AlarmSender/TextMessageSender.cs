using System;
using System.IO;
using System.Net;
using System.Text;

namespace DeviceExceptionChecker.AlarmSender
{
    public class TextMessageSender
    {
        private const string UserName = "api";

        private const string Password = "key-3238b07e280986f5274b907a2af74238";

        private const string Url = "http://sms-api.luosimao.com/v1/send.json";

        public static void Send(string mobile, StringBuilder message)
        {
            var byteArray = Encoding.UTF8.GetBytes($"mobile={mobile}&message={message}");
            var webRequest = (HttpWebRequest)WebRequest.Create(new Uri(Url));
            var auth = "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes(UserName + ":" + Password));
            webRequest.Headers.Add("Authorization", auth);
            webRequest.Method = "POST";
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.ContentLength = byteArray.Length;

            var newStream = webRequest.GetRequestStream();
            newStream.Write(byteArray, 0, byteArray.Length);
            newStream.Close();
            var streamReader = ((HttpWebResponse)webRequest.GetResponse()).GetResponseStream();
            if (streamReader == null) return;
            using (var streamReader1 = new StreamReader(streamReader))
            {
                var msg = streamReader1.ReadToEnd();
                Console.WriteLine(msg);
            }
        }
    }
}
