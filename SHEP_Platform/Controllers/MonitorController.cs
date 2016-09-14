using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web.Mvc;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using SHEP_Platform.Enum;
using SHEP_Platform.Models.Monitor;
using SHEP_Platform.ShwdResult;

// ReSharper disable PossibleInvalidOperationException

namespace SHEP_Platform.Controllers
{
    public class MonitorController : ControllerBase
    {
        public MonitorController()
        {
            WdContext.SiteMapMenu.ControllerMenu.Name = "查询导出";
            WdContext.SiteMapMenu.ControllerMenu.LinkAble = false;
        }

        // GET: Monitor
        public ActionResult ActualStatus(string id)
        {
            WdContext.SiteMapMenu.ActionMenu.Name = "各工程当前情况";
            var dict = new Dictionary<object, StatHourInfo>();
            var cameraurl = string.Empty;
            foreach (var stat in WdContext.StatList)
            {
                var hour = DateTime.Now.AddHours(-1);
                var value =
                    DbContext.T_ESHour.First(item => item.StatId == stat.Id && item.UpdateTime.Hour > hour.Hour);
                var current = DbContext.T_ESMin.OrderByDescending(item => item.UpdateTime).First(obj => obj.StatId == stat.Id);

                var info = new StatHourInfo
                {
                    Hour = value,
                    Current = current
                };

                dict.Add(stat, info);
            }

            ViewBag.StatDict = dict;
            var defaultId = -1;
            var defaultName = "null";
            if (!string.IsNullOrWhiteSpace(id))
            {
                defaultId = int.Parse(id);
                defaultName = WdContext.StatList.First(stat => stat.Id == defaultId).StatName;
            }
            else if (WdContext.StatList.Count != 0)
            {
                defaultId = WdContext.StatList[0].Id;
                defaultName = WdContext.StatList.First(stat => stat.Id == defaultId).StatName;
            }

            var devs = DbContext.T_Devs.Where(dev => dev.StatId == defaultId.ToString()).ToList();
            if (devs.Count > 0)
            {
                cameraurl = devs[0].VideoURL;
            }

            ViewBag.defaultId = defaultId;
            ViewBag.defaultName = defaultName;
            ViewBag.StatViewUrl = cameraurl;

            return DynamicView("ActualStatus");
        }

        public ActionResult HistoryChange()
        {
            WdContext.SiteMapMenu.ActionMenu.Name = "各工程历史污染物变化情况";
            var dict = new Dictionary<object, StatHourInfo>();
            foreach (var stat in WdContext.StatList)
            {
                var hour = DateTime.Now.AddHours(-1);
                var value =
                    DbContext.T_ESHour.FirstOrDefault(item => item.StatId == stat.Id && item.UpdateTime.Hour > hour.Hour);
                var current = DbContext.T_ESMin.OrderByDescending(item => item.UpdateTime).FirstOrDefault(obj => obj.StatId == stat.Id);

                var info = new StatHourInfo
                {
                    Hour = value,
                    Current = current
                };

                dict.Add(stat, info);
            }

            ViewBag.StatDict = dict;

            var defaultId = -1;
            if (WdContext.StatList.Count > 0)
            {
                defaultId = WdContext.StatList[0].Id;
            }
            ViewBag.defaultId = defaultId;

            return DynamicView("HistoryChange");
        }

