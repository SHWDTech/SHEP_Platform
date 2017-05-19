using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using Newtonsoft.Json;
using static HikCameraConfiger.Camera.CHCNetSDK;
using HikCameraConfiger.Model;

namespace HikCameraConfiger
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private NET_DVR_DEVICEINFO_V30 _deviceInfo;

        private int _loginUserId;

        public MainWindow()
        {
            InitializeComponent();
            TxtCameraIp.Text = ConfigurationManager.AppSettings["cameraIp"];
            TxtCameraPort.Text = ConfigurationManager.AppSettings["cameraPort"];
            NET_DVR_Init();
        }

        private void StartRegister(object sender, RoutedEventArgs e)
        {
            TxtMessage.AppendText("尝试激活设备。\r\n");
            if (!ActivateCamera())
            {
                TxtMessage.AppendText("激活设备失败了，找老张。\r\n");
                return;
            }
            TxtMessage.AppendText("激活设备成功，尝试登陆摄像头。\r\n");

            if (!CameraLogin())
            {
                TxtMessage.AppendText("登陆设备失败了，找老张。\r\n");
                return;
            }
            TxtMessage.AppendText("登陆设备成功，尝试配置摄像头。\r\n");

            if (!ConfigCamera())
            {
                TxtMessage.AppendText("配置摄像头失败了，找老张。\r\n");
                return;
            }

            TxtMessage.AppendText("配置摄像头成功，尝试注册摄像头到萤石云。\r\n");
            if (!OpenYsRegiste())
            {
                TxtMessage.AppendText("注册摄像头到萤石云失败了，找老张。\r\n");
                return;
            }
            TxtMessage.AppendText("注册摄像头到萤石云成功，尝试调整网络配置。\r\n");
            if (!CameraNetCfg())
            {
                TxtMessage.AppendText("调整网络配置失败了，找老张。\r\n");
                return;
            }
            TxtMessage.AppendText("调整网络配置成功，自动配置完成。\r\n");
        }

        private bool ActivateCamera()
        {
            var pwd = new byte[16];
            var pwdBytes = Encoding.UTF8.GetBytes("juli#406");
            Array.Copy(pwdBytes, pwd, pwdBytes.Length);
            var activateCfg = new NET_DVR_ACTIVATECFG();
            activateCfg.dwSize = (uint)Marshal.SizeOf(activateCfg);
            activateCfg.byRes = new byte[108];
            activateCfg.sPassword = pwd;
            var ptr = Marshal.AllocHGlobal((int)activateCfg.dwSize);
            Marshal.StructureToPtr(activateCfg, ptr, false);
            var registed = NET_DVR_ActivateDevice(TxtCameraIp.Text, uint.Parse(TxtCameraPort.Text), ptr);
            Marshal.FreeHGlobal(ptr);
            if (registed) return true;
            var lastError = NET_DVR_GetLastError();
            return lastError == 252;
        }

        private bool ConfigCamera()
        {
            if (!ConfigurationCompressingCfg())
            {
                TxtMessage.AppendText("配置摄像头视频压缩参数失败了，找老张。\r\n");
                return false;
            }

            return true;
        }

        private bool ConfigurationCompressingCfg()
        {
            var vedioCompress = new NET_DVR_COMPRESSIONCFG_V30();
            vedioCompress.dwSize = (uint)Marshal.SizeOf(vedioCompress);
            var ptr = Marshal.AllocHGlobal((int)vedioCompress.dwSize);
            Marshal.StructureToPtr(vedioCompress, ptr, false);
            uint cfgLength = 0;
            var read = NET_DVR_GetDVRConfig(_loginUserId, NET_DVR_GET_COMPRESSCFG_V30, 1, ptr, vedioCompress.dwSize,
                ref cfgLength);
            if (!read)
            {
                return false;
            }
            vedioCompress = (NET_DVR_COMPRESSIONCFG_V30)Marshal.PtrToStructure(ptr, typeof(NET_DVR_COMPRESSIONCFG_V30));
            vedioCompress.struNormHighRecordPara.byStreamType = 0;
            vedioCompress.struNormHighRecordPara.byResolution = 19;
            vedioCompress.struNormHighRecordPara.byBitrateType = 0;
            vedioCompress.struNormHighRecordPara.byPicQuality = 0;
            vedioCompress.struNormHighRecordPara.dwVideoBitrate = 23;
            vedioCompress.struNormHighRecordPara.dwVideoFrameRate = 14;
            vedioCompress.struNormHighRecordPara.byVideoEncComplexity = 2;
            Marshal.StructureToPtr(vedioCompress, ptr, false);
            var write = NET_DVR_SetDVRConfig(_loginUserId, NET_DVR_SET_COMPRESSCFG_V30, 1, ptr, vedioCompress.dwSize);
            Marshal.FreeHGlobal(ptr);
            if (!write)
            {
                return false;
            }
            return true;
        }

        private bool CameraLogin()
        {
            _deviceInfo = new NET_DVR_DEVICEINFO_V30();
            _loginUserId = NET_DVR_Login_V30(TxtCameraIp.Text, int.Parse(TxtCameraPort.Text), "admin", "juli#406", ref _deviceInfo);
            return _loginUserId >= 0;
        }

        private bool OpenYsRegiste()
        {
            var request = (HttpWebRequest) WebRequest.Create("https://open.ys7.com/api/lapp/device/add");
            request.Host = "open.ys7.com";
            request.ContentType = "application/x-www-form-urlencoded";
            request.Method = "POST";
            var accessToken = GetAccessToken();
            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write($"accessToken={accessToken}&deviceSerial={TxtCameraSerialNumber.Text}&validateCode={TxtCameraSecureCode.Text}");
                streamWriter.Flush();
                streamWriter.Close();

                var streamReader = ((HttpWebResponse)request.GetResponse()).GetResponseStream();
                var response = streamReader != null ? new StreamReader(streamReader).ReadToEnd() : string.Empty;
                var code = JsonConvert.DeserializeObject<RegisterDeviceResponse>(response);
                return code.code == "200" || code.code == "20017";
            }
        }

        private static string GetAccessToken()
        {
            var request = (HttpWebRequest)WebRequest.Create("https://open.ys7.com/api/lapp/token/get");
            request.Host = "open.ys7.com";
            request.ContentType = "application/x-www-form-urlencoded";
            request.Method = "POST";
            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write($"appKey=9f88209c239d4bf28156d3f880bb8321&appSecret=f013a79dd3c9966123fd408be34c557e");
                streamWriter.Flush();
                streamWriter.Close();

                var streamReader = ((HttpWebResponse)request.GetResponse()).GetResponseStream();
                var response = streamReader != null ? new StreamReader(streamReader).ReadToEnd() : string.Empty;
                var accessToken = JsonConvert.DeserializeObject<AccessTokenResponse>(response);

                return accessToken.data.accessToken;
            }
        }

        private bool CameraNetCfg()
        {
            var netcfg = new NET_DVR_NETCFG_V30();
            netcfg.dwSize = (uint)Marshal.SizeOf(netcfg);
            var ptr = Marshal.AllocHGlobal((int)netcfg.dwSize);
            Marshal.StructureToPtr(netcfg, ptr, false);
            uint cfgLength = 0;
            var read = NET_DVR_GetDVRConfig(_loginUserId, NET_DVR_GET_NETCFG_V30, -1, ptr, netcfg.dwSize,
                ref cfgLength);
            if (!read)
            {
                return false;
            }
            netcfg = (NET_DVR_NETCFG_V30)Marshal.PtrToStructure(ptr, typeof(NET_DVR_NETCFG_V30));
            netcfg.byUseDhcp = 1;
            Marshal.StructureToPtr(netcfg, ptr, false);
            var write = NET_DVR_SetDVRConfig(_loginUserId, NET_DVR_SET_NETCFG_V30, -1, ptr, netcfg.dwSize);
            Marshal.FreeHGlobal(ptr);
            if (!write)
            {
                var error = NET_DVR_GetLastError();
                Console.WriteLine(error);
                return false;
            }

            return true;
        }

        private void OnClose(object sender, EventArgs e)
        {
            NET_DVR_Cleanup();
        }
    }
}
