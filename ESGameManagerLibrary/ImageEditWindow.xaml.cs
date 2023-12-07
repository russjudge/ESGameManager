using System.Windows;

namespace ESGameManagerLibrary
{
    /// <summary>
    /// Interaction logic for TextEditWindow.xaml
    /// </summary>
    public partial class ImageEditWindow : Window
    {
        private string? _relativeFolder;
        public static string? ShowEditDialog(string relativeFolder, string originalText, string originalFullPath, string title = "")
        {

            string? retVal = null;
            ImageEditWindow win = new();
            win._relativeFolder = relativeFolder;
            if (!string.IsNullOrEmpty(title))
            {
                win.Title = title;
            }
            win.OriginalText = originalText;
            win.OriginalFullPath = originalFullPath;
            win.EditText = originalText;

            var result = win.ShowDialog();
            if (result == true)
            {
                retVal = win.EditTextFullPath;
            }
            return retVal;
        }
        public ImageEditWindow()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty OriginalTextProperty =
           DependencyProperty.Register(
               nameof(OriginalText),
               typeof(string),
               typeof(ImageEditWindow));
        public string OriginalText
        {
            get
            {
                return (string)this.GetValue(OriginalTextProperty);
            }

            set
            {
                this.SetValue(OriginalTextProperty, value);
            }
        }
        public static readonly DependencyProperty OriginalFullPathProperty =
           DependencyProperty.Register(
               nameof(OriginalFullPath),
               typeof(string),
               typeof(ImageEditWindow));
        public string OriginalFullPath
        {
            get
            {
                return (string)this.GetValue(OriginalFullPathProperty);
            }

            set
            {
                this.SetValue(OriginalFullPathProperty, value);
            }
        }

        public static readonly DependencyProperty EditTextProperty =
           DependencyProperty.Register(
               nameof(EditText),
               typeof(string),
               typeof(ImageEditWindow));
        public string EditText
        {
            get
            {
                return (string)this.GetValue(EditTextProperty);
            }

            set
            {
                this.SetValue(EditTextProperty, value);
            }
        }
        public static readonly DependencyProperty EditTextFullPathProperty =
           DependencyProperty.Register(
               nameof(EditTextFullPath),
               typeof(string),
               typeof(ImageEditWindow), new PropertyMetadata(OnEditTextFullPathChanged));

        private static void OnEditTextFullPathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ImageEditWindow me && !string.IsNullOrEmpty(GameListControl.RootGamesListFolder) && !string.IsNullOrEmpty(me._relativeFolder))
            {
                string extractPath = System.IO.Path.Combine(GameListControl.RootGamesListFolder, me._relativeFolder);
                me.EditText = me.EditTextFullPath.Replace(extractPath, ".").Replace("\\", "/");
            }
        }

        public string EditTextFullPath
        {
            get
            {
                return (string)this.GetValue(EditTextFullPathProperty);
            }

            set
            {
                this.SetValue(EditTextFullPathProperty, value);
            }
        }

        private void OnOK(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void OnCancel(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void OnBrowse(object sender, RoutedEventArgs e)
        {
            var path = MetaDetailControl.BrowseForFile("Select image file", MetaDetailControl.imageFilesFilter);
            if (!string.IsNullOrEmpty(path))
            {
                EditTextFullPath = path;
            }
        }
    }
}
