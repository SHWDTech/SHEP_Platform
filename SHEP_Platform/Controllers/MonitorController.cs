using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Newtonsoft.Json;
using SHEP_Platform.Enum;
using SHEP_Platform.Models.Monitor;
// ReSharper disable PossibleInvalidOperationException

namespace SHEP_Platform.Controllers
{
    public class MonitorController : ControllerBase
    {
        public MonitorController()
        {
            WdContext.SiteMapMenu.ControllerMenu.Name = "查询导出";
            WdContext.SiteMapMenu.ControllerMenu.LinkAble = false;
        }

        // GET: Monitor
        public ActionResult ActualStatus(string id)
        {
            WdContext.SiteMapMenu.ActionMenu.Name = "各工程当前情况";
            var dict = new Dictionary<object, StatHourInfo>();
            var cameraurl = string.Empty;
            foreach (var stat in WdContext.StatList)
            {
                var hour = DateTime.Now.AddHours(-1);
                var value =
                    DbContext.T_ESHour.FirstOrDefault(item => item.StatId == stat.Id && item.UpdateTime.Hour > hour.Hour);
                var current = DbContext.T_ESMin.Where(obj => obj.StatId == stat.Id).ToList().LastOrDefault();

                var info = new StatHourInfo
                {
                    Hour = value,
                    Current = current
                };

                dict.Add(stat, info);
            }

            ViewBag.StatDict = dict;
            var defaultId = -1;
            var defaultName = "null";
            if (!string.IsNullOrWhiteSpace(id))
            {
                defaultId = int.Parse(id);
                defaultName = WdContext.StatList.First(stat => stat.Id == defaultId).StatName;
            }
            else if (WdContext.StatList.Count != 0)
            {
                defaultId = WdContext.StatList[0].Id;
                defaultName = WdContext.StatList.First(stat => stat.Id == defaultId).StatName;
            }

            var devs = DbContext.T_Devs.Where(dev => dev.StatId == defaultId.ToString()).ToList();
            if (devs.Count > 0)
            {
                cameraurl = devs[0].VideoURL;
            }

            ViewBag.defaultId = defaultId;
            ViewBag.defaultName = defaultName;
            ViewBag.StatViewUrl = cameraurl;

            return DynamicView("ActualStatus");
        }

        public ActionResult HistoryChange()
        {
            WdContext.SiteMapMenu.ActionMenu.Name = "各工程历史污染物变化情况";
            var dict = new Dictionary<object, StatHourInfo>();
            foreach (var stat in WdContext.StatList)
            {
                var hour = DateTime.Now.AddHours(-1);
                var value =
                    DbContext.T_ESHour.FirstOrDefault(item => item.StatId == stat.Id && item.UpdateTime.Hour > hour.Hour);
                var current = DbContext.T_ESMin.Where(obj => obj.StatId == stat.Id).ToList().LastOrDefault();

                var info = new StatHourInfo
                {
                    Hour = value,
                    Current = current
                };

                dict.Add(stat, info);
            }

            ViewBag.StatDict = dict;

            var defaultId = -1;
            if (WdContext.StatList.Count > 0)
            {
                defaultId = WdContext.StatList[0].Id;
            }
            ViewBag.defaultId = defaultId;

            return DynamicView("HistoryChange");
        }

        public ActionResult ScheduleCompare()
        {
            WdContext.SiteMapMenu.ActionMenu.Name = "按工期进行综合对比";
            var basic = WdContext.StatList.Where(stat => stat.Stage == (int)Stage.Basic).Select(obj => obj.Id).ToList();
            var structure =
                WdContext.StatList.Where(stat => stat.Stage == (int)Stage.Structure).Select(obj => obj.Id).ToList();

            var startDate = DateTime.Now.AddMonths(-1);
            var basicSource = DbContext.T_ESDay.Where(obj => basic.Contains(obj.StatId) && obj.UpdateTime > startDate)
                .ToList()
                .Select(item => new
                {
                    TP = double.Parse((item.TP / 1000).ToString("f2")),
                    DB = double.Parse(item.DB.ToString("f2")),
                    UpdateTime = item.UpdateTime.ToString("yyyy-MM-dd")
                });
            var structureSource = DbContext.T_ESDay.Where(
                obj => structure.Contains(obj.StatId) && obj.UpdateTime > startDate).ToList()
                .Select(item => new
                {
                    TP = double.Parse((item.TP / 1000).ToString("f2")),
                    DB = double.Parse(item.DB.ToString("f2")),
                    UpdateTime = item.UpdateTime.ToString("yyyy-MM-dd")
                });

            var dict = new Dictionary<string, object> { { "basic", basicSource }, { "structure", structureSource } };

            var basicList = basicSource.ToList();
            var structureList = structureSource.ToList();
            var model = new ScheduleCompareViewModel
            {
                BasicAvgTp = basicList.Any() ? basicList.Average(i => i.TP).ToString("f2") : "暂无数据",
                BasicAvgDb = basicList.Any() ? basicList.Average(i => i.DB).ToString("f2") : "暂无数据",
                StructureTp = structureList.Any() ? structureList.Average(i => i.TP).ToString("f2") : "暂无数据",
                StructureDb = structureList.Any() ? structureList.Average(i => i.DB).ToString("f2") : "暂无数据"
            };

            ViewBag.Dict = JsonConvert.SerializeObject(dict);

            return DynamicView("ScheduleCompare", model);
        }

