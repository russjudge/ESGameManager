using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace ESGameManagerLibrary
{
    public class GridViewSort
    {
        /// <summary>
        /// Using a DependencyProperty as the backing store for SortGlyphDescending.  This enables animation, styling, binding, etc... 
        /// </summary>
        public static readonly DependencyProperty SortGlyphDescendingProperty =
            DependencyProperty.RegisterAttached("SortGlyphDescending", typeof(ImageSource), typeof(GridViewSort), new UIPropertyMetadata(null));

        // Using a DependencyProperty as the backing store for Command.  This enables animation, styling, binding, etc...

        /// <summary>
        /// Command Property.
        /// </summary>
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached(
                "Command",
                typeof(ICommand),
                typeof(GridViewSort),
                new UIPropertyMetadata(
                    null,
                    (o, e) =>
                    {
                        if (o is ItemsControl listView)
                        {
                            // Don't change click handler if AutoSort enabled
                            if (!GetAutoSort(listView))
                            {
                                if (e.OldValue != null && e.NewValue == null)
                                {
                                    listView.RemoveHandler(GridViewColumnHeader.ClickEvent, new RoutedEventHandler(ColumnHeader_Click));
                                }

                                if (e.OldValue == null && e.NewValue != null)
                                {
                                    listView.AddHandler(GridViewColumnHeader.ClickEvent, new RoutedEventHandler(ColumnHeader_Click));
                                }
                            }
                        }
                    }));

        // Using a DependencyProperty as the backing store for AutoSort.  This enables animation, styling, binding, etc...

        /// <summary>
        /// Auto Sort property.
        /// </summary>
        public static readonly DependencyProperty AutoSortProperty =
            DependencyProperty.RegisterAttached(
                "AutoSort",
                typeof(bool),
                typeof(GridViewSort),
                new UIPropertyMetadata(
                    false,
                    (o, e) =>
                    {
                        if (o is ListView listView)
                        {
                            // Don't change click handler if a command is set
                            if (GetCommand(listView) == null)
                            {
                                bool oldValue = (bool)e.OldValue;
                                bool newValue = (bool)e.NewValue;
                                if (oldValue && !newValue)
                                {
                                    listView.RemoveHandler(GridViewColumnHeader.ClickEvent, new RoutedEventHandler(ColumnHeader_Click));
                                }

                                if (!oldValue && newValue)
                                {
                                    listView.AddHandler(GridViewColumnHeader.ClickEvent, new RoutedEventHandler(ColumnHeader_Click));
                                }
                            }
                        }
                    }));

        // Using a DependencyProperty as the backing store for PropertyName.  This enables animation, styling, binding, etc...

        /// <summary>
        /// Property name property.
        /// </summary>
        public static readonly DependencyProperty PropertyNameProperty =
            DependencyProperty.RegisterAttached(
                "PropertyName",
                typeof(string),
                typeof(GridViewSort),
                new UIPropertyMetadata(null));

        // Using a DependencyProperty as the backing store for ShowSortGlyph.  This enables animation, styling, binding, etc...

        /// <summary>
        /// Show sort glyph property.
        /// </summary>
        public static readonly DependencyProperty ShowSortGlyphProperty =
            DependencyProperty.RegisterAttached("ShowSortGlyph", typeof(bool), typeof(GridViewSort), new UIPropertyMetadata(true));

        // Using a DependencyProperty as the backing store for SortGlyphAscending.  This enables animation, styling, binding, etc...

        /// <summary>
        /// Sort Glyph ascending prperty.
        /// </summary>
        public static readonly DependencyProperty SortGlyphAscendingProperty =
            DependencyProperty.RegisterAttached("SortGlyphAscending", typeof(ImageSource), typeof(GridViewSort), new UIPropertyMetadata(null));

        // Using a DependencyProperty as the backing store for SortedColumn.  This enables animation, styling, binding, etc...

        /// <summary>
        /// Sorted column header property.
        /// </summary>
        private static readonly DependencyProperty SortedColumnHeaderProperty =
            DependencyProperty.RegisterAttached(
                "SortedColumnHeader",
                typeof(GridViewColumnHeader),
                typeof(GridViewSort),
                new UIPropertyMetadata(null));

        /// <summary>
        /// Get Command.
        /// </summary>
        /// <param name="obj">object to get command from.</param>
        /// <returns>The command.</returns>
        public static ICommand GetCommand(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(CommandProperty);
        }

        /// <summary>
        /// Sets command.
        /// </summary>
        /// <param name="obj">object.</param>
        /// <param name="value">command.</param>
        public static void SetCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(CommandProperty, value);
        }

        /// <summary>
        /// Get auto sort.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>true if auto sort is on.</returns>
        public static bool GetAutoSort(DependencyObject obj)
        {
            return (bool)obj.GetValue(AutoSortProperty);
        }

        /// <summary>
        /// Set auto sort.
        /// </summary>
        /// <param name="obj">Object to set.</param>
        /// <param name="value">value to set to object.</param>
        public static void SetAutoSort(DependencyObject obj, bool value)
        {
            obj.SetValue(AutoSortProperty, value);
        }

        /// <summary>
        /// Get property name.
        /// </summary>
        /// <param name="obj">object.</param>
        /// <returns>The property name.</returns>
        public static string GetPropertyName(DependencyObject obj)
        {
            return (string)obj.GetValue(PropertyNameProperty);
        }

        /// <summary>
        /// Set property name.
        /// </summary>
        /// <param name="obj">the object to use.</param>
        /// <param name="value">The value to set.</param>
        public static void SetPropertyName(DependencyObject obj, string value)
        {
            obj.SetValue(PropertyNameProperty, value);
        }

        /// <summary>
        /// Get show sort glyph.
        /// </summary>
        /// <param name="obj">object.</param>
        /// <returns>glyph.</returns>
        public static bool GetShowSortGlyph(DependencyObject obj)
        {
            return (bool)obj.GetValue(ShowSortGlyphProperty);
        }

        /// <summary>
        /// Set show sort glyph.
        /// </summary>
        /// <param name="obj">the object.</param>
        /// <param name="value">the glyph.</param>
        public static void SetShowSortGlyph(DependencyObject obj, bool value)
        {
            obj.SetValue(ShowSortGlyphProperty, value);
        }

        /// <summary>
        /// Get sort glyph ascending.
        /// </summary>
        /// <param name="obj">the object.</param>
        /// <returns>glyph.</returns>
        public static ImageSource GetSortGlyphAscending(DependencyObject obj)
        {
            return (ImageSource)obj.GetValue(SortGlyphAscendingProperty);
        }

        /// <summary>
        /// Set sort glyph ascending.
        /// </summary>
        /// <param name="obj">the object.</param>
        /// <param name="value">the glyph.</param>
        public static void SetSortGlyphAscending(DependencyObject obj, ImageSource value)
        {
            obj.SetValue(SortGlyphAscendingProperty, value);
        }

        /// <summary>
        /// Get sort glyph descending.
        /// </summary>
        /// <param name="obj">the object.</param>
        /// <returns>the glyph.</returns>
        public static ImageSource GetSortGlyphDescending(DependencyObject obj)
        {
            return (ImageSource)obj.GetValue(SortGlyphDescendingProperty);
        }

        /// <summary>
        /// Set sort glyph for descending.
        /// </summary>
        /// <param name="obj">object.</param>
        /// <param name="value">value.</param>
        public static void SetSortGlyphDescending(DependencyObject obj, ImageSource value)
        {
            obj.SetValue(SortGlyphDescendingProperty, value);
        }

        /// <summary>
        /// Get Ancestor.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="reference">The reference to get an ancestor for.</param>
        /// <returns>The ancestor.</returns>
        public static T? GetAncestor<T>(DependencyObject reference)
            where T : DependencyObject
        {
            DependencyObject parent = VisualTreeHelper.GetParent(reference);
            while (parent is not T)
            {
                parent = VisualTreeHelper.GetParent(parent);
            }

            if (parent != null)
            {
                return (T)parent;
            }
            else
            {
                return default;
            }
        }

        /// <summary>
        /// Apply sort.
        /// </summary>
        /// <param name="view">view.</param>
        /// <param name="propertyName">Property name.</param>
        /// <param name="listView">List view.</param>
        /// <param name="sortedColumnHeader">Sorted columnn header.</param>
        public static void ApplySort(ICollectionView view, string propertyName, ListView listView, GridViewColumnHeader sortedColumnHeader)
        {
            ListSortDirection direction = ListSortDirection.Ascending;
            if (view.SortDescriptions.Count > 0)
            {
                SortDescription currentSort = view.SortDescriptions[0];
                if (currentSort.PropertyName == propertyName)
                {
                    if (currentSort.Direction == ListSortDirection.Ascending)
                    {
                        direction = ListSortDirection.Descending;
                    }
                    else
                    {
                        direction = ListSortDirection.Ascending;
                    }
                }

                view.SortDescriptions.Clear();

                GridViewColumnHeader currentSortedColumnHeader = GetSortedColumnHeader(listView);
                if (currentSortedColumnHeader != null)
                {
                    RemoveSortGlyph(currentSortedColumnHeader);
                }
            }

            if (!string.IsNullOrEmpty(propertyName))
            {
                view.SortDescriptions.Add(new SortDescription(propertyName, direction));
                if (GetShowSortGlyph(listView))
                {
                    AddSortGlyph(
                        sortedColumnHeader,
                        direction,
                        direction == ListSortDirection.Ascending ? GetSortGlyphAscending(listView) : GetSortGlyphDescending(listView));
                }

                SetSortedColumnHeader(listView, sortedColumnHeader);
            }
        }

        private static GridViewColumnHeader GetSortedColumnHeader(DependencyObject obj)
        {
            return (GridViewColumnHeader)obj.GetValue(SortedColumnHeaderProperty);
        }

        private static void SetSortedColumnHeader(DependencyObject obj, GridViewColumnHeader value)
        {
            obj.SetValue(SortedColumnHeaderProperty, value);
        }

        private static void ColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is GridViewColumnHeader headerClicked && headerClicked.Column != null)
            {
                string propertyName = GetPropertyName(headerClicked.Column);
                if (!string.IsNullOrEmpty(propertyName))
                {
                    ListView? listView = GetAncestor<ListView>(headerClicked);
                    if (listView != null)
                    {
                        ICommand command = GetCommand(listView);
                        if (command != null)
                        {
                            if (command.CanExecute(propertyName))
                            {
                                command.Execute(propertyName);
                            }
                        }
                        else if (GetAutoSort(listView))
                        {
                            ApplySort(listView.Items, propertyName, listView, headerClicked);
                        }
                    }
                }
            }
        }

        private static void AddSortGlyph(GridViewColumnHeader columnHeader, ListSortDirection direction, ImageSource sortGlyph)
        {
            AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(columnHeader);
            adornerLayer.Add(
                new SortGlyphAdorner(
                    columnHeader,
                    direction,
                    sortGlyph));
        }

        private static void RemoveSortGlyph(GridViewColumnHeader columnHeader)
        {
            AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(columnHeader);
            Adorner[] adorners = adornerLayer.GetAdorners(columnHeader);
            if (adorners != null)
            {
                foreach (Adorner adorner in adorners)
                {
                    if (adorner is SortGlyphAdorner)
                    {
                        adornerLayer.Remove(adorner);
                    }
                }
            }
        }

        private class SortGlyphAdorner : Adorner
        {

            private GridViewColumnHeader columnHeader;
            private ListSortDirection direction;
            private ImageSource sortGlyph;


            public SortGlyphAdorner(GridViewColumnHeader columnHeader, ListSortDirection direction, ImageSource sortGlyph)
              : base(columnHeader)
            {
                this.columnHeader = columnHeader;
                this.direction = direction;
                this.sortGlyph = sortGlyph;
            }

            protected override void OnRender(DrawingContext drawingContext)
            {
                base.OnRender(drawingContext);

                if (this.sortGlyph != null)
                {
                    double x = this.columnHeader.ActualWidth - 13;
                    double y = (this.columnHeader.ActualHeight / 2) - 5;
                    Rect rect = new(x, y, 10, 10);
                    drawingContext.DrawImage(this.sortGlyph, rect);
                }
                else
                {
                    drawingContext.DrawGeometry(Brushes.LightGray, new Pen(Brushes.Gray, 1.0), this.GetDefaultGlyph());
                }
            }

            private Geometry GetDefaultGlyph()
            {
                double x1 = this.columnHeader.ActualWidth - 13;
                double x2 = x1 + 10;
                double x3 = x1 + 5;
                double y1 = (this.columnHeader.ActualHeight / 2) - 3;
                double y2 = y1 + 5;

                if (this.direction == ListSortDirection.Ascending)
                {
                    (y2, y1) = (y1, y2);
                }

                PathSegmentCollection pathSegmentCollection = new()
                {
                    new LineSegment(new Point(x2, y1), true),
                    new LineSegment(new Point(x3, y2), true),
                };

                PathFigure pathFigure = new(
                    new Point(x1, y1),
                    pathSegmentCollection,
                    true);

                PathFigureCollection pathFigureCollection = new()
                {
                    pathFigure,
                };

                PathGeometry pathGeometry = new(pathFigureCollection);
                return pathGeometry;
            }
        }
    }
}

