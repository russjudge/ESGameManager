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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ESGameManagerLibrary
{
    /// <summary>
    /// Interaction logic for ColorSelectorControl.xaml
    /// </summary>
    public partial class ColorSelectorControl : UserControl
    {
        public ColorSelectorControl()
        {
            InitializeComponent();
        }
        public static readonly DependencyProperty SelectedColorProperty =
            DependencyProperty.Register(
            nameof(SelectedColor),
            typeof(ColorList),
            typeof(ColorSelectorControl));
        public ColorList SelectedColor
        {
            get
            {
                return (ColorList)this.GetValue(SelectedColorProperty);
            }

            set
            {
                this.SetValue(SelectedColorProperty, value);
            }
        }

        public static readonly DependencyProperty SelectedSymbolProperty =
            DependencyProperty.Register(
            nameof(SelectedSymbol),
            typeof(string),
            typeof(ColorSelectorControl));
        public string SelectedSymbol
        {
            get
            {
                return (string)this.GetValue(SelectedSymbolProperty);
            }

            set
            {
                this.SetValue(SelectedSymbolProperty, value);
            }
        }

        private void OnColorLeft(object sender, RoutedEventArgs e)
        {
            int selectedColor = (int)SelectedColor;
            selectedColor--;
            if (selectedColor <0)
            {
                selectedColor = 6;
            }
            SelectedColor = (ColorList)selectedColor;
        }

        private void OnColorRight(object sender, RoutedEventArgs e)
        {
            int selectedColor = (int)SelectedColor;
            selectedColor++;
            if (selectedColor > 6)
            {
                selectedColor = 0;
            }
            SelectedColor = (ColorList)selectedColor;
        }
    }
}
