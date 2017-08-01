using System;
using Android.App;
using Android.Runtime;
using VehicleDustMonitor.Xamarin.Component;

namespace VehicleDustMonitor.Xamarin.application
{
    [Application(Label = "卫东车载扬尘监控")]
    public class VehicleDustApplication : Application
    {
        public VehicleDustApplication(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {

        }

        public override void OnCreate()
        {
            base.OnCreate();
            SdUtils.Init(this);
            UpdateUtil.Init(this);
            VehicleRecordHelper.Init(this);
        }
    }

}