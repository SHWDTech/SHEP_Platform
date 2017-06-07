namespace SHEP_Platform.Enums
{
    /// <summary>
    /// 风向
    /// </summary>
    public static class WindDirectionEnum
    {
        /// <summary>
        /// 风速值超出预定范围
        /// </summary>
        public static readonly WindDirection OutOfRange = new WindDirection {Code = "OutOfRange", Chinese = "数据超出范围"};

        /// <summary>
        /// 未知
        /// </summary>
        public static readonly WindDirection UnKnow = new WindDirection {Code= "UnKnow", Chinese = "未知"};

        /// <summary>
        /// 北
        /// </summary>
        public static readonly WindDirection North = new WindDirection {Code = "N", Chinese = "北"};

        /// <summary>
        /// 北东北
        /// </summary>
        public static readonly WindDirection NorthNorthEast = new WindDirection {Code = "NNE", Chinese = "北东北"};

        /// <summary>
        /// 东北
        /// </summary>
        public static readonly WindDirection NorthEast = new WindDirection {Code = "NE", Chinese = "东北"};

        /// <summary>
        /// 东东北
        /// </summary>
        public static readonly WindDirection EastNorthEast = new WindDirection {Code = "ENE", Chinese = "东东北"};

        /// <summary>
        /// 东
        /// </summary>
        public static readonly WindDirection East = new WindDirection {Code = "E", Chinese = "东"};

        /// <summary>
        /// 东东南
        /// </summary>
        public static readonly WindDirection EastSouthEast = new WindDirection {Code = "ESE", Chinese = "东东南"};

        /// <summary>
        /// 东南
        /// </summary>
        public static readonly WindDirection SouthEast = new WindDirection {Code = "SE", Chinese = "东南"};

        /// <summary>
        /// 南东南
        /// </summary>
        public static readonly WindDirection SouthSouthEast = new WindDirection {Code = "SSE", Chinese = "南东南"};

        /// <summary>
        /// 南
        /// </summary>
        public static readonly WindDirection South = new WindDirection {Code = "S", Chinese = "南"};

        /// <summary>
        /// 南西南
        /// </summary>
        public static readonly WindDirection SouthSouthWest = new WindDirection {Code = "SSW", Chinese = "南西南"};

        /// <summary>
        /// 西南
        /// </summary>
        public static readonly WindDirection SouthWest = new WindDirection {Code = "SW", Chinese = "西南"};

        /// <summary>
        /// 西西南
        /// </summary>
        public static readonly WindDirection WestSouthWest = new WindDirection {Code = "WSW", Chinese = "西西南"};

        /// <summary>
        /// 西
        /// </summary>
        public static readonly WindDirection West = new WindDirection {Code = "W", Chinese = "西"};

        /// <summary>
        /// 西西北
        /// </summary>
        public static readonly WindDirection WestNorthWest = new WindDirection {Code = "WNW", Chinese = "西西北"};

        /// <summary>
        /// 西北
        /// </summary>
        public static readonly WindDirection NorthWest = new WindDirection {Code = "NW", Chinese = "西北"};

        /// <summary>
        /// 北西北
        /// </summary>
        public static readonly WindDirection NorthNorthWest = new WindDirection {Code = "NNW", Chinese = "北西北"};

        /// <summary>
        /// 静风
        /// </summary>
        public static readonly WindDirection Const = new WindDirection {Code = "C", Chinese = "静风"};
    }
}

public struct WindDirection
{
    /// <summary>
    /// 风向代码
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// 风向中文值
    /// </summary>
    public string Chinese { get; set; }
}