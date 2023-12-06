using RussJudge.WPFListSorter;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;

namespace ESGameManagerLibrary
{
    /// <summary>
    /// Interaction logic for GameListControl.xaml
    /// </summary>
    public partial class GameListControl : UserControl
    {
        static GameListControl()
        {
            if (Properties.Settings.Default.UpgradeRequired)
            {
                Properties.Settings.Default.Upgrade();
                Properties.Settings.Default.UpgradeRequired = false;
                Properties.Settings.Default.Save();
            }

        }
        //bool isLoading = true;
        public GameListControl()
        {
            ValidLetterSelectionSort = true;
            NotProcessing = true;
            InitializeComponent();
            //DataContext = this;
        }
        public static string? RootGamesListFolder { get; set; }


        public static readonly DependencyProperty SelectedLetterProperty =
           DependencyProperty.Register(
               nameof(SelectedLetter),
               typeof(string),
               typeof(GameListControl), new PropertyMetadata(OnSelectedLetterChanged));

        private static void OnSelectedLetterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is GameListControl me && me.IsLoaded && !string.IsNullOrEmpty(me.SelectedLetter))
            {
                if (!me.settingSelectedLetter)
                {
                    me.settingSelectedLetter = true;
                    if (me.GameListScrollBar != null)
                    {
                        me.GameListScrollBar.ValueChanged -= me.OnScrollValueChanged;
                    }

                    if (me.lvGameList.FindStartsWith(me.SelectedLetter, true) is Game gm)
                    {
                        me.lvGameList.ScrollIntoView(gm);
                    }
                    me.settingSelectedLetter = false;
                    System.Threading.ThreadPool.QueueUserWorkItem(me.WatchOnScrollValueChanged);

                }
            }
        }
        void WatchOnScrollValueChanged(object? state)
        {
            this.Dispatcher.Invoke(() =>
            {
                if (GameListScrollBar != null)
                {
                    GameListScrollBar.ValueChanged += OnScrollValueChanged;
                }
            });
        }

        public string? SelectedLetter
        {
            get
            {
                return (string?)this.GetValue(SelectedLetterProperty);
            }

            set
            {
                this.SetValue(SelectedLetterProperty, value);
            }
        }



        public static readonly DependencyProperty GameFolderProperty =
           DependencyProperty.Register(
               nameof(GameFolder),
               typeof(GameList),
               typeof(GameListControl));
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
        public static readonly DependencyProperty NotProcessingProperty =
           DependencyProperty.Register(
               nameof(NotProcessing),
               typeof(bool),
               typeof(GameListControl));
        public bool NotProcessing
        {
            get
            {
                return (bool)this.GetValue(NotProcessingProperty);
            }

            set
            {
                this.SetValue(NotProcessingProperty, value);
            }
        }

        public static readonly DependencyProperty ValidLetterSelectionSortProperty =
           DependencyProperty.Register(
               nameof(ValidLetterSelectionSort),
               typeof(bool),
               typeof(GameListControl));
        public bool ValidLetterSelectionSort
        {
            get
            {
                return (bool)this.GetValue(ValidLetterSelectionSortProperty);
            }

            set
            {
                this.SetValue(ValidLetterSelectionSortProperty, value);
            }
        }

        public static readonly DependencyProperty ActivityProperty =
           DependencyProperty.Register(
               nameof(Activity),
               typeof(string),
               typeof(GameListControl));
        public string Activity
        {
            get
            {
                return (string)this.GetValue(ActivityProperty);
            }

            set
            {
                this.SetValue(ActivityProperty, value);
            }
        }
        public void UpdateActivity(string text)
        {
            this.Dispatcher.Invoke(() => { Activity = text; });
        }

        public static readonly DependencyProperty SelectedGameProperty =
           DependencyProperty.Register(
               nameof(SelectedGame),
               typeof(Game),
               typeof(GameListControl), new PropertyMetadata(OnSelectedGameChanged));


        private static void OnSelectedGameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is GameListControl me)
            {
                if (me.GameFolder != null)
                {
                    if (Common.DetailWindow == null)
                    {
                        Common.ShowDetailWindow();
                    }
                    if (Common.DetailWindow != null)
                    {
                        Common.DetailWindow.Games = me.GameFolder.Games;
                        Common.DetailWindow.SelectedGame = me.SelectedGame;
                    }
                }
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

        private void OnDelete(object sender, RoutedEventArgs e)
        {
            if (sender is Button b)
            {
                if (b.CommandParameter is Game gm && GameFolder.Folder != null && !string.IsNullOrEmpty(RootGamesListFolder))
                {
                    GameFolder.RemoveGame(gm);
                }
            }
        }

        private void OnSave(object sender, RoutedEventArgs e)
        {
            GameFolder.Save();
        }

        private void OnShowDescription(object sender, RoutedEventArgs e)
        {
            if (sender is Button b && b.CommandParameter is Game gm)
            {
                MessageBox.Show(gm.Description, gm.Name);
            }
        }
        private void OnEditDescription(object sender, RoutedEventArgs e)
        {
            if (sender is Button b && b.CommandParameter is Game gm)
            {
                var newText = TextEditWindow.ShowEditDialog(gm.Description, "Edit Description for " + gm.Name);
                if (newText != null)
                {
                    gm.Description = newText;
                }
            }
        }

        private void OnScan(object sender, RoutedEventArgs e)
        {
            ScanGamesWindow.ShowScanDialog(GameFolder);
        }

        private void OnAdd(object sender, RoutedEventArgs e)
        {
            var path = MetaDetailWindow.BrowseForFile("Select ROM file", MetaDetailWindow.romFilesFilter);
            if (!string.IsNullOrEmpty(path))
            {
                GameFolder.AddGame(path);
            }
        }

        private void OnEditName(object sender, RoutedEventArgs e)
        {
            if (sender is Button b && b.CommandParameter is Game gm)
            {
                var newText = TextEditWindow.ShowEditDialog(gm.Name, "Change Game Name");
                if (newText != null)
                {
                    gm.Name = newText;
                }
            }
        }

        private void OnEditReleaseDate(object sender, RoutedEventArgs e)
        {
            if (sender is Button b && b.CommandParameter is Game gm)
            {
                var newText = TextEditWindow.ShowEditDialog(gm.DateReleased.ToString("yyyy"), "Edit Release year for " + gm.Name);
                if (newText != null)
                {
                    if (int.TryParse(newText, out int yyyy))
                    {
                        gm.DateReleased = new DateTime(yyyy, 1, 1);
                    }
                }
            }
        }

        private void OnChangeImage(object sender, RoutedEventArgs e)
        {
            if (sender is Button b && b.CommandParameter is Game gm && !string.IsNullOrEmpty(gm.Parent.Folder) && !string.IsNullOrEmpty(RootGamesListFolder) && !string.IsNullOrEmpty(GameFolder.Folder))
            {

                var newText = ImageEditWindow.ShowEditDialog(gm.Parent.Folder, gm.Image, gm.FullImagePath, "Change Image for " + gm.Name);
                if (newText != null)
                {
                    gm.SetFullImagePath(newText);

                }
            }
        }

        private void OnEditNotes(object sender, RoutedEventArgs e)
        {
            if (sender is Button b && b.CommandParameter is Game gm)
            {
                var newText = TextEditWindow.ShowEditDialog(gm.Notes, "Edit Notes for " + gm.Name);
                if (newText != null)
                {
                    gm.Notes = newText;
                }
            }
        }

        /*
         * If not already in Main folder, then undo current organization
         * into main folder
         * The do organization.
         * */

        private void RemoveEmptyFolders()
        {
            string? gameFolder = null;
            if (this.Dispatcher != System.Windows.Threading.Dispatcher.CurrentDispatcher)
            {
                this.Dispatcher.Invoke(() => { gameFolder = GameFolder.Folder; });
            }
            else
            {
                gameFolder = GameFolder.Folder;
            }
            if (!string.IsNullOrEmpty(RootGamesListFolder) && !string.IsNullOrEmpty(gameFolder))
            {
                RemoveEmptyFolders(new DirectoryInfo(System.IO.Path.Combine(RootGamesListFolder, gameFolder)));
            }
        }
        private static void RemoveEmptyFolders(DirectoryInfo root)
        {
            if (root != null && root.Exists)
            {
                foreach (var folder in root.GetDirectories())
                {
                    if (folder != null)
                    {
                        RemoveEmptyFolders(folder);
                        if (folder.Exists && folder.GetDirectories().Length == 0 && folder.GetFiles().Length == 0)
                        {
                            folder.Delete();
                        }
                    }
                }
            }
        }
        private static string GetTargetFile(Game gm, string startFolder, StructureOrganization organization)
        {
            string retVal = string.Empty;
            string? fullPath = null;
            string? genre = null;
            string? publisher = null;
            string? developer = null;
            string? name = null;
            gm.Dispatcher.Invoke(() =>
            {
                fullPath = gm.FullPath;
                genre = gm.Genre;
                publisher = gm.Publisher;
                developer = gm.Developer;
                name = gm.Name;
            });
            if (!string.IsNullOrEmpty(fullPath))
            {
                FileInfo currentFile = new(fullPath);
                switch (organization)
                {
                    case StructureOrganization.None:
                        retVal = System.IO.Path.Combine(startFolder, currentFile.Name);
                        break;
                    case StructureOrganization.ByFirstLetter:
                        if (!string.IsNullOrEmpty(name))
                        {
                            retVal = System.IO.Path.Combine(startFolder, Game.SetSingleLetterFolder(name), currentFile.Name);
                        }
                        break;
                    case StructureOrganization.Publisher:
                        retVal = System.IO.Path.Combine(startFolder, Game.SetOtherFolder(publisher), currentFile.Name);

                        break;
                    case StructureOrganization.ByGenre:
                        retVal = System.IO.Path.Combine(startFolder, Game.SetOtherFolder(genre), currentFile.Name);

                        break;
                    case StructureOrganization.Developer:
                        retVal = System.IO.Path.Combine(startFolder, Game.SetOtherFolder(developer), currentFile.Name);

                        break;
                    default:
                        retVal = System.IO.Path.Combine(startFolder, currentFile.Name);
                        break;
                }
            }
            return retVal;
        }
        int moveFileCount = 0;
        private void MoveFile(Game gm, string startFolder, StructureOrganization organization)
        {
            FileInfo? f = null;
            gm.Dispatcher.Invoke(() => { f = new(gm.FullPath); });
            if (f != null)
            {
                string newFullPath = GetTargetFile(gm, startFolder, organization);
                if (f.Exists)
                {
                    FileInfo newF = new(newFullPath);
                    if (newF.Directory != null && !newF.Directory.Exists)
                    {
                        newF.Directory.Create();
                    }
                    moveFileCount++;
                    if ((moveFileCount % 50 == 0) && newF.Directory != null)
                    {
                        UpdateActivity("Moving " + f.Name + " to " + newF.Directory.Name);
                    }
                    f.MoveTo(newFullPath);
                }
                gm.Dispatcher.Invoke(() =>
                {
                    gm.FullPath = newFullPath;
                });
            }
        }
        private void MoveFiles(StructureOrganization organization)
        {
            moveFileCount = 0;
            UpdateActivity("Reorganizing files...");
            System.Threading.ThreadPool.QueueUserWorkItem(MoveFiles, organization);
        }
        private void MoveFiles(object? state)
        {
            try
            {
                if (state is StructureOrganization organization)
                {
                    string? folder = null;
                    List<Game>? games = null;
                    this.Dispatcher.Invoke(() =>
                    {
                        folder = GameFolder.Folder;
                        games = new(GameFolder.Games);
                    });
                    if (!string.IsNullOrEmpty(RootGamesListFolder) && !string.IsNullOrEmpty(folder) && games != null)
                    {
                        string startFolder = System.IO.Path.Combine(RootGamesListFolder, folder);

                        foreach (var gm in games)
                        {
                            MoveFile(gm, startFolder, organization);
                        }
                        this.Dispatcher.Invoke(() =>
                        {
                            GameFolder.Organization = organization;
                            if (GameFolder.Changed)
                            {
                                GameFolder.Save();
                            }
                        });
                        UpdateActivity("Removing empty folders.");
                        RemoveEmptyFolders();
                        UpdateActivity("Process complete.");
                    }
                }
            }
            catch (Exception ex)
            {
                Common.FatalApplicationException(ex);
            }
        }

        private void OnReorgIntoMainFolder(object sender, RoutedEventArgs e)
        {
            MoveFiles(StructureOrganization.None);
        }

        private void OnReorgByGenre(object sender, RoutedEventArgs e)
        {
            MoveFiles(StructureOrganization.ByGenre);
        }

        private void OnReorgByFirstLetter(object sender, RoutedEventArgs e)
        {
            MoveFiles(StructureOrganization.ByFirstLetter);
        }

        private void OnReorgByPublisher(object sender, RoutedEventArgs e)
        {
            MoveFiles(StructureOrganization.Publisher);
        }

        private void OnReorgByDeveloper(object sender, RoutedEventArgs e)
        {
            MoveFiles(StructureOrganization.Developer);
        }
        private void OnPrint(object sender, RoutedEventArgs e)
        {
            PrintGameList.GenerateReport(GameFolder);
            //Not working--prints blank pages.
            //Trying to switch to using Flow Document--have a ways to go.
            //PrintGameList pgl = new(GameFolder);
            //pgl.SimpleReporting(startpoint);
            //pgl.SimpleReporting();
            //pgl.Print();
        }

        private void OnMoveFile(object sender, RoutedEventArgs e)
        {
            if (sender is Button b && b.CommandParameter is Game gm)
            {
                if (!string.IsNullOrEmpty(RootGamesListFolder) && !string.IsNullOrEmpty(GameFolder.Folder))
                {
                    string topFolder = System.IO.Path.Combine(RootGamesListFolder, GameFolder.Folder);
                    var dialog = new Microsoft.Win32.OpenFolderDialog
                    {
                        Title = "Select folder to move the ROM file to.",
                        InitialDirectory = topFolder,

                    };

                    dialog.RootDirectory = topFolder;
                    if (dialog.ShowDialog() == true)
                    {
                        string targetFolder = dialog.FolderName;
                        if (!targetFolder.StartsWith(topFolder))
                        {
                            MessageBox.Show(string.Format("Can only move file to within the selected system folder ({0}).", GameFolder.Folder), "Invalid move", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else
                        {
                            System.IO.FileInfo sourcePath = new(gm.FullPath);
                            string targetName = System.IO.Path.Combine(targetFolder, sourcePath.Name);
                            sourcePath.MoveTo(targetName);
                            gm.FullPath = targetName;
                            GameFolder.Organization = StructureOrganization.None;
                            GameFolder.Save();

                        }
                    }
                }
            }

        }

        public static T? GetVisualChild<T>(Visual referenceVisual) where T : Visual
        {
            Visual? child = null;
            for (Int32 i = 0; i < VisualTreeHelper.GetChildrenCount(referenceVisual); i++)
            {
                child = VisualTreeHelper.GetChild(referenceVisual, i) as Visual;
                if (child != null && child is T)
                {
                    break;
                }
                else if (child != null)
                {
                    child = GetVisualChild<T>(child);
                    if (child != null && child is T)
                    {
                        break;
                    }
                }
            }
            return child as T;
        }

        bool settingSelectedLetter = false;
        void OnScrollValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (!settingSelectedLetter)
            {
                //if (sender is ListView listView)
                //{
                ScrollViewer? scrollViewer = GetVisualChild<ScrollViewer>(lvGameList);
                if (scrollViewer != null)
                {
                    settingSelectedLetter = true;
                    var newTopItem = scrollViewer.VerticalOffset;
                    var itemCountvisibile = scrollViewer.ViewportHeight;
                    int index = (int)newTopItem;
                    Game? gm = null;
                    if (CollectionViewSource.GetDefaultView(lvGameList.ItemsSource) is ListCollectionView dataView)
                    {
                        var movesuccess = dataView.MoveCurrentToPosition(index);
                        gm = (Game)dataView.CurrentItem;
                    }

                    if (gm != null)
                    {
                        var sortColumn = Sorter.GetCurrentSortColumn(lvGameList);
                        if (sortColumn != null)
                        {

                            string? field = Sorter.GetSortColumnID(sortColumn);

                            if (!string.IsNullOrEmpty(field))
                            {
                                PropertyInfo? matchProperty = null;
                                foreach (PropertyInfo p in lvGameList.Items[0].GetType().GetProperties())
                                {
                                    if (p.Name.Equals(field, StringComparison.InvariantCultureIgnoreCase))
                                    {
                                        matchProperty = p;
                                        break;
                                    }
                                }
                                if (matchProperty != null)
                                {
                                    var val = matchProperty.GetValue(gm);
                                    if (val != null)
                                    {
                                        var thematchValue = val.ToString();
                                        if (!string.IsNullOrEmpty(thematchValue) && (SelectedLetter == null || !thematchValue.StartsWith(SelectedLetter)))
                                        {
                                            SelectedLetter = thematchValue[0].ToString();
                                        }
                                    }
                                }
                            }
                        }
                    }
                    //}
                }
                settingSelectedLetter = false;
            }
        }
        ScrollBar? GameListScrollBar = null;
        private void OnGameListViewLoaded(object sender, RoutedEventArgs e)
        {


            if (sender is ListView listView)
            {
                ScrollViewer? scrollViewer = GetVisualChild<ScrollViewer>(listView);
                if (scrollViewer != null)
                {
                    if (scrollViewer.Template.FindName("PART_VerticalScrollBar", scrollViewer) is ScrollBar scrollBar)
                    {
                        GameListScrollBar = scrollBar;
                        GameListScrollBar.ValueChanged += OnScrollValueChanged;
                        /*
                        (object sender, RoutedPropertyChangedEventArgs<double> e) =>
                        {
                            if (!settingSelectedLetter)
                            {
                                settingSelectedLetter = true;
                                var newTopItem = scrollViewer.VerticalOffset;
                                var itemCountvisibile = scrollViewer.ViewportHeight;
                                int index = (int)newTopItem;
                                Game gm = GameFolder.Games[index];
                                if (gm != null)
                                {
                                    var sortColumn = GridViewColumnHeaderSorter.GetCurrentSortColumn(listView);
                                    if (sortColumn != null)
                                    {

                                        string? field = GridViewColumnHeaderSorter.GetSortColumnID(sortColumn);

                                        if (!string.IsNullOrEmpty(field) && CollectionViewSource.GetDefaultView(listView.ItemsSource) is ListCollectionView dataView)
                                        {
                                            PropertyInfo? matchProperty = null;
                                            foreach (ItemPropertyInfo p in dataView.ItemProperties)
                                            {
                                                if (p.Name.ToUpperInvariant() == field.ToUpperInvariant())
                                                {
                                                    matchProperty = dataView.CurrentItem.GetType().GetProperty(p.Name);
                                                    break;
                                                }
                                            }
                                            if (matchProperty != null)
                                            {
                                                var val = matchProperty.GetValue(gm);
                                                if (val != null)
                                                {
                                                    var thematchValue = val.ToString();
                                                    if (!string.IsNullOrEmpty(thematchValue) && (SelectedLetter == null || !thematchValue.StartsWith(SelectedLetter)))
                                                    {
                                                        SelectedLetter = thematchValue[0].ToString();
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                settingSelectedLetter = false;
                            }
                        };
                        */
                    }
                }
            }

        }

        private void OnChangeSort(object sender, RoutedEventArgs e)
        {
            if (sender is DependencyObject d)
            {

                ValidLetterSelectionSort = (Sorter.GetSortColumnID(d) == "Name");
            }
        }

        /*
* 
* Print table of data:
* 
*  <DataGrid AutoGenerateColumns="False" Margin="12,0,0,0" Name="dataGrid1"  HorizontalAlignment="Left"  VerticalAlignment="Top"  ItemsSource="{Binding}" AlternatingRowBackground="LightGoldenrodYellow" AlternationCount="1">
<DataGrid.Columns>
<DataGridTemplateColumn Header="Image" Width="SizeToCells" IsReadOnly="True">
<DataGridTemplateColumn.CellTemplate>
<DataTemplate>
<Image Source="{Binding Path=Image}" Width="100" Height="50" />
</DataTemplate>
</DataGridTemplateColumn.CellTemplate>
</DataGridTemplateColumn>


<DataGridTextColumn Header="Make" Binding="{Binding Path=Make}"/>
<DataGridTextColumn Header="Model" Binding="{Binding Path=Model}"/>
<DataGridTextColumn Header="Price" Binding="{Binding Path=Price}"/>
<DataGridTextColumn Header="Color" Binding="{Binding Path=Color}"/>
</DataGrid.Columns>
</DataGrid>


private void OnDataGridPrinting(object sender, RoutedEventArgs e)
{
System.Windows.Controls.PrintDialog Printdlg = new System.Windows.Controls.PrintDialog();
if ((bool)Printdlg.ShowDialog().GetValueOrDefault())
{
Size pageSize = new Size(Printdlg.PrintableAreaWidth, Printdlg.PrintableAreaHeight);
// sizing of the element.
dataGrid1.Measure(pageSize);
dataGrid1.Arrange(new Rect(5, 5, pageSize.Width, pageSize.Height));
Printdlg.PrintVisual(dataGrid1, Title);
}

Other Info???: https://itecnote.com/tecnote/r-printing-a-wpf-flowdocument/
}
* */

        //private void PrintList()

        //{
        //    FlowDocument fd = new FlowDocument();

        //    //TaskViewModel.Tasks is the collection (List<> in my case) your ListView takes data from
        //    foreach (var item in TaskViewModel.Tasks)
        //    {
        //        fd.Blocks.Add(new Paragraph(new Run(item.ToString()))); //you may need to create a ToString method in your type, if it's string it's ok
        //    }

        //    PrintDialog pd = new PrintDialog();
        //    if (pd.ShowDialog() != true) return;

        //    fd.PageHeight = pd.PrintableAreaHeight;
        //    fd.PageWidth = pd.PrintableAreaWidth;

        //    IDocumentPaginatorSource idocument = fd as IDocumentPaginatorSource;

        //    pd.PrintDocument(idocument.DocumentPaginator, "Printing Flow Document...");
        //}

    }
}
