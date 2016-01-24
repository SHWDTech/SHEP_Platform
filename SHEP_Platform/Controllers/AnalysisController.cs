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
        public ActionResult AveragePolluteReport(string id)
        {
            WdContext.SiteMapMenu.ActionMenu.Name = "本区县颗粒物浓度月报表";

            ViewBag.ReportType = id;
            return View();
        }
    }
}