using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using Android.Widget;
using CheeseBind;
using MikePhil.Charting.Charts;
using MikePhil.Charting.Components;
using MikePhil.Charting.Data;
using MikePhil.Charting.Interfaces.Datasets;
using MikePhil.Charting.Util;
using Newtonsoft.Json;
using VehicleDustMonitor.Xamarin.Component;
using VehicleDustMonitor.Xamarin.Model;
using AlertDialog = Android.Support.V7.App.AlertDialog;
using Android.Graphics;

namespace VehicleDustMonitor.Xamarin.Activity
{
    [Activity(Label = nameof(VehicleDustMonitor))]
    public class MainActivity : AppCompatActivity
    {
        [BindView(Resource.Id.lineChart)] protected LineChart RecentDataChart { get; set; }

        [BindView(Resource.Id.txtDeviceName)] protected TextView TxtDeviceName { get; set; }

        [BindView(Resource.Id.txtDeviceNodeId)] protected TextView TxtDeviceNodeId { get; set; }

        [BindView(Resource.Id.txtGeneralAvg)] protected TextView TxtGeneralAvg { get; set; }

        [BindView(Resource.Id.txtLastData)] protected TextView TxtLastData { get; set; }

        [BindView(Resource.Id.txtLastDataTime)] protected TextView TxtLastDateTime { get; set; }

        [BindView(Resource.Id.txtCordinate)] protected TextView TxtCordinate { get; set; }

        [BindView(Resource.Id.txtStartDateTime)] protected TextView TxtStartDateTime { get; set; }

        [BindView(Resource.Id.txtEndDateTime)] protected TextView TxtEndDateTime { get; set; }

        [BindView(Resource.Id.txtProjectName)] protected EditText TxtProjectName { get; set; }

        [BindView(Resource.Id.inputProject)] protected TextView InputProjectEditText { get; set; }

        [BindView(Resource.Id.inputComment)] protected TextView InputCommentEditText { get; set; }

        [BindView(Resource.Id.txtComment)] protected EditText TxtComment { get; set; }

        [BindView(Resource.Id.btnRecord)] protected Button BtnRecord { get; set; }

        private string _deviceName;

        private string _deviceNodeId;

        private int _deviceId;

        private bool _isRecordStarted;

        private bool _isQuit;

        private VehicleRecord _vehicleRecord;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            Cheeseknife.Bind(this);
            LoadBasicInfomation();
        }

        protected override void OnStart()
        {
            base.OnStart();
            if (string.IsNullOrWhiteSpace(_deviceName) || string.IsNullOrWhiteSpace(_deviceNodeId) || _deviceId == -1)
            {
                using (var builder = new AlertDialog.Builder(this))
                {
                    builder.SetMessage("登陆数据丢失，请重新登陆！")
                        .SetPositiveButton("退出", delegate
                        {
                            Finish();
                        })
                        .SetNegativeButton("前往登陆", delegate
                        {
                            Logout();
                            var intent = new Intent(this, typeof(LoginActivity));
                            StartActivity(intent);
                            Finish();
                        })
                        .Create()
                        .Show();
                }
                return;
            }
            Task.Factory.StartNew(RequestLastData);
            InitChartView();
        }

        protected override void OnDestroy()
        {
            _isQuit = true;
            base.OnDestroy();
        }

        private void InitChartView()
        {
            RecentDataChart.SetTouchEnabled(true);
            RecentDataChart.DragEnabled = true;
            RecentDataChart.SetScaleEnabled(true);
            RecentDataChart.ScaleYEnabled = false;
            RecentDataChart.SetPinchZoom(true);
            RecentDataChart.EnableScroll();

            RecentDataChart.Description.Enabled = false;

            RecentDataChart.XAxis.Enabled = false;
            RecentDataChart.AxisLeft.Enabled = false;
            RecentDataChart.AxisRight.Enabled = false;
        }

        private void SetLineChartData()
        {
            RecentDataChart.Clear();
            var yVals1 = new List<Entry>();
            for (var i = 0; i < _vehicleRecord.RecordDatas.Count; i++)
            {
                var data = _vehicleRecord.RecordDatas[i];
                using (var entry = new Entry(i, (float)data))
                {
                    yVals1.Add(entry);
                }
            }

            LineDataSet set1;

            if (RecentDataChart.LineData != null && RecentDataChart.LineData.DataSetCount > 0)
            {
                set1 = (LineDataSet)RecentDataChart.LineData.GetDataSetByIndex(0);
                set1.Values = yVals1;

                RecentDataChart.LineData.NotifyDataChanged();
                RecentDataChart.NotifyDataSetChanged();
            }
            else
            {
                using (set1 = new LineDataSet(yVals1, string.Empty))
                {
                    set1.AxisDependency = YAxis.AxisDependency.Left;
                    set1.Color = ColorTemplate.HoloBlue;
                    set1.SetCircleColor(Color.White);
                    set1.LineWidth = 4f;
                    set1.CircleRadius = 3f;
                    set1.FillAlpha = 65;

                    set1.SetDrawValues(_vehicleRecord.RecordDatas.Count != 0);

                    set1.FillColor = ColorTemplate.HoloBlue;
                    set1.HighLightColor = Color.Rgb(244, 117, 117);
                    set1.SetDrawCircleHole(false);

                    var dataSets = new List<ILineDataSet> { set1 };

                    var data = new LineData(dataSets);
                    data.SetValueTextColor(Color.White);
                    data.SetValueTextSize(12f);

                    RecentDataChart.Data = data;
                    RecentDataChart.Invalidate();
                }
            }
        }

