using System.Web.Mvc;

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
    }
}