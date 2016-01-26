using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using SHEP_Platform.Common;

[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4net.config", Watch = true)]
namespace SHEP_Platform.Controllers
{

    public class ControllerBase : Controller
    {
        public WdContext WdContext { get; }

        protected ESMonitorEntities DbContext { get; set; }

        public ControllerBase()
        {
            WdContext = new WdContext();
            DbContext = new ESMonitorEntities();
        }

        protected override void OnActionExecuting(ActionExecutingContext ctx)
        {
            WdContext.UserId = HttpContext?.Request?.Cookies?.Get("UserId")?.Value;
            if (WdContext.UserId != null)
            {
                WdContext.User = DbContext.T_Users.FirstOrDefault(user => user.UserId.ToString() == WdContext.UserId);
                WdContext.Country =
                    DbContext.T_Country.FirstOrDefault(prov => prov.Id.ToString() == WdContext.User.Remark);
                WdContext.StatList = DbContext.T_Stats.Where(stat => stat.Country == WdContext.Country.Id).ToList();
                if (WdContext.Country != null) ViewBag.CityName = WdContext.Country.Country;

                ViewBag.SiteMapMenu = WdContext.SiteMapMenu;

                base.OnActionExecuting(ctx);
            }
            else
            {
                FormsAuthentication.SignOut();
                RedirectToAction("Login", "Account");
            }
        }
    }
}