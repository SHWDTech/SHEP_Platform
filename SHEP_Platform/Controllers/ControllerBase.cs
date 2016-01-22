using System.Linq;
using System.Web.Mvc;
using SHEP_Platform.Common;

namespace SHEP_Platform.Controllers
{
    public class ControllerBase : Controller
    {
        public WdContext WdContext { get; private set; }

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
            }
            base.OnActionExecuting(ctx);
        }
    }
}