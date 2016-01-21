using System.Web.Mvc;

namespace SHEP_Platform.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        [AllowAnonymous]
        public ActionResult Login()
        {
            ViewBag.LoginTitle = "请输入用户名密码";
            return View();
        }
    }
}