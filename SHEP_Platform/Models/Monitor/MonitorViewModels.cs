using System;
using System.Collections.Generic;
using System.Data;

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

        public string StartDate { get; set; }

        public string EndDate { get; set; }
    }

    public class WorkSheet
    {
        public WorkSheet()
        {
            WorkSheetDatas.Columns.Add("更新时间", typeof(DateTime));
            WorkSheetDatas.Columns.Add("总体扬尘值(mg/m³)", typeof(string));
            WorkSheetDatas.Columns.Add("PM2.5(mg/m³)", typeof(string));
            WorkSheetDatas.Columns.Add("PM10(mg/m³)", typeof(string));
            WorkSheetDatas.Columns.Add("噪音值(dB)", typeof(string));
        }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 工作表数据
        /// </summary>
        public DataTable WorkSheetDatas { get; set; } = new DataTable();
    }
}