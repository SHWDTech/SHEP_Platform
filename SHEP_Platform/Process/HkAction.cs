using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SHEP_Platform.Process
{
    public static class HkAction
    {
        private const string ApiUrl = "https://open.ys7.com/api/method";

        private const string AuthAddr = "https://auth.ys7.com";

        private const string PlatformAddr = "https://open.ys7.com";

        private const string AppKey = "9f88209c239d4bf28156d3f880bb8321";

        private const string SecretKey = "f013a79dd3c9966123fd408be34c557e";

        private const string PhoneNumber = "18701987043";

        private static string AccessToken { get; set; }

        public static bool IsLoaded = false;

        private const string UserId = "106567315865215f";

        private static readonly HkSdk.MsgHandler CallBack = HandlerWork;

        private static IntPtr _sessionId;

        private static int _sessionIdLength;

        private static string _sessionIdstr;

        private static int _playLever = 2;

        private static string _safeKey = "YNPWBD";

        private static readonly List<string> CameraList = new List<string>();

        private static readonly List<string> CameraIdList = new List<string>();

        public static bool Start()
            => (HkSdk.OpenSDK_InitLib(AuthAddr, PlatformAddr, AppKey) == 0);

        public static IntPtr AllocSession()
        {
            var userId = Marshal.StringToHGlobalAnsi(UserId);

            HkSdk.OpenSDK_AllocSession(CallBack, userId, ref _sessionId, ref _sessionIdLength, false, uint.MaxValue);

            _sessionIdstr = Marshal.PtrToStringAnsi(_sessionId, _sessionIdLength);

            return _sessionId;
        }

        public static bool CloseAllocion(IntPtr sid)
        {
            var sid1 = sid.ToString();

            return (HkSdk.OpenSDK_FreeSession(sid1) == 0);
        }

        public static bool CapturePicture(string fileName)
        {
            if (!string.IsNullOrWhiteSpace(fileName))
            {
                return HkSdk.OpenSDK_CapturePicture(_sessionId, fileName) == 0;
            }

            return false;
        }


        private static int HandlerWork(IntPtr sessionId, uint msgType, uint error, string info, IntPtr pUser)
        {
            switch (msgType)
            {
                case 20:
                    JObject obj = (JObject)JsonConvert.DeserializeObject(info);
                    Console.WriteLine(obj);
                    break;
                case 3:
                    break;
                case 4:
                    break;
                case 5:
                    break;
            }

            return 0;
        }

        public static string GetAccessToken()
        {
            IntPtr iMessage;
            int iLength;
            var jsonStr = BuildParams("token");

            var result = HkSdk.OpenSDK_HttpSendWithWait(ApiUrl, jsonStr, "", out iMessage, out iLength);

            var returnStr = Marshal.PtrToStringAnsi(iMessage, iLength);

            if (result == 0)
            {
                JObject jObject = (JObject)JsonConvert.DeserializeObject(returnStr);
                if (jObject["result"]["code"].ToString() == "200")
                {
                    AccessToken = jObject["result"]["data"]["accessToken"].ToString();

                    Debug.WriteLine(AccessToken);

                }
                else
                {
                    Debug.WriteLine(jObject["result"]["code"].ToString());
                }
            }

            return AccessToken;
        }

        private static string BuildParams(string type)
        {
            var str = string.Empty;
            var typestr = type.ToLower();

            if (typestr != "token")
            {
                if (typestr == "msg")
                {
                    str = "msg/get";
                }
            }
            else
            {
                str = "token/getAccessToken";
            }

            var span = DateTime.Now - TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(0x7b2, 1, 1, 0, 0, 0));
            var totalSeconds = $"{Math.Round(span.TotalSeconds, 0)}";
            var md5 = GetMd5($"phone:{PhoneNumber},method:{str},time:{totalSeconds},secret:{SecretKey}");

            return ("{\"id\":\"100\",\"system\":{\"key\":\"" + AppKey + "\",\"sign\":\"" + md5 + "\",\"time\":\"" + totalSeconds +
                "\",\"ver\":\"1.0\"},\"method\":\"" + str + "\",\"params\":{\"phone\":\"" + PhoneNumber + "\"}}");
        }

        public static string GetMd5(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return string.Empty;
            }

            var md5 = new MD5CryptoServiceProvider();
            return BitConverter.ToString(md5.ComputeHash(Encoding.UTF8.GetBytes(str))).ToLower().Replace("-", "");
        }

        public static void GetCameraList()
        {
            IntPtr iMessage;
            int iLength;

            int result = HkSdk.OpenSDK_Data_GetDevList(AccessToken, 0, 50, out iMessage, out iLength);
            var resultStr = Marshal.PtrToStringAnsi(iMessage);

            if (result != 0) return;

            var jsonObj = JObject.Parse(resultStr);

            var cameraList = JArray.Parse(jsonObj["cameraList"].ToString());

            foreach (var cam in cameraList)
            {
                var cameraObj = JObject.Parse(cam.ToString());
                CameraList.Add(cameraObj["cameraName"].ToString());
                CameraIdList.Add(cameraObj["cameraId"].ToString());
            }
        }

        public static void StartPlay()
        {
            var pic = new PictureBox();
            var flag = HkSdk.OpenSDK_StartRealPlay(_sessionId, pic.Handle, CameraIdList[0], AccessToken, _playLever, _safeKey, AppKey, 0);
            Console.WriteLine(flag == 0);
        }
    }
}
