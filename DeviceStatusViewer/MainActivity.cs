using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Android.App;
using Android.OS;
using Android.Widget;
using DeviceStatusViewer.Models;
using DeviceStatusViewer.Utility;
using System;
using System.Threading.Tasks;
using Android.Views;
using Android.Content;

namespace DeviceStatusViewer
{
    [Activity(Label = @"上海卫东设备测试工具", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : ListActivity
    {
        private readonly List<ServerInfomation> _serverInfos = new List<ServerInfomation>();

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            Task.Factory.StartNew(() =>
            {
                try
                {
                    var request =
                        WebRequest.Create(
                            "http://yun.shweidong.com:10888/Ajax/GetDeviceTestServices") as HttpWebRequest;
                    if (request != null)
                    {
                        request.Method = "GET";
                        request.ContentType = "application/x-www-form-urlencoded";
                        var httpresponse = (HttpWebResponse) request.GetResponse();
                        using (var stream = new StreamReader(httpresponse.GetResponseStream()))
                        {
                            var infos = stream.ReadToEnd();
                            _serverInfos.AddRange(XmlSerializerHelper.DeSerialize<List<ServerInfomation>>(infos));
                            var servers = _serverInfos.Select(ser => ser.ServerName).ToArray();
                            RunOnUiThread(() =>
                            {
                                ListAdapter =
                                    new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, servers);
                            });
                        }
                    }
                }
                catch (Exception)
                {
                    using (var callDialog = new AlertDialog.Builder(this))
                    {
                        callDialog.SetMessage("获取服务器信息失败了。");
                        callDialog.SetPositiveButton("好的", delegate { });
                        callDialog.Show();
                    }
                }
            });
        }

        protected override void OnListItemClick(ListView l, View v, int position, long id)
        {
            //base.OnListItemClick(l, v, position, id);
            var activity = new Intent(this, typeof(DeviceActivity));
            var address = _serverInfos[position].ServerAddress;
            activity.PutExtra("ServerAddress", address);
            StartActivity(activity);
        }
    }
}

