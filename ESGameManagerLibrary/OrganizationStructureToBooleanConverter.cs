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
    [ValueConversion(typeof(StructureOrganization), typeof(bool))]
    public class OrganizationStructureToBooleanConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is StructureOrganization org)
            {
                StructureOrganization MatchValue = StructureOrganization.None;
                bool ValueOnMatch = true;
                if (parameter is string parm)
                {
                    if (!string.IsNullOrEmpty(parm))
                    {
                        string[] parmList = parm.Split('|');
                        if (parmList.Length > 0)
                        {
                            MatchValue = (StructureOrganization)Enum.Parse(typeof(StructureOrganization), parmList[0]);
                        }
                        if (parmList.Length > 1)
                        {
                            if (bool.TryParse(parmList[1], out bool testValue))
                            {
                                ValueOnMatch = testValue;
                            }
                        }
                    }
                }
               
                if (MatchValue == org)
                {
                    return ValueOnMatch;
                }
                else
                {
                    return !ValueOnMatch;
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
