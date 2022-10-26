using System;
using System.Globalization;
using System.Windows.Data;

namespace CharaTools
{
    [ValueConversion(typeof(int), typeof(bool))]
    public class IntToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int)
            {
                if (parameter != null && int.TryParse(parameter.ToString(), out int p))
                    return (int)value == p;

                return (int)value > 0;
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool)
            {
                if (parameter != null && int.TryParse(parameter.ToString(), out int p))
                    return p;

                return (bool)value == true ? 1 : 0;
            }
            return 0;
        }
    }
}
