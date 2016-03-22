using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using SHEP_Platform.Enum;

namespace SHEP_Platform.Controllers
{
    public class AjaxController : AjaxControllerBase
    {
        public JsonResult Access() => ParseRequest();

        // GET: Ajax
        protected override JsonResult ExecuteFun(string functionName)
        {
            switch (functionName)
            {
                case "getStatAvgReport":
                    return GetStatAvgReport();
                case "getStatsActualData":
                    return GetStatsActualData();
                case "load":
                    return GetHistoryReport();
                case "getAlarmChange":
                    return GetAlarmChange();
            }

            return null;
        }

        private JsonResult GetStatAvgReport()
        {
            var pollutantType = Request["pollutantType"];
            var queryDateRange = Request["queryDateRange"];
            var datePickerValue = Request["datePickerValue"]?.Split(',');

            var startDate = DateTime.MinValue;
            var endDate = DateTime.Now;
            switch (queryDateRange)
            {
                case QueryDateRange.LastWeek:
                    startDate = DateTime.Now.AddDays(-7);
                    break;
                case QueryDateRange.LastMonth:
                    startDate = DateTime.Now.AddMonths(-1);
                    break;
                case QueryDateRange.LastYear:
                    startDate = DateTime.Now.AddYears(-1);
                    break;
                case QueryDateRange.Customer:
                    if (datePickerValue == null || datePickerValue.Length < 2)
                    {
                        throw new Exception("参数错误");
                    }
                    startDate = DateTime.Parse(datePickerValue[0]);
                    endDate = DateTime.Parse(datePickerValue[1]);
                    break;
            }

            var stringBuilder = new StringBuilder();

            if (pollutantType == PollutantType.ParticulateMatter)
            {
                stringBuilder.AppendFormat("UpdateTime >='{0}' and UpdateTime <='{1}'",
                    startDate.ToString("yyyy-MM-dd HH:mm:ss"),
                    endDate.ToString("yyyy-MM-dd HH:mm:ss"));
                var ret =
                    from item in DbContext.T_ESDay_GetAvgCMPStatList(WdContext.User.Remark, stringBuilder.ToString())
                    group item by item.StatId
                    into newresult
                    select newresult;

                var dict = ret.ToDictionary(p => p.Key)
                    .Where(obj => WdContext.StatList.FirstOrDefault(item => item.Id == obj.Key) != null)
                    .Select(item => new
                    {
                        Name = WdContext.StatList.First(o => o.Id == item.Key).StatName,
                        MaxVal = double.Parse((item.Value.OrderBy(i => i.AvgTP).First().AvgTP / 1000).ToString()).ToString("f2"),
                        AvgVal = double.Parse((item.Value.Average(j => j.AvgTP) / 1000).ToString()).ToString("f2"),
                        MinVal = double.Parse((item.Value.OrderByDescending(k => k.AvgTP).First().AvgTP / 1000).ToString()).ToString("f2"),
                        ValidNum = item.Value.Count()
                    }).ToList();

                return Json(dict);
            }

            if (pollutantType == PollutantType.Noise)
            {
                stringBuilder.AppendFormat("UpdateTime >='{0}' and UpdateTime <='{1}'",
                    startDate.ToString("yyyy-MM-dd HH:mm:ss"),
                    endDate.ToString("yyyy-MM-dd HH:mm:ss"));
                var ret =
                    from item in DbContext.T_ESDay_GetAvgNoiseStatList(WdContext.User.Remark, stringBuilder.ToString())
                    group item by item.StatId
                    into newresult
                    select newresult;

                var dict = ret.ToDictionary(p => p.Key)
                    .Where(obj => WdContext.StatList.FirstOrDefault(item => item.Id == obj.Key) != null)
                    .Select(item => new
                    {
                        Name = WdContext.StatList.First(o => o.Id == item.Key).StatName,
                        MaxVal = double.Parse(item.Value.OrderBy(i => i.AvgDB).First().AvgDB.ToString()).ToString("f2"),
                        AvgVal = double.Parse(item.Value.Average(j => j.AvgDB).ToString()).ToString("f2"),
                        MinVal = double.Parse(item.Value.OrderByDescending(k => k.AvgDB).First().AvgDB.ToString()).ToString("f2"),
                        ValidNum = item.Value.Count()
                    }).ToList();

                return Json(dict);
            }

            return null;
        }

        private JsonResult GetStatsActualData()
        {
            var statId = int.Parse(Request["statId"]);
            var startDate = DateTime.Now.AddHours(-1);
            var ret = DbContext.T_ESMin.Where(item => item.StatId == statId && item.UpdateTime > startDate)
                .OrderBy(obj => obj.UpdateTime).ToList()
                // ReSharper disable once PossibleInvalidOperationException
                .Select(i => new { TP = (i.TP / 1000).ToString("f2"), DB = i.DB.ToString("f2"), UpdateTime = ((DateTime)i.UpdateTime).ToString("HH:mm:ss") });

            return Json(ret);
        }

