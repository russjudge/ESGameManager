using ESGameManagerLibrary;
using Microsoft.Win32;
using Ookii.Dialogs.Wpf;
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
            Common.UIDispatcher = Dispatcher;
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
            VistaFolderBrowserDialog dialog = new VistaFolderBrowserDialog();

            if (dialog.ShowDialog(this) == true)
            {
              
                RootGamesListFolder = dialog.SelectedPath;
                
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
    }
}
