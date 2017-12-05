using System.Configuration;
using System.Threading.Tasks;
using SHEP_Platform.UnicomPlatform;

namespace SHEP_Platform.Common
{
    public class UnicomService
    {
        /// <summary>
        /// 获取联通服务实例
        /// </summary>
        /// <returns></returns>
        private static PushResourceServiceClient Instance { get; }

        /// <summary>
        /// 公司代号
        /// </summary>
        public static string VendorCode { get; }

        /// <summary>
        /// 项目简称
        /// </summary>
        /// public static string ProjectShortTitle { get; private set; }

        static UnicomService()
        {
            //ProjectShortTitle = ConfigurationManager.AppSettings[nameof(ProjectShortTitle)];
            Instance = new PushResourceServiceClient();
            VendorCode = Instance.registerVendor(ConfigurationManager.AppSettings["vendorName"]);
        }

        /// <summary>
        /// 获取工程性质信息
        /// </summary>
        /// <returns></returns>
        public emsPrjCategory[] PullProjectCategory()
            => Instance.pullProjectCategory(VendorCode);

        /// <summary>
        /// 异步获取工程性质信息
        /// </summary>
        /// <returns></returns>
        public Task<pullProjectCategoryResponse> PullProjectCategoryAsync()
            => Instance.pullProjectCategoryAsync(VendorCode);

        /// <summary>
        /// 获取工程类型信息
        /// </summary>
        /// <returns></returns>
        public emsPrjType[] PullProjectType()
            => Instance.pullProjectType(VendorCode);

        /// <summary>
        /// 异步获取工程类型信息
        /// </summary>
        /// <returns></returns>
        public Task<pullProjectTypeResponse> PullProjectTypeAsync()
            => Instance.pullProjectTypeAsync(VendorCode);

        /// <summary>
        /// 获取工程工期信息
        /// </summary>
        /// <returns></returns>
        public emsPrjPeriod[] PullProjectPeriod()
            => Instance.pullProjectPeriod(VendorCode);

        /// <summary>
        /// 异步获取工程工期信息
        /// </summary>
        /// <returns></returns>
        public Task<pullProjectPeriodResponse> PullProjectPeriodAsync()
            => Instance.pullProjectPeriodAsync(VendorCode);

        /// <summary>
        /// 新增（更新）设备信息
        /// </summary>
        /// <param name="emsDeviceList"></param>
        /// <returns></returns>
        public resultData PushDevices(emsDevice[] emsDeviceList)
            => Instance.pushDevices(VendorCode, emsDeviceList);

        /// <summary>
        /// 异步新增（更新）设备信息
        /// </summary>
        /// <param name="emsDeviceList"></param>
        /// <returns></returns>
        public Task<pushDevicesResponse1> PushDevicesAsync(emsDevice[] emsDeviceList)
            => Instance.pushDevicesAsync(VendorCode, emsDeviceList);

        /// <summary>
        /// 新增小时均值数据
        /// </summary>
        /// <param name="emsDataList"></param>
        /// <returns></returns>
        public resultData PushHourlyData(emsData[] emsDataList)
            => Instance.pushHourlyData(VendorCode, emsDataList);

        /// <summary>
        /// 异步新增小时均值数据
        /// </summary>
        /// <param name="emsDataList"></param>
        /// <returns></returns>
        public Task<pushHourlyDataResponse1> PushHourlyDataAsync(emsData[] emsDataList)
            => Instance.pushHourlyDataAsync(VendorCode, emsDataList);

        /// <summary>
        /// 新增日均值数据
        /// </summary>
        /// <param name="emsDataList"></param>
        /// <returns></returns>
        public resultData PushDailyData(emsData[] emsDataList)
            => Instance.pushDailyData(VendorCode, emsDataList);

        /// <summary>
        /// 异步新增日均值数据
        /// </summary>
        /// <param name="emsDataList"></param>
        /// <returns></returns>
        public Task<pushDailyDataResponse1> PushDailyDataAsync(emsData[] emsDataList)
            => Instance.pushDailyDataAsync(VendorCode, emsDataList);

