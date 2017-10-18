using System.Collections.Generic;
using Android.Content;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using VehicleDustMonitor.Xamarin.activity;
using VehicleDustMonitor.Xamarin.Component;
using VehicleDustMonitor.Xamarin.Model;

namespace VehicleDustMonitor.Xamarin.adapter
{
    internal class HistoryRecordAdapter : BaseAdapter<HistoryRecordItem>
    {
        private readonly HistoryRecordActivity _context;

        private List<HistoryRecordItem> Items { get; }

        public HistoryRecordAdapter(HistoryRecordActivity context, List<HistoryRecordItem> items)
        {
            _context = context;
            Items = items;
        }


        public override Java.Lang.Object GetItem(int position)
        {
            return position;
        }

        public override HistoryRecordItem this[int position] => Items[position];

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = Items[position];
            var view = _context.LayoutInflater.Inflate(Resource.Layout.item_vehicle_record, null);
            view.FindViewById<TextView>(Resource.Id.recordName).Text = item.RecordName;
            view.FindViewById<TextView>(Resource.Id.devId).Text = $"{item.DevId}";
            view.FindViewById<TextView>(Resource.Id.recordComment).Text = item.Comment;
            view.FindViewById<TextView>(Resource.Id.startDateTime).Text = $"{item.StartDateTime:yyyy-MM-dd HH:mm:ss}";
            view.FindViewById<TextView>(Resource.Id.endDateTime).Text = $"{item.EndDateTime:yyyy-MM-dd HH:mm:ss}";
            view.FindViewById<TextView>(Resource.Id.average).Text = $"{item.AverageValue}";
            view.FindViewById<TextView>(Resource.Id.hasupload).Text = item.HasUpload ? "已上传" : "未上传";
            var uploadBtn = view.FindViewById<ImageView>(Resource.Id.doupload);
            if (item.HasUpload)
            {
                uploadBtn.Visibility = ViewStates.Gone;
            }
            uploadBtn.Click += (sender, clickArgs) =>
            {
                var record = new VehicleRecord
                {
                    RecordName = item.RecordName,
                    Comment = item.Comment,
                    DevId = item.DevId,
                    StartDateTime = item.StartDateTime,
                    EndDateTime = item.EndDateTime,
                    DatabaseId = item.Id
                };
                ApiManager.UploadRecord(record, item.Lat, item.Lng, new HttpResponseHandler
                {
                    OnResponse = args =>
                    {
                        var success = JsonConvert.DeserializeObject<bool>(args.Response);
                        if (success)
                        {
                            _context.RunOnUiThread(() =>
                            {
                                Toast.MakeText(_context, "上传成功！", ToastLength.Short).Show();
                                var values = new ContentValues();
                                values.Put(VehicleRecordEntity.ColumnNameUploaded, 1);
                                VehicleRecordEntity.DoUpdate(VehicleRecordHelper.Instance.WritableDatabase, record.DatabaseId,
                                    values);
                                uploadBtn.Visibility = ViewStates.Gone;
                                view.FindViewById<TextView>(Resource.Id.hasupload).Text = "已上传";
                            });
                        }
                        else
                        {
                            _context.RunOnUiThread(() =>
                            {
                                Toast.MakeText(_context, "上传失败！", ToastLength.Short).Show();
                            });
                        }
                    },
                    OnError = args =>
                    {
                        _context.RunOnUiThread(() =>
                        {
                            Toast.MakeText(_context, "上传失败！", ToastLength.Short).Show();
                        });
                    }
                });
            };
            return view;
        }

        //Fill in cound here, currently 0
        public override int Count => Items.Count;
    }
}