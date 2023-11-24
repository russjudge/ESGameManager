using ESGameManagerLibrary;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace ESGameManagerLibrary
{
    [XmlRoot("gameList")]
    public class GameList : DependencyObject
    {
        static GameList()
        {
            Letters = new();
            for (int i = (int)'A'; i <= (int)'Z'; i++)
            {
                Letters.Add(((char)i).ToString());
            }
        }
        const string gameListXML = "gameList.xml";
        public static ObservableCollection<string> Letters { get; private set; }
        private List<Game> DeletedGames { get; set; } = new List<Game>();
        public void RemoveGame(Game gm)
        {
            Games.Remove(gm);
            DeletedGames.Add(gm);
            Changed = true;
        }
        public string? GetListRoot()
        {
            string? startFolder = null;
            if (!string.IsNullOrEmpty(GameListControl.RootGamesListFolder) && !string.IsNullOrEmpty(Folder))
            {
                startFolder = System.IO.Path.Combine(GameListControl.RootGamesListFolder, Folder);
            }
            return startFolder;
        }
        public void AddGame(Game game, string targetSubfolder = "")
        {
            //FullPath, FullImagePath, FullMarqueePath, FullVideoPath
            if (string.IsNullOrEmpty(targetSubfolder))
            {
                game.SetFullROMPath(game.FullPath);
            }
            else
            {
                game.FullPath = game.SetFileLocation(game.FullPath, targetSubfolder, false);
            }
            if (!string.IsNullOrEmpty(game.FullImagePath))
            {
                game.SetFullImagePath(game.FullImagePath);
            }
            if (!string.IsNullOrEmpty(game.FullMarqueePath))
            {
                game.SetFullMarqueePath(game.FullMarqueePath);
            }
            if (!string.IsNullOrEmpty(game.FullVideoPath))
            {
                game.SetFullVideoPath(game.FullVideoPath);
            }
            Games.Add(game);
            Changed = true;
        }
        
        public Game? AddGame(string sourcefullFilePath)
        {
            if (!string.IsNullOrEmpty(GameListControl.RootGamesListFolder) && !string.IsNullOrEmpty(Folder))
            {
                string startFolder = System.IO.Path.Combine(GameListControl.RootGamesListFolder, Folder);
                

                string relativeFolder = sourcefullFilePath.Replace(startFolder, ".").Replace("\\", "/");
                Game gm = new Game()
                {
                    Flags = 0,
                    Parent = this,
                    FullPath = sourcefullFilePath,
                    Path = relativeFolder,
                    Name = new FileInfo(sourcefullFilePath).Name,
                    Source = string.Empty,
                    Description = string.Empty,
                    Rating = string.Empty,
                    ReleaseDate = "00010101T00000",
                    DateReleased = DateTime.MinValue,
                    Developer = string.Empty,
                    Publisher = string.Empty,
                    Genre = string.Empty,
                    Players = string.Empty,
                    Image = string.Empty,
                    FullImagePath = string.Empty,
                    GenreId = string.Empty,
                };
                gm.SetFullROMPath(sourcefullFilePath);
                Games.Add(gm);
                Changed = true;
                return gm;
            }
            else
            {
                return null;
            }
        }
        [XmlElement("provider")]
        public required Provider Provider { get; set; }

        public void SortGames()
        {
            switch(Organization)
            {
                case StructureOrganization.None:
                    Games = new ObservableCollection<Game>(Games.OrderBy(game => game.Name));
                    break;
                case StructureOrganization.ByGenre:
                    Games = new ObservableCollection<Game>(Games.OrderBy(game => Game.SetOtherFolder(game.Genre)).ThenBy(game => game.Name));
                    break;
                case StructureOrganization.ByFirstLetter:
                    Games = new ObservableCollection<Game>(Games.OrderBy(game => Game.SetSingleLetterFolder(game.Path)).ThenBy(game => game.Name));
                    break;
                case StructureOrganization.Publisher:
                    Games = new ObservableCollection<Game>(Games.OrderBy(game => Game.SetOtherFolder(game.Publisher)).ThenBy(game => game.Name));
                    break;
                case StructureOrganization.Developer:
                    Games = new ObservableCollection<Game>(Games.OrderBy(game => Game.SetOtherFolder(game.Developer)).ThenBy(game => game.Name));
                    break;
            }
            
        }

        public static readonly DependencyProperty GamesProperty =
          DependencyProperty.Register(
              nameof(Games),
              typeof(ObservableCollection<Game>),
              typeof(GameList));

        [XmlElement("game")]
        public required ObservableCollection<Game> Games
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
        public static readonly DependencyProperty ChangedProperty =
         DependencyProperty.Register(
             nameof(Changed),
             typeof(bool),
             typeof(GameList));
        [XmlIgnore]
        public bool Changed
        {
            get
            {
                return (bool)this.GetValue(ChangedProperty);
            }

            set
            {
                this.SetValue(ChangedProperty, value);
            }
        }
        public static readonly DependencyProperty FolderProperty =
          DependencyProperty.Register(
              nameof(Folder),
              typeof(string),
              typeof(GameList));
        [XmlIgnore]
        public string? Folder
        {
            get
            {
                return (string)this.GetValue(FolderProperty);
            }

            set
            {
                this.SetValue(FolderProperty, value);
            }
        }
        public static readonly DependencyProperty OrganizationProperty =
         DependencyProperty.Register(
             nameof(Organization),
             typeof(StructureOrganization),
             typeof(GameList));
        [XmlIgnore]
        public StructureOrganization Organization
        {
            get
            {
                return (StructureOrganization)this.GetValue(OrganizationProperty);
            }

            set
            {
                this.SetValue(OrganizationProperty, value);
            }
        }
       
        public void Save(bool suppressConfirmation = false)
        {
            if (!string.IsNullOrEmpty(Folder) && !string.IsNullOrEmpty(GameListControl.RootGamesListFolder))
            {
                SortGames();
                var serializer = new XmlSerializer(typeof(GameList));

                using StringWriter writer = new();
                serializer.Serialize(writer, this);
                string xmlResult = writer.ToString();
                using (StreamWriter sw = new(System.IO.Path.Combine(GameListControl.RootGamesListFolder, Folder, "gameList.xml")))
                {
                    sw.Write(xmlResult);
                }
                ProcessDeletedGames();
                Changed = false;
                if (!suppressConfirmation)
                {
                    MessageBox.Show("GameList.xml file saved:\r\n" + Folder);
                }
            }
        }
        private void ProcessDeletedGames()
        {
            List<Game> deleted = new List<Game>(DeletedGames);
            List<Game> games = new List<Game>(Games);
            Tuple<List<Game>, IEnumerable<Game>> parms = new(deleted, games);
            System.Threading.ThreadPool.QueueUserWorkItem(ProcessDeletedGames, parms);
            DeletedGames.Clear();
        }
        private void ProcessDeletedGames(object? state)
        {
            try
            {
                Tuple<List<Game>, IEnumerable<Game>>? parms = state as Tuple<List<Game>, IEnumerable<Game>>;

                if (parms != null)
                {
                    foreach (var game in parms.Item1)
                    {
                        game.DeleteGameFiles(parms.Item2);
                    }
                }
            }
            catch (Exception ex)
            {
                Common.FatalApplicationException(ex);
            }
        }

        public static void LoadGameListFiles()
        {
            System.Threading.ThreadPool.QueueUserWorkItem(LoadGameListFiles);
        }
        public static string GetFullPath(string relativePath, string startFolder)
        {
            return relativePath.Replace("./", startFolder + "/").Replace("/", "\\");
        }
        public static event EventHandler<NewGameListEventArgs>? NewGameList;
        private static void LoadGameListFiles(object? state)
        {
            try
            {
                
                if (Common.UIDispatcher != null && Directory.Exists(GameListControl.RootGamesListFolder))
                {
                    DirectoryInfo root = new(GameListControl.RootGamesListFolder);
                    XmlSerializer serializer = new(typeof(GameList));


                    string xml = string.Empty;
                    foreach (var dir in root.GetDirectories())
                    {
                        string gameListFile = System.IO.Path.Combine(dir.FullName, gameListXML);

                        if (File.Exists(gameListFile))
                        {
                            FileInfo f = new(gameListFile);
                            if (!string.IsNullOrEmpty(f.DirectoryName) && f.Directory != null)
                            {
                                string folder = f.Directory.Name;
                                using (StreamReader sr = new(gameListFile))
                                {
                                    xml = sr.ReadToEnd();
                                }

                                using StringReader reader = new(xml);
                                GameList? gameList = null;
                                Common.UIDispatcher.Invoke(() =>
                                {
                                    gameList = (GameList?)serializer.Deserialize(reader);
                                    if (gameList != null)
                                    {
                                        gameList.Folder = dir.Name;
                                        foreach (var game in gameList.Games)
                                        {
                                            game.Parent = gameList;

                                            game.FullPath = GetFullPath(game.Path, dir.FullName);
                                            if (!string.IsNullOrEmpty(game.Image))
                                            {
                                                game.FullImagePath = GetFullPath(game.Image, dir.FullName);
                                            }
                                            if (!string.IsNullOrEmpty(game.Marquee))
                                            {
                                                game.FullMarqueePath = GetFullPath(game.Marquee, dir.FullName);
                                            }
                                            if (!string.IsNullOrEmpty(game.Video))
                                            {
                                                game.FullVideoPath = GetFullPath(game.Video, dir.FullName);
                                            }
                                            game.IsLoading = false;
                                        }
                                        gameList.DetermineOrganization();
                                        gameList.Changed = false;
                                    }
                                });

                                if (gameList != null)
                                {
                                    NewGameList?.Invoke(null, new(gameList));
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.FatalApplicationException(ex);
            }
        }

        public void Print()
        {
            throw new NotImplementedException();
        }
        public static void GenerateCSV(string sourceRoot, GameList[] gameLists, string targetCSV)
        {
            string csvFile = targetCSV;
            if (File.Exists(csvFile))
            {
                File.Delete(csvFile);
            }
            using StreamWriter sw = new(csvFile, true);
            sw.Write("\"ROM Folder\",\"System\",\"id\",\"file\",\"Name\",\"Developer\",\"Genre\",\"Publisher\",\"Release Year\",\"Notes\"");
            sw.Write(",\"{0}\"", Properties.Settings.Default.Flag1);
            sw.Write(",\"{0}\"", Properties.Settings.Default.Flag2);
            sw.Write(",\"{0}\"", Properties.Settings.Default.Flag3);
            sw.Write(",\"{0}\"", Properties.Settings.Default.Flag4);
            sw.Write(",\"{0}\"", Properties.Settings.Default.Flag5);
            sw.WriteLine(",\"{0}\"", Properties.Settings.Default.Flag6);
            foreach (var gamelist in gameLists)
            {
                if (gamelist != null)
                {
                    foreach (var game in gamelist.Games)
                    {
                        string yr = "";
                        if (game.ReleaseDate?.Length >= 4)
                        {
                            yr = game.ReleaseDate.Substring(0, 4);
                        }
                        string path = string.Empty;
                        string actualPath = string.Empty;
                        string folder = string.Empty;
                        if (!string.IsNullOrEmpty(gamelist.Folder))
                        {
                            folder = gamelist.Folder;
                        }
                        if (!string.IsNullOrEmpty(game.Path))
                        {
                            string wrkPath = game.Path;
                            if (wrkPath.StartsWith("./"))
                            {
                                wrkPath = System.IO.Path.Combine(sourceRoot, folder, wrkPath.Substring(2));
                            }
                            FileInfo fi = new(wrkPath);
                            path = fi.Name;
                            actualPath = fi.FullName;
                        }

                        List<string> columns = new()
                    {
                                folder,
                                gamelist.Provider.System,
                                game.Id.ToString(),
                                path,
                                game.Name,
                                game.Developer,
                                game.Genre,
                                game.Publisher,
                                yr,
                                game.Notes,
                                game.Flag1 ? Properties.Settings.Default.Flag1Symbol : string.Empty,
                                game.Flag2 ? Properties.Settings.Default.Flag2Symbol : string.Empty,
                                game.Flag3 ? Properties.Settings.Default.Flag3Symbol : string.Empty,
                                game.Flag4 ? Properties.Settings.Default.Flag4Symbol : string.Empty,
                                game.Flag5 ? Properties.Settings.Default.Flag5Symbol : string.Empty,
                                game.Flag6 ? Properties.Settings.Default.Flag6Symbol : string.Empty
                            };
                        sw.WriteLine("\"" + String.Join("\",\"", columns.ToArray()) + "\"");
                    }
                }
            }
        }
        public void DetermineOrganization()
        {
            Organization = StructureOrganization.None;
            bool foundByPublisher = false;
            bool foundByDeveloper = false;
            bool foundByLetter = false;
            bool foundByGenre = false;
            foreach(var gm in Games)
            {
                int i = gm.Path.IndexOf('/');
                if (i >= 0)
                {
                    int j = gm.Path.IndexOf('/', i + 1);
                    if (j >= 0)
                    {
                        if (j == i + 2)
                        {
                            foundByLetter = true;
                        }
                        else
                        {
                            foundByLetter = false;
                            string folder = gm.Path.Substring(i + 1, j - (i+1));
                            foundByGenre = (Game.GetRelativeFolderPath(gm.Genre, false) == folder);
                            foundByPublisher = (Game.GetRelativeFolderPath(gm.Publisher, false) == folder);
                            foundByDeveloper = (Game.GetRelativeFolderPath(gm.Developer, false) == folder);
                            break;
                        }
                    }
                }
            }
            if (foundByLetter)
            {
                Organization = StructureOrganization.ByFirstLetter;
            }
            else if (foundByDeveloper)
            {
                Organization = StructureOrganization.Developer;
            }
            else if (foundByGenre)
            {
                Organization = StructureOrganization.ByGenre;
            }
            else if (foundByPublisher)
            {
                Organization = StructureOrganization.Publisher;
            }
            else
            {
                Organization = StructureOrganization.None;
            }
        }
    }
}