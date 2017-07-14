using System;
using System.Collections.Generic;
using System.Linq;
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

        [BindView(Resource.Id.btnUploadRecord)] protected Button BtnUploadRecord { get; set; }

        [BindView(Resource.Id.title)] protected TextView TxtTitle { get; set; }

        private string _deviceName;

        private string _deviceNodeId;

        private int _deviceId;

        private bool _isRecordStarted;

        private bool _isQuit;

        private RecentDataRequestHandler _requestDataHandler;

        private VehicleRecord _vehicleRecord;

        private DateTime _lastDataDateTime = DateTime.MinValue;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            Cheeseknife.Bind(this);
            TxtTitle.Text = "未开始记录";
            _requestDataHandler = new RecentDataRequestHandler(this);

            _requestDataHandler.SendEmptyMessage(0);
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
            var yVals1 = _vehicleRecord.RecordDatas.Select((data, i) => new Entry(i, (float) data)).ToList();

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
                set1 = new LineDataSet(yVals1, string.Empty)
                {
                    AxisDependency = YAxis.AxisDependency.Left,
                    Color = ColorTemplate.HoloBlue
                };
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

        public void RequestLastData()
        {
            ApiManager.GetLastData($"{_deviceId}", new HttpResponseHandler
            {
                OnResponse = args =>
                {
                    if (string.IsNullOrWhiteSpace(args.Response)) return;
                    var data = JsonConvert.DeserializeObject<DeviceRecentData>(args.Response);
                    
                    UpdateInformation(data);
                    if (_isQuit) return;
                    _requestDataHandler.SendEmptyMessageDelayed(0, 15000);
                }
            });
        }

        private void UpdateInformation(DeviceRecentData lastData)
        {
            RunOnUiThread(() =>
            {
                var dataTime = DateTime.Parse(lastData.UpdateTime);
                if (_vehicleRecord != null && _isRecordStarted && dataTime > _lastDataDateTime)
                {
                    _lastDataDateTime = dataTime;
                    _vehicleRecord.RecordDatas.Add(Math.Round(int.Parse(lastData.Tp) / 1000.0, 3));
                    SetLineChartData();
                }
                TxtLastData.Text = $"{Math.Round(int.Parse(lastData.Tp) / 1000.0, 3)} mg/m³";
                TxtLastDateTime.Text = lastData.UpdateTime;
                TxtGeneralAvg.Text = _vehicleRecord == null || _vehicleRecord.RecordDatas.Count <= 0 ? "-" : $"{Math.Round(_vehicleRecord.RecordDatas.Average(), 3)}";
            });
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
            InputCommentEditText.Text = "编辑";
            if (!TxtProjectName.Enabled)
            {
                TxtProjectName.Enabled = true;
                TxtProjectName.RequestFocus();
                InputProjectEditText.Text = "编辑完成";
            }
            else
            {
                TxtProjectName.Enabled = false;
                InputProjectEditText.Text = "编辑";
            }

        }

        [OnClick(Resource.Id.inputComment)]
        protected void InputComment(object sender, EventArgs args)
        {
            TxtProjectName.Enabled = false;
            InputProjectEditText.Text = "编辑";
            if (!TxtComment.Enabled)
            {
                TxtComment.Enabled = true;
                TxtComment.RequestFocus();
                InputCommentEditText.Text = "编辑完成";
            }
            else
            {
                TxtComment.Enabled = false;
                InputCommentEditText.Text = "编辑";
            }

        }

        [OnClick(Resource.Id.btnUploadRecord)]
        protected void UploadRecord(object sender, EventArgs args)
        {
            if (_vehicleRecord == null || _isRecordStarted)
            {
                Toast.MakeText(this, "需要先完成一次记录。", ToastLength.Short).Show();
                return;
            }
            CheckUploadRecord();
        }

        private void StartRecord(bool force = false)
        {
            if (_vehicleRecord != null && !force)
            {
                var hint = _isRecordStarted 
                    ? "当前记录未结束，重新开始会丢失记录信息，是否确认重新开始记录?" 
                    : "当前记录未上传，重新开始会丢失记录信息，是否确认重新开始记录?";
                using (var builder = new AlertDialog.Builder(this))
                {
                    builder.SetMessage(hint)
                        .SetPositiveButton("确定", delegate
                        {
                            StartRecord(true);
                        })
                        .SetNegativeButton("取消", delegate { })
                        .Create()
                        .Show();
                }

                return;
            }
            _isRecordStarted = true;
            TxtTitle.Text = "记录中";
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
            TxtTitle.Text = "记录结束，未上传";
            BtnUploadRecord.Enabled = true;
            BtnRecord.Text = "开始记录";
            var now = DateTime.Now;
            _vehicleRecord.EndDateTime = now;
            TxtEndDateTime.Text = $"{now: MM-dd HH:mm:ss}";

            using (var builder = new AlertDialog.Builder(this))
            {
                builder.SetMessage("是否立即上传本次记录?")
                    .SetPositiveButton("立即上传", delegate
                    {
                        CheckUploadRecord();
                    })
                    .SetNegativeButton("稍候上传", delegate { })
                    .Create()
                    .Show();
            }
        }

        private void CheckUploadRecord()
        {
            if (string.IsNullOrWhiteSpace(TxtProjectName.Text) ||
                string.IsNullOrWhiteSpace(TxtComment.Text))
            {
                using (var builder = new AlertDialog.Builder(this))
                {
                    builder.SetMessage("测试点信息或备注信息未填写，确认上传吗?")
                        .SetPositiveButton("立即上传", delegate
                        {
                            UploadRecord();
                        })
                        .SetNegativeButton("完善信息", delegate { })
                        .Create()
                        .Show();
                }
                return;
            }
            UploadRecord();
        }

        private void UploadRecord()
        {
            ApiManager.UploadRecord(_vehicleRecord, new HttpResponseHandler
            {
                OnResponse = args =>
                {
                    
                }
            });
        }
    }

    public class RecentDataRequestHandler : Handler
    {
        private readonly MainActivity _activity;

        public RecentDataRequestHandler(MainActivity activity)
        {
            _activity = activity;
        }

        public override void HandleMessage(Message msg)
        {
            _activity.RequestLastData();
        }
    }
}

