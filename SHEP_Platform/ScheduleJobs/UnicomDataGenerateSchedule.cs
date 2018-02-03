using System.Collections.Generic;

namespace SHEP_Platform.ScheduleJobs
{
    public class UnicomDataGenerateSchedule
    {
        public static Dictionary<string, UnicomDataGenerateSchedule> CachedSchedules { get; } =
            new Dictionary<string, UnicomDataGenerateSchedule>();

        public string ScheduleName { get; set; }

        public int SchedulePriority { get; set; }

        public Dictionary<string, DataRange> DataRanges { get; set; }

        public List<int> DeviceList { get; set; } = new List<int>();
    }

    public class DataRange
    {
        public string DataName { get; set; }

        public double MaxValue { get; set; }

        public double MinValue { get; set; }
    }
}