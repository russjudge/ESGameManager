using ESGameManagerLibrary;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace ESGameManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
       
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            SearchText = string.Empty;
            //RootGamesListFolder = @"E:\DefaultUser\Documents\roms";
            GamesList = new();
            GameList.NewGameList += GameList_NewGameList;
        }

        private void GameList_NewGameList(object? sender, NewGameListEventArgs e)
        {
            this.Dispatcher.BeginInvoke(() =>
            {
                GamesList.Add(e.GameList);
                if (GamesList.Count == 1)
                {
                    tc.SelectedIndex = 0;
                }
            });
        }

        public static readonly DependencyProperty RootGamesListFolderProperty =
           DependencyProperty.Register(
               nameof(RootGamesListFolder),
               typeof(string),
               typeof(MainWindow), new PropertyMetadata(OnROMFolderChanged));

        private static void OnROMFolderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is MainWindow me)
            {
                GameListControl.RootGamesListFolder = me.RootGamesListFolder;
                me.GamesList.Clear();

                GameList.LoadGameListFiles();
            }
        }

        public string RootGamesListFolder
        {
            get
            {
                return (string)this.GetValue(RootGamesListFolderProperty);
            }

            set
            {
                this.SetValue(RootGamesListFolderProperty, value);
            }
        }
        public static readonly DependencyProperty GamesListProperty =
           DependencyProperty.Register(
               nameof(GamesList),
               typeof(ObservableCollection<GameList>),
               typeof(MainWindow));
        public ObservableCollection<GameList> GamesList
        {
            get
            {
                return (ObservableCollection<GameList>)this.GetValue(GamesListProperty);
            }

            set
            {
                this.SetValue(GamesListProperty, value);
            }
        }


        public static readonly DependencyProperty SearchTextProperty =
           DependencyProperty.Register(
               nameof(SearchText),
               typeof(string),
               typeof(MainWindow));
        public string SearchText
        {
            get
            {
                return (string)this.GetValue(SearchTextProperty);
            }

            set
            {
                this.SetValue(SearchTextProperty, value);
            }
        }


        public static readonly DependencyProperty SearchIncludeDescriptionProperty =
           DependencyProperty.Register(
               nameof(SearchIncludeDescription),
               typeof(bool),
               typeof(MainWindow));
        public bool SearchIncludeDescription
        {
            get
            {
                return (bool)this.GetValue(SearchIncludeDescriptionProperty);
            }

            set
            {
                this.SetValue(SearchIncludeDescriptionProperty, value);
            }
        }

        public static readonly DependencyProperty SearchIncludePublisherProperty =
           DependencyProperty.Register(
               nameof(SearchIncludePublisher),
               typeof(bool),
               typeof(MainWindow));
        public bool SearchIncludePublisher
        {
            get
            {
                return (bool)this.GetValue(SearchIncludePublisherProperty);
            }

            set
            {
                this.SetValue(SearchIncludePublisherProperty, value);
            }
        }


        public static readonly DependencyProperty SearchIncludeDeveloperProperty =
           DependencyProperty.Register(
               nameof(SearchIncludeDeveloper),
               typeof(bool),
               typeof(MainWindow));
        public bool SearchIncludeDeveloper
        {
            get
            {
                return (bool)this.GetValue(SearchIncludeDeveloperProperty);
            }

            set
            {
                this.SetValue(SearchIncludeDeveloperProperty, value);
            }
        }


        public static readonly DependencyProperty SearchIncludeGenreProperty =
           DependencyProperty.Register(
               nameof(SearchIncludeGenre),
               typeof(bool),
               typeof(MainWindow));
        public bool SearchIncludeGenre
        {
            get
            {
                return (bool)this.GetValue(SearchIncludeGenreProperty);
            }

            set
            {
                this.SetValue(SearchIncludeGenreProperty, value);
            }
        }

        private void OnGenerateCSV(object sender, RoutedEventArgs e)
        {
            if (GamesList != null)
            {
                var diag = new SaveFileDialog();
                diag.Filter = "*.csv|CSV files";
                diag.DefaultExt = "csv";
                diag.Title = "Select CSV File to save";
                if (diag.ShowDialog() == true)
                {
                    GameList.GenerateCSV(RootGamesListFolder, GamesList.ToArray<GameList>(), diag.FileName);
                }
            }
        }

        private void OnBrowseForROMFolder(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFolderDialog
            {
                Title = "Select folder where all ROMs for all systems are stored"
            };
            if (dialog.ShowDialog() == true)
            {
                RootGamesListFolder = dialog.FolderName;
            }
        }

        private void OnClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            foreach (var list in GamesList)
            {
                if (list.Changed)
                {
                    if (MessageBox.Show("There are unsaved changes.\r\n\r\nAre you sure you wish to quit?", "Exit Manager", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                    {
                        e.Cancel = true;
                    }
                    break;
                }
            }
        }
        
        private void OnClosed(object sender, EventArgs e)
        {
            if (Common.DetailWindow != null)
            {
                Common.DetailWindow.Close();
            }
        }

        private void OnSettings(object sender, RoutedEventArgs e)
        {
            SettingsWindow win = new SettingsWindow();
            win.ShowDialog();
        }

        private void OnSearch(object sender, RoutedEventArgs e)
        {
            List<Game> matchedGames = new();
            if (!string.IsNullOrEmpty(SearchText.Trim()))
            {
                foreach (var list in GamesList)
                {
                    foreach (var game in list.Games)
                    {
                        if (game.Name.Contains(SearchText, StringComparison.InvariantCultureIgnoreCase))
                        {
                            matchedGames.Add(game);
                        }
                        else if (SearchIncludeDescription && !string.IsNullOrEmpty(game.Description) && game.Description.Contains(SearchText, StringComparison.InvariantCultureIgnoreCase))
                        {
                            matchedGames.Add(game);
                        }
                        else if (SearchIncludeDeveloper && !string.IsNullOrEmpty(game.Developer) && game.Developer.Contains(SearchText, StringComparison.InvariantCultureIgnoreCase))
                        {
                            matchedGames.Add(game);
                        }
                        else if (SearchIncludeGenre && !string.IsNullOrEmpty(game.Genre) && game.Genre.Contains(SearchText, StringComparison.InvariantCultureIgnoreCase))
                        {
                            matchedGames.Add(game);
                        }
                        else if (SearchIncludePublisher && !string.IsNullOrEmpty(game.Publisher) && game.Publisher.Contains(SearchText, StringComparison.InvariantCultureIgnoreCase))
                        {
                            matchedGames.Add(game);
                        }
                    }
                }
            }
            if (matchedGames.Count > 0)
            {
                MetaDetailWindow win = new MetaDetailWindow();
                win.Games =new( matchedGames);
                win.ShowList = true;
                win.Show();
            }
            else
            {
                MessageBox.Show("No matches.");
            }
        }

        private void OnMerge(object sender, RoutedEventArgs e)
        {
            MergeWindow win = new MergeWindow();
            win.GamesList = new(this.GamesList);
            win.ShowDialog();
        }

        private void OnTest(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException("test");
        }
    }
}
