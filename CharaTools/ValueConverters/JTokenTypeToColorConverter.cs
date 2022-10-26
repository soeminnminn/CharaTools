using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using Newtonsoft.Json.Linq;

namespace CharaTools
{
    public class JTokenTypeToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is JTokenType)
            {
                var valueType = (JTokenType)value;
                
                if (valueType == JTokenType.String || valueType == JTokenType.Uri || valueType == JTokenType.Guid
                            || valueType == JTokenType.Date || valueType == JTokenType.TimeSpan)
                    return new SolidColorBrush(Colors.DarkRed);
                else if (valueType == JTokenType.Null || valueType == JTokenType.Undefined || valueType == JTokenType.None)
                    return new SolidColorBrush(Colors.Gray);
                else if (valueType == JTokenType.Boolean || valueType == JTokenType.Float || valueType == JTokenType.Integer)
                    return new SolidColorBrush(Colors.Green);
                else
                    return SystemColors.WindowTextBrush;
            }
            return SystemColors.WindowTextBrush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
