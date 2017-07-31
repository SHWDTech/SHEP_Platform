using MikePhil.Charting.Data;
using MikePhil.Charting.Formatter;
using MikePhil.Charting.Util;

namespace VehicleDustMonitor.Xamarin.Component
{
    public class LineChartFormatter : Java.Lang.Object, IValueFormatter
    {
        public string GetFormattedValue(float value, Entry entry, int dataSetIndex, ViewPortHandler viewPortHandler)
        {
            return $"{value:F3}";
        }
    }
}