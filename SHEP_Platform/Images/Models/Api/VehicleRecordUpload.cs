using System;

namespace SHEP_Platform.Models.Api
{
    public class VehicleRecordUpload
    {
        public DateTime StartDateTime { get; set; } = DateTime.MinValue;

        public DateTime EndDateTime { get; set; } = DateTime.MinValue;

        public int DevId { get; set; }

        public string RecordName { get; set; }

        public string Comment { get; set; }

        public float Lat { get; set; }

        public float Lng { get; set; }
    }

    public class VehicleAndroidVersionInfo
    {
        public string VersionName { get; set; }

        public int VersionCode { get; set; }

        public string ApkAddress { get; set; }
    }
}