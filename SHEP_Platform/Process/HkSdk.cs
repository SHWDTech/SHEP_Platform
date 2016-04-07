using System;
using System.Runtime.InteropServices;

namespace SHEP_Platform.Process
{
    public class HkSdk
    {

        [DllImport(@"OpenNetStream.dll")]//SDK启动
        public static extern int OpenSDK_InitLib(string authAddr, string platform, string appId);


        [DllImport(@"OpenNetStream.dll")]//SDK关闭
        public static extern int OpenSDK_FiniLib();


        [DllImport(@"OpenNetStream.dll")]//SDK第三方登陆
        public static extern int OpenSDK_Mid_Login(ref string pToken, ref int tokenLth);

        [DllImport(@"OpenNetStream.dll")]
        public static extern int OpenSDK_HttpSendWithWait(string szUri, string szHeaderParam, string szBody, out IntPtr iMessage, out int iLength);

        [DllImport(@"OpenNetStream.dll")]//SDK申请会话
        public static extern int OpenSDK_AllocSession(MsgHandler callBack, IntPtr userId, ref IntPtr pSid, ref int sidLth, bool bSync, uint timeout);
        //及其回调函数格式
        public delegate int MsgHandler(IntPtr sid, uint msgType, uint error, string info, IntPtr pUser);

        //回调实例
        public static int HandlerWork(IntPtr sid, uint msgType, uint error, string info, IntPtr pUser) => 0;

        [DllImport(@"OpenNetStream.dll")]//SDK关闭会话
        public static extern int OpenSDK_FreeSession(string sid);

        [DllImport(@"OpenNetStream.dll")]//SDK开始播放
        public static extern int OpenSDK_StartRealPlay(IntPtr sid, IntPtr playWnd, string cameraId, string token, int videoLevel, string safeKey, string appKey, uint pNscbMsg);

        [DllImport(@"OpenNetStream.dll")]//SDK关闭播放
        public static extern int OpenSDK_StopRealPlay(IntPtr sid, uint pNscbMsg);


        [DllImport(@"OpenNetStream.dll")]//截屏
        public static extern int OpenSDK_CapturePicture(IntPtr sid, string szFileName);

        [DllImport(@"OpenNetStream.dll")]//设置数据回调窗口
        public static extern int OpenSDK_SetDataCallBack(IntPtr sessionId, OpenSdkDataCallBack pDataCallBack, string pUser);

        //SDK获取所有设备摄像机列表
        [DllImport(@"OpenNetStream.dll")]
        public static extern int OpenSDK_Data_GetDevList(string accessToken, int iPageStart, int iPageSize, out IntPtr iMessage, out int iLength);

        //回调函数格式
        public delegate void OpenSdkDataCallBack(CallBackDateType dateType, IntPtr dateContent, int dataLen, string pUser);
        public static void DataCallBackHandler(CallBackDateType dataType, IntPtr dataContent, int dataLen, string pUser)
        {

        }
        //数据回调设置
        public enum CallBackDateType
        {
            NetDvrSyshead = 0,
            NetDvrStreamdata = 1,
            NetDvrRecvEnd = 2,
        };

        /*
        //SDK Http请求接口
        [DllImport(@"\OpenNetStream.dll")]
        public static extern int OpenSDK_HttpSendWithWait(string szUri, string szHeaderParam, string szBody, out IntPtr iMessage, out int iLength);

        
        //SDK获取指定设备摄像机信息
        [DllImport(@"\OpenNetStream.dll")]
        public static extern int OpenSDK_Data_GetDeviceInfo(string accessToken, string szDeviceSerial, out IntPtr iMessage, out int iLength);

        //SDK播放指定摄像机
        [DllImport(@"\OpenNetStream.dll", EntryPoint = "OpenSDK_StartRealPlay", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern int OpenSDK_StartRealPlay(string szSessionId, IntPtr hPlayWnd, string szCameraId, string szAccessToken, int iVideoLevel, string szSafeKey, string szAppKey);
        */
    }
}
