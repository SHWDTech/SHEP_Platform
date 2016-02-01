using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SHEP_Platform.Models.Admin;

namespace SHEP_Platform.Controllers
{
    public class AdminController : ControllerBase
    {
        public AdminController()
        {
            WdContext.SiteMapMenu.ControllerMenu.Name = "系统管理";
            WdContext.SiteMapMenu.ControllerMenu.LinkAble = false;
        }

        // GET: Admin
        public ActionResult StatManage()
        {
            if (!ViewBag.IsAdmin)
            {
                return RedirectToAction("Index", "Home");
            }

            WdContext.SiteMapMenu.ActionMenu.Name = "监测点管理";

            var model = new StatManageViewModel
            {
                PageIndex = 0,
                PageSize = 10,
                StatList = DbContext.T_Stats.ToList()
            };
            return View(model);
        }

        [HttpGet]
        public ActionResult StatEdit(string id)
        {
            if (!ViewBag.IsAdmin)
            {
                return RedirectToAction("Index", "Home");
            }

            WdContext.SiteMapMenu.ActionMenu.Name = "编辑监测点";
            StatEditViewModel model = new StatEditViewModel();
            if (string.IsNullOrWhiteSpace(id))
            {
                WdContext.SiteMapMenu.ActionMenu.Name = "新增监测点";
                model.Id = -1;
                model.IsNew = true;
                model.ProStartTime = DateTime.Now;
            }
            else
            {
                var stat = DbContext.T_Stats.First(item => item.Id.ToString() == id);
                model.Id = stat.Id;
                model.StatCode = stat.StatCode;
                model.StatName = stat.StatName;
                model.ChargeMan = stat.ChargeMan;
                model.Telepone = stat.Telepone;
                model.Longitude = stat.Longitude;
                model.Latitude = stat.Latitude;
                model.Department = stat.Department;
                model.Address = stat.Address;
                model.Street = stat.Street;
                model.Square = stat.Square;
                model.ProStartTime = stat.ProStartTime;
                model.T_Stage = stat.T_Stage;
                model.Stage = stat.Stage;
                model.ProType = stat.ProType;
                model.Country = stat.Country;
                model.T_Country = stat.T_Country;
            }

            ViewBag.ReturnUrl = "/Admin/DevManage";

            model.StageList = new SelectList(DbContext.T_Stage, "Id", "StageName", model.Stage);

            model.CountryList = new SelectList(DbContext.T_Country, "Id", "Country", model.Country);

            return View(model);
        }

        [HttpPost]
        public ActionResult StatEdit(StatEditViewModel model)
        {
            if (!ViewBag.IsAdmin)
            {
                return RedirectToAction("Index", "Home");
            }

            var stat = model.Id == -1 ? new T_Stats() : DbContext.T_Stats.First(item => item.Id == model.Id);

            stat.Country = model.Country;
            stat.Longitude = model.Longitude;
            stat.Latitude = model.Latitude;
            stat.Stage = model.Stage;
            stat.Address = model.Address;
            stat.ProType = "商业地产";
            stat.ChargeMan = model.ChargeMan;
            stat.Department = model.Department;
            stat.ProStartTime = model.ProStartTime;
            stat.Square = model.Square;
            stat.StatCode = model.StatCode;
            stat.StatName = model.StatName;
            stat.Street = model.Street;
            stat.Telepone = model.Telepone;

            if (model.Id == -1)
            {
                DbContext.T_Stats.Add(stat);
            }

            DbContext.SaveChanges();

            return RedirectToAction("StatManage", "Admin");
        }

