using System;
using System.Windows.Data;

namespace LightPlayer.Converters
{
    public class MultiValueParams : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values.Length == 2)
            {
                return new Tuple<dynamic, dynamic>(values[0], values[1]);
            }

            return values;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
