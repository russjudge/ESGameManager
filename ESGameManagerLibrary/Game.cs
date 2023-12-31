﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Serialization;
namespace ESGameManagerLibrary
{
    public class Game : DependencyObject
    {

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("{0}",Name);
            sb.AppendFormat("\t{0}", DateReleased.ToString("yyyy"));
            sb.AppendFormat("\t{0}", Publisher);
            sb.AppendFormat("\t{0}", Developer);
            sb.AppendFormat("\t{0}", Genre);
            sb.AppendFormat("\t{0}", Flag1 ? Properties.Settings.Default.Flag1Symbol : " ");
            sb.AppendFormat("\t{0}", Flag2 ? Properties.Settings.Default.Flag2Symbol : " ");
            sb.AppendFormat("\t{0}", Flag3 ? Properties.Settings.Default.Flag3Symbol : " ");
            sb.AppendFormat("\t{0}", Notes);

            return sb.ToString();
        }
        
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


        private bool flagsBeingSet = false;
        public static readonly DependencyProperty FlagsProperty =
            DependencyProperty.Register(
            nameof(Flags),
            typeof(int),
            typeof(Game), new PropertyMetadata(OnFlagsChanged));

        private static void OnFlagsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Game gm)
            {
                if (!gm.flagsBeingSet)
                {
                    gm.flagsBeingSet = true;
                    gm.Flag1 = ((int)gm.Flags & 1) == 1;
                    gm.Flag2 = ((int)gm.Flags & 2) == 2;
                    gm.Flag3 = ((int)gm.Flags & 4) == 4;
                    gm.Flag4 = ((int)gm.Flags & 8) == 8;
                    gm.Flag5 = ((int)gm.Flags & 16) == 16;
                    gm.Flag6 = ((int)gm.Flags & 32) == 32;
                    gm.Flag7 = ((int)gm.Flags & 64) == 64;
                    gm.flagsBeingSet = false;
                }
                OnPropertyValueChanged(d, e);
            }
        }

        [XmlAttribute("flags")]
        public required int Flags
        {
            get
            {
                return (int)this.GetValue(FlagsProperty);
            }

            set
            {
                this.SetValue(FlagsProperty, value);
            }
        }



        public static readonly DependencyProperty Flag1Property =
            DependencyProperty.Register(
            nameof(Flag1),
            typeof(bool),
            typeof(Game), new PropertyMetadata(OnFlagBoolChanged));

        private static void OnFlagBoolChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Game gm)
            {
                if (!gm.flagsBeingSet)
                {
                    gm.flagsBeingSet = true;
                    gm.Flags = (int)(gm.Flag1 ? 1 : 0)
                        + (int)(gm.Flag2 ? 2 : 0)
                        + (int)(gm.Flag3 ? 4: 0)
                        + (int)(gm.Flag4 ? 8 : 0)
                        + (int)(gm.Flag5 ? 16 : 0)
                        + (int)(gm.Flag6 ? 32 : 0)
                        + (int)(gm.Flag7 ? 64 : 0);
                    gm.flagsBeingSet = false;
                }
            }
        }

        [XmlIgnore]
        public bool Flag1
        {
            get
            {
                return (bool)this.GetValue(Flag1Property);
            }

            set
            {
                this.SetValue(Flag1Property, value);
            }
        }

        public static readonly DependencyProperty Flag2Property =
           DependencyProperty.Register(
           nameof(Flag2),
           typeof(bool),
           typeof(Game), new PropertyMetadata(OnFlagBoolChanged));
        [XmlIgnore]
        public bool Flag2
        {
            get
            {
                return (bool)this.GetValue(Flag2Property);
            }

            set
            {
                this.SetValue(Flag2Property, value);
            }
        }
        public static readonly DependencyProperty Flag3Property =
           DependencyProperty.Register(
           nameof(Flag3),
           typeof(bool),
           typeof(Game), new PropertyMetadata(OnFlagBoolChanged));
        [XmlIgnore]
        public bool Flag3
        {
            get
            {
                return (bool)this.GetValue(Flag3Property);
            }

            set
            {
                this.SetValue(Flag3Property, value);
            }
        }
        public static readonly DependencyProperty Flag4Property =
           DependencyProperty.Register(
           nameof(Flag4),
           typeof(bool),
           typeof(Game), new PropertyMetadata(OnFlagBoolChanged));
        [XmlIgnore]
        public bool Flag4
        {
            get
            {
                return (bool)this.GetValue(Flag4Property);
            }

            set
            {
                this.SetValue(Flag4Property, value);
            }
        }
        public static readonly DependencyProperty Flag5Property =
           DependencyProperty.Register(
           nameof(Flag5),
           typeof(bool),
           typeof(Game), new PropertyMetadata(OnFlagBoolChanged));
        [XmlIgnore]
        public bool Flag5
        {
            get
            {
                return (bool)this.GetValue(Flag5Property);
            }

            set
            {
                this.SetValue(Flag5Property, value);
            }
        }
        public static readonly DependencyProperty Flag6Property =
           DependencyProperty.Register(
           nameof(Flag6),
           typeof(bool),
           typeof(Game), new PropertyMetadata(OnFlagBoolChanged));
        [XmlIgnore]
        public bool Flag6
        {
            get
            {
                return (bool)this.GetValue(Flag6Property);
            }

            set
            {
                this.SetValue(Flag6Property, value);
            }
        }
        public static readonly DependencyProperty Flag7Property =
           DependencyProperty.Register(
           nameof(Flag7),
           typeof(bool),
           typeof(Game)); //, new PropertyMetadata(OnFlagBoolChanged)
        [XmlIgnore]
        public bool Flag7
        {
            get
            {
                return (bool)this.GetValue(Flag7Property);
            }

            set
            {
                this.SetValue(Flag7Property, value);
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
  
            //Need to find if image is elsewhere and delete it if not
            string? fullImagePath = null;
            string? fullPath = null;
            string? fullVideoPath = null;
            string? fullMarqueePath = null;
           
            this.Dispatcher.Invoke(() =>
            {
                fullImagePath = FullImagePath;
                fullPath = FullPath;
                fullVideoPath = FullVideoPath;
                fullMarqueePath = FullMarqueePath;
                foreach (var game in GameFolderGames)
                {
                    if (game.FullPath != fullPath)
                    {
                        if (!string.IsNullOrEmpty(fullVideoPath) && fullVideoPath == game.FullVideoPath)
                        {
                            fullVideoPath = null;
                        }
                        if (!string.IsNullOrEmpty(fullImagePath) && fullImagePath == game.FullImagePath)
                        {
                            fullImagePath = null;
                        }
                        if (!string.IsNullOrEmpty(fullMarqueePath) && fullMarqueePath == game.FullMarqueePath)
                        {
                            fullMarqueePath = null;
                        }
                        if (string.IsNullOrEmpty(fullVideoPath) && string.IsNullOrEmpty(fullImagePath) && string.IsNullOrEmpty(fullMarqueePath))
                        {
                            break;
                        }
                    }
                }
            });

                    
            if (!string.IsNullOrEmpty(fullImagePath) && System.IO.File.Exists(fullImagePath))
            {
                System.IO.File.Delete(fullImagePath);
            }
            if (!string.IsNullOrEmpty(fullMarqueePath) && System.IO.File.Exists(fullMarqueePath))
            {
                System.IO.File.Delete(fullMarqueePath);
            }
            if (!string.IsNullOrEmpty(fullVideoPath) && System.IO.File.Exists(fullVideoPath))
            {
                System.IO.File.Delete(fullVideoPath);
            }
            //need to delete file.
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }
        }
       

        /// <summary>
        /// Below method written entirely with ChatGPT.  Yes, I'm that lazy.
        /// </summary>
        /// <param name="path"></param>
        public static string ShrinkImageIfNecessary(string filePath, string target)
        {
            string retVal = target;
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
        public string SetFileLocation(string path, string expectedSubFolder, bool IsImage = false)
        {
            string retVal = string.Empty;
            string? startFolder = Parent.GetListRoot();
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
                        f.CopyTo(retVal, true);
                    }
                }
            }
            return retVal;
        }
        public static string GetRelativeFolderPath(string? pathOrFactor, bool forSingleLetter = false)
        {
            if (forSingleLetter && !string.IsNullOrEmpty(pathOrFactor))
            {
                FileInfo f = new FileInfo(pathOrFactor);
                string finaldir = f.Name.Substring(0, 1);
                if (finaldir.CompareTo("A") < 0)
                {
                    finaldir = "#";
                }
                else if (finaldir.CompareTo("Z") > 0)
                {
                    finaldir = "Z";
                }
                return finaldir;
            }
            else
            {
                if (pathOrFactor != null)
                {
                    if (!string.IsNullOrEmpty(pathOrFactor.Trim()))
                    {
                        string retVal = pathOrFactor
                            .Replace('/', '-')
                            .Replace('\\', '-')
                            .Replace('\u00A0', ' ')
                            .Replace('*', '-')
                            .Replace("<", "&lt;")
                            .Replace(">", "&gt;")
                            .Replace(':', '-')
                            .Replace('"', '\'')
                            .Replace('|', '-')
                            .Replace('?', ' ').Trim();
                        StringBuilder sb = new StringBuilder(retVal);

                        for(int i =0;i< sb.Length; i++)
                        {
                            if (sb[i] < ' ')
                            {
                                sb[i] = ' ';
                            }
                        }
                        retVal = sb.ToString();
                        if (retVal.EndsWith('.'))
                        {
                            retVal = retVal.Substring(0, retVal.Length - 1);
                        }
                        return retVal;
                    }
                    else
                    {
                        return "-Other-";
                    }
                }
                else
                {
                    return "-Other-";
                }
            }
        }
        public static string SetSingleLetterFolder(string path)
        {
            return GetRelativeFolderPath(path, true);
        }
        public static string SetOtherFolder(string? folderName)
        {
            return GetRelativeFolderPath(folderName, false);
        }
        public string GetRomRelativePath()
        {
            string expectedFileLocation = string.Empty;
            switch (Parent.Organization)
            {
                case StructureOrganization.None:
                    expectedFileLocation = string.Empty;
                    break;
                case StructureOrganization.ByFirstLetter:
                    expectedFileLocation = SetSingleLetterFolder(Name);
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
            return expectedFileLocation;
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
                    expectedFileLocation = SetSingleLetterFolder(Name);
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
        public string GetFullMarqueePath(string path)
        {
            return SetFileLocation(path, "media\\marquee\\" + SetSingleLetterFolder(path), true);
        }
        public void SetFullMarqueePath(string path)
        {
            FullMarqueePath = GetFullMarqueePath(path);
        }
        public string GetFullImagePath(string path)
        {
            return SetFileLocation(path, "media\\screenshot\\" + SetSingleLetterFolder(path), true);
        }
        public void SetFullImagePath(string path)
        {
            FullImagePath = GetFullImagePath(path);
        }
        public string GetFullVideoPath(string path)
        {
            return SetFileLocation(path, "media\\video\\" + SetSingleLetterFolder(path), true);
        }
        public void SetFullVideoPath(string path)
        {
            FullVideoPath = GetFullVideoPath(path);
        }
        public Game Copy(GameList newParent)
        {

            return new Game()
            {
                Flags = 0,
                Id = Id,
                Source = Source,
                Name = Name,
                FullPath = FullPath,
                Path = Path,
                Parent = newParent,
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
                IsLoading=false,
            };
        }
    }
}
