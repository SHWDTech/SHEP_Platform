using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SHEP_Platform.Models.Admin
{
    public class StatManageViewModel
    {
        public StatManageViewModel()
        {
            StatList = new List<T_Stats>();
            PageIndex = 0;
            PageSize = 10;
        }

        public List<T_Stats> StatList { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }
    }

    public class StatEditViewModel
    {
        public int Id { get; set; }

        [MaxLength(25)]
        [DisplayName("监测点编码")]
        public string StatCode { get; set; }

        public int? StatCodeUp { get; set; }

        [MaxLength(25)]
        [DisplayName("监测点名称")]
        public string StatName { get; set; }

        [MaxLength(10)]
        [DisplayName("负责人")]
        public string ChargeMan { get; set; }
        
        [DataType(DataType.PhoneNumber)]
        [DisplayName("联系电话")]
        public string Telepone { get; set; }
        
        [Range(-180, 180)]
        public decimal Longitude { get; set; }

        [Range(-180, 180)]
        public decimal Latitude { get; set; }

        [MaxLength(50)]
        [DisplayName("施工单位")]
        public string Department { get; set; }

        [MaxLength(50)]
        [DisplayName("施工地址")]
        public string Address { get; set; }

        [Required]
        [DisplayName("所属区域")]
        public int Country { get; set; }

        [DisplayName("街道名称")]
        public string Street { get; set; }

        [Range(0, 100000)]
        [DisplayName("占地面积")]
        public double Square { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("开始时间")]
        public DateTime ProStartTime { get; set; }

        [Required]
        [DisplayName("施工进展")]
        public byte Stage { get; set; }

        [Required]
        [DisplayName("阶段")]
        public string ProType { get; set; }

        public int? AlarmType { get; set; }

        public bool IsNew  {get; set;}

        public virtual T_AlarmType T_AlarmType { get; set; }

        [Required]
        [DisplayName("所属区域")]
        public virtual T_Country T_Country { get; set; }

        [Required]
        [DisplayName("施工进展")]
        public virtual T_Stage T_Stage { get; set; }
        public SelectList StageList { get; set; } 

        public SelectList CountryList { get; set; }

        public SelectList TypeList { get; set; }
    }

    public class DevManageViewModel
    {
        public DevManageViewModel()
        {
            DevList = new List<Devs>();
            PageIndex = 0;
            PageSize = 10;
        }

        public List<Devs> DevList { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }
    }

    public class Devs
    {
        public int Id { get; set; }
        public string StatName { get; set; }

        public string DevCode { get; set; }

        public string StartTime { get; set; }

        public string PreEndTime { get; set; }

        public string EndTime { get; set; }

        public string DevStatus { get; set; }

        public string VideoURL { get; set; }
    }

    public class DevEditViewModel
    {
        [Required]
        public int Id { get; set; }

        [Display(Name = "所属监测点")]
        [Required]
        public int StatId { get; set; }

        [Display(Name = "设备编号")]
        [Required]
        public string DevCode { get; set; }

        [Display(Name = "开始时间")]
        [DataType(DataType.Date)]
        public DateTime StartTime { get; set; }

        [Display(Name = "预计结束时间")]
        [DataType(DataType.Date)]
        public DateTime PreEndTime { get; set; }

        [Display(Name = "结束时间")]
        [DataType(DataType.Date)]
        public DateTime EndTime { get; set; }

        [Display(Name = "设备是否启用")]
        [Required]
        public byte DevStatus { get; set; }

        [Display(Name = "视频地址")]
        public string VideoUrl { get; set; }

        public bool IsNew { get; set; }

        public SelectList StatList { get; set; } 

        public SelectList StatusLIst { get; set; }
    }
}
