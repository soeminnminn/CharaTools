using System;
using System.Globalization;
using System.Windows.Data;

namespace CharaTools
{
    [ValueConversion(typeof(bool), typeof(bool))]
    public class ReverseBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool)
                return (bool)value == false;
            return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool)
                return (bool)value == false;
            return false;
        }
    }
}