        private JsonResult GetHistoryReport()
        {
            var statId = int.Parse(Request["id"]);
            var queryDateRange = Request["queryDateRange"];
            var datePickerValue = Request["datePickerValue"]?.Split(',');

            var startDate = DateTime.MinValue;
            var endDate = DateTime.Now;
            var dtType = string.Empty;
            switch (queryDateRange)
            {
                case QueryDateRange.LastHour:
                    startDate = DateTime.Now.AddHours(-1);
                    dtType = "Min";
                    break;
                case QueryDateRange.LastDay:
                    startDate = DateTime.Now.AddDays(-1);
                    dtType = "Hour";
                    break;
                case QueryDateRange.LastWeek:
                    startDate = DateTime.Now.AddDays(-7);
                    dtType = "Day";
                    break;
                case QueryDateRange.LastMonth:
                    startDate = DateTime.Now.AddMonths(-1);
                    dtType = "Day";
                    break;
                case QueryDateRange.LastYear:
                    startDate = DateTime.Now.AddYears(-1);
                    dtType = "Day";
                    break;
                case QueryDateRange.Customer:
                    if (datePickerValue == null || datePickerValue.Length < 2)
                    {
                        throw new Exception("参数错误");
                    }
                    startDate = DateTime.Parse(datePickerValue[0]);
                    endDate = DateTime.Parse(datePickerValue[1]);
                    dtType = "Day";
                    break;
            }

            var dict = new Dictionary<string, object>();
            if (dtType == "Min")
            {
                var ret =
                    DbContext.T_ESMin.Where(
                        item => item.StatId == statId && item.UpdateTime > startDate && item.UpdateTime < endDate)
                        .ToList()
                        // ReSharper disable once PossibleInvalidOperationException
                        .Select(obj => new { TP = (obj.TP / 1000).ToString("f2"), DB = obj.DB.ToString("f2"), UpdateTime = ((DateTime)obj.UpdateTime).ToString("HH:mm:ss") });
                dict.Add("data", ret);
            }
            else if (dtType == "Hour")
            {
                var ret =
                    DbContext.T_ESHour.Where(
                        item => item.StatId == statId && item.UpdateTime > startDate && item.UpdateTime < endDate)
                        .ToList()
                        // ReSharper disable once PossibleInvalidOperationException
                        .Select(obj => new { TP = (obj.TP / 1000).ToString("f2"), DB = obj.DB.ToString("f2"), UpdateTime = obj.UpdateTime.ToString("HH:mm:ss") });
                dict.Add("data", ret);
            }
            else
            {
                var ret =
                    DbContext.T_ESDay.Where(
                        item => item.StatId == statId && item.UpdateTime > startDate && item.UpdateTime < endDate)
                        .ToList()
                        // ReSharper disable once PossibleInvalidOperationException
                        .Select(obj => new { TP = (obj.TP / 1000).ToString("f2"), DB = obj.DB.ToString("f2"), UpdateTime = obj.UpdateTime.ToString("yyyy-MM-dd") });
                dict.Add("data", ret);
            }

            return Json(dict);
        }

        private JsonResult GetAlarmChange()
        {
            var pollutantType = Request["pollutantType"];
            var startDate = DateTime.Now.AddMonths(-1);
            var dict = new List<object>();

            if (pollutantType == PollutantType.ParticulateMatter)
            {
                var alarms = DbContext.T_Alarms.Where(item => item.Country == WdContext.Country.Id.ToString()
                && item.UpdateTime > startDate && item.DustType == 0).ToList()
                    .GroupBy(obj => obj.UpdateTime.Value.ToString("yyyy-MM-dd")).ToList();

                var list = new List<object>();
                foreach (var alarm in alarms)
                {
                    list.Add(new { UpdateTime = alarm.Key, Count = alarm.Count() });
                }

                dict.Add(list);
            }
            else
            {
                var alarms = DbContext.T_Alarms.Where(item => item.Country == WdContext.Country.Id.ToString()
                && item.UpdateTime > startDate && item.DustType == 1).ToList()
                    .GroupBy(obj => obj.UpdateTime.Value.ToString("yyyy-MM-dd")).ToList();

                var list = new List<object>();
                foreach (var alarm in alarms)
                {
                    list.Add(new { UpdateTime = alarm.Key, Count = alarm.Count() });
                }

                dict.Add(list);
            }

            return Json(dict);
        }
    }
}