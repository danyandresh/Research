using System;
using System.Windows.Data;

namespace LightPlayer.Converters
{
    public class TimeConverterFromSeconds : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var seconds = 0.0;
            if (value != null)
            {
                double.TryParse(value.ToString(), out seconds);
            }

            var timespan = TimeSpan.FromSeconds(seconds);

            var format = timespan.Hours > 0
                ? @"hh\:mm\:ss"
                : @"mm\:ss";
            var result = timespan.ToString(format);

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}