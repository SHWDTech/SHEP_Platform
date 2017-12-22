using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using SHEP_Platform.Common;
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
            if (ctx.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true)
                || ctx.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true))
            {
                base.OnActionExecuting(ctx);
                return;
            }

            WdContext.UserId = HttpContext?.Request?.Cookies?.Get("UserId")?.Value;
            if (WdContext.UserId != null)
            {
                WdContext.User = DbContext.T_Users.FirstOrDefault(user => user.UserId.ToString() == WdContext.UserId);
                var stats = DbContext.T_UserStats.Where(obj => obj.UserId.ToString() == WdContext.UserId)
                        .Select(item => item.StatId)
                        .ToList();
                ViewBag.IsAdmin = WdContext.User != null && WdContext.User.UserName == "admin";
                if (ViewBag.IsAdmin)
                {
                    WdContext.StatList.AddRange(DbContext.T_Stats.ToList());
                }
                else
                {
                    WdContext.StatList.AddRange(DbContext.T_Stats.Where(obj => stats.Contains(obj.Id)));
                }

                ViewBag.SiteMapMenu = WdContext.SiteMapMenu;

                ViewBag.IsMobileDevice = WdContext.IsMobileDevice = Global.IsMobileDevice(ctx.HttpContext.Request.UserAgent);

                base.OnActionExecuting(ctx);
            }
            else
            {
                 FormsAuthentication.SignOut();
                 ctx.Result = RedirectToAction("Login", "Account");
            }
        }

        protected ViewResult DynamicView(string viewName) => View(WdContext.IsMobileDevice ? $"{viewName}_m" : viewName);

        protected ViewResult DynamicView(string viewName, object model) => View(WdContext.IsMobileDevice ? $"{viewName}_m" : viewName, model);
    }
}