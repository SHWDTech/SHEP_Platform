using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Newtonsoft.Json;
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
            ViewBag.CityName = WdContext.Country.Country;

            var stats = DbContext.T_Stats.Where(stat => stat.Country == WdContext.Country.Id).ToList();

            var list = new List<StatStatus>();

            foreach (var stat in stats)
            {
                var statId = stat.Id.ToString();
                var devIds = DbContext.T_Devs.Where(dev => dev.StatId == statId).Select(devId => devId.Id).ToArray();

                var tpTotal = 0.0d;
                var dbTotal = 0.0d;
                var validDev = 0;
                var lastUpdateTime = DateTime.Now;
                foreach (var id in devIds)
                {
                    var esMin = DbContext.T_ESMin.OrderByDescending(obj => obj.Id).FirstOrDefault(es => es.DevId == id);
                    if (esMin != null)
                    {
                        tpTotal += esMin.TP;
                        dbTotal += esMin.DB;
                        validDev += 1;
                        if (esMin.UpdateTime != null) lastUpdateTime = esMin.UpdateTime.Value;
                    }
                }

                list.Add(new StatStatus()
                {
                    Name = stat.StatName,
                    AvgTp = (tpTotal / validDev / 1000.0).ToString("f2"),
                    AvgDb = (dbTotal / validDev).ToString("f2"),
                    UpdateTime = lastUpdateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                    Longitude = stat.Longitude,
                    Latitude = stat.Latitude
                });
            }

            ViewBag.Status = JsonConvert.SerializeObject(list);

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public JsonResult GeneralInfo()
        {
            // Context.T_ESMin.Select()

            return null;
        }
    }
}