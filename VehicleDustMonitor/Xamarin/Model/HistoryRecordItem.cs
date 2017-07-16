using System;

namespace VehicleDustMonitor.Xamarin.Model
{
    public class HistoryRecordItem
    {
        public DateTime StartDateTime { get; set; } = DateTime.MinValue;

        public DateTime EndDateTime { get; set; } = DateTime.MinValue;

        public int DevId { get; set; }

        public string RecordName { get; set; }

        public string Comment { get; set; }

        public double AverageValue { get; set; }
    }
}