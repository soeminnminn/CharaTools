using System;
using System.Globalization;
using System.Windows.Data;

namespace CharaTools
{
    public class NumericValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType == typeof(decimal))
            {
                try
                {
                    return (decimal)value;
                }
                catch(Exception)
                {
                    var val = value.ToString();
                    if (decimal.TryParse(val, out decimal res))
                    {
                        return res;
                    }
                    return 0;
                }
            }
            else if (targetType == typeof(string))
            {
                return value.ToString();
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType == typeof(byte))
            {
                try
                {
                    return (byte)value;
                }
                catch(Exception)
                {
                    var val = value.ToString();
                    if (byte.TryParse(val, out byte res))
                    {
                        return res;
                    }
                    return (byte)0;
                }
            }
            else if (targetType == typeof(int))
            {
                try
                {
                    return (int)value;
                }
                catch (Exception)
                {
                    var val = value.ToString();
                    if (int.TryParse(val, out int res))
                    {
                        return res;
                    }
                    return 0;
                }
            }
            return value;
        }
    }
}
