using System;
using System.Text;
using DatabaseModel;

namespace DeviceExceptionChecker.MessageWarpper
{
    public class DustMessageWarpper
    {
        public static StringBuilder WarpperMessage(DeviceExceptionType type, string devName, string statName, object data = null)
        {
            switch (type)
            {
                case DeviceExceptionType.OfflineException:
                    return OfflineExceptionMessage(devName, statName);
                case DeviceExceptionType.TspDataException:
                    return TspDataExceptionMessage(devName, statName, data);
                default:
                    return null;
            }
        }

        private static StringBuilder OfflineExceptionMessage(string devName, string statName)
        {
            var builder = new StringBuilder($"\r\n【***扬尘设备断线异常提醒***】\r\n扬尘设备已断线，工地名：{statName}，设备名：{devName}，异常时间：{DateTime.Now:yyyy-MM-dd HH:mm:ss}\r\n【上海卫东】");
            return builder;
        }

        private static StringBuilder TspDataExceptionMessage(string devName, string statName, object data)
        {
            var builder = new StringBuilder($"\r\n【***扬尘设备颗粒物异常提醒***】\r\n扬尘设备颗粒物数据异常，工地名：{statName}，设备名：{devName}，异常值：{data}，异常时间：{DateTime.Now:yyyy-MM-dd HH:mm:ss}\r\n【上海卫东】");
            return builder;
        }
    }
}
