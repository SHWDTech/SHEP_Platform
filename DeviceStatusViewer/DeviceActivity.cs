using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Android.App;
using Android.OS;
using Android.Widget;
using DeviceStatusViewer.Models;
using DeviceStatusViewer.Utility;
using System.Linq;
using Android.Content;
using Android.Views;

namespace DeviceStatusViewer
{
    [Activity(Label = @"设备列表")]
    public class DeviceActivity : ListActivity
    {
        private readonly List<DeviceInfomation> _deviceInfos = new List<DeviceInfomation>();

        private string _serverAddress;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            try
            {
                _serverAddress = Intent.GetStringExtra("ServerAddress") ?? string.Empty;
                _serverAddress = _serverAddress.Trim();
                var request = WebRequest.Create($"http://{_serverAddress}/Ajax/GetSystemDevices") as HttpWebRequest;
                if (request != null)
                {
                    request.Method = "GET";
                    request.ContentType = "application/x-www-form-urlencoded";
                    var httpresponse = (HttpWebResponse)request.GetResponse();
                    using (var stream = new StreamReader(httpresponse.GetResponseStream()))
                    {
                        var infos = stream.ReadToEnd();
                        _deviceInfos.AddRange(XmlSerializerHelper.DeSerialize<List<DeviceInfomation>>(infos));
                        var servers = _deviceInfos.Select(ser => ser.DeviceName).ToArray();
                        ListAdapter =
                            new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, servers);
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

        protected override void OnListItemClick(ListView l, View v, int position, long id)
        {
            var activity = new Intent(this, typeof(DeviceStatusActivity));
            activity.PutExtra("ServerAddress", _serverAddress);
            activity.PutExtra("DeviceId", _deviceInfos[position].DeviceId);
            StartActivity(activity);
        }
    }
}