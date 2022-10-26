using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace CharaTools
{
    public class MinusOneConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var val = value.ToString();
            if (int.TryParse(val, out int res))
            {
                return Math.Max(0, res - 1);
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var val = value == null ? 0 : (int)value;
            if (targetType == typeof(byte))
            {
                return (byte)val + 1;
            }
            return val + 1;
        }
    }
}
