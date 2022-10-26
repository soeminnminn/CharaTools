using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace CharaTools
{
    public class EnumStateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is AIChara.ChaFileDefine.State)
            {
                var val = (AIChara.ChaFileDefine.State)value;
                if (targetType == typeof(string))
                {
                    return val.ToString();
                }
                else if (targetType == typeof(int))
                {
                    return (int)val;
                }
            }
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string)
            {
                var names = Enum.GetNames(typeof(AIChara.ChaFileDefine.State));
                return (AIChara.ChaFileDefine.State)names.ToList().IndexOf(value.ToString());
            }
            else if (value is int)
            {
                return (AIChara.ChaFileDefine.State)value;
            }
            return AIChara.ChaFileDefine.State.Blank;
        }
    }
}