        public ActionResult ScheduleCompare()
        {
            WdContext.SiteMapMenu.ActionMenu.Name = "按工期进行综合对比";
            var basic = WdContext.StatList.Where(stat => stat.Stage == (int)Stage.Basic).Select(obj => obj.Id).ToList();
            var structure =
                WdContext.StatList.Where(stat => stat.Stage == (int)Stage.Structure).Select(obj => obj.Id).ToList();

            var startDate = DateTime.Now.AddMonths(-1);
            var basicSource = DbContext.T_ESDay.Where(obj => basic.Contains(obj.StatId) && obj.UpdateTime > startDate)
                .ToList()
                .Select(item => new
                {
                    TP = double.Parse((item.TP / 1000).ToString("f2")),
                    DB = double.Parse(item.DB.ToString("f2")),
                    UpdateTime = item.UpdateTime.ToString("yyyy-MM-dd")
                });
            var structureSource = DbContext.T_ESDay.Where(
                obj => structure.Contains(obj.StatId) && obj.UpdateTime > startDate).ToList()
                .Select(item => new
                {
                    TP = double.Parse((item.TP / 1000).ToString("f2")),
                    DB = double.Parse(item.DB.ToString("f2")),
                    UpdateTime = item.UpdateTime.ToString("yyyy-MM-dd")
                });

            var dict = new Dictionary<string, object> { { "basic", basicSource }, { "structure", structureSource } };

            var basicList = basicSource.ToList();
            var structureList = structureSource.ToList();
            var model = new ScheduleCompareViewModel
            {
                BasicAvgTp = basicList.Any() ? basicList.Average(i => i.TP).ToString("f2") : "暂无数据",
                BasicAvgDb = basicList.Any() ? basicList.Average(i => i.DB).ToString("f2") : "暂无数据",
                StructureTp = structureList.Any() ? structureList.Average(i => i.TP).ToString("f2") : "暂无数据",
                StructureDb = structureList.Any() ? structureList.Average(i => i.DB).ToString("f2") : "暂无数据"
            };

            ViewBag.Dict = JsonConvert.SerializeObject(dict);

            return DynamicView("ScheduleCompare", model);
        }

        /// <summary>
        /// 数据导出
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DataExport()
        {
            WdContext.SiteMapMenu.ActionMenu.Name = "历史数据表格导出";

            var model = new DataExport();
            foreach (var statse in WdContext.StatList)
            {
                model.StatList.Add(statse);
                model.DevList.AddRange(DbContext.T_Devs.Where(obj => obj.StatId == statse.Id.ToString()).ToList());
            }

            return DynamicView("DataExport", model);
        }

