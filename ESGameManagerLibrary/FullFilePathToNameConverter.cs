using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ESGameManagerLibrary
{
    [ValueConversion(typeof(string), (typeof(string)))]
    public class FullFilePathToNameConverter : IValueConverter
    {
        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                string? val = value.ToString();
                if (!string.IsNullOrEmpty(val))
                {
                    FileInfo f = new(val);
                    return f.Name;
                }
                else
                {
                    return default;
                }
            }
            else
            {
                return default;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
