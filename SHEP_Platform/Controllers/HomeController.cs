using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace SHEP_Platform.Controllers
{
    public class HomeController : ControllerBase
    {
        /// <summary>
        /// 数据库Context
        /// </summary>
        public ActionResult Index()
        {
            ViewBag.CityName = DbContext.T_Country.FirstOrDefault(p => p.Id.ToString() == WdContext.Current.User.Remark)?.Country;
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