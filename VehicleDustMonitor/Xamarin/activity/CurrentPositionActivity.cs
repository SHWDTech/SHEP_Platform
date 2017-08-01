using System;
using Android.App;
using Android.OS;
using Android.Support.V7.App;
using CheeseBind;
using Com.Amap.Api.Maps2d.Model;
using VehicleDustMonitor.Xamarin.fragment;

namespace VehicleDustMonitor.Xamarin.activity
{
    [Activity(Label = nameof(CurrentPositionActivity))]
    public class CurrentPositionActivity : AppCompatActivity
    {
        private LatLng _currentLatLng;

        [OnClick(Resource.Id.back)]
        protected void Back(object sender, EventArgs args)
        {
            Finish();
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Current_Position_Map);
            Cheeseknife.Bind(this);
            var bundle = Intent.Extras;
            var lat = bundle.GetDouble("Lat", 0);
            var lng = bundle.GetDouble("Lng", 0);
            _currentLatLng = new LatLng(lat, lng);
            InitData();
        }

        private void InitData()
        {
            var bundle = Intent.Extras;
            var transaction = FragmentManager.BeginTransaction();
            var mapFragment = new CategoryMapFragment(_currentLatLng) {Arguments = bundle};
            transaction.Add(Resource.Id.map_content, mapFragment).Commit();
        }
    }
}