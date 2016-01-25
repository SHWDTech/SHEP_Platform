using System;
using System.Reflection;
using System.Web.Mvc;
using log4net;

namespace SHEP_Platform.Controllers
{
    public class AjaxControllerBase : ControllerBase
    {
        protected string FunctionName { get; private set; }

        // GET: AjaxControllerBase
        protected JsonResult ParseRequest()
        {
            FunctionName = Request["fun"];
            try
            {
                return ExecuteFun(FunctionName);
            }
            catch (Exception e)
            {
                LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType).Error("AjaxRequestException", e);
                Response.Write("请求失败！");
            }

            return null;
        }

        protected virtual JsonResult ExecuteFun(string functionName)
        {
            return null;
        }
    }
}