using System;
using System.Collections.Generic;
using Android.App;
using Android.OS;
using Android.Widget;
using CheeseBind;
using VehicleDustMonitor.Xamarin.adapter;
using VehicleDustMonitor.Xamarin.Component;
using VehicleDustMonitor.Xamarin.Model;

namespace VehicleDustMonitor.Xamarin.activity
{
    [Activity(Label = nameof(HistoryRecordActivity))]
    public class HistoryRecordActivity : Activity
    {
        private VehicleRecordHelper _sqlHelper;

        [BindView(Resource.Id.history_records_list)]
        protected ListView HistoryListView;

        [OnClick(Resource.Id.back)]
        protected void Back(object sender, EventArgs args)
        {
            Finish();
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_history_records);
            Cheeseknife.Bind(this);
            _sqlHelper = VehicleRecordHelper.Instance;
            LoadRecoreds();
        }

        private void LoadRecoreds()
        {
            var readingColumn = new []
            {
                VehicleRecordEntity.ColumnNameId,
                VehicleRecordEntity.ColumnNameDevId,
                VehicleRecordEntity.ColumnNameRecordName,
                VehicleRecordEntity.ColumnNameComment,
                VehicleRecordEntity.ColumnNameStartDateTime,
                VehicleRecordEntity.ColumnNameEndDateTIme,
                VehicleRecordEntity.ColumnNameAverage,
                VehicleRecordEntity.ColumnNameLat,
                VehicleRecordEntity.ColumnNameLng,
                VehicleRecordEntity.ColumnNameUploaded
            };

            var cursor = _sqlHelper.ReadableDatabase.Query(VehicleRecordEntity.TableName, readingColumn, null, null,
                null, null, null, null);
            var items = new List<HistoryRecordItem>();
            var canRead = cursor.MoveToFirst();
            while (canRead)
            {
                var item = new HistoryRecordItem
                {
                    Id = cursor.GetLong(cursor.GetColumnIndex(VehicleRecordEntity.ColumnNameId)),
                    DevId = cursor.GetInt(cursor.GetColumnIndex(VehicleRecordEntity.ColumnNameDevId)),
                    RecordName = cursor.GetString(cursor.GetColumnIndex(VehicleRecordEntity.ColumnNameRecordName)),
                    Comment = cursor.GetString(cursor.GetColumnIndex(VehicleRecordEntity.ColumnNameComment)),
                    StartDateTime = DateTime.Parse(cursor.GetString(cursor.GetColumnIndex(VehicleRecordEntity.ColumnNameStartDateTime))),
                    EndDateTime = DateTime.Parse(cursor.GetString(cursor.GetColumnIndex(VehicleRecordEntity.ColumnNameEndDateTIme))),
                    AverageValue = cursor.GetDouble(cursor.GetColumnIndex(VehicleRecordEntity.ColumnNameAverage)),
                    Lat = cursor.GetString(cursor.GetColumnIndex(VehicleRecordEntity.ColumnNameLat)),
                    Lng = cursor.GetString(cursor.GetColumnIndex(VehicleRecordEntity.ColumnNameLng)),
                    HasUpload = cursor.GetInt(cursor.GetColumnIndex(VehicleRecordEntity.ColumnNameUploaded)) == 1
                };
                items.Add(item);
                canRead = cursor.MoveToNext();
            }

            HistoryListView.Adapter = new HistoryRecordAdapter(this, items);
        }
    }
}