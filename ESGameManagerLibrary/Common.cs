using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace ESGameManagerLibrary
{
    public static class Common
    {
        public static MetaDetailWindow? DetailWindow { get; set; }
        public static Dispatcher? UIDispatcher { get; set; }
        public static void ShowDetailWindow()
        {
            if (Common.DetailWindow == null)
            {
                Common.DetailWindow = new MetaDetailWindow();
                Common.DetailWindow.Show();
            }
        }
		public static void ResizeImage(string filePath)
    {
        try
        {
            using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite))
            {
                BitmapDecoder originalDecoder = BitmapDecoder.Create(fileStream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);

                int originalWidth = originalDecoder.Frames[0].PixelWidth;
                int originalHeight = originalDecoder.Frames[0].PixelHeight;

                if (originalWidth > 1000)
                {
                    int newWidth = 1000;
                    int newHeight = (int)((float)originalHeight / originalWidth * newWidth);

                    TransformedBitmap resizedImage = new TransformedBitmap(originalDecoder.Frames[0], new ScaleTransform((double)newWidth / originalWidth, (double)newHeight / originalHeight));

                    PngBitmapEncoder encoder = new();
                    encoder.Frames.Add(BitmapFrame.Create(resizedImage));

                    fileStream.SetLength(0); // Clear the file content
                    encoder.Save(fileStream);
                    fileStream.Close();

                    //Console.WriteLine("Image resized and saved successfully.");
                }
                else
                {
                    //Console.WriteLine("Image width is already less than or equal to 1000 pixels. No resizing needed.");
                }
            }
        }
        catch //(Exception ex)
        {
            //Console.WriteLine($"Error: {ex.Message}");
        }
    }
    }
}
