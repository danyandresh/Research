using System;
using System.Windows.Controls;
using System.Windows.Data;

namespace LightPlayer.Converters
{
    public class MediaInteractionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return MediaElementProxy.BuildProxy((MediaElement)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
