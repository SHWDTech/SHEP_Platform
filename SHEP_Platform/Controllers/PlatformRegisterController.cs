using System;
using System.Linq;
using System.Web.Mvc;
using SHEP_Platform.Common;
using SHEP_Platform.Models.Api;
using SHEP_Platform.Models.PlatformRegister;
using SHEP_Platform.UnicomPlatform;
using SHWDTech.Platform.Utility;
using System.Collections.Generic;

namespace SHEP_Platform.Controllers
{
    public class PlatformRegisterController : ControllerBase
    {

        public ActionResult UniComProjectManage()
        {
            return View();
        }

        public ActionResult UnicomProjectTable(TablePost post)
        {
            var regedStats = DbContext.T_UnicomProject.ToList();

            var count = WdContext.StatList.Count;

            var statInfos = (from stat in WdContext.StatList.Skip(post.offset).Take(post.limit)
                let reg = regedStats.FirstOrDefault(r => r.StatId == stat.Id)
                select new
                {
                    stat.Id,
                    stat.StatName,
                    UnicomCode = reg?.UnicomCode ?? string.Empty,
                    Stopped = reg?.Stopped ?? true
                }).ToList();

            return Json(new
            {
                total = count,
                rows = statInfos
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult UnicomProject(int projectId)
        {
            var prj = DbContext.T_Stats.First(s => s.Id == projectId);
            var model = new UnicomPlatformRegisterModel
            {
                StatId = prj.Id,
                RegisterCode = prj.StatCode,
                Name = prj.StatName,
                Longitude = $"{prj.Longitude}",
                Latitude = $"{prj.Latitude}",
                StartDate = prj.ProStartTime,
                Street = prj.Street,
                Contractors = prj.Department,
                Superintendent = prj.ChargeMan,
                Telephone = prj.Telepone,
                Address = prj.Address,
                SiteArea = (float)prj.Square,
                BuildingArea = (float)prj.Square,
                IsCompleted = false,
                Status = true
            };

            LoadParams(model);
            return View(model);
        }

        [HttpPost]
        public ActionResult UnicomProject(UnicomPlatformRegisterModel model)
        {
            var project = new emsProject
            {
                name = model.Name,
                code = model.RegisterCode,
                district = model.District,
                prjType = model.ProjectType,
                prjTypeSpecified = true,
                prjCategory = model.ProjectCategory,
                prjCategorySpecified = true,
                prjPeriod = model.ProjectPeriod,
                prjPeriodSpecified = true,
                region = model.Region,
                regionSpecified = true,
                street = model.Street,
                longitude = model.Longitude,
                latitude = model.Latitude,
                contractors = model.Contractors,
                superintendent = model.Superintendent,
                telephone = model.Telephone,
                address = model.Address,
                siteArea = model.SiteArea,
                siteAreaSpecified = true,
                buildingArea = model.BuildingArea,
                buildingAreaSpecified = true,
                startDate = model.StartDate,
                startDateSpecified = true,
                endDate = model.EndDate,
                endDateSpecified = true,
                stage = model.Stage,
                status = true,
                statusSpecified = true,
                isCompleted = model.IsCompleted,
                isCompletedSpecified = true
            };
            var service = new UnicomService();
            var result = service.PushProjects(new[] { project });
            if (result.result.Length > 0 && !result.result[0].value.ToString().Contains("ERROR"))
            {
                try
                {
                    model.RegisterCode = result.result[0].value.ToString();
                    var unicomProject = new T_UnicomProject
                    {
                        StatId = model.StatId,
                        UnicomCode = model.RegisterCode,
                        Stopped = false
                    };
                    DbContext.T_UnicomProject.Add(unicomProject);
                    DbContext.SaveChanges();
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("RegisterError", "注册工程信息成功，但保存至服务器时遇到异常，请记录工程信息并提供给管理员手动添加。");
                    LoadParams(model);
                    LogService.Instance.Error($"添加联通工程信息失败，联通注册编码：{model.RegisterCode},工地ID{model.StatId}", ex);
                    return View(model);
                }
                ModelState.AddModelError("RegisterError", "注册工程信息成功。");
                LoadParams(model);
                return View(model);
            }
            foreach (var entry in result.result)
            {
                ModelState.AddModelError("RegisterError", entry.value.ToString());
            }
            LoadParams(model);
            return View(model);
        }

        public ActionResult UnicomCancel(int projectId)
        {
            var stat = DbContext.T_UnicomProject.FirstOrDefault(p => p.StatId == projectId);
            if (stat == null) return Json("failed", JsonRequestBehavior.AllowGet);
            stat.Stopped = true;
            try
            {
                DbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                LogService.Instance.Error("注销联通平台异常。", ex);
                return Json("failed", JsonRequestBehavior.AllowGet);
            }
            return Json("success", JsonRequestBehavior.AllowGet);
        }

        public ActionResult UniComDeviceManage()
        {
            return View();
        }

        public ActionResult UnicomDeviceTable(TablePost post)
        {
            var regedDevs = DbContext.T_UnicomDevice.ToList();

            var count = DbContext.T_Devs.Count();

            var devsInfo = new List<UnicomDeviceManageModel>();

            foreach (var dev in DbContext.T_Devs.OrderBy(d => d.Id).Skip(post.offset).Take(post.limit))
            {
                var regDev = regedDevs.FirstOrDefault(r => r.DevId == dev.Id);
                var stat = DbContext.T_Stats.First(p => p.Id.ToString() == dev.StatId);
                var model = new UnicomDeviceManageModel
                {
                    Id = dev.Id,
                    StatName = stat.StatName,
                    DeviceName = dev.DevCode,
                    UnicomCode = regDev == null ? string.Empty : regDev.UnicomCode,
                    Stopped = regDev == null || regDev.OnCalc
                };
                devsInfo.Add(model);
            }

            return Json(new
            {
                total = count,
                rows = devsInfo
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult UnicomDevice(int deviceId)
        {
            var dev = DbContext.T_Devs.FirstOrDefault(d => d.Id == deviceId);
            if (dev == null) return null;
            var stat = DbContext.T_Stats.First(s => s.Id.ToString() == dev.StatId);
            var model = new UnicomPlatformDeviceModel
            {
                Name = dev.DevCode,
                DevId = dev.Id,
                UnicomName = string.Empty,
                IpAddr = string.Empty,
                MacAddr = string.Empty,
                Port = string.Empty,
                Version = "YCZY-V3.0.0.0",
                ProjectCode = string.Empty,
                Longitude = $"{stat.Longitude}",
                Latitude = $"{stat.Latitude}",
                StartDate = dev.StartTime,
                EndDate = dev.EndTime,
                InstallDate = dev.StartTime,
                OnlineStatus = true,
                VideoUrl = string.Empty,
                IsTransfer = true
            };

            LoadDeviceParams(model);
            return View(model);
        }

        [HttpPost]
        public ActionResult UnicomDevice(UnicomPlatformDeviceModel model)
        {
            var device = new emsDevice
            {
                code = model.Code,
                name = model.UnicomName,
                ipAddr = model.IpAddr,
                macAddr = model.MacAddr,
                port = model.Port,
                version = model.Version,
                projectCode = model.ProjectCode,
                longitude = model.Longitude,
                latitude = model.Latitude,
                startDate = model.StartDate,
                startDateSpecified = true,
                endDate = model.EndDate,
                endDateSpecified = true,
                installDate = model.InstallDate,
                installDateSpecified = true,
                onlineStatus = model.OnlineStatus,
                onlineStatusSpecified = true,
                videoUrl = model.VideoUrl
            };
            var service = new UnicomService();
            var result = service.PushDevices(new[] { device });
            if (result.result.Length > 0 && !result.result[0].value.ToString().Contains("ERROR"))
            {
                try
                {
                    var stat = DbContext.T_UnicomProject.First(s => s.UnicomCode == model.ProjectCode);
                    model.Code = result.result[0].key.ToString();
                    var unicomDevice = new T_UnicomDevice
                    {
                        DevId = model.DevId,
                        StatId = stat.StatId,
                        UnicomCode = model.Code,
                        OnCalc = true,
                        Max = model.TpMax,
                        Min = model.TpMin
                    };
                    DbContext.T_UnicomDevice.Add(unicomDevice);
                    DbContext.SaveChanges();
                }
                catch (Exception ex)
                {
                    LogService.Instance.Error("注册联通平台设备信息异常", ex);
                    ModelState.AddModelError("RegisterError", "注册设备信息成功，但保存至服务器时遇到异常，请记录工程信息并提供给管理员手动添加。");
                    LoadDeviceParams(model);
                    return View(model);
                }
            }

            foreach (var entry in result.result)
            {
                ModelState.AddModelError("RegisterError", entry.value.ToString());
            }
            LoadDeviceParams(model);
            return View(model);
        }

        public ActionResult UnicomDeviceCancel(int deviceId)
        {
            var dev = DbContext.T_UnicomDevice.FirstOrDefault(p => p.DevId == deviceId);
            if (dev == null) return Json("failed", JsonRequestBehavior.AllowGet);
            dev.OnCalc = false;
            try
            {
                DbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                LogService.Instance.Error("注销设备联通平台异常。", ex);
                return Json("failed", JsonRequestBehavior.AllowGet);
            }
            return Json("success", JsonRequestBehavior.AllowGet);
        }

        private static void LoadParams(UnicomPlatformRegisterModel model)
        {
            var service = new UnicomService();
            model.Districts = service.PullDistrict(null).Select(r => new UnicomPlatformParams
            {
                Code = r.code,
                Name = r.name
            }).ToList();
            model.ProjectTypes = service.PullProjectType().Select(r => new UnicomPlatformParams
            {
                Code = $"{r.code}",
                Name = r.name
            }).ToList();
            model.ProjectCategories = service.PullProjectCategory().Select(r => new UnicomPlatformParams
            {
                Code = $"{r.code}",
                Name = r.name
            }).ToList();
            model.ProjectPeriods = service.PullProjectPeriod().Select(r => new UnicomPlatformParams
            {
                Code = $"{r.code}",
                Name = r.name
            }).ToList();
            model.Regions = service.PullRegion().Select(r => new UnicomPlatformParams
            {
                Code = $"{r.code}",
                Name = r.name
            }).ToList();
        }

        private void LoadDeviceParams(UnicomPlatformDeviceModel model)
        {
            model.Projects = DbContext.T_UnicomProject.Select(p => new UnicomPlatformParams
            {
                Code = p.UnicomCode,
                Name = DbContext.T_Stats.FirstOrDefault(s => s.Id == p.StatId).StatName
            }).ToList();
        }
    }
}