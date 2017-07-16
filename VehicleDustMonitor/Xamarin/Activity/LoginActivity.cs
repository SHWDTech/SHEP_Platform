using System;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using Android.Widget;
using CheeseBind;
using Newtonsoft.Json;
using VehicleDustMonitor.Xamarin.Component;
using VehicleDustMonitor.Xamarin.Model;


namespace VehicleDustMonitor.Xamarin.activity
{
    [Activity(Label = nameof(VehicleDustMonitor), MainLauncher = true, Icon = "@drawable/icon")]
    public class LoginActivity : AppCompatActivity
    {
        [BindView(Resource.Id.txtAccount)] protected AutoCompleteTextView AccountTextView { get; set; }

        [BindView(Resource.Id.textLoginPassword)] protected EditText PasswordTextView { get; set; }

        [BindView(Resource.Id.btnLogin)] protected Button BtnLoginButton { get; set; }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.activity_login);
            Cheeseknife.Bind(this);
            var authed = GetSharedPreferences(nameof(VehicleDustMonitor), FileCreationMode.Private).GetBoolean("Authenticated", false);
            if (authed)
            {
                var intent = new Intent(this, typeof(MainActivity));
                StartActivity(intent);
                Finish();
            }
        }


        [OnClick(Resource.Id.btnLogin)]
        protected void Login(object sender, EventArgs args)
        {
            BtnLoginButton.Text = "登陆中，请稍候...";
            BtnLoginButton.Enabled = false;
            ApiManager.Login(AccountTextView.Text, BaseUtils.GetHashSha256(PasswordTextView.Text), new HttpResponseHandler
            {
                OnResponse = eventArgs =>
                {
                    var result = JsonConvert.DeserializeObject<VehicleLoginResult>(eventArgs.Response);
                    if (result.LoginSuccessed)
                    {
                        var edit = GetSharedPreferences(nameof(VehicleDustMonitor), FileCreationMode.Private).Edit();
                        edit.PutBoolean("Authenticated", true);
                        edit.PutString("SavedDeviceName", result.DeviceName);
                        edit.PutInt("SavedDeviceId", result.DeviceId);
                        edit.PutString("SavedDeviceNodeId", result.DeviceNodeId);
                        edit.Commit();
                        Toast.MakeText(this, "登陆成功！", ToastLength.Short).Show();
                        Task.Factory.StartNew(() =>
                        {
                            Thread.Sleep(2000);
                            RunOnUiThread(() =>
                            {
                                var intent = new Intent(this, typeof(MainActivity));
                                StartActivity(intent);
                                Finish();
                            });
                        });
                    }
                    else
                    {
                        RunOnUiThread(LoginFailed);
                    }
                },
                OnError = eventArgs =>
                {
                    RunOnUiThread(LoginFailed);
                }
            });
            
        }

        private void LoginFailed()
        {
            BtnLoginButton.Text = "登陆";
            BtnLoginButton.Enabled = true;
            Toast.MakeText(this, "登陆失败，请重新尝试！", ToastLength.Short).Show();
        }
    }
}