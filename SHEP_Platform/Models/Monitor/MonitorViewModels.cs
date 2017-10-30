using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
// ReSharper disable InconsistentNaming

namespace SHEP_Platform.Models.Monitor
{
    public class ScheduleCompareViewModel
    {
        public string BasicAvgTp { get; set; }

        public string BasicAvgDb { get; set; }

        public string StructureTp { get; set; }

        public string StructureDb { get; set; }
    }

    public class ActualStatusViewModel
    {
        public int DefaultId { get; set; } = -1;

        public string DefaultName { get; set; } = "未找到指定工地";

        public string StatViewUrl { get; set; } = string.Empty;

        public StatHourInfo StatHourInfo { get; set; }
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

        public bool IsReaded { get; set; }
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

    public class RecentData
    {
        public RecentData(T_ESMin esMin)
        {
            if (esMin != null)
            {
                TP = $"{esMin.TP / 1000:F3}";
                DB = $"{esMin.DB}";
                PM25 = $"{esMin.PM25 / 1000:F3}";
                PM100 = $"{esMin.PM100 / 1000:F3}";
                Temperature = $"{esMin.Temperature}";
                Humidity = $"{esMin.Humidity}";
                WindSpeed = $"{esMin.WindSpeed}";
                WindDirection = $"{esMin.WindDirection}";
                UpdateTime = $"{esMin.UpdateTime:yyyy-MM-dd HH:mm:ss}";
                Valid = true;
            }
        }
        public string TP { get; set; } = "暂无数据";

        public string DB { get; set; } = "暂无数据";

        public string PM25 { get; set; } = "暂无数据";

        public string PM100 { get; set; } = "暂无数据";

        public string Temperature { get; set; } = "暂无数据";

        public string Humidity { get; set; } = "暂无数据";

        public string WindSpeed { get; set; } = "暂无数据";

        public string WindDirection { get; set; } = "暂无数据";

        public string UpdateTime { get; set; } = "尚未收到任何数据";

        public bool Valid { get; set; }
    }

    public class PuTianDataExport
    {
        [Required]
        public DateTime? StartDateTime { get; set; }

        [Required]
        public DateTime? EndDateTime { get; set; }

        [Required]
        public ExportDataType? DataType { get; set; }
    }

    public enum ExportDataType
    {
        Hour = 0x00,

        Day = 0x01
    }
}