        public ActionResult ExportHistoryDataSheet()
        {
            string[] stats = null;
            if (!string.IsNullOrWhiteSpace(Request["stat"]))
            {
                stats = Request["stat"]?.Split(',');
            }

            string[] devs = null;
            if (!string.IsNullOrWhiteSpace(Request["devs"]))
            {
                devs = Request["devs"]?.Split(',');
            }
            var startDate = DateTime.Parse(Request["startDate"]);
            var endDate = DateTime.Parse(Request["endDate"]);

            var dataSource = new List<WorkSheet>();
            if (stats != null)
            {
                foreach (var stat in stats)
                {
                    var original = DbContext.T_ESMin.Where(obj => obj.StatId.ToString() == stat && obj.UpdateTime < endDate && obj.UpdateTime > startDate)
                        .OrderBy(item => item.UpdateTime).ToList();

                    var sheet = new WorkSheet();
                    foreach (var esMin in original)
                    {
                        var row = sheet.WorkSheetDatas.NewRow();
                        row["更新时间"] = esMin.UpdateTime.Value;
                        row["总体扬尘值(mg/m³)"] = (esMin.TP / 1000).ToString("F2");
                        row["PM2.5(mg/m³)"] = (esMin.PM25.Value / 1000).ToString("F2");
                        row["PM10(mg/m³)"] = (esMin.PM100.Value / 1000).ToString("F2");
                        row["噪音值(dB)"] = esMin.DB.ToString("F2");
                        sheet.WorkSheetDatas.Rows.Add(row);
                    }

                    var currentStat = DbContext.T_Stats.First(obj => obj.Id.ToString() == stat);
                    sheet.Title = currentStat.StatName;
                    dataSource.Add(sheet);
                }
            }

            if (devs != null)
            {
                foreach (var dev in devs)
                {
                    var original = DbContext.T_ESMin.Where(obj => obj.DevId.ToString() == dev && obj.UpdateTime < endDate && obj.UpdateTime > startDate)
                        .OrderBy(item => item.UpdateTime).ToList();

                    var sheet = new WorkSheet();
                    foreach (var esMin in original)
                    {
                        var row = sheet.WorkSheetDatas.NewRow();
                        row["更新时间"] = esMin.UpdateTime.Value;
                        row["总体扬尘值(mg/m³)"] = (esMin.TP / 1000).ToString("F2");
                        row["PM2.5(mg/m³)"] = (esMin.PM25.Value / 1000).ToString("F2");
                        row["PM10(mg/m³)"] = (esMin.PM100.Value / 1000).ToString("F2");
                        row["噪音值(dB)"] = esMin.DB.ToString("F2");
                        sheet.WorkSheetDatas.Rows.Add(row);
                    }

                    var currentStat = DbContext.T_Devs.First(obj => obj.Id.ToString() == dev);
                    sheet.Title = currentStat.DevCode;
                    dataSource.Add(sheet);
                }
            }

            if (dataSource.Count <= 0) return null;

            var package = new ExcelPackage();
            foreach (var workSheet in dataSource)
            {
                var currentSheet = package.Workbook.Worksheets.Add(workSheet.Title);
                currentSheet.Column(1).Width = 35.0;
                currentSheet.Column(1).Style.Numberformat.Format = "yyyy-MM-dd hh:mm:ss";
                currentSheet.Column(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                currentSheet.Column(1).Style.Font.Size = 14;
                currentSheet.Column(2).Width = 30.0;
                currentSheet.Column(2).Style.Font.Size = 14;
                currentSheet.Column(2).Style.Numberformat.Format = "0.00";
                currentSheet.Column(3).Width = 24.0;
                currentSheet.Column(3).Style.Font.Size = 14;
                currentSheet.Column(3).Style.Numberformat.Format = "0.00";
                currentSheet.Column(4).Width = 24.0;
                currentSheet.Column(4).Style.Font.Size = 14;
                currentSheet.Column(4).Style.Numberformat.Format = "0.00";
                currentSheet.Column(5).Width = 24.0;
                currentSheet.Column(5).Style.Font.Size = 14;
                currentSheet.Column(5).Style.Numberformat.Format = "0.00";

                using (var range = currentSheet.Cells["A1:E1"])
                {
                    range.Merge = true;
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    range.Value = workSheet.Title;
                    range.Style.Font.Size = 24;
                }

                using (var range = currentSheet.Cells["A2:E2"])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(26, 188, 156));
                    range.Style.Font.Color.SetColor(Color.White);
                }

                currentSheet.View.FreezePanes(3, 6);
                currentSheet.Cells["A2"].LoadFromDataTable(workSheet.WorkSheetDatas, true);
            }

            return new ExcelResult(package, $"环境监控历史数据-{DateTime.Now.ToLongDateString()}.xlsx");
        }

        [HttpPost]
        public ActionResult DataExportResult()
        {
            var stats = Request["stat"]?.Split(',');
            var devs = Request["devs"]?.Split(',');

            var model = new DataExport
            {
                StartDate = Request["startDate"],
                EndDate = Request["endDate"]
            };
            if (stats != null)
            {
                foreach (var stat in stats)
                {
                    model.StatList.Add(WdContext.StatList.FirstOrDefault(obj => obj.Id.ToString() == stat));
                }
            }

            if (devs != null)
            {
                foreach (var dev in devs)
                {
                    model.DevList.Add(DbContext.T_Devs.FirstOrDefault(obj => obj.Id.ToString() == dev));
                }
            }

            return PartialView(model);
        }

