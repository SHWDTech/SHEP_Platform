using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using Newtonsoft.Json;
using VehicleDustMonitor.Xamarin.application;
using VehicleDustMonitor.Xamarin.Component;
using VehicleDustMonitor.Xamarin.Model;

namespace VehicleDustMonitor.Xamarin.activity
{
    [Activity(Label = "卫东车载扬尘监控", Icon = "@drawable/icon", MainLauncher = true)]
    public class SplashActivity : AppCompatActivity
    {
        private StartHandler _startHandler;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_splash);
            _startHandler = new StartHandler(this);
        }

        protected override void OnStart()
        {
            base.OnStart();

            ApiManager.GetVersionCode(new HttpResponseHandler
            {
                OnResponse = args =>
                {
                    SharedData.VersionInfo = JsonConvert.DeserializeObject<VehicleAndroidVersionInfo>(args.Response);
                    if (!IsAuthenticated())
                    {
                        var intent = new Intent(this, typeof(LoginActivity));
                        StartActivity(intent);
                        Finish();
                    }
                    else
                    {
                        _startHandler.SendEmptyMessageDelayed(0, 2000);
                    }
                }
            });
        }

        public void GoMain()
        {
            var intent = new Intent(this, typeof(MainActivity));
            StartActivity(intent);
            Finish();
        }

        private bool IsAuthenticated()
        {
            var preferences = GetSharedPreferences(nameof(VehicleDustMonitor), FileCreationMode.Private);
            var authed = preferences.GetBoolean("Authenticated", false);
            if (!authed) return false;

            var deviceName = preferences.GetString("SavedDeviceName", string.Empty);
            if (string.IsNullOrWhiteSpace(deviceName)) return false;

            var deviceNodeId = preferences.GetString("SavedDeviceNodeId", string.Empty);
            if (string.IsNullOrWhiteSpace(deviceNodeId)) return false;

            var deviceId = preferences.GetInt("SavedDeviceId", -1);
            if (deviceId <= 0) return false;

            return true;
        }
    }

    public class StartHandler : Handler
    {
        private readonly SplashActivity _activity;

        public StartHandler(SplashActivity activity)
        {
            _activity = activity;
        }

        public override void HandleMessage(Message msg)
        {
            _activity.GoMain();
        }
    }
}