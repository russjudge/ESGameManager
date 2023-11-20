using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace ESGameManagerLibrary
{
    [ValueConversion(typeof(StructureOrganization), typeof(string))]
    internal class OrganizationStructureToTextConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is  StructureOrganization org)
            {
                switch (org)
                {
                    case StructureOrganization.None:
                        return "In Main";
                    case StructureOrganization.ByGenre:
                        return "By Genre";
                    case StructureOrganization.Publisher:
                        return "By Publisher";
                    case StructureOrganization.Developer:
                        return "By Developer";
                    case StructureOrganization.ByFirstLetter:
                        return "By First Letter";
                    default:
                        return "Unknown";

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
