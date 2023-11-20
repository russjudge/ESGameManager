using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace ESGameManagerLibrary
{
    [ValueConversion(typeof(int), typeof(System.Windows.Media.Brush) )]
    public class ColorListToBrushConverter : IValueConverter
    {
        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int c)
            {
                ColorList col = (ColorList)c;
                
                var converter = System.Windows.Media.ColorConverter.ConvertFromString(col.ToString());
                var color = (System.Windows.Media.Color)converter;
                var retVal = new SolidColorBrush(color);
                return retVal;
            }
            else
            {
                return DependencyProperty.UnsetValue;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
