using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;

namespace ESGameManagerLibrary
{
    /// <summary>
    /// Interaction logic for MetaDetailControl.xaml
    /// </summary>
    public partial class MetaDetailControl : UserControl
    {
        public MetaDetailControl()
        {
            InitializeComponent();
            //DataContext = this;
        }
        public static readonly DependencyProperty SelectedGameProperty =
           DependencyProperty.Register(
               nameof(SelectedGame),
               typeof(Game),
               typeof(MetaDetailControl));
        public Game SelectedGame
        {
            get
            {
                return (Game)this.GetValue(SelectedGameProperty);
            }

            set
            {
                this.SetValue(SelectedGameProperty, value);
            }
        }
        public const string romFilesFilter = "Zip Files|*.zip;*.7z;*.bin;*.gz;*.a26|ISO|*.iso|Executable|*.exe|ROM Files|*.rom";
        public const string imageFilesFilter = "Image Files|*.jpg;*.png;*.bmp;*.gif;*.svg";
        public const string videoFilesFilter = "Video files|*.mp4";
        public static string? BrowseForFile(string title, string filter)
        {
            string? retVal = null;
            OpenFileDialog diag = new();
            diag.Filter = filter + "|All Files|*.*";
            diag.Multiselect = false;
            diag.Title = title;
            diag.CheckFileExists = true;
            if (diag.ShowDialog() == true)
            {
                retVal = diag.FileName;
            }
            return retVal;
        }

        private void BrowseForPath(object sender, RoutedEventArgs e)
        {
            var path = BrowseForFile("Select ROM file", romFilesFilter);
            if (!string.IsNullOrEmpty(path))
            {
                SelectedGame.SetFullROMPath(path);
                SelectedGame.FullPath = path;
            }
        }
        private void BrowseForMarquee(object sender, RoutedEventArgs e)
        {
            var path = BrowseForFile("Select Marquee file", imageFilesFilter);
            if (!string.IsNullOrEmpty(path))
            {
                SelectedGame.SetFullMarqueePath(path);
                SelectedGame.FullMarqueePath = path;
            }
        }
        private void BrowseForImage(object sender, RoutedEventArgs e)
        {
            var path = BrowseForFile("Select Image file", imageFilesFilter);
            if (!string.IsNullOrEmpty(path))
            {
                SelectedGame.SetFullImagePath(path);
                SelectedGame.FullImagePath = path;
            }
        }
        private void BrowseForVideo(object sender, RoutedEventArgs e)
        {
            var path = BrowseForFile("Select Video file", videoFilesFilter);
            if (!string.IsNullOrEmpty(path))
            {
                SelectedGame.SetFullVideoPath(path);
                SelectedGame.FullImagePath = path;
            }
        }
    }
}
