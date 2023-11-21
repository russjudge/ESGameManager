using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Policy;
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
    /// Interaction logic for MergeWindow.xaml
    /// </summary>
    public partial class MergeWindow : Window
    {
        const int MaxStep=4;
        const int MinStep=1;

        const int sourceSelectStep = 1;
        const int targetSelectStep = 3;
        const int gameSelectStep = 2;
        public MergeWindow()
        {
            InitializeComponent();
            DataContext = this;
            Step = 1;
        }
        public static readonly DependencyProperty GamesListProperty =
           DependencyProperty.Register(
               nameof(GamesList),
               typeof(ObservableCollection<GameList>),
               typeof(MergeWindow));
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
        public static readonly DependencyProperty SourceGameListProperty =
           DependencyProperty.Register(
               nameof(SourceGameList),
               typeof(GameList),
               typeof(MergeWindow));
        public GameList SourceGameList
        {
            get
            {
                return (GameList)this.GetValue(SourceGameListProperty);
            }

            set
            {
                this.SetValue(SourceGameListProperty, value);
            }
        }
        public static readonly DependencyProperty TargetGameListProperty =
           DependencyProperty.Register(
               nameof(TargetGameList),
               typeof(GameList),
               typeof(MergeWindow));
        public GameList TargetGameList
        {
            get
            {
                return (GameList)this.GetValue(TargetGameListProperty);
            }

            set
            {
                this.SetValue(TargetGameListProperty, value);
            }
        }

        public static readonly DependencyProperty DeleteFromSourceProperty =
          DependencyProperty.Register(
              nameof(DeleteFromSource),
              typeof(bool),
              typeof(MergeWindow));
        public bool DeleteFromSource
        {
            get
            {
                return (bool)this.GetValue(DeleteFromSourceProperty);
            }

            set
            {
                this.SetValue(DeleteFromSourceProperty, value);
            }
        }

        public static readonly DependencyProperty IntoSpecialFolderProperty =
          DependencyProperty.Register(
              nameof(IntoSpecialFolder),
              typeof(bool),
              typeof(MergeWindow));
        public bool IntoSpecialFolder
        {
            get
            {
                return (bool)this.GetValue(IntoSpecialFolderProperty);
            }

            set
            {
                this.SetValue(IntoSpecialFolderProperty, value);
            }
        }
        public static readonly DependencyProperty SpecialFolderProperty =
            DependencyProperty.Register(
            nameof(SpecialFolder),
            typeof(string),
            typeof(MergeWindow));
        public string SpecialFolder
        {
            get
            {
                return (string)this.GetValue(SpecialFolderProperty);
            }

            set
            {
                this.SetValue(SpecialFolderProperty, value);
            }
        }
        public static readonly DependencyProperty SetFlag1Property =
         DependencyProperty.Register(
             nameof(SetFlag1),
             typeof(bool),
             typeof(MergeWindow));
        public bool SetFlag1
        {
            get
            {
                return (bool)this.GetValue(SetFlag1Property);
            }

            set
            {
                this.SetValue(SetFlag1Property, value);
            }
        }

        public static readonly DependencyProperty SetFlag2Property =
         DependencyProperty.Register(
             nameof(SetFlag2),
             typeof(bool),
             typeof(MergeWindow));
        public bool SetFlag2
        {
            get
            {
                return (bool)this.GetValue(SetFlag2Property);
            }

            set
            {
                this.SetValue(SetFlag2Property, value);
            }
        }
        public static readonly DependencyProperty SetFlag3Property =
         DependencyProperty.Register(
             nameof(SetFlag3),
             typeof(bool),
             typeof(MergeWindow));
        public bool SetFlag3
        {
            get
            {
                return (bool)this.GetValue(SetFlag3Property);
            }

            set
            {
                this.SetValue(SetFlag3Property, value);
            }
        }
        public static readonly DependencyProperty SetFlag4Property =
         DependencyProperty.Register(
             nameof(SetFlag4),
             typeof(bool),
             typeof(MergeWindow));
        public bool SetFlag4
        {
            get
            {
                return (bool)this.GetValue(SetFlag4Property);
            }

            set
            {
                this.SetValue(SetFlag4Property, value);
            }
        }
        public static readonly DependencyProperty SetFlag5Property =
         DependencyProperty.Register(
             nameof(SetFlag5),
             typeof(bool),
             typeof(MergeWindow));
        public bool SetFlag5
        {
            get
            {
                return (bool)this.GetValue(SetFlag5Property);
            }

            set
            {
                this.SetValue(SetFlag5Property, value);
            }
        }
        public static readonly DependencyProperty SetFlag6Property =
         DependencyProperty.Register(
             nameof(SetFlag6),
             typeof(bool),
             typeof(MergeWindow));
        public bool SetFlag6
        {
            get
            {
                return (bool)this.GetValue(SetFlag6Property);
            }

            set
            {
                this.SetValue(SetFlag6Property, value);
            }
        }

        public static readonly DependencyProperty StepProperty =
            DependencyProperty.Register(
            nameof(Step),
            typeof(int),
            typeof(MergeWindow));
        public int Step
        {
            get
            {
                return (int)this.GetValue(StepProperty);
            }

            set
            {
                this.SetValue(StepProperty, value);
            }
        }

        private void OnNext(object sender, RoutedEventArgs e)
        {
            Step++;
            if (Step > MaxStep)
            {
                DoAction();
            }
        }
        void DoAction()
        {
            if (SourceGameList == null)
            {
                MessageBox.Show("Select a game folder to merge.", "Source folder not selected", MessageBoxButton.OK, MessageBoxImage.Error);
                Step = sourceSelectStep;
            }
            else if (TargetGameList == null)
            {
                MessageBox.Show("Select a game folder to merge into.", "Target folder not selected", MessageBoxButton.OK, MessageBoxImage.Error);
                Step = targetSelectStep;
            }
            else if (SourceGameList.Folder == TargetGameList.Folder)
            {
                MessageBox.Show("Source game folder is same as target game folder.\r\n\r\nPlease change source or target.", "Source and Target are the same.", MessageBoxButton.OK, MessageBoxImage.Error);
                Step = targetSelectStep;
            }
            else
            {
                List<Game> GamesToMerge = new();
                foreach (var game in SourceGameList.Games)
                {
                    if (game.Flag7)
                    {
                        GamesToMerge.Add(game);
                    }
                }
                if (GamesToMerge.Count <= 0)
                {
                    MessageBox.Show("No games selected to merge.\r\n\r\nPlease select games to merge.", "No games selected.", MessageBoxButton.OK, MessageBoxImage.Error);
                    Step = gameSelectStep;
                }
                else
                {
                    string subfolder = string.Empty;
                    if (IntoSpecialFolder)
                    {
                        subfolder = SpecialFolder;
                    }
                    foreach (var game in GamesToMerge)
                    {
                        game.Flag7 = false;
                        Game newGame = game.Copy();
                        newGame.Flag1 = SetFlag1;
                        newGame.Flag2 = SetFlag2;
                        newGame.Flag3 = SetFlag3;
                        newGame.Flag4 = SetFlag4;
                        newGame.Flag5 = SetFlag5;
                        newGame.Flag6 = SetFlag6;
                        TargetGameList.AddGame(newGame, subfolder);
                        if (DeleteFromSource)
                        {
                            SourceGameList.RemoveGame(game);
                        }
                    }
                }
            }
        }

        private void OnPrevious(object sender, RoutedEventArgs e)
        {
            Step--;
            if (Step < MinStep)
            {
                Step = MinStep;
            }
        }

        private void OnCheckedSelectAll(object sender, RoutedEventArgs e)
        {
            foreach (var gm in SourceGameList.Games)
            {
                gm.Flag7 = true;
            }
        }

        private void OnUncheckedSelectAll(object sender, RoutedEventArgs e)
        {
            foreach (var gm in SourceGameList.Games)
            {
                gm.Flag7 = false;
            }
        }
    }
}
