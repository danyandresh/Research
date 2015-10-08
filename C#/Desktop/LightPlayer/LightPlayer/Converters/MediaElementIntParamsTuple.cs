using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Forms;

namespace LightPlayer.Converters
{
    public class MediaElementIntParamsTuple : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var intVal = 0;
            RoutedPropertyChangedEventArgs<double> args;
            if (values.Length > 1 && (args = values[1] as RoutedPropertyChangedEventArgs<double>) != null)
            {
                intVal = (int) args.NewValue;
            }

            var result = new Tuple<IMediaElement, int>(MediaElementProxy.BuildProxy((MediaElement)values[0]), intVal);

            return result;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}