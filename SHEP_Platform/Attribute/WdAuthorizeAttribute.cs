using System;
using System.Web.Mvc;

namespace SHEP_Platform.Attribute
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class WdAuthorizeAttribute : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.Result = new JsonResult
                {
                    Data = new
                    {
                        message = "unAuthorized",
                        result = "failed"
                    },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            else
            {
                base.HandleUnauthorizedRequest(filterContext);
            }
        }
    }
}