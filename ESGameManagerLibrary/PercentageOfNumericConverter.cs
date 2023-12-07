using System.Globalization;
using System.Windows.Data;

namespace ESGameManagerLibrary
{
    [ValueConversion(typeof(double), typeof(double))]
    internal class PercentageOfNumericConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return default(double);
            }
            else
            {
                if (parameter == null)
                {
                    return default(double);
                }
                else
                {
                    if (double.TryParse(parameter.ToString(), out double percentAdjust))
                    {
                        return ((double)value) * percentAdjust;
                    }
                    else
                    {
                        return (double)value;
                    }
                }
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