        [HttpGet]
        public JsonResult StatDelete(int id)
        {
            if (!ViewBag.IsAdmin)
            {
                RedirectToAction("Index", "Home");

                return null;
            }

            var stat = DbContext.T_Stats.First(item => item.Id == id);
            DbContext.T_Stats.Remove(stat);
            DbContext.SaveChanges();

            var ret = new
            {
                msg = "success"
            };

            return Json(ret, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DevManage()
        {
            if (!ViewBag.IsAdmin)
            {
                return RedirectToAction("Index", "Home");
            }

            WdContext.SiteMapMenu.ActionMenu.Name = "设备管理";
            
            var list = DbContext.T_Devs.ToList().Select(source => new Devs
            {
                Id = source.Id,
                StatName = DbContext.T_Stats.FirstOrDefault(obj => obj.Id.ToString() == source.StatId)?.StatCode,
                DevCode = source.DevCode,
                StartTime = source.StartTime.ToString("yyyy-MM-dd"),
                PreEndTime = source.PreEndTime.ToString("yyyy-MM-dd"),
                EndTime = source.EndTime.ToString("yyyy-MM-dd"),
                DevStatus = source.DevStatus == 1 ? "是" : "否",
                VideoURL = source.VideoURL
            }).ToList();

            var model = new DevManageViewModel
            {
                PageIndex = 0,
                PageSize = 10,
                DevList = list
            };

            return View(model);
        }

        [HttpGet]
        public ActionResult DevEdit(string id)
        {
            if (!ViewBag.IsAdmin)
            {
                return RedirectToAction("Index", "Home");
            }

            WdContext.SiteMapMenu.ActionMenu.Name = "编辑监测点";
            var model = new DevEditViewModel();
            if (string.IsNullOrWhiteSpace(id))
            {
                WdContext.SiteMapMenu.ActionMenu.Name = "新增监测点";
                model.Id = -1;
                model.IsNew = true;
                model.StartTime = DateTime.Now;
                model.PreEndTime = DateTime.Now;
                model.EndTime = DateTime.Now;
            }
            else
            {
                var dev = DbContext.T_Devs.First(item => item.Id.ToString() == id);
                model.Id = dev.Id;
                model.DevCode = dev.DevCode;
                model.StartTime = dev.StartTime;
                model.PreEndTime = dev.PreEndTime;
                model.EndTime = dev.EndTime;
                model.DevStatus = dev.DevStatus;
                model.VideoUrl = dev.VideoURL;
            }

            ViewBag.ReturnUrl = "/Admin/DevManage";

            model.StatList = new SelectList(DbContext.T_Stats, "Id", "StatName", model.StatId);

            var statusList = new List<SelectListItem>
            {
                new SelectListItem()
                {
                    Text = "是",
                    Value = "1"
                },
                new SelectListItem()
                {
                    Text = "否",
                    Value = "0"
                }
            };
            model.StatusLIst = new SelectList(statusList, "Value", "Text", model.DevStatus);

            return View(model);
        }

        [HttpPost]
        public ActionResult DevEdit(DevEditViewModel model)
        {
            if (!ViewBag.IsAdmin)
            {
                return RedirectToAction("Index", "Home");
            }

            var dev = model.Id == -1 ? new T_Devs() : DbContext.T_Devs.First(item => item.Id == model.Id);

            dev.DevCode = model.DevCode;
            dev.StartTime = model.StartTime;
            dev.PreEndTime = model.PreEndTime;
            dev.EndTime = model.EndTime;
            dev.VideoURL = model.VideoUrl;
            dev.StatId = model.StatId.ToString();
            dev.DevStatus = model.DevStatus;

            if (model.Id == -1)
            {
                DbContext.T_Devs.Add(dev);
            }

            DbContext.SaveChanges();

            return RedirectToAction("DevManage", "Admin");
        }

        [HttpGet]
        public JsonResult DevDelete(int id)
        {
            if (!ViewBag.IsAdmin)
            {
                RedirectToAction("Index", "Home");

                return null;
            }

            var dev = DbContext.T_Devs.First(item => item.Id == id);
            DbContext.T_Devs.Remove(dev);
            DbContext.SaveChanges();

            var ret = new
            {
                msg = "success"
            };

            return Json(ret, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UserManage()
        {
            if (!ViewBag.IsAdmin)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }
    }
}