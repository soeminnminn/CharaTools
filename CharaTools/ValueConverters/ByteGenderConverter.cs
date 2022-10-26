using System;
using System.Globalization;
using System.Windows.Data;

namespace CharaTools
{
    public class ByteGenderConverter : IMultiValueConverter, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is byte)
            {
                if (parameter is bool && (bool)parameter == true)
                    return string.Empty;

                return (byte)value == 0 ? "Male" : "Female";
            }
            return string.Empty;
        }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values != null && values.Length == 2)
            {
                if (values[0] is byte)
                {
                    if (values[1] is bool && (bool)values[1] == true)
                        return string.Empty;

                    return (byte)values[0] == 0 ? "Male" : "Female";
                }
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string)
                return string.Equals(value.ToString(), "Male", StringComparison.InvariantCultureIgnoreCase) ? (byte)0 : (byte)1;
            else
            {
                if (byte.TryParse(value.ToString(), out byte sex))
                    return sex;
            }
            return 0;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
