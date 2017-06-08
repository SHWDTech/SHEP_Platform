using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DatabaseModel;
using PagedList;

namespace SHEP_Platform.Models.Analysis
{
    public class AlarmDetailsViewModel
    {
        public IPagedList<AlarmFullDetail> Details { get; set; }

        public int Page { get; set; } = 1;
    }

    public class AlarmFullDetail
    {
        public int AlarmId { get; set; }

        public string StatName { get; set; }

        public string ChargeMan { get; set; }

        public string Telephone { get; set; }

        public string DevCode { get; set; }

        public DateTime AlarmDateTime { get; set; }

        public double FaultValue { get; set; }

        public bool IsReaded { get; set; }
    }

    public class AlarmProcessViewModel
    {
        public int DevId { get; set; }

        [Display(Name = "设备名称")]
        public string DevName { get; set; }

        public int StatId { get; set; }

        [Display(Name = "工地名称")]
        public string StatName { get; set; }

        public List<DeviceExceptionType> Exceptions { get; set; }

        public List<DeviceExceptionType> CheckedExceptions { get; set; }

        [Required(ErrorMessage = "必须填写执行人")]
        [Display(Name = "异常处理执行人")]
        public string ProgressMan { get; set; }

        [Required(ErrorMessage = "必须填写异常原因")]
        [Display(Name = "异常出现原因")]
        public string DeviceExceptionReason { get; set; }

        [Required(ErrorMessage = "必须填写处理结果")]
        [Display(Name = "异常处理结果")]
        public string ProgressResult { get; set; }

        public bool IsPostBack { get; set; }
    }
}