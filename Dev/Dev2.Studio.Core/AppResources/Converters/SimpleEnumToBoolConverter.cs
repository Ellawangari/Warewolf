#pragma warning disable
using System;
using System.Globalization;
using System.Windows.Data;

namespace Dev2.AppResources.Converters
{
    public class SimpleEnumToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => value.Equals(parameter);

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => value.Equals(true) ? parameter : Binding.DoNothing;
    }
}