using System;
using System.IO;
using System.Net;
using Android.App;
using Android.OS;
using Android.Widget;
using DeviceStatusViewer.Models;
using DeviceStatusViewer.Utility;

namespace DeviceStatusViewer
{
    [Activity(Label = @"设备状态")]
    public class DeviceStatusActivity : Activity
    {
        private string _serverAddress;

        private string _devId;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Main);
            _serverAddress = Intent.GetStringExtra("ServerAddress") ?? string.Empty;
            _serverAddress = _serverAddress.Trim();
            _devId = Intent.GetStringExtra("DeviceId") ?? string.Empty;
            var freashBtn = FindViewById<Button>(Resource.Id.btnFreash);
            freashBtn.Click += (sender, e) =>
            {
                GetDeviceStatus();
            };
            var recentBtn = FindViewById<Button>(Resource.Id.btnMostRecentData);
            recentBtn.Click += (sender, e) =>
            {
                GetDeviceRecentData();
            };
        }

        private void GetDeviceStatus()
        {
            try
            {
                var request = WebRequest.Create($"http://{_serverAddress}/Ajax/AppGetDeviceActivity?devId={_devId}") as HttpWebRequest;
                if (request != null)
                {
                    request.Method = "GET";
                    request.ContentType = "application/x-www-form-urlencoded";
                    var httpresponse = (HttpWebResponse)request.GetResponse();
                    using (var stream = new StreamReader(httpresponse.GetResponseStream()))
                    {
                        var infos = stream.ReadToEnd();
                        var devActivity = XmlSerializerHelper.DeSerialize<Models.DeviceActivity>(infos);
                        var lastConnect = FindViewById<TextView>(Resource.Id.LastConnect);
                        lastConnect.Text = devActivity.LastConnect;
                        var lastAutoUpload = FindViewById<TextView>(Resource.Id.LastAutoUpload);
                        lastAutoUpload.Text = devActivity.LastAutoUpload;
                        var lastHeartBeat = FindViewById<TextView>(Resource.Id.LastHeartBeat);
                        lastHeartBeat.Text = devActivity.LastConnect;
                    }
                }
            }
            catch (Exception)
            {
                using (var callDialog = new AlertDialog.Builder(this))
                {
                    callDialog.SetMessage("获取设备信息失败了。");
                    callDialog.SetPositiveButton("好的", delegate { });
                    callDialog.Show();
                }
            }
        }

        private void GetDeviceRecentData()
        {
            try
            {
                var request = WebRequest.Create($"http://{_serverAddress}/Ajax/DeviceLastData?devId={_devId}") as HttpWebRequest;
                if (request != null)
                {
                    request.Method = "GET";
                    request.ContentType = "application/x-www-form-urlencoded";
                    var httpresponse = (HttpWebResponse)request.GetResponse();
                    using (var stream = new StreamReader(httpresponse.GetResponseStream()))
                    {
                        var infos = stream.ReadToEnd();
                        if (string.IsNullOrWhiteSpace(infos)) return;
                        var recentData = XmlSerializerHelper.DeSerialize<DeviceRecentData>(infos);
                        var tp = FindViewById<TextView>(Resource.Id.TP);
                        tp.Text = recentData.Tp.Trim();
                        var db = FindViewById<TextView>(Resource.Id.DB);
                        db.Text = recentData.Db.Trim();
                        var pm25 = FindViewById<TextView>(Resource.Id.PM25);
                        pm25.Text = recentData.Pm25.Trim();
                        var pm100 = FindViewById<TextView>(Resource.Id.PM100);
                        pm100.Text = recentData.Pm100.Trim();
                        var windspeed = FindViewById<TextView>(Resource.Id.WindSpeed);
                        windspeed.Text = recentData.WindSpeed.Trim();
                        var winddir = FindViewById<TextView>(Resource.Id.WindDirection);
                        winddir.Text = recentData.WindDirection.Trim();
                        var temp = FindViewById<TextView>(Resource.Id.Temp);
                        temp.Text = recentData.Temp.Trim();
                        var humidity = FindViewById<TextView>(Resource.Id.Humidity);
                        humidity.Text = recentData.Humidity.Trim();
                        var updateTime = FindViewById<TextView>(Resource.Id.UpdateTime);
                        updateTime.Text = recentData.UpdateTime.Trim();
                    }
                }
            }
            catch (Exception)
            {
                using (var callDialog = new AlertDialog.Builder(this))
                {
                    callDialog.SetMessage("获取设备信息失败了。");
                    callDialog.SetPositiveButton("好的", delegate { });
                    callDialog.Show();
                }
            }
        }
    }
}