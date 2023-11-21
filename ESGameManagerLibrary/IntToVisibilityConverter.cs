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
    //[ValueConversion(typeof(int), typeof(Visibility))]
    public class IntToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int i)
            {
                if (parameter is string parm)
                {
                    var parms = parm.Split("|");
                    if (parms.Length > 0)
                    {
                      
                        if (int.TryParse(parms[0], out int valueMatch))
                        {
                            Visibility visibilityIfMatch = Visibility.Visible;
                            Visibility visibilityIfNotMatch = Visibility.Collapsed;
                            if (parms.Length > 1)
                            {
                                try
                                {
                                    visibilityIfMatch = (Visibility)Enum.Parse(typeof(Visibility), parms[1]);
                                }
                                catch
                                {

                                }
                                visibilityIfNotMatch = (visibilityIfMatch == Visibility.Visible) ? Visibility.Collapsed : Visibility.Visible;
                            }
                            return (valueMatch == i) ? visibilityIfMatch : visibilityIfNotMatch;
                        }
                        else
                        {
                            return DependencyProperty.UnsetValue;
                        }
                    }
                    else
                    {
                        return DependencyProperty.UnsetValue;
                    }
                }
                else
                {
                    return DependencyProperty.UnsetValue;
                }
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
