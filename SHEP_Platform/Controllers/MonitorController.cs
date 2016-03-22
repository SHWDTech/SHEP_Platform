using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Newtonsoft.Json;
using SHEP_Platform.Enum;
using SHEP_Platform.Models.Monitor;

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
            foreach (var stat in WdContext.StatList)
            {
                var hour = DateTime.Now.AddHours(-1);
                var value = DbContext.T_ESHour.FirstOrDefault(item => item.StatId == stat.Id && item.UpdateTime.Hour > hour.Hour);
                var current = DbContext.T_ESMin.LastOrDefault(obj => obj.StatId == stat.Id && obj.UpdateTime.Value.Hour > hour.Hour);

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
            else if(WdContext.StatList.Count != 0)
            {
                defaultId = WdContext.StatList[0].Id;
                defaultName = WdContext.StatList.First(stat => stat.Id == defaultId).StatName;
            }
            ViewBag.defaultId = defaultId;
            ViewBag.defaultName = defaultName;

            return DynamicView("ActualStatus");
        }

        public ActionResult HistoryChange()
        {
            WdContext.SiteMapMenu.ActionMenu.Name = "各工程历史污染物变化情况";
            var dict = new Dictionary<object, StatHourInfo>();
            foreach (var stat in WdContext.StatList)
            {
                var hour = DateTime.Now.AddHours(-1);
                var value = DbContext.T_ESHour.FirstOrDefault(item => item.StatId == stat.Id && item.UpdateTime.Hour > hour.Hour);
                var current = DbContext.T_ESMin.LastOrDefault(obj => obj.StatId == stat.Id && obj.UpdateTime.Value.Hour > hour.Hour);

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
            var basic = WdContext.StatList.Where(stat => stat.Stage == (int) Stage.Basic).Select(obj => obj.Id).ToList();
            var structure = WdContext.StatList.Where(stat => stat.Stage == (int) Stage.Structure).Select(obj =>  obj.Id ).ToList();

            var startDate = DateTime.Now.AddMonths(-1);
            var basicList = DbContext.T_ESDay.Where(obj => basic.Contains(obj.StatId) && obj.UpdateTime > startDate) .ToList()
                .Select(item => new {
                    TP = double.Parse((item.TP / 1000).ToString("f2")),
                    DB = double.Parse(item.DB.ToString("f2")),
                    UpdateTime = item.UpdateTime.ToString("yyyy-MM-dd")
                });
            var structureList = DbContext.T_ESDay.Where(obj => structure.Contains(obj.StatId) && obj.UpdateTime > startDate).ToList()
                .Select(item => new {
                    TP = double.Parse((item.TP / 1000).ToString("f2")),
                    DB = double.Parse(item.DB.ToString("f2")),
                    UpdateTime = item.UpdateTime.ToString("yyyy-MM-dd") });

            var dict = new Dictionary<string, object> {{"basic", basicList}, {"structure", structureList}};
            
            var model = new ScheduleCompareViewModel
            {
                BasicAvgTp = basicList.Any() ? basicList.Average(i => i.TP).ToString("f2") : "暂无数据",
                BasicAvgDb = basicList.Any() ? basicList.Average(i => i.DB).ToString("f2"): "暂无数据",
                StructureTp = structureList.Any() ? structureList.Average(i => i.TP).ToString("f2") : "暂无数据",
                StructureDb = structureList.Any() ? structureList.Average(i => i.DB).ToString("f2") : "暂无数据"
            };
            
            ViewBag.Dict = JsonConvert.SerializeObject(dict);

            return DynamicView("ScheduleCompare", model);
        }
    }
}