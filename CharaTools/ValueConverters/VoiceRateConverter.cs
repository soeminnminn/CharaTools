using System;
using System.Globalization;
using System.Windows.Data;

namespace CharaTools
{
    public class VoiceRateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var val = value == null ? 0 : (float)value;
            return Math.Round(val * 100, 0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is decimal)
            {
                var val = (decimal)value;
                return (float)Math.Round(val / 100, 2);
            }
            else if (value is string)
            {
                if (float.TryParse(value.ToString(), out float val))
                {
                    return Math.Round(val / 100, 2);
                }
            }
            return 0.0f;
        }
    }
}
