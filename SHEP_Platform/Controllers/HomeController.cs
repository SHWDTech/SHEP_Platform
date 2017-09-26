using System.Linq;
using System.Web.Mvc;
using Newtonsoft.Json;
using Platform.Cache;
using SHEP_Platform.Models.Home;

namespace SHEP_Platform.Controllers
{
    public class HomeController : ControllerBase
    {
        /// <summary>
        /// 数据库Context
        /// </summary>
        public ActionResult Index()
        {
            var stats = WdContext.StatList;

            var homestatList = new HomeStatList();

            var status = PlatformCaches.GetCachesByType("statStatus");
            var list = status.Select(statu => (StatStatus) statu.CacheItem).Where(item => stats.Any(s => s.Id == item.Id)).ToList();

            var statList = PlatformCaches.GetCachesByType("statList");
            foreach (var platformCach in statList)
            {
                var item = (StatList)platformCach.CacheItem;
                if (stats.Any(s => s.Id == item.Id))
                {
                    homestatList.StatLists.Add((StatList)platformCach.CacheItem);
                }
            }

            ViewBag.Status = JsonConvert.SerializeObject(list);

            return DynamicView("Index", homestatList);
        }
    }
}