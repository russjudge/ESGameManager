using System;
using System.Globalization;
using System.Windows.Data;

namespace ESGameManagerLibrary
{
    /// <summary>
    /// Date formatter class.
    /// </summary>
    [ValueConversion(typeof(DateTime), typeof(string))]
    public class DateFormatter : IValueConverter
    {
        /// <summary>
        /// Convert object.
        /// </summary>
        /// <param name="value">Object to convert.</param>
        /// <param name="targetType">Target type.</param>
        /// <param name="parameter">Parameter.</param>
        /// <param name="culture">Culture.</param>
        /// <returns>Converted Object.</returns>
        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                DateTime val = (DateTime)value;
                if (parameter != null)
                {
                    string? parm = parameter.ToString();
                    string? format = parm;
                    string retVal;
                    if (parm != null && parm.Contains('|'))
                    {
                        string[] parmList = parm.Split('|');
                        format = parmList[0];

                        if (parmList.Length > 1 && (val.CompareTo(DateTime.MinValue) == 0 || (val.CompareTo(new DateTime(1754, 1, 1)) < 0 && val.CompareTo(new DateTime(1753, 1, 1)) >= 0)))
                        {
                            //Use for no value return (or default).
                            retVal = parmList[1];
                        }
                        else if (val.CompareTo(DateTime.MinValue.AddTicks(1)) == 0 && parmList.Length > 2)
                        {
                            //Use for indicating an error of some kind.
                            retVal = parmList[2];
                        }
                        else
                        {
                            retVal = val.ToString(format);
                        }
                    }
                    else
                    {
                        retVal = val.ToString(format);
                    }

                    return retVal;
                }
                else
                {
                    return val.ToString();
                }
            }
            else
            {
                return default;
            }
        }

        /// <summary>
        /// Unconverts an object (not implemented).
        /// </summary>
        /// <param name="value">Object to unconvert.</param>
        /// <param name="targetType">Target Type.</param>
        /// <param name="parameter">Parameter.</param>
        /// <param name="culture">Culture.</param>
        /// <returns>unconverted object.</returns>
        /// <exception cref="NotImplementedException">Thrown.</exception>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