        private void LoadBasicInfomation()
        {
            var preferences = GetSharedPreferences(nameof(VehicleDustMonitor), FileCreationMode.Private);
            var deviceName = preferences.GetString("SavedDeviceName", string.Empty);
            var deviceNodeId = preferences.GetString("SavedDeviceNodeId", string.Empty);
            _deviceId = preferences.GetInt("SavedDeviceId", -1);

            TxtDeviceName.Text = _deviceName = deviceName;
            TxtDeviceNodeId.Text = _deviceNodeId = deviceNodeId;
            var cordinate = preferences.GetString("SavedCordinate", string.Empty);
            TxtCordinate.Text = cordinate;
        }

        private void RequestLastData()
        {
            while (!_isQuit)
            {
                ApiManager.GetLastData($"{_deviceId}", new HttpResponseHandler
                {
                    OnResponse = args =>
                    {
                        if (string.IsNullOrWhiteSpace(args.Response)) return;
                        var data = JsonConvert.DeserializeObject<DeviceRecentData>(args.Response);
                        
                        RunOnUiThread(() => UpdateInformation(data));
                    }
                });
                Thread.Sleep(60000);
            }
        }

        private void UpdateInformation(DeviceRecentData lastData)
        {
            TxtLastData.Text = $"{Math.Round(int.Parse(lastData.Tp) / 1000.0, 3)} mg/m³";
            TxtLastDateTime.Text = lastData.UpdateTime;
            if (_vehicleRecord != null && _isRecordStarted)
            {
                _vehicleRecord.RecordDatas.Add(Math.Round(int.Parse(lastData.Tp) / 1000.0, 3));
                SetLineChartData();
                TxtGeneralAvg.Text = $"{_vehicleRecord.RecordDatas.Average()}";
            }
        }

        private void Logout()
        {
            GetSharedPreferences(nameof(VehicleDustMonitor), FileCreationMode.Private).Edit().Clear().Apply();
        }

        [OnClick(Resource.Id.btnRecord)]
        protected void RecordControl(object sender, EventArgs args)
        {
            if (!_isRecordStarted)
            {
                StartRecord();
            }
            else
            {
                StopRecord();
            }
        }

        [OnClick(Resource.Id.inputProject)]
        protected void InputProject(object sender, EventArgs args)
        {
            TxtComment.Enabled = false;
            InputCommentEditText.Text = "输入";
            if (!TxtProjectName.Enabled)
            {
                TxtProjectName.Enabled = true;
                TxtProjectName.RequestFocus();
                InputProjectEditText.Text = "输入完成";
            }
            else
            {
                TxtProjectName.Enabled = false;
                InputProjectEditText.Text = "输入";
            }
            
        }

        [OnClick(Resource.Id.inputComment)]
        protected void InputComment(object sender, EventArgs args)
        {
            TxtProjectName.Enabled = false;
            InputProjectEditText.Text = "输入";
            if (!TxtComment.Enabled)
            {
                TxtComment.Enabled = true;
                TxtComment.RequestFocus();
                InputCommentEditText.Text = "输入完成";
            }
            else
            {
                TxtComment.Enabled = false;
                InputCommentEditText.Text = "输入";
            }
            
        }

        private void StartRecord(bool force = false)
        {
            if (_vehicleRecord != null && !_vehicleRecord.IsFinished && !force)
            {
                using (var builder = new AlertDialog.Builder(this))
                {
                    builder.SetMessage("当前记录未结束，是否确认重新开始记录?")
                        .SetPositiveButton("确定", delegate
                        {
                            StartRecord(true);
                        })
                        .SetNegativeButton("取消", delegate{})
                        .Create()
                        .Show();
                }
                return;
            }
            _isRecordStarted = true;
            _vehicleRecord = new VehicleRecord
            {
                DevId = _deviceId
            };
            BtnRecord.Text = "停止记录";
            var now = DateTime.Now;
            _vehicleRecord.StartDateTime = now;
            TxtStartDateTime.Text = $"{now: M-dd HH:mm:ss}";
            TxtEndDateTime.Text = "-";
        }

        private void StopRecord()
        {
            _isRecordStarted = false;
            BtnRecord.Text = "开始记录";
            var now = DateTime.Now;
            _vehicleRecord.EndDateTime = now;
            TxtEndDateTime.Text = $"{now: MM-dd HH:mm:ss}";
        }
    }
}

