using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace ESGameManagerLibrary
{
    [ValueConversion(typeof(string), typeof(BitmapImage))]
    public class PathToImageSourceConverter : IValueConverter
    {
        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string filePath)
            {
               
                if (System.IO.File.Exists(filePath))
                {
                    try
                    {
                        BitmapImage image = new BitmapImage();
                        image.BeginInit();
                        image.CacheOption = BitmapCacheOption.OnLoad;
                        image.UriSource = new Uri(filePath, UriKind.RelativeOrAbsolute);
                        image.EndInit();
                        return image;
                    }
                    catch (Exception ex)
                    {
                        // Handle exception if loading image fails
                        Console.WriteLine($"Error loading image from path: {ex.Message}");
                    }
                }
                else
                {
                    BitmapImage image = new BitmapImage();
                    image.BeginInit();
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.UriSource = new Uri("Resources\\na.png", UriKind.RelativeOrAbsolute);
                    image.EndInit();
                    return image;
                }
            }

            return default;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
