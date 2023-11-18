using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace ESGameManagerLibrary
{
    [ValueConversion(typeof(object), typeof(Visibility))]
    public class NullToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility visibilityIfNotNull = Visibility.Visible;
            Visibility visibilityIfNull = Visibility.Collapsed;
            if (parameter is string parm)
            {
                visibilityIfNull = (Visibility)Enum.Parse(typeof(Visibility), parm);
                visibilityIfNotNull = (visibilityIfNull == Visibility.Visible) ? Visibility.Collapsed : Visibility.Visible;
            }
            return (value == null || (value is string val && string.IsNullOrEmpty(val))) ? visibilityIfNull : visibilityIfNotNull;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
