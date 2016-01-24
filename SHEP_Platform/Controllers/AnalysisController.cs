using System.Web.Mvc;
using Newtonsoft.Json;

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
            var pageName = string.Empty;
            switch (id)
            {
                case "month":
                pageName = "本区县颗粒物浓度月度报表";
                    break;
                case "season":
                    pageName = "本区县颗粒物浓度季度报表";
                    break;
                case "year":
                    pageName = "本区县颗粒物浓度年度报表";
                    break;
            }

            WdContext.SiteMapMenu.ActionMenu.Name = pageName;

            ViewBag.ReportType = id;
            return View();
        }

        [HttpGet]
        public string AveragePollute()
        {

            return JsonConvert.SerializeObject("a");
        }
    }
}