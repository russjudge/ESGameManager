using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ESGameManagerLibrary
{
    /// <summary>
    /// Interaction logic for ScanGamesWindow.xaml
    /// </summary>
    public partial class ScanGamesWindow : Window
    {
        public static void ShowScanDialog(GameList gameFolder)
        {
            ScanGamesWindow win = new();
            win.GameFolder = gameFolder;
            win.ShowDialog();
        }
        public ScanGamesWindow()
        {
            OrphanImages = new();
            NewGames = new();
            DeleteGames = new();
            InitializeComponent();
        }
        public static readonly DependencyProperty GameFolderProperty =
           DependencyProperty.Register(
               nameof(GameFolder),
               typeof(GameList),
               typeof(ScanGamesWindow));
        public GameList GameFolder
        {
            get
            {
                return (GameList)this.GetValue(GameFolderProperty);
            }

            set
            {
                this.SetValue(GameFolderProperty, value);
            }
        }


        public static readonly DependencyProperty NewGamesProperty =
           DependencyProperty.Register(
               nameof(NewGames),
               typeof(ObservableCollection<string>),
               typeof(ScanGamesWindow));
        public ObservableCollection<string> NewGames
        {
            get
            {
                return (ObservableCollection<string>)this.GetValue(NewGamesProperty);
            }

            set
            {
                this.SetValue(NewGamesProperty, value);
            }
        }


        public static readonly DependencyProperty SelectedNewROMProperty =
           DependencyProperty.Register(
               nameof(SelectedNewROM),
               typeof(string),
               typeof(ScanGamesWindow));
        public string SelectedNewROM
        {
            get
            {
                return (string)this.GetValue(SelectedNewROMProperty);
            }

            set
            {
                this.SetValue(SelectedNewROMProperty, value);
            }
        }
        public static readonly DependencyProperty DeleteGamesProperty =
           DependencyProperty.Register(
               nameof(DeleteGames),
               typeof(ObservableCollection<Game>),
               typeof(ScanGamesWindow));
        public ObservableCollection<Game> DeleteGames
        {
            get
            {
                return (ObservableCollection<Game>)this.GetValue(DeleteGamesProperty);
            }

            set
            {
                this.SetValue(DeleteGamesProperty, value);
            }
        }

        public static readonly DependencyProperty SelectedDeleteGameProperty =
           DependencyProperty.Register(
               nameof(SelectedDeleteGame),
               typeof(Game),
               typeof(ScanGamesWindow));
        public Game SelectedDeleteGame
        {
            get
            {
                return (Game)this.GetValue(SelectedDeleteGameProperty);
            }

            set
            {
                this.SetValue(SelectedDeleteGameProperty, value);
            }
        }

        public static readonly DependencyProperty OrphanImagesProperty =
           DependencyProperty.Register(
               nameof(OrphanImages),
               typeof(ObservableCollection<string>),
               typeof(ScanGamesWindow));
        public ObservableCollection<string> OrphanImages
        {
            get
            {
                return (ObservableCollection<string>)this.GetValue(OrphanImagesProperty);
            }

            set
            {
                this.SetValue(OrphanImagesProperty, value);
            }
        }

        void Scan()
        {
            if (!string.IsNullOrEmpty(GameListControl.RootGamesListFolder) && !string.IsNullOrEmpty(GameFolder.Folder))
            {
                List<string> existingGames = new();
                
                List<string> usedImages = new();
               
                foreach (var gm in GameFolder.Games)
                {
                    if (System.IO.File.Exists(gm.FullPath))
                    {
                        existingGames.Add(gm.FullPath);
                    }
                    else
                    {
                        DeleteGames.Add(gm);
                    }
                    usedImages.Add(gm.FullImagePath);
                }
                existingGames.Sort();

                string startFolder = System.IO.Path.Combine(GameListControl.RootGamesListFolder, GameFolder.Folder);
                DirectoryInfo startDir = new(startFolder);
                ScanDir(startDir, existingGames, usedImages);
               
            }
            else
            {
                MessageBox.Show("Invalid data");
            }
        }
        static readonly List<string> gameExtensions = new List<string>()
        {
            ".zip"
        };
        static readonly List<string> imageExtensions = new List<string>()
        {
            ".jpg",
            ".jpeg",
            ".png",
            ".gif",
            ".svg",
            ".bmp",
            ".tif",
            ".tiff"
        };
        void ScanDir(DirectoryInfo parentDir, List<string> existingGames, List<string> usedImages)
        {
            foreach (var f in parentDir.GetFiles())
            {
                if (gameExtensions.Contains(f.Extension))
                {
                    if (!existingGames.Contains(f.FullName))
                    {
                        NewGames.Add(f.FullName);
                    }
                }
                if (imageExtensions.Contains(f.Extension))
                {
                    if (!usedImages.Contains(f.FullName))
                    {
                        OrphanImages.Add(f.FullName);
                    }
                }

            }
            foreach (var d in parentDir.GetDirectories())
            {
                ScanDir(d, existingGames, usedImages);
            }
        }
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Scan();
        }

        private void OnDeleteImage(object sender, RoutedEventArgs e)
        {
            if (sender is Button b && b.CommandParameter is string file)
            {
                File.Delete(file);
                OrphanImages.Remove(file);
            }
        }

        private void OnDeleteGame(object sender, RoutedEventArgs e)
        {
            if (sender is Button b && b.CommandParameter is Game gm)
            {
                GameFolder.RemoveGame(gm);
            }
        }

        private void OnAddGame(object sender, RoutedEventArgs e)
        {
            if (sender is Button b && b.CommandParameter is string file)
            {
                GameFolder.AddGame(file);
                NewGames.Remove(file);
            }
        }

        private void OnDeleteRom(object sender, RoutedEventArgs e)
        {
            if (sender is Button b && b.CommandParameter is string file)
            {
                if (File.Exists(file))
                {
                    File.Delete(file);
                }
                NewGames.Remove(file);
            }
        }

        private void OnReplaceROM(object sender, RoutedEventArgs e)
        {
            if (SelectedDeleteGame == null || string.IsNullOrEmpty(SelectedNewROM))
            {
                MessageBox.Show("You need to select the Game with the bad ROM to be updated AND the Orphaned ROM file.",
                    "Update ROM file",MessageBoxButton.OK, MessageBoxImage.Stop);
            }
            else
            {
                SelectedDeleteGame.FullPath = SelectedNewROM;
                DeleteGames.Remove(SelectedDeleteGame);
                NewGames.Remove(SelectedNewROM);
            }
        }
    }
}
