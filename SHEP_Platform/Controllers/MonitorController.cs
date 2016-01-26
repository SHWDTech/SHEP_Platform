using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

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
        public ActionResult ActualStatus()
        {
            WdContext.SiteMapMenu.ActionMenu.Name = "各工程当前情况";
            var dict = new Dictionary<object, object>();
            foreach (var stat in WdContext.StatList)
            {
                var hour = DateTime.Now.AddHours(-1);
                var value = DbContext.T_ESHour.FirstOrDefault(item => item.StatId == stat.Id && item.UpdateTime.Hour > hour.Hour);
                dict.Add(stat, value);
            }

            ViewBag.StatDict = dict;
            ViewBag.defaultId = WdContext.StatList[0].Id;
            ViewBag.defaultName = WdContext.StatList[0].StatName;

            return View();
        }
    }
}