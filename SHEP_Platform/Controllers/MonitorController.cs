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

            return View();
        }
    }
}