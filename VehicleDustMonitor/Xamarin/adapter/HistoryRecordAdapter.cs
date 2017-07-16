using System.Collections.Generic;
using Android.App;
using Android.Views;
using Android.Widget;
using VehicleDustMonitor.Xamarin.Model;

namespace VehicleDustMonitor.Xamarin.adapter
{
    class HistoryRecordAdapter : BaseAdapter<HistoryRecordItem>
    {
        readonly Activity _context;

        private List<HistoryRecordItem> Items { get; }

        public HistoryRecordAdapter(Activity context, List<HistoryRecordItem> items)
        {
            _context = context;
            Items = items;
        }


        public override Java.Lang.Object GetItem(int position)
        {
            return position;
        }

        public override HistoryRecordItem this[int position] => Items[position];

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = Items[position];
            var view = _context.LayoutInflater.Inflate(Resource.Layout.item_vehicle_record, null);
            view.FindViewById<TextView>(Resource.Id.recordName).Text = item.RecordName;
            view.FindViewById<TextView>(Resource.Id.recordComment).Text = item.Comment;
            view.FindViewById<TextView>(Resource.Id.startDateTime).Text = $"{item.StartDateTime:yyyy-MM-dd HH:mm:ss}";
            view.FindViewById<TextView>(Resource.Id.endDateTime).Text = $"{item.EndDateTime:yyyy-MM-dd HH:mm:ss}";
            view.FindViewById<TextView>(Resource.Id.average).Text = $"{item.AverageValue}";
            return view;
        }

        //Fill in cound here, currently 0
        public override int Count => Items.Count;
    }

    class HistoryRecordAdapterViewHolder : Java.Lang.Object
    {
        //Your adapter views to re-use
        //public TextView Title { get; set; }
    }
}