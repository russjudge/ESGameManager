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
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            
            Flag1Text = Properties.Settings.Default.Flag1;
            Flag2Text = Properties.Settings.Default.Flag2;
            Flag3Text = Properties.Settings.Default.Flag3;
            Flag4Text = Properties.Settings.Default.Flag4;
            Flag5Text = Properties.Settings.Default.Flag5;
            Flag6Text = Properties.Settings.Default.Flag6;
            Flag7Text = Properties.Settings.Default.Flag7;

            Flag1Color = (ColorList)Properties.Settings.Default.Flag1Color;
            Flag2Color = (ColorList)Properties.Settings.Default.Flag2Color;
            Flag3Color = (ColorList)Properties.Settings.Default.Flag3Color;
            Flag4Color = (ColorList)Properties.Settings.Default.Flag4Color;
            Flag5Color = (ColorList)Properties.Settings.Default.Flag5Color;
            Flag6Color = (ColorList)Properties.Settings.Default.Flag6Color;
            Flag7Color = (ColorList)Properties.Settings.Default.Flag7Color;

            Flag1Symbol = Properties.Settings.Default.Flag1Symbol;
            Flag2Symbol = Properties.Settings.Default.Flag2Symbol;
            Flag3Symbol = Properties.Settings.Default.Flag3Symbol;
            Flag4Symbol = Properties.Settings.Default.Flag4Symbol;
            Flag5Symbol = Properties.Settings.Default.Flag5Symbol;
            Flag6Symbol = Properties.Settings.Default.Flag6Symbol;
            Flag7Symbol = Properties.Settings.Default.Flag7Symbol;
            InitializeComponent();
        }

        public static readonly DependencyProperty Flag1SymbolProperty =
            DependencyProperty.Register(
            nameof(Flag1Symbol),
            typeof(string),
            typeof(SettingsWindow));
        public string Flag1Symbol
        {
            get
            {
                return (string)this.GetValue(Flag1SymbolProperty);
            }

            set
            {
                this.SetValue(Flag1SymbolProperty, value);
            }
        }


        public static readonly DependencyProperty Flag1TextProperty =
            DependencyProperty.Register(
            nameof(Flag1Text),
            typeof(string),
            typeof(SettingsWindow));
        public string Flag1Text
        {
            get
            {
                return (string)this.GetValue(Flag1TextProperty);
            }

            set
            {
                this.SetValue(Flag1TextProperty, value);
            }
        }
        public static readonly DependencyProperty Flag1ColorProperty =
            DependencyProperty.Register(
            nameof(Flag1Color),
            typeof(ColorList),
            typeof(SettingsWindow));
        public ColorList Flag1Color
        {
            get
            {
                return (ColorList)this.GetValue(Flag1ColorProperty);
            }

            set
            {
                this.SetValue(Flag1ColorProperty, value);
            }
        }

        public static readonly DependencyProperty Flag2SymbolProperty =
            DependencyProperty.Register(
            nameof(Flag2Symbol),
            typeof(string),
            typeof(SettingsWindow));
        public string Flag2Symbol
        {
            get
            {
                return (string)this.GetValue(Flag2SymbolProperty);
            }

            set
            {
                this.SetValue(Flag2SymbolProperty, value);
            }
        }

        public static readonly DependencyProperty Flag2TextProperty =
            DependencyProperty.Register(
            nameof(Flag2Text),
            typeof(string),
            typeof(SettingsWindow));
        public string Flag2Text
        {
            get
            {
                return (string)this.GetValue(Flag2TextProperty);
            }

            set
            {
                this.SetValue(Flag2TextProperty, value);
            }
        }
        public static readonly DependencyProperty Flag2ColorProperty =
            DependencyProperty.Register(
            nameof(Flag2Color),
            typeof(ColorList),
            typeof(SettingsWindow));
        public ColorList Flag2Color
        {
            get
            {
                return (ColorList)this.GetValue(Flag2ColorProperty);
            }

            set
            {
                this.SetValue(Flag2ColorProperty, value);
            }
        }

        public static readonly DependencyProperty Flag3SymbolProperty =
            DependencyProperty.Register(
            nameof(Flag3Symbol),
            typeof(string),
            typeof(SettingsWindow));
        public string Flag3Symbol
        {
            get
            {
                return (string)this.GetValue(Flag3SymbolProperty);
            }

            set
            {
                this.SetValue(Flag3SymbolProperty, value);
            }
        }
        public static readonly DependencyProperty Flag3TextProperty =
            DependencyProperty.Register(
            nameof(Flag3Text),
            typeof(string),
            typeof(SettingsWindow));
        public string Flag3Text
        {
            get
            {
                return (string)this.GetValue(Flag3TextProperty);
            }

            set
            {
                this.SetValue(Flag3TextProperty, value);
            }
        }
        public static readonly DependencyProperty Flag3ColorProperty =
            DependencyProperty.Register(
            nameof(Flag3Color),
            typeof(ColorList),
            typeof(SettingsWindow));
        public ColorList Flag3Color
        {
            get
            {
                return (ColorList)this.GetValue(Flag3ColorProperty);
            }

            set
            {
                this.SetValue(Flag3ColorProperty, value);
            }
        }

        public static readonly DependencyProperty Flag4SymbolProperty =
            DependencyProperty.Register(
            nameof(Flag4Symbol),
            typeof(string),
            typeof(SettingsWindow));
        public string Flag4Symbol
        {
            get
            {
                return (string)this.GetValue(Flag4SymbolProperty);
            }

            set
            {
                this.SetValue(Flag4SymbolProperty, value);
            }
        }
        public static readonly DependencyProperty Flag4TextProperty =
           DependencyProperty.Register(
            nameof(Flag4Text),
            typeof(string),
            typeof(SettingsWindow));
        public string Flag4Text
        {
            get
            {
                return (string)this.GetValue(Flag4TextProperty);
            }

            set
            {
                this.SetValue(Flag4TextProperty, value);
            }
        }
        public static readonly DependencyProperty Flag4ColorProperty =
            DependencyProperty.Register(
            nameof(Flag4Color),
            typeof(ColorList),
            typeof(SettingsWindow));
        public ColorList Flag4Color
        {
            get
            {
                return (ColorList)this.GetValue(Flag4ColorProperty);
            }

            set
            {
                this.SetValue(Flag4ColorProperty, value);
            }
        }

        public static readonly DependencyProperty Flag5SymbolProperty =
            DependencyProperty.Register(
            nameof(Flag5Symbol),
            typeof(string),
            typeof(SettingsWindow));
        public string Flag5Symbol
        {
            get
            {
                return (string)this.GetValue(Flag5SymbolProperty);
            }

            set
            {
                this.SetValue(Flag5SymbolProperty, value);
            }
        }
        public static readonly DependencyProperty Flag5TextProperty =
            DependencyProperty.Register(
            nameof(Flag5Text),
            typeof(string),
            typeof(SettingsWindow));
        public string Flag5Text
        {
            get
            {
                return (string)this.GetValue(Flag5TextProperty);
            }

            set
            {
                this.SetValue(Flag5TextProperty, value);
            }
        }
        public static readonly DependencyProperty Flag5ColorProperty =
            DependencyProperty.Register(
            nameof(Flag5Color),
            typeof(ColorList),
            typeof(SettingsWindow));
        public ColorList Flag5Color
        {
            get
            {
                return (ColorList)this.GetValue(Flag5ColorProperty);
            }

            set
            {
                this.SetValue(Flag5ColorProperty, value);
            }
        }

        public static readonly DependencyProperty Flag6SymbolProperty =
            DependencyProperty.Register(
            nameof(Flag6Symbol),
            typeof(string),
            typeof(SettingsWindow));
        public string Flag6Symbol
        {
            get
            {
                return (string)this.GetValue(Flag6SymbolProperty);
            }

            set
            {
                this.SetValue(Flag6SymbolProperty, value);
            }
        }
        public static readonly DependencyProperty Flag6TextProperty =
            DependencyProperty.Register(
            nameof(Flag6Text),
            typeof(string),
            typeof(SettingsWindow));
        public string Flag6Text
        {
            get
            {
                return (string)this.GetValue(Flag6TextProperty);
            }

            set
            {
                this.SetValue(Flag6TextProperty, value);
            }
        }
        public static readonly DependencyProperty Flag6ColorProperty =
            DependencyProperty.Register(
            nameof(Flag6Color),
            typeof(ColorList),
            typeof(SettingsWindow));
        public ColorList Flag6Color
        {
            get
            {
                return (ColorList)this.GetValue(Flag6ColorProperty);
            }

            set
            {
                this.SetValue(Flag6ColorProperty, value);
            }
        }

        public static readonly DependencyProperty Flag7SymbolProperty =
            DependencyProperty.Register(
            nameof(Flag7Symbol),
            typeof(string),
            typeof(SettingsWindow));
        public string Flag7Symbol
        {
            get
            {
                return (string)this.GetValue(Flag7SymbolProperty);
            }

            set
            {
                this.SetValue(Flag7SymbolProperty, value);
            }
        }
        public static readonly DependencyProperty Flag7TextProperty =
            DependencyProperty.Register(
            nameof(Flag7Text),
            typeof(string),
            typeof(SettingsWindow));
        public string Flag7Text
        {
            get
            {
                return (string)this.GetValue(Flag7TextProperty);
            }

            set
            {
                this.SetValue(Flag7TextProperty, value);
            }
        }
        public static readonly DependencyProperty Flag7ColorProperty =
            DependencyProperty.Register(
            nameof(Flag7Color),
            typeof(ColorList),
            typeof(SettingsWindow));
        public ColorList Flag7Color
        {
            get
            {
                return (ColorList)this.GetValue(Flag7ColorProperty);
            }

            set
            {
                this.SetValue(Flag7ColorProperty, value);
            }
        }

        private void OnSave(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Flag1 = Flag1Text;
            Properties.Settings.Default.Flag2 = Flag2Text;
            Properties.Settings.Default.Flag3 = Flag3Text;
            Properties.Settings.Default.Flag4 = Flag4Text;
            Properties.Settings.Default.Flag5 = Flag5Text;
            Properties.Settings.Default.Flag6 = Flag6Text;
            Properties.Settings.Default.Flag7 = Flag7Text;

            Properties.Settings.Default.Flag1Color = (int)Flag1Color;
            Properties.Settings.Default.Flag2Color = (int)Flag2Color;
            Properties.Settings.Default.Flag3Color = (int)Flag3Color;
            Properties.Settings.Default.Flag4Color = (int)Flag4Color;
            Properties.Settings.Default.Flag5Color = (int)Flag5Color;
            Properties.Settings.Default.Flag6Color = (int)Flag6Color;
            Properties.Settings.Default.Flag7Color = (int)Flag7Color;

            Properties.Settings.Default.Flag1Symbol = Flag1Symbol;
            Properties.Settings.Default.Flag2Symbol = Flag2Symbol;
            Properties.Settings.Default.Flag3Symbol = Flag3Symbol;
            Properties.Settings.Default.Flag4Symbol = Flag4Symbol;
            Properties.Settings.Default.Flag5Symbol = Flag5Symbol;
            Properties.Settings.Default.Flag6Symbol = Flag6Symbol;
            Properties.Settings.Default.Flag7Symbol = Flag7Symbol;

            Properties.Settings.Default.Save();
            Close();
        }

        
    }
}
