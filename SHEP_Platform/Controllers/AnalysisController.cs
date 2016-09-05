using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using SHEP_Platform.Models.Analysis;

namespace SHEP_Platform.Controllers
{
    public class AnalysisController : ControllerBase
    {
        public AnalysisController()
        {
            WdContext.SiteMapMenu.ControllerMenu.Name = "统计分析";
            WdContext.SiteMapMenu.ControllerMenu.LinkAble = false;
        }

        // GET: Analysis
        public ActionResult AveragePolluteReport()
        {
            WdContext.SiteMapMenu.ActionMenu.Name = "本区县污染物平均浓度报表";
            
            return DynamicView("AveragePolluteReport");
        }

        public ActionResult AlarmChange()
        {
            WdContext.SiteMapMenu.ControllerMenu.Name = "综合评价";
            WdContext.SiteMapMenu.ActionMenu.Name = "本曲线整体超标情况变化趋势";

            return DynamicView("AlarmChange");
        }

        public ActionResult VocViewer()
        {
            WdContext.SiteMapMenu.ControllerMenu.Name = "统计分析";
            WdContext.SiteMapMenu.ActionMenu.Name = "可挥发性有机物监测数值列表";

            return View();
        }

        public ActionResult AlarmDetails(AlarmDetailsViewModel model)
        {
            var statList = WdContext.StatList.Select(obj => obj.Id).ToList();
            var alarms = DbContext.T_Alarms
                .Where(obj => statList.Contains(obj.StatId.Value))
                .OrderByDescending(item => item.UpdateTime);

            var details = new List<AlarmFullDetail>();
            foreach (var alarm in alarms)
            {
                var stat = WdContext.StatList.First(obj => alarm.StatId != null && obj.Id == alarm.StatId.Value);
                var dev = DbContext.T_Devs.First(obj => obj.Id == alarm.DevId.Value);
                if (alarm.FaultVal != null)
                    details.Add(new AlarmFullDetail
                    {
                        AlarmId = alarm.Id,
                        StatName = stat.StatName,
                        ChargeMan = stat.ChargeMan,
                        Telephone = stat.Telepone,
                        DevCode = dev.DevCode,
                        // ReSharper disable once PossibleInvalidOperationException
                        AlarmDateTime = alarm.UpdateTime.Value,
                        FaultValue = (alarm.FaultVal.Value / 1000.0).ToString("f2"),
                        IsReaded = alarm.IsReaded
                    });
            }

            model.Details = details.ToPagedList(model.page, 10);

            return View(model);
        }
    }
}