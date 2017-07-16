﻿using System;
using System.Collections.Generic;
using Android.App;
using Android.OS;
using VehicleDustMonitor.Xamarin.adapter;
using VehicleDustMonitor.Xamarin.Component;
using VehicleDustMonitor.Xamarin.Model;

namespace VehicleDustMonitor.Xamarin.activity
{
    [Activity(Label = nameof(HistoryRecordActivity))]
    public class HistoryRecordActivity : ListActivity
    {
        private VehicleRecordHelper _sqlHelper;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            _sqlHelper = new VehicleRecordHelper(this);
            LoadRecoreds();
        }

        private void LoadRecoreds()
        {
            var readingColumn = new []
            {
                VehicleRecordEntity.ColumnNameRecordName,
                VehicleRecordEntity.ColumnNameComment,
                VehicleRecordEntity.ColumnNameStartDateTime,
                VehicleRecordEntity.ColumnNameEndDateTIme,
                VehicleRecordEntity.ColumnNameAverage
            };

            var cursor = _sqlHelper.ReadableDatabase.Query(VehicleRecordEntity.TableName, readingColumn, null, null,
                null, null, null, null);
            var items = new List<HistoryRecordItem>();
            var canRead = cursor.MoveToFirst();
            while (canRead)
            {
                var item = new HistoryRecordItem
                {
                    RecordName = cursor.GetString(cursor.GetColumnIndex(VehicleRecordEntity.ColumnNameRecordName)),
                    Comment = cursor.GetString(cursor.GetColumnIndex(VehicleRecordEntity.ColumnNameComment)),
                    StartDateTime = DateTime.Parse(cursor.GetString(cursor.GetColumnIndex(VehicleRecordEntity.ColumnNameStartDateTime))),
                    EndDateTime = DateTime.Parse(cursor.GetString(cursor.GetColumnIndex(VehicleRecordEntity.ColumnNameEndDateTIme))),
                    AverageValue = cursor.GetDouble(cursor.GetColumnIndex(VehicleRecordEntity.ColumnNameAverage))
                };
                items.Add(item);
                canRead = cursor.MoveToNext();
            }

            ListAdapter = new HistoryRecordAdapter(this, items);
        }
    }
}