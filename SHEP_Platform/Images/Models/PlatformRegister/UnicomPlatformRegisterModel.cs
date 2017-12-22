using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SHEP_Platform.Models.PlatformRegister
{
    public class UnicomPlatformRegisterModel
    {
        [Display(Name = "工地ID")]
        public int StatId { get; set; }

        [Display(Name = "注册编码")]
        public string RegisterCode { get; set; }

        [Display(Name = "工地名称")]
        public string Name { get; set; }

        [Display(Name = "所属区县")]
        public string District { get; set; }

        [Display(Name = "工程类型")]
        public int ProjectType { get; set; }

        [Display(Name = "工程性质")]
        public int ProjectCategory { get; set; }

        [Display(Name = "工程工期")]
        public int ProjectPeriod { get; set; }

        [Display(Name = "所在区域")]
        public int Region { get; set; }

        [Display(Name = "街道名称")]
        public string Street { get; set; }

        [Display(Name = "工程经度")]
        public string Longitude { get; set; }

        [Display(Name = "工程纬度")]
        public string Latitude { get; set; }

        [Display(Name = "承包商")]
        public string Contractors { get; set; }

        [Display(Name = "总负责人")]
        public string Superintendent { get; set; }

        [Display(Name = "联系电话")]
        public string Telephone { get; set; }

        [Display(Name = "工程地址")]
        public string Address { get; set; }

        [Display(Name = "占地面积")]
        public float SiteArea { get; set; }

        [Display(Name = "建筑面积")]
        public float BuildingArea { get; set; }

        [Display(Name = "启动时间")]
        public DateTime StartDate { get; set; }

        [Display(Name = "结束时间")]
        public DateTime EndDate { get; set; }

        [Display(Name = "工地工期")]
        public string Stage { get; set; }

        [Display(Name = "是否完工")]
        public bool IsCompleted { get; set; }

        [Display(Name = "工程状态")]
        public bool Status { get; set; }

        public List<UnicomPlatformParams> Districts { get; set; }

        public List<UnicomPlatformParams> ProjectTypes { get; set; }

        public List<UnicomPlatformParams> ProjectCategories { get; set; }

        public List<UnicomPlatformParams> ProjectPeriods { get; set; }

        public List<UnicomPlatformParams> Regions { get; set; }
    }

    public class UnicomPlatformParams
    {
        public string Code { get; set; }

        public string Name { get; set; }
    }

    public class UnicomPlatformDeviceModel
    {
        public int DevId { get; set; }

        [Display(Name = "注册编号")]
        public string Code { get; set; }

        [Display(Name = "设备名称")]
        public string Name { get; set; }

        [Display(Name = "上传名称")]
        public string UnicomName { get; set; }

        [Display(Name = "设备IP地址")]
        public string IpAddr { get; set; }

        [Display(Name = "设备MAC地址")]
        public string MacAddr { get; set; }

        [Display(Name = "设备端口号")]
        public string Port { get; set; }

        [Display(Name = "设备版本")]
        public string Version { get; set; }

        [Display(Name = "工程编号")]
        public string ProjectCode { get; set; }

        [Display(Name = "设备经度")]
        public string Longitude { get; set; }

        [Display(Name = "设备纬度")]
        public string Latitude { get; set; }

        [Display(Name = "开始时间")]
        public DateTime StartDate { get; set; }

        [Display(Name = "结束时间")]
        public DateTime EndDate { get; set; }

        [Display(Name = "安装时间")]
        public DateTime InstallDate { get; set; }

        [Display(Name = "在线状态")]
        public bool OnlineStatus { get; set; }

        [Display(Name = "视频地址")]
        public string VideoUrl { get; set; }

        [Display(Name = "是否开启上传")]
        public bool IsTransfer { get; set; }

        public bool IsHandlerValues { get; set; }

        public double TpMax { get; set; } = 50;

        public double TpMin { get; set; } = 350;

        public List<UnicomPlatformParams> Projects { get; set; }
    }

    public class UnicomDeviceManageModel
    {
        public int Id { get; set; }

        public string StatName { get; set; }

        public string DeviceName { get; set; }

        public string UnicomCode { get; set; }

        public bool Stopped { get; set; }
    }
}