using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace ESGameManagerLibrary
{
    /// <summary>
    /// Interaction logic for TextEditWindow.xaml
    /// </summary>
    public partial class TextEditWindow : Window
    {
        public static string? ShowEditDialog(string originalText, string title = "")
        {
            string? retVal = null;
            TextEditWindow win = new();
            if (!string.IsNullOrEmpty(title))
            {
                win.Title = title;
            }
            win.OriginalText = originalText;
            win.EditText = originalText;

            var result = win.ShowDialog();
            if (result == true)
            {
                retVal = win.EditText;
            }
            return retVal;
        }
        public TextEditWindow()
        {
            InitializeComponent();
        }
        
        public static readonly DependencyProperty OriginalTextProperty =
           DependencyProperty.Register(
               nameof(OriginalText),
               typeof(string),
               typeof(TextEditWindow));
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

        public static readonly DependencyProperty EditTextProperty =
           DependencyProperty.Register(
               nameof(EditText),
               typeof(string),
               typeof(TextEditWindow));
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
    }
}
