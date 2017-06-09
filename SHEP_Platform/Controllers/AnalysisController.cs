using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using DatabaseModel;
using SHEP_Platform.Common;
using SHEP_Platform.Models.Analysis;
using SHEP_Platform.Models.Api;
using SHWDTech.Platform.Utility;

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

            return DynamicView(nameof(AveragePolluteReport));
        }

        public ActionResult AlarmChange()
        {
            WdContext.SiteMapMenu.ControllerMenu.Name = "综合评价";
            WdContext.SiteMapMenu.ActionMenu.Name = "本曲线整体超标情况变化趋势";

            return DynamicView(nameof(AlarmChange));
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
            var rows = (from grp in exceptions.Skip(post.offset).Take(post.limit).Select(e => new
            {
                DevId = e.Key,
                Exceptions = e.Select(ex => new
                {
                    ExceptionId = ex.Id,
                    ex.ExceptionType,
                    ex.ExceptionTime,
                    ex.ExceptionValue,
                    ex.Comment
                })
            }).ToList()
                        let dev = DbContext.T_Devs.FirstOrDefault(d => d.Id == grp.DevId)
                        let stat = DbContext.T_Stats.FirstOrDefault(s => s.Id.ToString() == dev.StatId)
                        let devAddr = DbContext.T_DevAddr.FirstOrDefault(a => a.DevId == dev.Id)
                        select new
                        {
                            devId = dev.Id,
                            devName = dev.DevCode,
                            devNodeId = Global.BytesToInt32(devAddr.NodeId, 0, false),
                            statName = stat.StatName,
                            chargeMan = stat.ChargeMan,
                            telephone = stat.Telepone,
                            statId = dev.StatId,
                            exceptions = grp.Exceptions.Select(g => new
                            {
                                g.ExceptionId,
                                g.ExceptionType,
                                ExceptionTime = $"{g.ExceptionTime:yyyy-MM-dd HH:mm:ss}",
                                g.ExceptionValue,
                                Comment = g.Comment?.Length > 20 ? $"{g.Comment?.Substring(0, 20)}..." : g.Comment
                            })
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
                .Select(v => (DeviceExceptionType)v).ToList();
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
                    return RedirectToAction(nameof(ProcessDeviceException), model);
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
                    exp.ProcessDateTime = DateTime.Now;
                }

                DbContext.SaveChanges();
                return Content("异常处理完成！");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return RedirectToAction(nameof(ProcessDeviceException), model);
            }
        }

        [HttpGet]
        public ActionResult ExceptionComment(int exceptionId)
        {
            var exp = DbContext.DeviceException.FirstOrDefault(e => e.Id == exceptionId);
            if (exp == null)
            {
                ModelState.AddModelError("", "未找到指定异常数据。");
                return View();
            }
            var model = new ExceptionCommentViewModel
            {
                ExceptioId = exp.Id,
                ExceptionName = EnumHelper<DeviceExceptionType>.GetDisplayValue((DeviceExceptionType)exp.ExceptionType),
                ExceptionValue = exp.ExceptionValue,
                DevName = DbContext.T_Devs.First(dev => dev.Id == exp.DevId).DevCode,
                DevNodeId = Global.BytesToInt32(DbContext.T_DevAddr.First(a => a.DevId == exp.DevId).NodeId, 0 , false),
                StatName = DbContext.T_Stats.First(s => s.Id == exp.StatId).StatName,
                Comment = exp.Comment
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult ExceptionComment(ExceptionCommentViewModel model)
        {
            try
            {
                var exp = DbContext.DeviceException.FirstOrDefault(e => e.Id == model.ExceptioId);
                if (exp == null)
                {
                    ModelState.AddModelError("", "未找到指定异常数据。");
                    return View(model);
                }
                exp.Comment = model.Comment;
                DbContext.SaveChanges();
                return Content("保存完成！");
            }
            catch (Exception ex)
            {
                LogService.Instance.Error("保存异常备注信息失败！", ex);
                return View(model);
            }
        }

        public ActionResult ExceptionHistory(int? devId, bool? partial)
        {
            ViewBag.DevId = devId;
            if (partial == true)
            {
                return PartialView();
            }
            return View();
        }

        public ActionResult ExceptionHistoryTable(ExceptionHistoryTablePost post)
        {
            var query = DbContext.DeviceException.AsQueryable();
            if (post.StatId != null)
            {
                query = query.Where(q => q.StatId == post.StatId);
            }
            if (post.DevId != null)
            {
                query = query.Where(q => q.DevId == post.DevId);
            }
            var total = query.Count();
            var ret = query.OrderBy(q => q.Id).Skip(post.offset).Take(post.limit)
                .ToList();

            var rows = ret.Select(r => new
            {
                r.Id,
                DbContext.T_Stats.First(s => s.Id == r.StatId).StatName,
                DbContext.T_Devs.First(d => d.Id == r.DevId).DevCode,
                NodeId = Global.BytesToInt32(DbContext.T_DevAddr.First(a => a.DevId == r.DevId).NodeId, 0, false),
                ExceptionType = EnumHelper<DeviceExceptionType>.GetDisplayValue((DeviceExceptionType)r.ExceptionType),
                r.Comment,
                ExceptionStatus = r.Processed ? "已处理" : "未处理",
                r.DeviceExceptionReason,
                r.ProgressMan,
                r.ProgressResult,
                ProcessDateTime = $"{r.ProcessDateTime: yyyy-MM-dd HH:mm:ss}"
            });
            return Json(new
            {
                total,
                rows
            }, JsonRequestBehavior.AllowGet);
        }
    }
}