using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using DatabaseModel;
using OfficeOpenXml.FormulaParsing.Utilities;
using SHEP_Platform.Models.Analysis;
using SHEP_Platform.Models.Api;

namespace SHEP_Platform.Controllers
{
    public class AnalysisController : ControllerBase
    {
        public AnalysisController()
        {
            WdContext.SiteMapMenu.ControllerMenu.Name = "统计分析";
            WdContext.SiteMapMenu.ControllerMenu.LinkAble = false;
        }

        // GET: Analysis
        public ActionResult AveragePolluteReport()
        {
            WdContext.SiteMapMenu.ActionMenu.Name = "本区县污染物平均浓度报表";

            return DynamicView("AveragePolluteReport");
        }

        public ActionResult AlarmChange()
        {
            WdContext.SiteMapMenu.ControllerMenu.Name = "综合评价";
            WdContext.SiteMapMenu.ActionMenu.Name = "本曲线整体超标情况变化趋势";

            return DynamicView("AlarmChange");
        }

        public ActionResult VocViewer()
        {
            WdContext.SiteMapMenu.ControllerMenu.Name = "统计分析";
            WdContext.SiteMapMenu.ActionMenu.Name = "可挥发性有机物监测数值列表";

            return View();
        }

        public ActionResult AlarmDetails() => View();

        public ActionResult AlarmDetailsTable(TablePost post)
        {
            var exceptions = DbContext.DeviceException.Where(obj => !obj.Processed).GroupBy(e => e.DevId).OrderBy(ex => ex.Key);
            var total = exceptions.Count();
            var rows = (from grp in exceptions.Skip(post.offset).Take(post.limit).Select(e => new { DevId = e.Key, Exceptions = e.Select(ex => new { ex.ExceptionType, ex.ExceptionTime, ex.ExceptionValue }) }).ToList()
                        let dev = DbContext.T_Devs.FirstOrDefault(d => d.Id == grp.DevId)
                        let stat = DbContext.T_Stats.FirstOrDefault(s => s.Id.ToString() == dev.StatId)
                        select new
                        {
                            devId = dev.Id,
                            devName = dev.DevCode,
                            statName = stat.StatName,
                            statId = dev.StatId,
                            exceptions = grp.Exceptions.Select(g => new { g.ExceptionType, ExceptionTime = $"{g.ExceptionTime:yyyy-MM-dd HH:mm:ss}", g.ExceptionValue })
                        }).ToList();
            return Json(new
            {
                total,
                rows
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult StatPictures()
        {
            ViewBag.Stats = WdContext.StatList.Select(obj => new SelectListItem { Text = obj.StatName, Value = obj.Id.ToString() }).ToList();

            return View(new StatPicViewModel());
        }

        [HttpPost]
        public ActionResult StatPictures(StatPicViewModel model)
        {
            ViewBag.Stats = WdContext.StatList.Select(obj => new SelectListItem { Text = obj.StatName, Value = obj.Id.ToString() }).ToList();
            var stat = WdContext.StatList.First(obj => obj.Id == model.StatId);
            var devs = DbContext.T_Devs.Where(obj => obj.StatId == stat.Id.ToString());
            foreach (var dev in devs)
            {
                var camera = DbContext.T_Camera.FirstOrDefault(obj => obj.DevId == dev.Id);
                if (camera != null && Directory.Exists($"\\HikPicture\\{camera.UserName}\\AlarmPic"))
                {
                    var pics = Directory.GetFiles($"\\HikPicture\\{camera.UserName}\\AlarmPic");
                    model.PicUrls.AddRange(pics);
                }
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult ProcessDeviceException(AlarmProcessViewModel model)
        {
            model.Exceptions = DbContext.DeviceException
                .Where(obj => !obj.Processed && obj.DevId == model.DevId && obj.StatId == model.StatId)
                .Select(item => item.ExceptionType).ToList()
                .Select(v => (DeviceExceptionType) v).ToList();
            return View(model);
        }

        [HttpPost]
        public ActionResult ProgressDeviceException(AlarmProcessViewModel model)
        {
            model.IsPostBack = true;
            try
            {
                if (!ModelState.IsValid)
                {
                    return RedirectToAction("ProcessDeviceException", model);
                }
                var exceptionBytes = model.CheckedExceptions.Select(exp => (byte)exp).ToList();
                var exceptions =
                    DbContext.DeviceException.Where(e => e.DevId == model.DevId && e.StatId == model.StatId &&
                                                         exceptionBytes.Contains(e.ExceptionType))
                                                         .ToList();
                foreach (var exp in exceptions)
                {
                    exp.Processed = true;
                    exp.ProgressMan = model.ProgressMan;
                    exp.DeviceExceptionReason = model.DeviceExceptionReason;
                    exp.ProgressResult = model.ProgressResult;
                }

                DbContext.SaveChanges();
                return Content("异常处理完成！");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return RedirectToAction("ProcessDeviceException", model);
            }
        }
    }
}