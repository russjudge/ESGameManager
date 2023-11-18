using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;
namespace ESGameManagerLibrary
{
    public class Game : DependencyObject
    {
       
        public static readonly DependencyProperty IdProperty =
         DependencyProperty.Register(
             nameof(Id),
             typeof(int),
             typeof(Game), new PropertyMetadata(OnPropertyValueChanged));

        private static void OnPropertyValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Game gm && gm.Parent != null)
            {
                if (!gm.Parent.Changed && e.OldValue != e.NewValue)
                {
                    gm.Parent.Changed = true;
                }
            }
        }
        [XmlIgnore]
        public bool IsLoading { get; set; } = true;
        [XmlAttribute("id")]
        public int Id
        {
            get
            {
                return (int)this.GetValue(IdProperty);
            }

            set
            {
                this.SetValue(IdProperty, value);
            }
        }

        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register(
            nameof(Source),
            typeof(string),
            typeof(Game), new PropertyMetadata(OnPropertyValueChanged));
        [XmlAttribute("source")]
        public required string Source
        {
            get
            {
                return (string)this.GetValue(SourceProperty);
            }

            set
            {
                this.SetValue(SourceProperty, value);
            }
        }

        public static readonly DependencyProperty FullPathProperty =
           DependencyProperty.Register(
           nameof(FullPath),
           typeof(string),
           typeof(Game), new PropertyMetadata(OnFullPathChanged));

        private static void OnFullPathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Game gm && !gm.IsLoading && !string.IsNullOrEmpty(gm.Parent.Folder) && !string.IsNullOrEmpty(GameListControl.RootGamesListFolder))
            {
                string startFolder = System.IO.Path.Combine(GameListControl.RootGamesListFolder, gm.Parent.Folder);
                gm.Path = gm.FullPath.Replace(startFolder, ".").Replace("\\", "/");
            }
        }
        
        [XmlIgnore()]
        public required string FullPath
        {
            get
            {
                return (string)this.GetValue(FullPathProperty);
            }

            set
            {
                this.SetValue(FullPathProperty, value);
            }
        }
        public static readonly DependencyProperty PathProperty =
            DependencyProperty.Register(
            nameof(Path),
            typeof(string),
            typeof(Game), new PropertyMetadata(OnPropertyValueChanged));

        [XmlElement("path")]
        public required string Path
        {
            get
            {
                return (string)this.GetValue(PathProperty);
            }

            set
            {
                this.SetValue(PathProperty, value);
            }
        }

        public static readonly DependencyProperty ParentProperty =
            DependencyProperty.Register(
            nameof(Parent),
            typeof(GameList),
            typeof(Game));

        [XmlIgnore]
        public required GameList Parent
        {
            get
            {
                return (GameList)this.GetValue(ParentProperty);
            }

            set
            {
                this.SetValue(ParentProperty, value);
            }
        }


        public static readonly DependencyProperty NameProperty =
            DependencyProperty.Register(
            nameof(Name),
            typeof(string),
            typeof(Game), new PropertyMetadata(OnPropertyValueChanged));

        [XmlElement("name")]
        public required string Name
        {
            get
            {
                return (string)this.GetValue(NameProperty);
            }

            set
            {
                this.SetValue(NameProperty, value);
            }
        }

        public static readonly DependencyProperty DescriptionProperty =
            DependencyProperty.Register(
            nameof(Description),
            typeof(string),
            typeof(Game), new PropertyMetadata(OnPropertyValueChanged));
        [XmlElement("desc")]
        public required string Description
        {
            get
            {
                return (string)this.GetValue(DescriptionProperty);
            }

            set
            {
                this.SetValue(DescriptionProperty, value);
            }
        }

        public static readonly DependencyProperty RatingProperty =
            DependencyProperty.Register(
            nameof(Rating),
            typeof(string),
            typeof(Game), new PropertyMetadata(OnRatingChanged));

        private static void OnRatingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Game gm && double.TryParse(gm.Rating, out double rating))
            {
                gm.Rating = Math.Round(rating, 2).ToString();
            }
            OnPropertyValueChanged(d, e);
        }

        [XmlElement("rating")]
        public required string Rating
        {
            get
            {
                return (string)this.GetValue(RatingProperty);
            }

            set
            {
                this.SetValue(RatingProperty, value);
            }
        }
        private bool dateBeingSet = false;
        public static readonly DependencyProperty ReleaseDateProperty =
            DependencyProperty.Register(
            nameof(ReleaseDate),
            typeof(string),
            typeof(Game), new PropertyMetadata(OnReleaseDateChanged));

        private static void OnReleaseDateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            OnPropertyValueChanged(d, e);
            if (d is Game gm)
            {
                if (!gm.dateBeingSet && gm.ReleaseDate != null && gm.ReleaseDate.Length > 7)
                {
                    gm.dateBeingSet = true;
                    try
                    {
                        //if (int.TryParse(gm.ReleaseDate.Substring(0, 4), out int y)
                        //    && int.TryParse(gm.ReleaseDate.Substring(4, 2), out int m)
                        //    && int.TryParse(gm.ReleaseDate.Substring(4, 2), out int day))
                            if (int.TryParse(gm.ReleaseDate.AsSpan(0, 4), out int y)
                            && int.TryParse(gm.ReleaseDate.AsSpan(4, 2), out int m)
                            && int.TryParse(gm.ReleaseDate.AsSpan(4, 2), out int day))
                        {
                            DateTime dt = new(y, m, day);
                            gm.DateReleased = dt;
                        }
                    }
                    catch
                    {

                    }
                    gm.dateBeingSet = false;
                }
            }
        }

        [XmlElement("releasedate")]
        public required string ReleaseDate
        {
            get
            {
                return (string)this.GetValue(ReleaseDateProperty);
            }

            set
            {
                this.SetValue(ReleaseDateProperty, value);
            }
        }


        public static readonly DependencyProperty DateReleasedProperty =
            DependencyProperty.Register(
            nameof(DateReleased),
            typeof(DateTime),
            typeof(Game), new PropertyMetadata(OnDateReleasedChanged));

        private static void OnDateReleasedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Game gm)
            {
                if (!gm.dateBeingSet)
                {
                    gm.dateBeingSet = true;
                    gm.ReleaseDate = gm.DateReleased.ToString("yyyyMMdd") + "T000000";
                    gm.dateBeingSet = false;
                }
            }
        }
        [XmlIgnore]
        public required DateTime DateReleased
        {
            get
            {
                return (DateTime)this.GetValue(DateReleasedProperty);
            }

            set
            {
                this.SetValue(DateReleasedProperty, value);
            }
        }

        public static readonly DependencyProperty DeveloperProperty =
            DependencyProperty.Register(
            nameof(Developer),
            typeof(string),
            typeof(Game), new PropertyMetadata(OnPropertyValueChanged));

        [XmlElement("developer")]
        public required string Developer
        {
            get
            {
                return (string)this.GetValue(DeveloperProperty);
            }

            set
            {
                this.SetValue(DeveloperProperty, value);
            }
        }
        public static readonly DependencyProperty PublisherProperty =
            DependencyProperty.Register(
            nameof(Publisher),
            typeof(string),
            typeof(Game), new PropertyMetadata(OnPropertyValueChanged));

        [XmlElement("publisher")]
        public required string Publisher
        {
            get
            {
                return (string)this.GetValue(PublisherProperty);
            }

            set
            {
                this.SetValue(PublisherProperty, value);
            }
        }

        public static readonly DependencyProperty FavoriteProperty =
            DependencyProperty.Register(
            nameof(Favorite),
            typeof(string),
            typeof(Game), new PropertyMetadata(OnPropertyValueChanged));

        [XmlElement("favorite")]
        public string? Favorite
        {
            get
            {
                return (string?)this.GetValue(FavoriteProperty);
            }

            set
            {
                this.SetValue(FavoriteProperty, value);
            }
        }

        public static readonly DependencyProperty GenreProperty =
            DependencyProperty.Register(
            nameof(Genre),
            typeof(string),
            typeof(Game), new PropertyMetadata(OnPropertyValueChanged));

        [XmlElement("genre")]
        public required string Genre
        {
            get
            {
                return (string)this.GetValue(GenreProperty);
            }

            set
            {
                this.SetValue(GenreProperty, value);
            }
        }


        public static readonly DependencyProperty HiddenProperty =
            DependencyProperty.Register(
            nameof(Hidden),
            typeof(string),
            typeof(Game), new PropertyMetadata(OnPropertyValueChanged));

        [XmlElement("hidden")]
        public string? Hidden
        {
            get
            {
                return (string?)this.GetValue(HiddenProperty);
            }

            set
            {
                this.SetValue(HiddenProperty, value);
            }
        }

        public static readonly DependencyProperty KidGameProperty =
            DependencyProperty.Register(
            nameof(KidGame),
            typeof(string),
            typeof(Game), new PropertyMetadata(OnPropertyValueChanged));

        [XmlElement("kidgame")]
        public string? KidGame
        {
            get
            {
                return (string?)this.GetValue(KidGameProperty);
            }

            set
            {
                this.SetValue(KidGameProperty, value);
            }
        }


        public static readonly DependencyProperty LastPlayedProperty =
            DependencyProperty.Register(
            nameof(LastPlayed),
            typeof(string),
            typeof(Game), new PropertyMetadata(OnPropertyValueChanged));

        [XmlElement("lastplayed")]
        public string? LastPlayed
        {
            get
            {
                return (string?)this.GetValue(LastPlayedProperty);
            }

            set
            {
                this.SetValue(LastPlayedProperty, value);
            }
        }


        public static readonly DependencyProperty PlayCountProperty =
            DependencyProperty.Register(
            nameof(PlayCount),
            typeof(string),
            typeof(Game), new PropertyMetadata(OnPropertyValueChanged));

        [XmlElement("playcount")]
        public string? PlayCount
        {
            get
            {
                return (string?)this.GetValue(PlayCountProperty);
            }

            set
            {
                this.SetValue(PlayCountProperty, value);
            }
        }



        public static readonly DependencyProperty PlayersProperty =
            DependencyProperty.Register(
            nameof(Players),
            typeof(string),
            typeof(Game), new PropertyMetadata(OnPropertyValueChanged));

        [XmlElement("players")]
        public required string Players
        {
            get
            {
                return (string)this.GetValue(PlayersProperty);
            }

            set
            {
                this.SetValue(PlayersProperty, value);
            }
        }
        public static readonly DependencyProperty ImageProperty =
            DependencyProperty.Register(
            nameof(Image),
            typeof(string),
            typeof(Game), new PropertyMetadata(OnPropertyValueChanged));

        private static void OnImageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Game gm && !string.IsNullOrEmpty(gm.Parent.Folder) && !string.IsNullOrEmpty(GameListControl.RootGamesListFolder))
            {
                string startFolder = System.IO.Path.Combine(GameListControl.RootGamesListFolder, gm.Parent.Folder);
                gm.Image = gm.FullImagePath.Replace(startFolder, ".").Replace("\\", "/");
            }
        }

        [XmlElement("image")]
        public required string Image
        {
            get
            {
                return (string)this.GetValue(ImageProperty);
            }

            set
            {
                this.SetValue(ImageProperty, value);
            }
        }
        
        public static readonly DependencyProperty FullImagePathProperty =
           DependencyProperty.Register(
           nameof(FullImagePath),
           typeof(string),
           typeof(Game), new PropertyMetadata(OnImageChanged));
        [XmlIgnore]
        public required string FullImagePath
        {
            get
            {
                return (string)this.GetValue(FullImagePathProperty);
            }

            set
            {
                this.SetValue(FullImagePathProperty, value);
            }
        }

        /// <summary>
        /// /////////////
        /// </summary>
        public static readonly DependencyProperty VideoProperty =
            DependencyProperty.Register(
            nameof(Video),
            typeof(string),
            typeof(Game), new PropertyMetadata(OnPropertyValueChanged));

        private static void OnVideoChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Game gm
                && !string.IsNullOrEmpty(gm.Parent.Folder) 
                && !string.IsNullOrEmpty(GameListControl.RootGamesListFolder)
                && !string.IsNullOrEmpty(gm.FullVideoPath))
            {
                string startFolder = System.IO.Path.Combine(GameListControl.RootGamesListFolder, gm.Parent.Folder);
                gm.Video = gm.FullVideoPath.Replace(startFolder, ".").Replace("\\", "/");
            }
        }

        [XmlElement("video")]
        public string? Video
        {
            get
            {
                return (string?)this.GetValue(VideoProperty);
            }

            set
            {
                this.SetValue(VideoProperty, value);
            }
        }

        public static readonly DependencyProperty FullVideoPathProperty =
           DependencyProperty.Register(
           nameof(FullVideoPath),
           typeof(string),
           typeof(Game), new PropertyMetadata(OnVideoChanged));
        [XmlIgnore]
        public string? FullVideoPath
        {
            get
            {
                return (string?)this.GetValue(FullVideoPathProperty);
            }

            set
            {
                this.SetValue(FullVideoPathProperty, value);
            }
        }


        /// <summary>
        /// /////////////
        /// </summary>
        public static readonly DependencyProperty MarqueeProperty =
            DependencyProperty.Register(
            nameof(Marquee),
            typeof(string),
            typeof(Game), new PropertyMetadata(OnPropertyValueChanged));

        private static void OnMarqueeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Game gm 
                && !string.IsNullOrEmpty(gm.Parent.Folder)
                && !string.IsNullOrEmpty(GameListControl.RootGamesListFolder)
                && !string.IsNullOrEmpty(gm.FullMarqueePath))
            {
                string startFolder = System.IO.Path.Combine(GameListControl.RootGamesListFolder, gm.Parent.Folder);
                gm.Marquee = gm.FullMarqueePath.Replace(startFolder, ".").Replace("\\", "/");
            }
        }

        [XmlElement("marquee")]
        public string? Marquee
        {
            get
            {
                return (string?)this.GetValue(MarqueeProperty);
            }

            set
            {
                this.SetValue(MarqueeProperty, value);
            }
        }

        public static readonly DependencyProperty FullMarqueePathProperty =
           DependencyProperty.Register(
           nameof(FullMarqueePath),
           typeof(string),
           typeof(Game), new PropertyMetadata(OnMarqueeChanged));
        [XmlIgnore]
        public string? FullMarqueePath
        {
            get
            {
                return (string?)this.GetValue(FullMarqueePathProperty);
            }

            set
            {
                this.SetValue(FullMarqueePathProperty, value);
            }
        }




        public static readonly DependencyProperty GenreIdProperty =
            DependencyProperty.Register(
            nameof(GenreId),
            typeof(string),
            typeof(Game), new PropertyMetadata(OnPropertyValueChanged));

        [XmlElement("genreid")]
        public required string GenreId
        {
            get
            {
                return (string)this.GetValue(GenreIdProperty);
            }

            set
            {
                this.SetValue(GenreIdProperty, value);
            }
        }
        public static readonly DependencyProperty NotesProperty =
            DependencyProperty.Register(
            nameof(Notes),
            typeof(string),
            typeof(Game), new PropertyMetadata(OnPropertyValueChanged));
        [XmlElement("Notes")]
        public string Notes
        {
            get
            {
                return (string)this.GetValue(NotesProperty);
            }

            set
            {
                this.SetValue(NotesProperty, value);
            }
        }
        public static string GetActualPath(string path, string sourceRoot, string folder)
        {
            string retVal = string.Empty;
            if (!string.IsNullOrEmpty(path))
            {
                string wrkPath = path;
                if (wrkPath.StartsWith("./"))
                {
                    wrkPath = System.IO.Path.Combine(sourceRoot, folder, wrkPath.Substring(2));
                }
                FileInfo fi = new(wrkPath);
               
                retVal = fi.FullName;
            }
            return retVal;
        }
        public void DeleteGameFiles(IEnumerable<Game> GameFolderGames)
        {
            bool isNotSafe = false;
            //Need to find if image is elsewhere and delete it if not
          
            if (System.IO.File.Exists(FullImagePath))
            {
                foreach (var game in GameFolderGames)
                {
                    if (FullImagePath == game.FullImagePath && game.FullPath != FullPath)
                    {
                        isNotSafe = true;
                        break;
                    }
                }
                if (!isNotSafe)
                {
                    System.IO.File.Delete(FullImagePath);
                }
            }
            //need to delete file.
            if (System.IO.File.Exists(FullPath))
            {
                System.IO.File.Delete(FullPath);
            }
        }
        string? GetListRoot()
        {
            string? startFolder = null;
            if (!string.IsNullOrEmpty(GameListControl.RootGamesListFolder) && !string.IsNullOrEmpty(Parent.Folder))
            {
                startFolder = System.IO.Path.Combine(GameListControl.RootGamesListFolder, Parent.Folder);
            }
            return startFolder;
        }

        /// <summary>
        /// Below method written entirely with ChatGPT.  Yes, I'm that lazy.
        /// </summary>
        /// <param name="path"></param>
        public static string ShrinkImageIfNecessary(string filePath, string target)
        {
            string retVal = filePath;
            try
            {
                using (System.IO.FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    
                    BitmapDecoder originalDecoder = BitmapDecoder.Create(fileStream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);

                    int originalWidth = originalDecoder.Frames[0].PixelWidth;
                    int originalHeight = originalDecoder.Frames[0].PixelHeight;

                    if (originalWidth > 640)
                    {
                        int newWidth = 640;
                        int newHeight = (int)((float)originalHeight / originalWidth * newWidth);

                        TransformedBitmap resizedImage = new TransformedBitmap(originalDecoder.Frames[0], new ScaleTransform((double)newWidth / originalWidth, (double)newHeight / originalHeight));
////
                        PngBitmapEncoder encoder = new PngBitmapEncoder();
                        
                        encoder.Frames.Add(BitmapFrame.Create(resizedImage));
                        if (!target.ToLowerInvariant().EndsWith(".png"))
                        {
                            FileInfo f = new FileInfo(retVal);
                            if (!string.IsNullOrEmpty(f.DirectoryName))
                            {
                                retVal = System.IO.Path.Combine(
                                    f.DirectoryName,
                                    string.Concat(
                                        f.Name.AsSpan(0, f.Name.Length - f.Extension.Length), ".png"));
                            }
                        }
                        using (System.IO.FileStream outStream = new FileStream(retVal, FileMode.OpenOrCreate, FileAccess.Write))
                        {
                            outStream.SetLength(0); // Clear the file content
                            encoder.Save(outStream);
                            outStream.Close();
                        }
                        fileStream.Close();

                        //Console.WriteLine("Image resized and saved successfully.");
                    }
                    else
                    {
                        System.IO.File.Copy(filePath, target);
                        //Console.WriteLine("Image width is already less than or equal to 1000 pixels. No resizing needed.");
                    }
                }
            }
            catch //(Exception ex)
            {
                //Console.WriteLine($"Error: {ex.Message}");
            }
            return retVal;
        }
        private string SetFileLocation(string path, string expectedSubFolder, bool IsImage = false)
        {
            string retVal = string.Empty;
            string? startFolder = GetListRoot();
            if (!string.IsNullOrEmpty(startFolder))
            {
                string targetImageFolder = System.IO.Path.Combine(startFolder, expectedSubFolder);
                if (path.StartsWith(targetImageFolder))
                {
                    retVal = path;
                }
                else
                {
                    FileInfo f = new FileInfo(path);
                    if (!Directory.Exists(targetImageFolder))
                    {
                        Directory.CreateDirectory(targetImageFolder);
                    }
                    retVal = System.IO.Path.Combine(targetImageFolder, f.Name);
                    
                    if (IsImage)
                    {
                        retVal = ShrinkImageIfNecessary(f.FullName, retVal);
                    }
                    else
                    {
                        f.CopyTo(retVal);
                    }
                }
            }
            return retVal;
        }
        public static string SetSingleLetterFolder(string path)
        {
            FileInfo f = new FileInfo(path);
            string finaldir = f.Name.Substring(0, 1);
            if (finaldir.CompareTo("A") < 0)
            {
                finaldir = "#";
            }
            return finaldir;
        }
        public static string SetOtherFolder(string folderName)
        {
            if (!string.IsNullOrEmpty(folderName))
            {
                return folderName;
            }
            else
            {
                return "Other";
            }
        }

        public void SetFullROMPath(string path)
        {
            string expectedFileLocation = string.Empty;
            switch (Parent.Organization)
            {
                case StructureOrganization.None:
                    expectedFileLocation = string.Empty;
                    break;
                case StructureOrganization.ByFirstLetter:
                    expectedFileLocation = SetSingleLetterFolder(path);
                    break;
                case StructureOrganization.ByGenre:
                    expectedFileLocation = SetOtherFolder(Genre);
                    break;
                case StructureOrganization.Publisher:
                    expectedFileLocation = SetOtherFolder(Publisher);
                    break;

                case StructureOrganization.Developer:
                    expectedFileLocation = SetOtherFolder(Developer);
                    break;
                default:
                    expectedFileLocation = string.Empty;
                    break;
            }
            FullPath = SetFileLocation(path, expectedFileLocation, false);
        }
        public void SetFullMarqueePath(string path)
        {
            FullMarqueePath = SetFileLocation(path, "media\\screenshot\\" + SetSingleLetterFolder(path), true);
        }
        public void SetFullImagePath(string path)
        {
            FullImagePath = SetFileLocation(path, "media\\marquee\\" + SetSingleLetterFolder(path), true);
        }
        public void SetFullVideoPath(string path)
        {
            FullVideoPath = SetFileLocation(path, "media\\video\\" + SetSingleLetterFolder(path), false);
        }
        public Game Copy()
        {

            return new Game()
            {
                Id = Id,
                Source = Source,
                Name = Name,
                FullPath = FullPath,
                Path = Path,
                Parent = Parent,
                Description = Description,
                Rating = Rating,
                ReleaseDate = ReleaseDate,
                DateReleased = DateReleased,
                Developer = Developer,
                Publisher = Publisher,
                Favorite = Favorite,
                Genre = Genre,
                Hidden = Hidden,
                KidGame = KidGame,
                Players = Players,
                Image = Image,
                FullImagePath = FullImagePath,
                Video = Video,
                FullVideoPath = FullVideoPath,
                Marquee = Marquee,
                FullMarqueePath = FullMarqueePath,
                GenreId = GenreId,
            };
        }
    }
}
