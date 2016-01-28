using System.Web.Mvc;

namespace SHEP_Platform.Controllers
{
    public class AdminController : ControllerBase
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
    }
}