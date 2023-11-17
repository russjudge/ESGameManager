using ESGameManagerLibrary;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace ESGameManagerLibrary
{
    /// <summary>
    /// Interaction logic for MetaDetailWindow.xaml
    /// </summary>
    public partial class MetaDetailWindow : Window
    {
        public MetaDetailWindow()
        {
            InitializeComponent();
        }
        public static readonly DependencyProperty SelectedGameProperty =
           DependencyProperty.Register(
               nameof(SelectedGame),
               typeof(Game),
               typeof(MetaDetailWindow), new PropertyMetadata(OnSelectedGameChanged));

        private static void OnSelectedGameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SendDetailToForeground();
            if (d is MetaDetailWindow me)
            {
                int i = me.Games.IndexOf(me.SelectedGame);
                me.PreviousEnabled = i > 0;
                me.NextEnabled = i < me.Games.Count - 1;
            }
        }
        public static void SendDetailToForeground()
        {
            System.Threading.ThreadPool.QueueUserWorkItem(SendToForeGround);
        }
        /// <summary>
        /// Pushes the Metadata Detail window to the foreground.
        /// This weirdness is necessary or else the MainWindow jumps back to the foreground after this is done.
        /// </summary>
        /// <param name="state"></param>
        static void SendToForeGround(object? state)
        {
            if (Common.UIDispatcher != null && Common.DetailWindow != null)
            {
                Common.UIDispatcher.Invoke(() =>
                {
                    Common.DetailWindow.Activate();
                    Common.DetailWindow.Focus();
                });
            }
        }
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
        public static readonly DependencyProperty GamesProperty =
          DependencyProperty.Register(
              nameof(Games),
              typeof(ObservableCollection<Game>),
              typeof(MetaDetailWindow));


        public ObservableCollection<Game> Games
        {
            get
            {
                return (ObservableCollection<Game>)this.GetValue(GamesProperty);
            }

            set
            {
                this.SetValue(GamesProperty, value);
            }
        }
        public static readonly DependencyProperty NextEnabledProperty =
          DependencyProperty.Register(
              nameof(NextEnabled),
              typeof(bool),
              typeof(MetaDetailWindow));


        public bool NextEnabled
        {
            get
            {
                return (bool)this.GetValue(NextEnabledProperty);
            }

            set
            {
                this.SetValue(NextEnabledProperty, value);
            }
        }
        public static readonly DependencyProperty PreviousEnabledProperty =
          DependencyProperty.Register(
              nameof(PreviousEnabled),
              typeof(bool),
              typeof(MetaDetailWindow));


        public bool PreviousEnabled
        {
            get
            {
                return (bool)this.GetValue(PreviousEnabledProperty);
            }

            set
            {
                this.SetValue(PreviousEnabledProperty, value);
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

        private void OnClosed(object sender, EventArgs e)
        {
            Common.DetailWindow = null;
        }

        private void GoToPreviousGame(object sender, RoutedEventArgs e)
        {
            int i = Games.IndexOf(SelectedGame);
            if (i > 0)
            {
                SelectedGame = Games[i - 1];
            }
           
        }

        private void GoToNextGame(object sender, RoutedEventArgs e)
        {
            int i = Games.IndexOf(SelectedGame);
            if (i < Games.Count - 2)
            {
                SelectedGame = Games[i + 1];
            }
        }
    }
}
