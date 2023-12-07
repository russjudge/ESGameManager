using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

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
            DataContext = this;
        }
        public static readonly DependencyProperty ShowListProperty =
          DependencyProperty.Register(
              nameof(ShowList),
              typeof(bool),
              typeof(MetaDetailWindow));


        public bool ShowList
        {
            get
            {
                return (bool)this.GetValue(ShowListProperty);
            }

            set
            {
                this.SetValue(ShowListProperty, value);
            }
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
                if (me.SelectedGame == null)
                {
                    //me.Close();
                }
                else
                {
                    int i = me.Games.IndexOf(me.SelectedGame);
                    me.PreviousEnabled = i > 0;
                    me.NextEnabled = i < me.Games.Count - 1;
                }
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
            try
            {
                if (Common.UIDispatcher != null && Common.DetailWindow != null)
                {
                    Common.UIDispatcher.Invoke(() =>
                    {
                        if (Common.DetailWindow.SelectedGame != null)
                        {
                            Common.DetailWindow.Activate();
                            Common.DetailWindow.Focus();
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                Common.FatalApplicationException(ex);
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

        private void OnClosed(object sender, EventArgs e)
        {
            if (Common.DetailWindow == this)
            {
                Common.DetailWindow = null;
            }
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

        private void OnDeleteGame(object sender, RoutedEventArgs e)
        {
            if (sender is Button b && b.CommandParameter is Game gm)
            {
                gm.Parent.RemoveGame(gm);
                Games.Remove(gm);
            }
        }
    }
}
