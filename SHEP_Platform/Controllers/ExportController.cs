using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using SHEP_Platform.Models.Monitor;
using SHEP_Platform.ShwdResult;

namespace SHEP_Platform.Controllers
{
    public class ExportController : ControllerBase
    {
        public ActionResult PutianExport() => View();

        // GET: Export
        public ActionResult ExportReport(PuTianDataExport model)
        {
            if (!ModelState.IsValid) return null;

            if (model.DataType == ExportDataType.Hour)
            {
                return ExportHourReport(model);
            }

            return ExportDayReport(model);
        }

        public ActionResult ExportHourReport(PuTianDataExport model)
        {
            var dataSource = new List<WorkSheet>();
            var stats = WdContext.StatList.Select(s => s.Id.ToString()).ToList();
            var devs = DbContext.T_Devs.Where(d => stats.Contains(d.StatId)).ToList();
            foreach (var dev in devs)
            {
                var original = DbContext.T_ESHour.Where(obj =>
                        obj.DevId == dev.Id && obj.UpdateTime >= model.StartDateTime &&
                        obj.UpdateTime <= model.EndDateTime)
                    .OrderBy(item => item.UpdateTime).ToList();
                var sheet = new WorkSheet();
                foreach (var esHour in original)
                {
                    var row = sheet.WorkSheetDatas.NewRow();
                    row["更新时间"] = esHour.UpdateTime;
                    row["总体扬尘值(mg/m³)"] = (esHour.TP / 1000).ToString("F2");
                    row["PM2.5(mg/m³)"] = esHour.PM25 == null ? "0.00" : (esHour.PM25.Value / 1000).ToString("F2");
                    row["PM10(mg/m³)"] = esHour.PM100 == null ? "0.00" : (esHour.PM100.Value / 1000).ToString("F2");
                    row["噪音值(dB)"] = esHour.DB.ToString("F2");
                    sheet.WorkSheetDatas.Rows.Add(row);
                }
                sheet.Title = dev.DevCode;
                dataSource.Add(sheet);
            }

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

            return new ExcelResult(package, $"环境监控历史数据-{DateTime.Now:yyyy年MM月dd日}.xlsx");
        }

        private ActionResult ExportDayReport(PuTianDataExport model)
        {
            var dataSource = new List<WorkSheet>();
            var stats = WdContext.StatList.Select(s => s.Id.ToString()).ToList();
            var devs = DbContext.T_Devs.Where(d => stats.Contains(d.StatId)).ToList();
            foreach (var dev in devs)
            {
                var original = DbContext.T_ESDay.Where(obj =>
                        obj.DevId == dev.Id && obj.UpdateTime >= model.StartDateTime &&
                        obj.UpdateTime <= model.EndDateTime)
                    .OrderBy(item => item.UpdateTime).ToList();
                var sheet = new WorkSheet();
                foreach (var esHour in original)
                {
                    var row = sheet.WorkSheetDatas.NewRow();
                    row["更新时间"] = esHour.UpdateTime;
                    row["总体扬尘值(mg/m³)"] = (esHour.TP / 1000).ToString("F2");
                    row["PM2.5(mg/m³)"] = esHour.PM25 == null ? "0.00" : (esHour.PM25.Value / 1000).ToString("F2");
                    row["PM10(mg/m³)"] = esHour.PM100 == null ? "0.00" : (esHour.PM100.Value / 1000).ToString("F2");
                    row["噪音值(dB)"] = esHour.DB.ToString("F2");
                    sheet.WorkSheetDatas.Rows.Add(row);
                }
                sheet.Title = dev.DevCode;
                dataSource.Add(sheet);
            }

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

            return new ExcelResult(package, $"环境监控历史数据-{DateTime.Now:yyyy年MM月dd日}.xlsx");
        }
    }
}