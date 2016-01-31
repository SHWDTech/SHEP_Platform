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
        public System.DateTime ProStartTime { get; set; }

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
}
