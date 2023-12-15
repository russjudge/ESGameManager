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
    [ValueConversion(typeof(bool), typeof(double))]
    internal class BoolToNumberConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool val)
            {
                if (parameter is string parms)
                {
                    var parm = parms.Split('|');
                    double returnIfTrue;
                    double returnIfFalse;
                    if (!double.TryParse(parm[0], out returnIfTrue))
                    {
                        returnIfTrue = 1;
                    }
                    if (parm.Length > 1)
                    {
                        if (!double.TryParse(parm[1], out returnIfFalse))
                        {
                            returnIfFalse = 1;
                        }
                    }
                    else
                    {
                        returnIfFalse = 1;
                    }
                    return val ? returnIfTrue : returnIfFalse;
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
