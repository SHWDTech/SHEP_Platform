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
            if (ctx.ActionDescriptor.ActionName == "Login")
            {
                base.OnActionExecuting(ctx);
                return;
            }

            WdContext.UserId = HttpContext?.Request?.Cookies?.Get("UserId")?.Value;
            if (WdContext.UserId != null)
            {
                WdContext.User = DbContext.T_Users.FirstOrDefault(user => user.UserId.ToString() == WdContext.UserId);
                WdContext.Country =
                    DbContext.T_Country.FirstOrDefault(prov => prov.Id.ToString() == WdContext.User.Remark);
                WdContext.StatList = DbContext.T_Stats.Where(stat => stat.Country == WdContext.Country.Id).ToList();
                if (WdContext.Country != null) ViewBag.CityName = WdContext.Country.Country;
               var groups = DbContext.T_UserInGroups.Where(user => user.UserId.ToString() == WdContext.UserId)
                    .Select(group => new { GroupId = group.GroupId.ToString() }).ToList();

                if (groups.Count > 0)
                {
                    foreach (var item in groups)
                    {
                        WdContext.UserGroup.Add(item.ToString());
                    }
                }

                ViewBag.IsAdmin = WdContext.User != null && WdContext.User.UserName == "admin";

                ViewBag.SiteMapMenu = WdContext.SiteMapMenu;

                base.OnActionExecuting(ctx);
            }
            else
            {
                FormsAuthentication.SignOut();
                ctx.Result = RedirectToAction("Login", "Account");
            }
        }
    }
}