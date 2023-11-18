using ESGameManagerLibrary;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Serialization;

namespace ESGameManagerLibrary
{
    [XmlRoot("gameList")]
    public class GameList : DependencyObject
    {
        const string gameListXML = "gameList.xml";
        private List<Game> DeletedGames { get; set; } = new List<Game>();
        public void RemoveGame(Game gm)
        {
            Games.Remove(gm);
            DeletedGames.Add(gm);
            Changed = true;
        }
        
        public Game? AddGame(string fullFilePath)
        {
            if (!string.IsNullOrEmpty(GameListControl.RootGamesListFolder) && !string.IsNullOrEmpty(Folder))
            {
                string startFolder = System.IO.Path.Combine(GameListControl.RootGamesListFolder, Folder);
                string relativeFolder = fullFilePath.Replace(startFolder, ".").Replace("\\", "/");
                Game gm = new Game()
                {
                    Parent = this,
                    FullPath = fullFilePath,
                    Path = relativeFolder,
                    Name = new FileInfo(fullFilePath).Name,
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
                gm.SetFullROMPath(fullFilePath);
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
        public void Save()
        {
            if (!string.IsNullOrEmpty(Folder) && !string.IsNullOrEmpty(GameListControl.RootGamesListFolder))
            {
                SortGames();
                var serializer = new XmlSerializer(typeof(GameList));

                using StringWriter writer = new();
                serializer.Serialize(writer, this);
                string xmlResult = writer.ToString();
                using (StreamWriter sw = new(Path.Combine(GameListControl.RootGamesListFolder, Folder, "gameList.xml")))
                {
                    sw.Write(xmlResult);
                }
                ProcessDeletedGames();
                Changed = false;
                MessageBox.Show("GameList.xml file saved:\r\n" + Folder);
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
            Tuple<List<Game>, IEnumerable<Game>>? parms = state as Tuple<List<Game>, IEnumerable<Game>>;
           
            if (parms != null)
            {
                foreach (var game in parms.Item1)
                {
                    game.DeleteGameFiles(parms.Item2);
                }
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
            if (Common.UIDispatcher != null && Directory.Exists(GameListControl.RootGamesListFolder))
            {
                DirectoryInfo root = new(GameListControl.RootGamesListFolder);
                XmlSerializer serializer = new(typeof(GameList));


                string xml = string.Empty;
                foreach (var dir in root.GetDirectories())
                {
                    string gameListFile = Path.Combine(dir.FullName, gameListXML);

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
        
        public static void GenerateCSV(string sourceRoot, GameList[] gameLists, string targetCSV)
        {
            string csvFile = targetCSV; //Path.Combine(root.FullName, targetName);
            if (File.Exists(csvFile))
            {
                File.Delete(csvFile);
            }
            using StreamWriter sw = new(csvFile, true);
            sw.WriteLine("\"folder\",\"System\",\"id\",\"file\",\"Name\",\"Developer\",\"Genre\",\"Publisher\",\"Release Year\",\"Flight Stick?\",\"Verified\",\"Issues\"");
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
                                wrkPath = Path.Combine(sourceRoot, folder, wrkPath.Substring(2));
                            }
                            FileInfo fi = new(wrkPath);
                            path = fi.Name;
                            actualPath = fi.FullName;
                        }

                        bool exists = File.Exists(actualPath);
                         
                        string issue = exists ? string.Empty : "3";
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
                                string.Empty,
                                string.Empty,
                                issue
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
                            string folder = gm.Path.Substring(i + 1, j - i);
                            foundByGenre = (gm.Genre == folder);
                            foundByPublisher = (gm.Publisher == folder);

                            foundByDeveloper = (gm.Developer == folder);
                        }
                    }
                    
                }
            }
            if (foundByDeveloper)
            {
                Organization = StructureOrganization.Developer;
            }
            else if (foundByLetter)
            {
                Organization = StructureOrganization.ByFirstLetter;
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
