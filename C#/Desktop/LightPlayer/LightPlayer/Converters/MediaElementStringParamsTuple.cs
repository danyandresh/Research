using System;
using System.Windows.Controls;
using System.Windows.Data;

namespace LightPlayer.Converters
{
    public class MediaElementStringParamsTuple : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var result = new Tuple<IMediaElement, string>(MediaElementProxy.BuildProxy((MediaElement)values[0]), values[1] as string);

            return result;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
