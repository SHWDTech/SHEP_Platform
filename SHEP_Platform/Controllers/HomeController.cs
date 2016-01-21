using System.Linq;
using System.Web.Mvc;

namespace SHEP_Platform.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// 数据库Context
        /// </summary>
        private ESMonitorEntities Context { get; set; }

        public ActionResult Index()
        {
            ViewBag.Message = "Welcome To My WebSite!";
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public JsonResult GeneralInfo()
        {
           // Context.T_ESMin.Select()

            return null;
        }
    }
}