using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ESGameManagerLibrary
{
    [ValueConversion(typeof(string), typeof(bool))]
    public class StringToBoolConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return default;
            }
            else
            {
                if (bool.TryParse(value.ToString(), out bool result))
                {
                    return result;
                }
                else
                {
                    return default;
                }
            }
        }

        public object? ConvertBack(object? value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return default;
            }
            else
            {
                if (value is bool result)
                {
                    return result.ToString().ToLowerInvariant();
                }
                else
                {
                    return default;
                }
            }
        }
    }
}
