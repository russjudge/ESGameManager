using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace ESGameManagerLibrary
{
    [ValueConversion(typeof(ColorList), typeof(System.Windows.Media.Brush) )]
    public class ColorListToBrushConverter : IValueConverter
    {
        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ColorList c)
            {
                var retVal = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter
               .ConvertFromString(c.ToString()));
                return retVal;
            }
            else
            {
                return default(System.Windows.Media.Brush);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