        /// <summary>
        /// 数据导出
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DataExport()
        {
            WdContext.SiteMapMenu.ActionMenu.Name = "历史数据表格导出";

            var model = new DataExport();
            foreach (var statse in WdContext.StatList)
            {
                model.StatList.Add(statse);
                model.DevList.AddRange(DbContext.T_Devs.Where(obj => obj.StatId == statse.Id.ToString()).ToList());
            }

            return DynamicView("DataExport", model);
        }

        [HttpPost]
        public ActionResult DataExportResult()
        {
            var stats = Request["stat"]?.Split(',');
            var devs = Request["devs"]?.Split(',');

            var model = new DataExport
            {
                StartDate = Request["startDate"],
                EndDate = Request["endDate"]
            };
            if (stats != null)
            {
                foreach (var stat in stats)
                {
                    model.StatList.Add(WdContext.StatList.FirstOrDefault(obj => obj.Id.ToString() == stat));
                }
            }

            if (devs != null)
            {
                foreach (var dev in devs)
                {
                    model.DevList.Add(DbContext.T_Devs.FirstOrDefault(obj => obj.Id.ToString() == dev));
                }
            }

            return PartialView(model);
        }

        public ActionResult StatView()
        {
            WdContext.SiteMapMenu.ActionMenu.Name = "工程实时状况查看";

            var stats = WdContext.StatList;
            var devs = new List<T_Devs>();
            var cameras = new List<T_Camera>();
            foreach (var stat in stats)
            {
                devs.AddRange(DbContext.T_Devs.Where(obj => obj.StatId == stat.Id.ToString()));
            }

            foreach (var dev in devs)
            {
                cameras.AddRange(DbContext.T_Camera.Where(item => item.DevId == dev.Id));
            }

            ViewBag.Cameras = JsonConvert.SerializeObject(cameras);

            return DynamicView("StatView");
        }

        public ActionResult StatViewTest()
        {
            WdContext.SiteMapMenu.ActionMenu.Name = "工程实时状况查看";

            return DynamicView("StatViewTest");
        }

        public ActionResult StatTable(int id)
        {
            int totalCount;
            var dataList = GetDevDataList(id, out totalCount);
            var ret = new
            {
                total = totalCount,
                rows = dataList
            };

            return Json(ret, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DevTable(int id)
        {
            int totalCount;
            var dataList = GetDevDataList(id, out totalCount);
            var ret = new
            {
                total = totalCount,
                rows = dataList
            };

            return Json(ret, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetStatDataList(int id, out int totalCount)
        {
            var stat = WdContext.StatList.FirstOrDefault(obj => obj.Id == id);
            var limit = int.Parse(Request["limit"]);
            var offset = int.Parse(Request["offset"]);
            var startDate = DateTime.Parse(Request["start"]);
            var endDate = DateTime.Parse(Request["end"]);

            if (stat == null)
            {
                totalCount = 0;
                return null;
            }

            var total = DbContext.T_ESMin.Where(obj => obj.StatId == stat.Id && obj.UpdateTime < endDate && obj.UpdateTime > startDate)
                .OrderBy(item => item.UpdateTime);
            var esmin = total.Skip(offset)
                .Take(limit).ToList()
                .Select(obj => new {
                    UpdateTime = obj.UpdateTime.Value.ToString("yyyy-MM-dd hhhh:mm:ss"),
                    TP = (obj.TP / 1000).ToString("F2"),
                    PM25 = (obj.PM25.Value / 1000).ToString("F2"),
                    PM100 = (obj.PM100.Value / 1000).ToString("F2"),
                    DB = (obj.DB / 1000).ToString("F2")
                })
                .ToList();

            totalCount = total.Count();
            return esmin;
        }

        public dynamic GetDevDataList(int id, out int totalCount)
        {
            var dev = DbContext.T_Devs.FirstOrDefault(obj => obj.Id == id);
            var limit = int.Parse(Request["limit"]);
            var offset = int.Parse(Request["offset"]);
            var startDate = DateTime.Parse(Request["start"]);
            var endDate = DateTime.Parse(Request["end"]);

            if (dev == null)
            {
                totalCount = 0;
                return null;
            }

            var total = DbContext.T_ESMin.Where(obj => obj.DevId == dev.Id && obj.UpdateTime < endDate && obj.UpdateTime > startDate)
                .OrderBy(item => item.UpdateTime);
            var esmin = total.Skip(offset)
                .Take(limit).ToList()
                .Select(obj => new {
                    UpdateTime = obj.UpdateTime.Value.ToString("yyyy-MM-dd hhhh:mm:ss"),
                    TP = (obj.TP / 1000).ToString("F2"),
                    PM25 = (obj.PM25.Value / 1000).ToString("F2"),
                    PM100 = (obj.PM100.Value / 1000).ToString("F2"),
                    DB = (obj.DB / 1000).ToString("F2")
                })
                .ToList();

            totalCount = total.Count();
            return esmin;
        }

        public void ExportHistoryData()
        {
            
        }

        public ActionResult StatViewHik()
        {
            WdContext.SiteMapMenu.ActionMenu.Name = "工程实时状况查看";

            return View();
        }


        public ActionResult StatViewCircle()
        {
            WdContext.SiteMapMenu.ActionMenu.Name = "工程实时状况查看";

            return View();
        }

        public ActionResult StatViewHikSecond()
        {
            WdContext.SiteMapMenu.ActionMenu.Name = "工程实时状况查看";

            return View();
        }
    }
}