        /// <summary>
        /// 新增实时数据
        /// </summary>
        /// <param name="emsDataList"></param>
        /// <returns></returns>
        public resultData PushRealTimeData(emsData[] emsDataList)
            => Instance.pushRealTimeData(VendorCode, emsDataList);

        /// <summary>
        /// 异步新增实时数据
        /// </summary>
        /// <param name="emsDataList"></param>
        /// <returns></returns>
        public Task<pushRealTimeDataResponse1> PushRealTimeDataAsync(emsData[] emsDataList)
            => Instance.pushRealTimeDataAsync(VendorCode, emsDataList);

        /// <summary>
        /// 更新设备状态信息
        /// </summary>
        /// <param name="emsDeviceList"></param>
        /// <returns></returns>
        public resultData PushDeviceStatus(emsDevice[] emsDeviceList)
            => Instance.pushDeviceStatus(VendorCode, emsDeviceList);

        /// <summary>
        /// 异步更新设备状态信息
        /// </summary>
        /// <param name="emsDeviceList"></param>
        /// <returns></returns>
        public Task<pushDeviceStatusResponse1> PushDeviceStatusAsync(emsDevice[] emsDeviceList)
            => Instance.pushDeviceStatusAsync(VendorCode, emsDeviceList);

        /// <summary>
        /// 获取区域信息
        /// </summary>
        /// <returns></returns>
        public emsRegion[] PullRegion()
            => Instance.pullRegion(VendorCode);

        /// <summary>
        /// 异步获取区域信息
        /// </summary>
        /// <returns></returns>
        public Task<pullRegionResponse> PullRegionAsync()
            => Instance.pullRegionAsync(VendorCode);

        /// <summary>
        /// 新增（更新）工地信息
        /// </summary>
        /// <param name="emsProjectList"></param>
        /// <returns></returns>
        public resultData PushProjects(emsProject[] emsProjectList)
            => Instance.pushProjects(VendorCode, emsProjectList);

        /// <summary>
        /// 异步新增（更新）工地信息
        /// </summary>
        /// <param name="emsProjectList"></param>
        /// <returns></returns>
        public Task<pushProjectsResponse1> PushProjectsAsync(emsProject[] emsProjectList)
            => Instance.pushProjectsAsync(VendorCode, emsProjectList);

        /// <summary>
        /// 更新工地状态
        /// </summary>
        /// <param name="emsProjectList"></param>
        /// <returns></returns>
        public resultData PushProjectStatus(emsProject[] emsProjectList)
            => Instance.pushProjectStatus(VendorCode, emsProjectList);

        /// <summary>
        /// 异步更新工地状态
        /// </summary>
        /// <param name="emsProjectList"></param>
        /// <returns></returns>
        public Task<pushProjectStatusResponse1> PushProjectStatusAsync(emsProject[] emsProjectList)
            => Instance.pushProjectStatusAsync(VendorCode, emsProjectList);

        /// <summary>
        /// 注册供应商
        /// </summary>
        /// <param name="vendorName"></param>
        /// <returns></returns>
        public string RegisterVendor(string vendorName)
            => Instance.registerVendor(vendorName);

        /// <summary>
        /// 异步注册供应商
        /// </summary>
        /// <param name="vendorName"></param>
        /// <returns></returns>
        public Task<registerVendorResponse1> RegisterVendorAsync(string vendorName)
            => Instance.registerVendorAsync(vendorName);

        /// <summary>
        /// 获取区县信息
        /// </summary>
        /// <param name="parentDistrict"></param>
        /// <returns></returns>
        public emsDistrict[] PullDistrict(string parentDistrict)
            => Instance.pullDistrict(VendorCode, parentDistrict);

        /// <summary>
        /// 异步获取区县信息
        /// </summary>
        /// <param name="parentDistrict"></param>
        /// <returns></returns>
        public Task<pullDistrictResponse> PullDistrictAsync(string parentDistrict)
            => Instance.pullDistrictAsync(VendorCode, parentDistrict);
    }
}