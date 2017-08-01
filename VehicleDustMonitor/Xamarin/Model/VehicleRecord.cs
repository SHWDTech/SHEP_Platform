using System;
using System.Collections.Generic;

namespace VehicleDustMonitor.Xamarin.Model
{
    public class VehicleRecord
    {
        public DateTime StartDateTime { get; set; } = DateTime.MinValue;

        public DateTime EndDateTime { get; set; } = DateTime.MinValue;

        public int DevId { get; set; }

        public string RecordName { get; set; }

        public string Comment { get; set; }

        public List<double> RecordDatas { get; set; } = new List<double>();

        public bool IsSaved { get; set; }

        public long DatabaseId { get; set; }
    }
}