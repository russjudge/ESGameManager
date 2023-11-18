using ESGameManagerLibrary;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ESGameManagerLibrary
{
    /// <summary>
    /// Interaction logic for GameListControl.xaml
    /// </summary>
    public partial class GameListControl : UserControl
    {
        public GameListControl()
        {
            InitializeComponent();
        }
        public static string? RootGamesListFolder { get; set; }
       
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


        public static readonly DependencyProperty SelectedGameProperty =
           DependencyProperty.Register(
               nameof(SelectedGame),
               typeof(Game),
               typeof(GameListControl), new PropertyMetadata(OnSelectedGameChanged));

       
        private static void OnSelectedGameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is GameListControl me )
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
            if (!string.IsNullOrEmpty(RootGamesListFolder) && !string.IsNullOrEmpty(GameFolder.Folder))
            {
                RemoveEmptyFolders(System.IO.Path.Combine(RootGamesListFolder, GameFolder.Folder));
            }
        }
        private static void RemoveEmptyFolders(string root)
        {
            if (!string.IsNullOrEmpty(root))
            {
                foreach (var folder in new DirectoryInfo(root).GetDirectories())
                {
                    if (folder.GetDirectories().Length > 0)
                    {
                        RemoveEmptyFolders(folder.FullName);
                    }
                    if (folder.GetDirectories().Length == 0 && folder.GetFiles().Length == 0)
                    {
                        folder.Delete();
                    }
                }
            }
        }
        private static string GetTargetFile(Game gm, string startFolder, StructureOrganization organization)
        {
            string retVal = string.Empty;
            FileInfo currentFile = new(gm.FullPath);
            switch (organization)
            {
                case StructureOrganization.None:
                    retVal = System.IO.Path.Combine(startFolder, currentFile.Name);
                    break;
                case StructureOrganization.ByFirstLetter:
                    retVal = System.IO.Path.Combine(startFolder, Game.SetSingleLetterFolder(gm.FullPath), currentFile.Name);
                    break;
                case StructureOrganization.Publisher:
                    retVal = System.IO.Path.Combine(startFolder, Game.SetOtherFolder(gm.Publisher), currentFile.Name);
                    break;
                case StructureOrganization.ByGenre:
                    retVal = System.IO.Path.Combine(startFolder, Game.SetOtherFolder(gm.Genre), currentFile.Name);
                    break;
                case StructureOrganization.Developer:
                    retVal = System.IO.Path.Combine(startFolder, Game.SetOtherFolder(gm.Developer), currentFile.Name);
                    break;
                default:
                    retVal = System.IO.Path.Combine(startFolder, currentFile.Name);
                    break;
            }
            return retVal;
        }
        private static void MoveFile(Game gm, string startFolder, StructureOrganization organization)
        {
            FileInfo f = new(gm.FullPath);
            string newFullPath = GetTargetFile(gm, startFolder, organization);
            FileInfo newF = new(newFullPath);
            if (newF.Directory != null && !newF.Directory.Exists)
            {
                newF.Directory.Create();
            }
            f.MoveTo(newFullPath);
            gm.FullPath = newFullPath;
        }
        private void MoveFiles(StructureOrganization organization)
        {
            if (!string.IsNullOrEmpty(RootGamesListFolder) && !string.IsNullOrEmpty(GameFolder.Folder))
            {
                string startFolder = System.IO.Path.Combine(RootGamesListFolder, GameFolder.Folder);
                foreach (var gm in GameFolder.Games)
                {
                    MoveFile(gm, startFolder, organization);
                }
                GameFolder.Organization = organization;
                if (GameFolder.Changed)
                {
                    GameFolder.Save();
                }
                RemoveEmptyFolders();
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
    }
}
