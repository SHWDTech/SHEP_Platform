using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SHWDTech.Platform.Utility;

namespace SHEP_Platform.Common
{
    public static class HikAction
    {
        private static string _apiUrl = "https://open.ys7.com/api/method";

        private static string _authAddr = "https://auth.ys7.com";

        private static string _platformAddr = "https://open.ys7.com";

        private static string _appKey = "9f88209c239d4bf28156d3f880bb8321";

        private static string _secretKey = "f013a79dd3c9966123fd408be34c557e";

        private static string _phoneNumber = "18701987043";

        private static string AccessToken { get; set; }

        public static string SafeKey { get; set; }

        public static bool IsLoaded = false;

        private static string _userId = "106567315865215f";

        private static readonly HkSdk.MsgHandler CallBack = HandlerWork;

        private static IntPtr _sessionId;

        private static int _sessionIdLength;

        public static string SessionIdstr { get; private set; }

        private static int _playLever = 2;

        public static readonly List<string> CameraList = new List<string>();

        public static readonly List<string> CameraIdList = new List<string>();

        public static int Speed { get; set; } = 4;

        public static int InitLib()
        {
            var result = HkSdk.OpenSDK_InitLib(_authAddr, _platformAddr, _appKey);
            if (result != 0) return result;

            result = AllocSession();
            if (result != 0) return result;

            result = GetAccessToken();
            if (result != 0) return result;

            result = FatchCameraList();
            if (result != 0) return result;

            return result;
        }

        public static int SetUpParams(Dictionary<string, string> paramDictionary)
        {
            _apiUrl = paramDictionary["ApiUrl"];
            _authAddr = paramDictionary["AuthAddr"];
            _platformAddr = paramDictionary["PlatformAddr"];
            _appKey = paramDictionary["AppKey"];
            _secretKey = paramDictionary["SecretKey"];
            _phoneNumber = paramDictionary["PhoneNumber"];
            _userId = paramDictionary["UserId"];

            return 0;
        }

        public static int Close() => CloseAllocion(_sessionId);

        private static int AllocSession()
        {
            var userId = Marshal.StringToHGlobalAnsi(_userId);

            var result = HkSdk.OpenSDK_AllocSession(CallBack, userId, ref _sessionId, ref _sessionIdLength, false, uint.MaxValue);

            SessionIdstr = Marshal.PtrToStringAnsi(_sessionId, _sessionIdLength);

            return result;
        }

        private static int CloseAllocion(IntPtr sid)
        {
            var sid1 = sid.ToString();

            return HkSdk.OpenSDK_FreeSession(sid1);
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

        private static int GetAccessToken()
        {
            IntPtr iMessage;
            int iLength;
            var jsonStr = BuildParams("token");

            var result = HkSdk.OpenSDK_HttpSendWithWait(_apiUrl, jsonStr, "", out iMessage, out iLength);

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

            return result;
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
            var md5 = GetMd5($"phone:{_phoneNumber},method:{str},time:{totalSeconds},secret:{_secretKey}");

            return ("{\"id\":\"100\",\"system\":{\"key\":\"" + _appKey + "\",\"sign\":\"" + md5 + "\",\"time\":\"" + totalSeconds +
                "\",\"ver\":\"1.0\"},\"method\":\"" + str + "\",\"params\":{\"phone\":\"" + _phoneNumber + "\"}}");
        }

        public static string GetCameraId(string productId)
        {
            var camera = CameraList.First(obj => obj.ToString().Contains(productId));
            var index = CameraList.IndexOf(camera);
            return index > 0 ? CameraIdList[index] : string.Empty;
        }

        private static string GetMd5(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return string.Empty;
            }

            var md5 = new MD5CryptoServiceProvider();
            return BitConverter.ToString(md5.ComputeHash(Encoding.UTF8.GetBytes(str))).ToLower().Replace("-", "");
        }

        private static int FatchCameraList()
        {
            IntPtr iMessage;
            int iLength;

            int result = HkSdk.OpenSDK_Data_GetDevList(AccessToken, 0, 50, out iMessage, out iLength);
            var resultStr = Marshal.PtrToStringAnsi(iMessage);

            if (result != 0) return result;

            var jsonObj = JObject.Parse(resultStr);

            var cameraList = JArray.Parse(jsonObj["cameraList"].ToString());

            foreach (var cam in cameraList)
            {
                var cameraObj = JObject.Parse(cam.ToString());
                CameraList.Add(cameraObj["cameraName"].ToString());
                CameraIdList.Add(cameraObj["cameraId"].ToString());
            }

            return result;
        }

        public static int StartPlay(IntPtr handleIntPtr, string cameraId, string safeKey)
        => HkSdk.OpenSDK_StartRealPlay(_sessionId, handleIntPtr, cameraId, AccessToken, _playLever, safeKey, _appKey, 0);

        public static int StopPlay()
        {
            CloseAllocion(_sessionId);
            return HkSdk.OpenSDK_StopRealPlay(_sessionId, 0);
        }

        public static object[] GetCameraList()
        {
            var cameraObjects = new object[CameraList.Count];

            for (var i = 0; i < CameraList.Count; i++)
            {
                cameraObjects[i] = CameraList[i];
            }

            return cameraObjects;
        }

        /// <summary>
        /// 云台控制
        /// </summary>
        /// <param name="cameraId"></param>
        /// <param name="command"></param>
        /// <param name="action"></param>
        /// <param name="speed"></param>
        /// <returns></returns>
        public static int PtzCtrl(string cameraId, PTZCommand command,
            PTZACtion action, int speed)
            => HkSdk.OpenSDK_PTZCtrl(_sessionId, AccessToken, cameraId, command, action, speed, 0);


        public static object[] GetCameraIdList()
        {
            var cameraIdObjects = new object[CameraIdList.Count];

            for (var i = 0; i < CameraIdList.Count; i++)
            {
                cameraIdObjects[i] = CameraIdList[i];
            }

            return cameraIdObjects;
        }

        public static int TakePicture(string path, string fileName)
        {
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                return HkSdk.OpenSDK_CapturePicture(_sessionId, $"{path}//{fileName}");
            }
            catch (Exception ex)
            {
                LogService.Instance.Error("拍摄照片失败", ex);
                return -1;
            }

        }
    }
}
