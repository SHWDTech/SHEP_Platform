using System;
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

            return View();
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