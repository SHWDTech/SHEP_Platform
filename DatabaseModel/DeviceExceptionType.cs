using System.ComponentModel.DataAnnotations;

namespace DatabaseModel
{
    public enum DeviceExceptionType : byte
    {
        [Display(Name = "设备断线异常")]
        OfflineException = 0x00,

        [Display(Name = "颗粒物数据异常")]
        TspDataException = 0x01,

        [Display(Name = "噪音夜间超标")]
        NoiseNightExcessive = 0x02,

        [Display(Name = "颗粒物数据超标")]
        TspExcessive = 0x03
    }
}
