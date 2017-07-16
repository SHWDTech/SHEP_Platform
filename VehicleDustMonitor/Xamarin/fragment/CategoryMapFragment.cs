using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Com.Amap.Api.Maps2d;
using Com.Amap.Api.Maps2d.Model;

namespace VehicleDustMonitor.Xamarin.fragment
{
    public class CategoryMapFragment : Fragment, AMap.IOnMarkerClickListener, AMap.IOnInfoWindowClickListener, AMap.IInfoWindowAdapter
    {
        private MapView _mapView;

        private AMap _map;

        public LatLng CurrentLatLng { get; set; }

        public CategoryMapFragment(LatLng latLng)
        {
            CurrentLatLng = latLng;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            return inflater.Inflate(Resource.Layout.fragment_category_map, container, false);
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);
            _mapView = (MapView)View.FindViewById(Resource.Id.map);
            _mapView.OnCreate(savedInstanceState);
            _map = _mapView.Map;
            _map.SetOnInfoWindowClickListener(this);
            _map.SetInfoWindowAdapter(this);
            _map.SetOnMarkerClickListener(this);


            var markerOption = new MarkerOptions();
            markerOption.InvokePosition(CurrentLatLng);
            var textView = new TextView(Activity);
            markerOption.Draggable(true);
            markerOption.InvokeTitle(string.Empty);
            markerOption.InvokeIcon(BitmapDescriptorFactory.FromView(textView));
            _map.AddMarker(markerOption);
            if (CurrentLatLng != null)
            {
                var bounds = new LatLngBounds.Builder().Include(CurrentLatLng).Build();
                _map.MoveCamera(CameraUpdateFactory.NewLatLngBounds(bounds, 10));
            }
        }

        public bool OnMarkerClick(Marker p0)
        {
            return false;
        }

        public void OnInfoWindowClick(Marker p0)
        {

        }

        public View GetInfoContents(Marker p0)
        {
            return null;
        }

        public View GetInfoWindow(Marker p0)
        {
            return null;
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            _mapView.OnDestroy();
        }

        public override void OnResume()
        {
            base.OnResume();
            _mapView.OnResume();
        }

        public override void OnPause()
        {
            base.OnPause();
            _mapView.OnPause();
        }

        public override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);
            _mapView.OnSaveInstanceState(outState);
        }

    }
}

public class CloseViewClickListener : Java.Lang.Object, View.IOnClickListener
{
    private readonly Marker _marker;

    public CloseViewClickListener(Marker mapMarker)
    {
        _marker = mapMarker;
    }

    public void OnClick(View v)
    {
        _marker.HideInfoWindow();
    }
}

public class InfoLayoutClickListener : Java.Lang.Object, View.IOnClickListener
{

    public void OnClick(View v)
    {

    }
}