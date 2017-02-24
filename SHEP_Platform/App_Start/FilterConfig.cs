using System.Web.Mvc;
using SHEP_Platform.Attribute;

namespace SHEP_Platform
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new WdAuthorizeAttribute());
        }
    }
}