        public ActionResult StatView()
        {
            WdContext.SiteMapMenu.ActionMenu.Name = "工程实时状况查看";

            var stats = WdContext.StatList;
            var devs = new List<T_Devs>();
            var cameras = new List<T_Camera>();
            foreach (var stat in stats)
            {
                devs.AddRange(DbContext.T_Devs.Where(obj => obj.StatId == stat.Id.ToString()));
            }

            foreach (var dev in devs)
            {
                cameras.AddRange(DbContext.T_Camera.Where(item => item.DevId == dev.Id));
            }

            ViewBag.Cameras = JsonConvert.SerializeObject(cameras);

            return DynamicView("StatView");
        }

        public ActionResult StatViewTest()
        {
            WdContext.SiteMapMenu.ActionMenu.Name = "工程实时状况查看";

            return DynamicView("StatViewTest");
        }

        public ActionResult StatTable(int id)
        {
            int totalCount;
            var dataList = GetStatDataList(id, out totalCount);

            var ret = new
            {
                total = totalCount,
                rows = dataList
            };

            return Json(ret, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DevTable(int id)
        {
            int totalCount;
            var dataList = GetDevDataList(id, out totalCount);

            var ret = new
            {
                total = totalCount,
                rows = dataList
            };

            return Json(ret, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetStatDataList(int id, out int totalCount)
        {
            var stat = WdContext.StatList.FirstOrDefault(obj => obj.Id == id);
            var limit = int.Parse(Request["limit"]);
            var offset = int.Parse(Request["offset"]);
            var startDate = DateTime.Parse(Request["start"]);
            var endDate = DateTime.Parse(Request["end"]);

            if (stat == null)
            {
                totalCount = 0;
                return null;
            }

            var total = DbContext.T_ESMin.Where(obj => obj.StatId == stat.Id && obj.UpdateTime < endDate && obj.UpdateTime > startDate)
                .OrderBy(item => item.UpdateTime);
            var esmin = total.Skip(offset)
                .Take(limit).ToList()
                .Select(obj => new
                {
                    UpdateTime = obj.UpdateTime.Value,
                    TP = (obj.TP / 1000).ToString("F2"),
                    PM25 = (obj.PM25.Value / 1000).ToString("F2"),
                    PM100 = (obj.PM100.Value / 1000).ToString("F2"),
                    DB = (obj.DB).ToString("F2")
                })
                .ToList();

            totalCount = total.Count();
            return esmin;
        }

        public dynamic GetDevDataList(int id, out int totalCount)
        {
            var dev = DbContext.T_Devs.FirstOrDefault(obj => obj.Id == id);
            var limit = int.Parse(Request["limit"]);
            var offset = int.Parse(Request["offset"]);
            var startDate = DateTime.Parse(Request["start"]);
            var endDate = DateTime.Parse(Request["end"]);

            if (dev == null)
            {
                totalCount = 0;
                return null;
            }

            var total = DbContext.T_ESMin.Where(obj => obj.DevId == dev.Id && obj.UpdateTime < endDate && obj.UpdateTime > startDate)
                .OrderBy(item => item.UpdateTime);
            var esmin = total.Skip(offset)
                .Take(limit).ToList()
                .Select(obj => new
                {
                    UpdateTime = obj.UpdateTime.Value,
                    TP = (obj.TP / 1000).ToString("F2"),
                    PM25 = (obj.PM25.Value / 1000).ToString("F2"),
                    PM100 = (obj.PM100.Value / 1000).ToString("F2"),
                    DB = (obj.DB).ToString("F2")
                })
                .ToList();

            totalCount = total.Count();
            return esmin;
        }

        public void ExportHistoryData()
        {

        }

        public ActionResult StatViewHik()
        {
            WdContext.SiteMapMenu.ActionMenu.Name = "工程实时状况查看";

            return View();
        }


        public ActionResult StatViewCircle()
        {
            WdContext.SiteMapMenu.ActionMenu.Name = "工程实时状况查看";

            return View();
        }

        public ActionResult StatViewHikSecond(string id)
        {
            WdContext.SiteMapMenu.ActionMenu.Name = "工程实时状况查看";
            ViewBag.CameraId = id;
            return View();
        }
    }
}