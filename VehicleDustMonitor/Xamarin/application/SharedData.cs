using System;
using VehicleDustMonitor.Xamarin.Model;

namespace VehicleDustMonitor.Xamarin.application
{
    public class SharedData
    {
        public static DateTime LastUpdateDateTime { get; set; }

        public static double LastDustValue { get; set; }

        public static VehicleAndroidVersionInfo VersionInfo { get; set; }
    }
}