using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web.Mvc;
using Newtonsoft.Json;
using SHEP_Platform.Enum;
using SHEP_Platform.Models.Analysis;
using SHEP_Platform.Models.Monitor;
using SHEP_Platform.Process;

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
                case "setDeviceMin":
                    return SetDeviceMin();
                case "questTaskResult":
                    return QuestTaskResult();
                case "startProof":
                    return StartProof();
                case "cameraMoveControl":
                    return CameraMoveControl();
                case "cameraMoveStop":
                    return CameraMoveStop();
                case "capturePicture":
                    return CapturePicture();
                case "getAlarmInfo":
                    return GetAlarmInfo();
                case "getVocValues":
                    return GetVocValues();
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


            if (pollutantType == PollutantType.Pm25)
            {
                stringBuilder.AppendFormat("UpdateTime >='{0}' and UpdateTime <='{1}'",
                    startDate.ToString("yyyy-MM-dd HH:mm:ss"),
                    endDate.ToString("yyyy-MM-dd HH:mm:ss"));
                var ret =
                    from item in DbContext.T_ESDay_GetAvgPM25StatList(WdContext.User.Remark, stringBuilder.ToString())
                    group item by item.StatId
                    into newresult
                    select newresult;

                var dict = ret.ToDictionary(p => p.Key)
                    .Where(obj => WdContext.StatList.FirstOrDefault(item => item.Id == obj.Key) != null)
                    .Select(item => new
                    {
                        Name = WdContext.StatList.First(o => o.Id == item.Key).StatName,
                        MaxVal = double.Parse((item.Value.OrderBy(i => i.AvgPM25).First().AvgPM25 / 1000).ToString()).ToString("f2"),
                        AvgVal = double.Parse((item.Value.Average(j => j.AvgPM25) / 1000).ToString()).ToString("f2"),
                        MinVal = double.Parse((item.Value.OrderByDescending(k => k.AvgPM25).First().AvgPM25 / 1000).ToString()).ToString("f2"),
                        ValidNum = item.Value.Count()
                    }).ToList();

                return Json(dict);
            }

            if (pollutantType == PollutantType.Pm100)
            {
                stringBuilder.AppendFormat("UpdateTime >='{0}' and UpdateTime <='{1}'",
                    startDate.ToString("yyyy-MM-dd HH:mm:ss"),
                    endDate.ToString("yyyy-MM-dd HH:mm:ss"));
                var ret =
                    from item in DbContext.T_ESDay_GetAvgPM100StatList(WdContext.User.Remark, stringBuilder.ToString())
                    group item by item.StatId
                    into newresult
                    select newresult;

                var dict = ret.ToDictionary(p => p.Key)
                    .Where(obj => WdContext.StatList.FirstOrDefault(item => item.Id == obj.Key) != null)
                    .Select(item => new
                    {
                        Name = WdContext.StatList.First(o => o.Id == item.Key).StatName,
                        MaxVal = double.Parse((item.Value.OrderBy(i => i.AvgPM100).First().AvgPM100 / 1000).ToString()).ToString("f2"),
                        AvgVal = double.Parse((item.Value.Average(j => j.AvgPM100) / 1000).ToString()).ToString("f2"),
                        MinVal = double.Parse((item.Value.OrderByDescending(k => k.AvgPM100).First().AvgPM100 / 1000).ToString()).ToString("f2"),
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
                .Select(i => new
                {
                    TP = (i.TP / 1000).ToString("f2"),
                    DB = i.DB.ToString("f2"),
                    PM25 = (i.PM25 / 1000).GetValueOrDefault().ToString("f2"),
                    PM100 = (i.PM100 / 1000).GetValueOrDefault().ToString("f2"),
                    // ReSharper disable once PossibleInvalidOperationException
                    UpdateTime = ((DateTime)i.UpdateTime).ToString("HH:mm:ss")
                });

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
                        .Select(obj => new
                        {
                            TP = (obj.TP / 1000).ToString("f2"),
                            DB = obj.DB.ToString("f2"),
                            PM25 = (obj.PM25.GetValueOrDefault() / 1000).ToString("f2"),
                            PM100 = (obj.PM100.GetValueOrDefault() / 1000).ToString("f2"),
                            // ReSharper disable once PossibleInvalidOperationException
                            UpdateTime = ((DateTime)obj.UpdateTime).ToString("HH:mm:ss")
                        });
                dict.Add("data", ret);
            }
            else if (dtType == "Hour")
            {
                var ret =
                    DbContext.T_ESHour.Where(
                        item => item.StatId == statId && item.UpdateTime > startDate && item.UpdateTime < endDate)
                        .ToList()
                        .Select(obj => new
                        {
                            TP = (obj.TP / 1000).ToString("f2"),
                            DB = obj.DB.ToString("f2"),
                            PM25 = (obj.PM25.GetValueOrDefault() / 1000).ToString("f2"),
                            PM100 = (obj.PM100.GetValueOrDefault() / 1000).ToString("f2"),
                            // ReSharper disable once PossibleInvalidOperationException
                            UpdateTime = obj.UpdateTime.ToString("HH:mm:ss")
                        });
                dict.Add("data", ret);
            }
            else
            {
                var ret =
                    DbContext.T_ESDay.Where(
                        item => item.StatId == statId && item.UpdateTime > startDate && item.UpdateTime < endDate)
                        .ToList()
                        .Select(obj => new
                        {
                            TP = (obj.TP / 1000).ToString("f2"),
                            DB = obj.DB.ToString("f2"),
                            PM25 = (obj.PM25.GetValueOrDefault() / 1000).ToString("f2"),
                            PM100 = (obj.PM100.GetValueOrDefault() / 1000).ToString("f2"),
                            // ReSharper disable once PossibleInvalidOperationException
                            UpdateTime = obj.UpdateTime.ToString("yyyy-MM-dd")
                        });
                dict.Add("data", ret);
            }

            return Json(dict);
        }

        private JsonResult GetAlarmChange()
        {
            var pollutantType = Request["pollutantType"];
            var dict = new List<object>();

            if (pollutantType == PollutantType.ParticulateMatter)
            {
                var alarms = DbContext.T_Alarms.Where(item => item.Country == WdContext.Country.Id.ToString()
                /*&& item.UpdateTime > startDate*/ && item.DustType == 0).ToList()
                    // ReSharper disable once PossibleInvalidOperationException
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
                /*&& item.UpdateTime > startDate*/ && item.DustType == 1).ToList()
                    // ReSharper disable once PossibleInvalidOperationException
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

        private JsonResult SetDeviceMin()
        {
            var statId = Request["statid"];

            var context = new ESMonitorEntities();

            var taskList = new List<T_Tasks>();

            foreach (var dev in context.T_Devs.Where(devs => devs.StatId == statId))
            {
                var cmd = new DevCtrlCmd();
                cmd.EncodeSwitchAutoReport(0);
                var task = new T_Tasks();
                cmd.GetTaskModel(dev.Id, ref task);

                context.T_Tasks.Add(task);
                taskList.Add(task);
            }

            var taskAdd = true;
            try
            {
                context.SaveChanges();
            }
            catch (Exception)
            {
                taskAdd = false;
            }

            var ret = new
            {
                taskAdd,
                tasks = taskAdd ? taskList.Select(taskse => taskse.TaskId).ToList() : null
            };

            return Json(ret);
        }

        private JsonResult QuestTaskResult()
        {
            var taskList = JsonConvert.DeserializeObject<List<long>>(Request["tasks"]);

            var context = new ESMonitorEntities();

            var complete = false;

            var repeat = 0;
            while (repeat < 5)
            {
                if (complete) break;
                foreach (var notice in taskList.Select(taskId => context.T_TaskNotice.FirstOrDefault(obj => obj.TaskId == taskId)))
                {
                    complete = notice != null;

                    if (!complete) break;
                }

                repeat++;
                Thread.Sleep(1000);
            }

            var rets = new
            {
                success = complete
            };

            return Json(rets);
        }

        private JsonResult StartProof()
        {
            var statId = Request["statid"];

            var context = new ESMonitorEntities();

            var devs = context.T_Devs.Where(dev => dev.StatId == statId).ToList();

            var success = false;

            if (devs.Count == 0)
            {
            }
            else
            {
                for (var i = 0; i < 3; i++)
                {
                    foreach (var devse in devs)
                    {
                        var rd = new Random();
                        var baseVal = rd.Next(400, 800);

                        var minData = new T_ESMin
                        {
                            Country = WdContext.Country.Id.ToString(),
                            Airpressure = 0,
                            DB = rd.NextDouble() * 30 + 30,
                            DevId = devse.Id,
                            DataStatus = "n",
                            Humidity = 0,
                            PM100 = baseVal / 3 * (2 - i),
                            PM25 = baseVal / 3 * (2 - i),
                            Rain = 0,
                            StatCode = 9527,
                            StatId = int.Parse(statId),
                            TP = baseVal / 3 * (2 - i),
                            Temperature = 0,
                            VOCs = 0,
                            UpdateTime = DateTime.Now,
                            WindDirection = 0,
                            WindSpeed = 0
                        };

                        context.T_ESMin.Add(minData);
                    }
                    context.SaveChanges();
                    Thread.Sleep(60000);
                }

                for (var i = 0; i < 5; i++)
                {
                    foreach (var devse in devs)
                    {
                        var rd = new Random();

                        var minData = new T_ESMin
                        {
                            Country = WdContext.Country.Id.ToString(),
                            Airpressure = 0,
                            DB = rd.NextDouble() * 30 + 30,
                            DevId = devse.Id,
                            DataStatus = "n",
                            Humidity = 0,
                            PM100 = 0,
                            PM25 = 0,
                            Rain = 0,
                            StatCode = 9527,
                            StatId = int.Parse(statId),
                            TP = 0,
                            Temperature = 0,
                            VOCs = 0,
                            UpdateTime = DateTime.Now,
                            WindDirection = 0,
                            WindSpeed = 0
                        };

                        context.T_ESMin.Add(minData);
                    }
                    context.SaveChanges();
                    Thread.Sleep(60000);
                }

                foreach (var devse in devs)
                {
                    var cmd = new DevCtrlCmd();
                    cmd.EncodeSwitchAutoReport(60);
                    var task = new T_Tasks();
                    cmd.GetTaskModel(devse.Id, ref task);

                    context.T_Tasks.Add(task);
                }

                context.SaveChanges();

                success = true;
            }

            var ret = new
            {
                success
            };

            return Json(ret);
        }

        private JsonResult CameraMoveControl()
        {
            var dir = Request["dir"];

            var controlResult = HikCameraControl.ControlPlatform(dir);

            var ret = new
            {
                success = controlResult
            };

            return Json(ret);
        }

        private JsonResult CameraMoveStop()
        {
            var controlResult = HikCameraControl.StopControlPlatform();

            var ret = new
            {
                success = controlResult
            };

            return Json(ret);
        }

        private JsonResult CapturePicture()
        {
            var success = false;
            var started = HkAction.Start();
            if (started)
            {
                HkAction.GetAccessToken();
                HkAction.AllocSession();
                HkAction.GetCameraList();
                HkAction.StartPlay();

                success = HkAction.CapturePicture($"D:\\CameraPicture\\{DateTime.Now.ToString("yyyyMMddHHmmssffff")}.jpg");
            }


            var ret = new
            {
                success
            };
            return Json(ret);
        }

        private JsonResult GetAlarmInfo()
        {
            var start = DateTime.Now.AddMinutes(-5);

            var todayPmAlarm = DbContext.T_Alarms.Where(obj => obj.UpdateTime > start && obj.DustType == 0);

            var todayDbAlarm = DbContext.T_Alarms.Where(obj => obj.UpdateTime > start && obj.DustType == 1);

            var details = new List<AlarmDetail>();
            foreach (var pmAlarm in todayPmAlarm)
            {
                var stat = DbContext.T_Stats.First(obj => obj.Id == pmAlarm.StatId);
                if (pmAlarm.StatId != null)
                {
                    var detail = new AlarmDetail {StatName = stat.StatName, Id = pmAlarm.StatId.Value };
                    if (pmAlarm.UpdateTime != null) detail.AlarmDateTime = pmAlarm.UpdateTime.Value.ToString("hh:mm:ss");
                    detail.AlarmType = "扬尘值";
                    if (pmAlarm.FaultVal != null) detail.AlarmValue = ((pmAlarm.FaultVal.Value) / 1000.0).ToString("f2");

                    details.Add(detail);
                }
            }

            foreach (var dbAlarm in todayDbAlarm)
            {
                var stat = DbContext.T_Stats.First(obj => obj.Id == dbAlarm.StatId);
                if (dbAlarm.StatId != null)
                {
                    var detail = new AlarmDetail { StatName = stat.StatName, Id = dbAlarm.StatId.Value };
                    if (dbAlarm.UpdateTime != null) detail.AlarmDateTime = dbAlarm.UpdateTime.Value.ToString("hh:mm:ss");
                    detail.AlarmType = "扬尘值";
                    if (dbAlarm.FaultVal != null) detail.AlarmValue = dbAlarm.FaultVal.Value.ToString("f2");

                    details.Add(detail);
                }
            }

            var ret = new
            {
                total = todayDbAlarm.Count() + todayPmAlarm.Count(),
                details
            };

            return Json(ret);
        }

        private JsonResult GetVocValues()
        {
            var model = new VocViewModel();

            foreach (var stat in WdContext.StatList)
            {
                var vocs =
                    DbContext.T_ESMin.Where(obj => obj.StatId == stat.Id && obj.VOCs != null)
                        .Select(item => new { UpdateTime = item.UpdateTime.Value, item.VOCs.Value })
                        .ToList();


                foreach (var voc in vocs)
                {
                    model.VocValueList.Add(new VocValue() { UpdateTime = voc.UpdateTime.ToString("yyyy-MM-dd HH:mm:ss"), Value = voc.Value });
                }
            }

            var ret = new
            {
                success = true,
                vocValues = model.VocValueList
            };

            return Json(ret);
        }
    }
}