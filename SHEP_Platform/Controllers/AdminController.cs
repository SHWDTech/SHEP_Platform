using System;
using System.Linq;
using System.Web.Mvc;
using SHEP_Platform.Common;
using SHEP_Platform.Models.Admin;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Transactions;
using SHEP_Platform.Models.Api;
using SHWDTech.Platform.Utility;

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
            return View();
        }

        public ActionResult StatsTable(NameQueryTablePost post)
        {
            var query = DbContext.T_Stats.AsQueryable();
            if (!string.IsNullOrWhiteSpace(post.QueryName))
            {
                query = query.Where(d => d.StatName.Contains(post.QueryName));
            }

            var total = query.Count();
            var rows = query.OrderBy(d => d.Id).Skip(post.offset).Take(post.limit).ToList()
                .Select(s => new
                {
                    s.Id,
                    s.StatCode,
                    s.StatName,
                    s.ChargeMan,
                    s.Telepone,
                    s.Department,
                    s.Address,
                    s.Square
                });
            return Json(new
            {
                total,
                rows
            }, JsonRequestBehavior.AllowGet);
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

            ViewBag.ReturnUrl = "/Admin/StatManage";

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
            stat.StatCodeUp = (new Random()).Next(2000000, 2100000);

            if (model.Id == -1)
            {
                DbContext.T_Stats.Add(stat);
            }

            try
            {
                DbContext.SaveChanges();

            }
            catch (DbEntityValidationException ex)
            {
                foreach (var error in ex.EntityValidationErrors)
                {
                    foreach (var dbValidationError in error.ValidationErrors)
                    {
                        ModelState.AddModelError(dbValidationError.PropertyName, dbValidationError.ErrorMessage);
                    }
                }

                model.StageList = new SelectList(DbContext.T_Stage, "Id", "StageName", model.Stage);

                model.CountryList = new SelectList(DbContext.T_Country, "Id", "Country", model.Country);
                return View(model);
            }

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

            return View();
        }

        public ActionResult DevsTable(NameQueryTablePost post)
        {
            var query = DbContext.T_Devs.AsQueryable();
            if (!string.IsNullOrWhiteSpace(post.QueryName))
            {
                query = query.Where(d => d.DevCode.Contains(post.QueryName));
            }

            var total = query.Count();
            var rows = query.OrderBy(d => d.Id).Skip(post.offset).Take(post.limit).ToList()
                .Select(dev => new
                {
                    dev.Id,
                    dev.DevCode,
                    DbContext.T_Stats.FirstOrDefault(obj => obj.Id.ToString() == dev.StatId)?.StatName,
                    NodeId = Global.BytesToInt32(DbContext.T_DevAddr.First(d => d.DevId == dev.Id).NodeId, 0, false),
                    Status = dev.DevStatus == 1 ? "启用" : "停用"
                });
            return Json(new
            {
                total,
                rows
            }, JsonRequestBehavior.AllowGet);
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
                model.StatId = int.Parse(dev.StatId);
                model.VideoUrl = dev.VideoURL;

                model.Addr = Global.BytesToInt32(DbContext.T_DevAddr.First(d => d.DevId == dev.Id).NodeId, 0, false).ToString();
            }

            ViewBag.ReturnUrl = "/Admin/DevManage";

            model.StatList = new SelectList(DbContext.T_Stats, "Id", "StatName", model.StatId);

            return View(model);
        }

        [HttpPost]
        public ActionResult DevEdit(DevEditViewModel model)
        {
            if (!ViewBag.IsAdmin)
            {
                return RedirectToAction("Index", "Home");
            }

            using (var transaction = new TransactionScope())
            {
                try
                {
                    model.StatList = new SelectList(DbContext.T_Stats, "Id", "StatName", model.StatId);
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
                        if (string.IsNullOrWhiteSpace(model.Addr))
                        {
                            ModelState.AddModelError("Addr", "设备ID不能为空。");
                            return View(model);
                        }
                        var addr = Global.StringTohexStringByte(model.Addr);
                        if (DbContext.T_DevAddr.Any(a => a.NodeId.Equals(addr)))
                        {
                            ModelState.AddModelError("Addr", "设备ID已经存在。");
                            return View(model);
                        }
                        DbContext.T_Devs.Add(dev);
                        DbContext.SaveChanges();
                        var devAddr = new T_DevAddr
                        {
                            DevId = dev.Id,
                            NodeId = addr
                        };
                        DbContext.T_DevAddr.Add(devAddr);
                    }
                    DbContext.SaveChanges();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    ModelState.AddModelError("UnKnow", "保存设备信息失败。");
                    return View(model);
                }

                transaction.Complete();
            }

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

        public ActionResult UsersTable(NameQueryTablePost post)
        {
            var query = DbContext.T_Users.AsQueryable();
            if (!string.IsNullOrWhiteSpace(post.QueryName))
            {
                query = query.Where(d => d.UserName.Contains(post.QueryName));
            }

            var total = query.Count();
            var rows = query.OrderBy(d => d.UserId).Skip(post.offset).Take(post.limit).ToList()
                .Select(u => new
                {
                    u.UserId,
                    u.UserName,
                    RoleName = GetRoleName(u.RoleId),
                    RegTime = u.RegTime.ToString("yyyy-MM-dd HH:mm:ss"),
                    LastLogin = u.LastTime?.ToString("yyyy-MM-dd HH:mm:ss"),
                    CountryName = DbContext.T_Country.FirstOrDefault(obj => obj.Id.ToString() == u.Remark)?.Country,
                    Status = Global.GetUserStatus(u.Status)
                });
            return Json(new
            {
                total,
                rows
            }, JsonRequestBehavior.AllowGet);
        }

        private string GetRoleName(int roleId)
        {
            switch (roleId)
            {
                case 1:
                    return "超级管理员";
                case 2:
                    return "管理员";
                case 3:
                    return "工地负责人";
            }

            return null;
        }

        public ActionResult Lock(string id)
        {
            if (!ViewBag.IsAdmin)
            {
                return RedirectToAction("Index", "Home");
            }

            var user = DbContext.T_Users.First(obj => obj.UserId.ToString() == id);
            user.Status = 1;
            DbContext.SaveChanges();

            return RedirectToAction("UserManage", "Admin");
        }

        [HttpGet]
        public ActionResult EditPwd(int id)
        {
            var user = DbContext.T_Users.First(u => u.UserId == id);
            var model = new UserPassword
            {
                Id = user.UserId,
                UserName = user.UserName
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult EditPwd(UserPassword model)
        {
            if (string.IsNullOrWhiteSpace(model.Password))
            {
                ModelState.AddModelError("", "密码不能为空。");
                return View(model);
            }
            if (model.Password != model.ConfirmPassword)
            {
                ModelState.AddModelError("", "两次输入的密码不一致，请重新输入。");
                return View(model);
            }

            var user = DbContext.T_Users.First(u => u.UserId == model.Id);
            user.Pwd = Global.GetMd5(model.Password);
            DbContext.SaveChanges();

            return RedirectToAction("UserManage", "Admin");
        }

        public ActionResult UnLock(string id)
        {
            if (!ViewBag.IsAdmin)
            {
                return RedirectToAction("Index", "Home");
            }

            var user = DbContext.T_Users.First(obj => obj.UserId.ToString() == id);
            user.Status = 2;
            DbContext.SaveChanges();

            return RedirectToAction("UserManage", "Admin");
        }

        [HttpGet]
        public JsonResult UserDelete(int id)
        {
            if (!ViewBag.IsAdmin)
            {
                RedirectToAction("Index", "Home");

                return null;
            }

            var user = DbContext.T_Users.First(item => item.UserId == id);
            DbContext.T_Users.Remove(user);
            DbContext.SaveChanges();

            var ret = new
            {
                msg = "success"
            };

            return Json(ret, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult UserEdit(string id)
        {
            if (!ViewBag.IsAdmin)
            {
                return RedirectToAction("Index", "Home");
            }

            WdContext.SiteMapMenu.ActionMenu.Name = "编辑用户";
            var model = new UserEditViewModel();
            if (string.IsNullOrWhiteSpace(id))
            {
                WdContext.SiteMapMenu.ActionMenu.Name = "新增用户";
                model.UserId = -1;
                model.IsNew = true;
                model.RegTime = DateTime.Now;
                model.LastTime = DateTime.Now;
                model.Status = 1;
            }
            else
            {
                var user = DbContext.T_Users.First(item => item.UserId.ToString() == id);
                model.UserId = user.UserId;
                model.Mobile = user.Mobile;
                model.EmailAddress = user.Email;
                model.Remark = user.Remark;
                model.Status = user.Status;
                model.UserName = user.UserName;
                model.RoleId = user.RoleId;
            }

            ViewBag.ReturnUrl = "/Admin/UserManage";

            model.CountryList = new SelectList(DbContext.T_Country, "Id", "Country", model.Remark);

            var statusList = new List<SelectListItem>
            {
                new SelectListItem()
                {
                    Text = "超级管理员",
                    Value = "1"
                },
                new SelectListItem()
                {
                    Text = "管理员",
                    Value = "2"
                },
                new SelectListItem()
                {
                    Text = "工地负责人",
                    Value = "3"
                }
            };
            model.RoleList = new SelectList(statusList, "Value", "Text", model.RoleId);

            var stats = DbContext.T_Stats.ToList();
            var userStats = DbContext.T_UserStats.Where(obj => obj.UserId == model.UserId).ToList();
            foreach (var stat in stats)
            {
                var select = new SelectListItem { Value = stat.Id.ToString(), Text = stat.StatName };
                if (userStats.FirstOrDefault(obj => obj.StatId == stat.Id) != null)
                {
                    select.Selected = true;
                }
                model.StatsList.Add(select);
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult UserEdit(UserEditViewModel model)
        {
            if (!ViewBag.IsAdmin)
            {
                return RedirectToAction("Index", "Home");
            }

            if (DbContext.T_Users.Any(obj => obj.UserName == model.UserName) && model.UserId == -1)
            {
                ModelState.AddModelError("UserName", "系统已存在同名用户");
                return UserEditError(model);
            }

            var user = model.UserId == -1 ? new T_Users() : DbContext.T_Users.First(item => item.UserId == model.UserId);

            user.Mobile = model.Mobile;
            user.Email = model.EmailAddress;
            user.Remark = model.Remark;
            user.RoleId = model.RoleId;
            user.UserName = model.UserName;
            user.RoleId = model.RoleId;

            if (model.UserId == -1)
            {
                user.Status = 1;
                user.RegTime = DateTime.Now;
                user.Pwd = Global.GetMd5("31c5bb73ee74ecb23b4bb656b79bfd6792c863d98fd978aaee216c6738427101");
                DbContext.T_Users.Add(user);
            }

            try
            {
                DbContext.SaveChanges();
                var authedStats = DbContext.T_UserStats.Where(obj => obj.UserId == user.UserId).ToList();
                foreach (var authedStat in authedStats)
                {
                    DbContext.T_UserStats.Remove(authedStat);
                }
                var authStats = Request["stats"];
                if (!string.IsNullOrWhiteSpace(authStats))
                {
                    var statLIst = authStats.Split(',').Select(int.Parse);
                    foreach (var stat in statLIst)
                    {
                        DbContext.T_UserStats.Add(new T_UserStats { UserId = user.UserId, StatId = stat });
                    }
                }
                DbContext.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var error in ex.EntityValidationErrors)
                {
                    foreach (var dbValidationError in error.ValidationErrors)
                    {
                        ModelState.AddModelError(dbValidationError.PropertyName, dbValidationError.ErrorMessage);
                    }
                }

                return UserEditError(model);
            }

            return RedirectToAction("UserManage", "Admin");
        }

        private ActionResult UserEditError(UserEditViewModel model)
        {
            var statusList = new List<SelectListItem>
            {
                new SelectListItem()
                {
                    Text = "超级管理员",
                    Value = "1"
                },
                new SelectListItem()
                {
                    Text = "管理员",
                    Value = "2"
                },
                new SelectListItem()
                {
                    Text = "工地负责人",
                    Value = "3"
                }
            };

            model.RoleList = new SelectList(statusList, "Value", "Text", model.RoleId);
            model.CountryList = new SelectList(DbContext.T_Country, "Id", "Country", model.Remark);

            var stats = DbContext.T_Stats.ToList();
            var userStats = DbContext.T_UserStats.Where(obj => obj.UserId == model.UserId).ToList();
            foreach (var stat in stats)
            {
                var select = new SelectListItem { Value = stat.Id.ToString(), Text = stat.StatName };
                if (userStats.FirstOrDefault(obj => obj.StatId == stat.Id) != null)
                {
                    select.Selected = true;
                }
                model.StatsList.Add(select);
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult Camera(string id)
        {
            var statIds = WdContext.StatList.Select(obj => obj.Id.ToString()).ToList();
            ViewBag.DevList = DbContext.T_Devs.Where(obj => statIds.Contains(obj.StatId)).Select(item => new SelectListItem() { Text = item.DevCode, Value = item.Id.ToString() }).ToList();

            T_Camera model;
            if (string.IsNullOrWhiteSpace(id))
            {
                WdContext.SiteMapMenu.ActionMenu.Name = "新增摄像头";
                model = new T_Camera { ID = -1 };
            }
            else
            {
                WdContext.SiteMapMenu.ActionMenu.Name = "编辑摄像头";
                model = DbContext.T_Camera.FirstOrDefault(obj => obj.ID.ToString() == id);
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Camera(T_Camera model)
        {
            if (model.ID == -1)
            {
                model.Type = "Static";
                model.DnsAddr = "dust.shweidong.com";
                model.Port = "80";
                DbContext.T_Camera.Add(model);
            }
            else
            {
                var dbModel = DbContext.T_Camera.First(obj => obj.ID == model.ID);
                dbModel.CameraName = model.CameraName;
                dbModel.DevId = model.DevId;
                dbModel.UserName = model.UserName;
                dbModel.PassWord = model.PassWord;
            }

            try
            {
                DbContext.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                LogService.Instance.Error("保存摄像头信息失败！", ex);
                var statIds = WdContext.StatList.Select(obj => obj.Id.ToString()).ToList();
                ViewBag.DevList = DbContext.T_Devs.Where(obj => statIds.Contains(obj.StatId)).Select(item => new SelectListItem() { Text = item.DevCode, Value = item.Id.ToString() }).ToList();

                foreach (var error in ex.EntityValidationErrors)
                {
                    foreach (var dbValidationError in error.ValidationErrors)
                    {
                        ModelState.AddModelError(dbValidationError.PropertyName, dbValidationError.ErrorMessage);
                    }
                }
                return View(model);
            }

            return RedirectToAction("CameraManage");
        }

        [HttpGet]
        public ActionResult CameraManage() => View();

        public ActionResult CamsTable(NameQueryTablePost post)
        {
            var query = DbContext.T_Camera.AsQueryable();
            if (!string.IsNullOrWhiteSpace(post.QueryName))
            {
                query = query.Where(d => d.CameraName.Contains(post.QueryName));
            }

            var total = query.Count();
            var rows = query.OrderBy(d => d.ID).Skip(post.offset).Take(post.limit).ToList()
                .Select(c => new
                {
                    c.ID,
                    c.CameraName,
                    DbContext.T_Devs.FirstOrDefault(d => d.Id == c.DevId)?.DevCode,
                    c.UserName
                });
            return Json(new
            {
                total,
                rows
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult CameraDelete(int id)
        {
            if (!ViewBag.IsAdmin)
            {
                RedirectToAction("Index", "Home");

                return null;
            }

            var dev = DbContext.T_Camera.First(item => item.ID == id);
            DbContext.T_Camera.Remove(dev);
            DbContext.SaveChanges();

            var ret = new
            {
                msg = "success"
            };

            return Json(ret, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Protocol()
        {
            var devs = DbContext.T_Devs.Select(dev => new SelectListItem
            {
                Text = dev.DevCode,
                Value = dev.Id.ToString()
            }).ToList();

            return View(devs);
        }

        public ActionResult DeviceRecent()
        {
            WdContext.SiteMapMenu.ActionMenu.Name = "设备测试";
            return View();
        }

        public ActionResult AllDevice()
        {
            return Json(DbContext.T_Devs.Select(d => new
            {
                id = d.Id,
                text = d.DevCode
            })
                .ToList(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeviceActivity(int devId)
        {
            return Json(new DeviceActivity(devId), JsonRequestBehavior.AllowGet);
        }

        public ActionResult DevRecentData(int devId)
        {
            var rows = DbContext.T_ESMin.Where(min => min.DevId == devId)
                .OrderByDescending(o => o.UpdateTime)
                .Take(15)
                .ToList()
                .Select(d => new
                {
                    d.TP,
                    d.DB,
                    d.PM25,
                    d.PM100,
                    d.WindSpeed,
                    d.WindDirection,
                    d.Temperature,
                    d.Humidity,
                    UpdateTime = $"{d.UpdateTime:yyyy-MM-dd HH:mm:ss fff}"
                });
            return Json(rows, JsonRequestBehavior.AllowGet);
        }
    }
}