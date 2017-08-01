using System;

namespace VehicleDustMonitor.Xamarin.Model
{
    public class HistoryRecordItem
    {
        public long Id { get; set; }

        public DateTime StartDateTime { get; set; } = DateTime.MinValue;

        public DateTime EndDateTime { get; set; } = DateTime.MinValue;

        public int DevId { get; set; }

        public string RecordName { get; set; }

        public string Comment { get; set; }

        public string Lat { get; set; }

        public string Lng { get; set; }

        public double AverageValue { get; set; }

        public bool HasUpload { get; set; }
    }
}