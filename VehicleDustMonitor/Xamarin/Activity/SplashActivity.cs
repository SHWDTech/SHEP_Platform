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
                    var authed = GetSharedPreferences(nameof(VehicleDustMonitor), FileCreationMode.Private).GetBoolean("Authenticated", false);
                    if (!authed)
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