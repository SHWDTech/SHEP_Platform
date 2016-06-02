using System.Collections.Generic;

namespace SHEP_Platform.Models.Monitor
{
    public class ScheduleCompareViewModel
    {
        public string BasicAvgTp { get; set; }

        public string BasicAvgDb { get; set; }

        public string StructureTp { get; set; }

        public string StructureDb { get; set; }
    }

    public class StatHourInfo
    {
        public T_ESHour Hour { get; set; }

        public T_ESMin Current { get; set; }
    }

    public class AlarmDetail
    {
        public string StatName { get; set; }

        public int Id { get; set; } 

        public string AlarmType { get; set; }

        public string AlarmDateTime { get; set; }

        public string AlarmValue { get; set; }
    }

    public class DataExport
    {
        public List<T_Stats> StatList { get; set; } = new List<T_Stats>();

        public List<T_Devs> DevList { get; set; } = new List<T_Devs>();

        public string DateRange { get; set; }

        public string StartDate { get; set; }

        public string EndDate { get; set; }
    }
}