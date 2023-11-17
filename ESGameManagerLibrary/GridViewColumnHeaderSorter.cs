using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows;

namespace ESGameManagerLibrary
{

    /// <summary>
    /// Grid view column header sorter.
    /// </summary>
    public static class GridViewColumnHeaderSorter
    {
        //To use: Binding for ItemsSource should be ObservableCollection<T>.
        //Currently only works for ListViews.
        // Add <GridColumnHeader Extensions.SortColumnID="xxx"> where "xxx" is the field name in the binding
        //  to sort on.  only works with simple (one field) sorting.

        /// <summary>
        /// Sort Enabled property.
        /// </summary>
        public static readonly DependencyProperty SortEnabledProperty =
            DependencyProperty.RegisterAttached(
                "SortEnabled",
                typeof(bool),
                typeof(GridViewColumnHeaderSorter),
                new FrameworkPropertyMetadata(true));

        /// <summary>
        /// Sort direction.
        /// </summary>
        public static readonly DependencyProperty SortDirectionProperty =
            DependencyProperty.RegisterAttached(
                "SortDirection",
                typeof(ListSortDirection),
                typeof(GridViewColumnHeaderSorter),
                new FrameworkPropertyMetadata(ListSortDirection.Ascending));

        /// <summary>
        /// Is Default property.
        /// </summary>
        public static readonly DependencyProperty IsDefaultProperty =
           DependencyProperty.RegisterAttached(
               "IsDefault",
               typeof(bool),
               typeof(GridViewColumnHeaderSorter));

        /// <summary>
        /// Sort column id property.
        /// </summary>
        public static readonly DependencyProperty SortColumnIDProperty =
            DependencyProperty.RegisterAttached(
                "SortColumnID",
                typeof(string),
                typeof(GridViewColumnHeaderSorter),
                new FrameworkPropertyMetadata(OnSortColumnIDChanged));

        /// <summary>
        /// Parent listview property.
        /// </summary>
        public static readonly DependencyProperty ParentListViewProperty =
            DependencyProperty.RegisterAttached(
                "ParentListView",
                typeof(ListView),
                typeof(GridViewColumnHeaderSorter),
                new FrameworkPropertyMetadata(new PropertyChangedCallback(OnParentListViewChanged)));

        private static readonly DependencyProperty CurrentSortColumnProperty =
            DependencyProperty.RegisterAttached(
                "CurrentSortColumn",
                typeof(GridViewColumnHeader),
                typeof(GridViewColumnHeaderSorter),
                new FrameworkPropertyMetadata());

        private static readonly DependencyProperty CurrentSortAdornerProperty =
            DependencyProperty.RegisterAttached(
                "CurrentSortAdorner",
                typeof(SortAdorner),
                typeof(GridViewColumnHeaderSorter),
                new FrameworkPropertyMetadata(OnCurrentSortAdornerChanged));

        private static readonly DependencyProperty IsSortingProperty =
          DependencyProperty.RegisterAttached(
              "IsSorting",
              typeof(bool),
              typeof(GridViewColumnHeaderSorter),
              new FrameworkPropertyMetadata());

        /// <summary>
        /// Set sort direction.
        /// </summary>
        /// <param name="element">Element.</param>
        /// <param name="value">sort direction.</param>
        public static void SetSortDirection(DependencyObject element, ListSortDirection value)
        {
            if (element != null)
            {
                element.SetValue(SortDirectionProperty, value);
            }
        }

        /// <summary>
        /// Get sort direction.
        /// </summary>
        /// <param name="element">element.</param>
        /// <returns>sort direction.</returns>
        public static ListSortDirection GetSortDirection(DependencyObject element)
        {
            ListSortDirection value = ListSortDirection.Ascending;
            if (element != null)
            {
                object result = element.GetValue(SortDirectionProperty);
                if (result != null)
                {
                    value = (ListSortDirection)result;
                }
                else
                {
                    value = ListSortDirection.Ascending;
                }
            }

            return value;
        }

        /// <summary>
        /// Set is default.
        /// </summary>
        /// <param name="element">Element.</param>
        /// <param name="value">true if default.</param>
        public static void SetIsDefault(DependencyObject element, bool value)
        {
            if (element != null)
            {
                element.SetValue(IsDefaultProperty, value);
            }
        }

        /// <summary>
        /// Get is default.
        /// </summary>
        /// <param name="element">element.</param>
        /// <returns>true if default.</returns>
        public static bool GetIsDefault(DependencyObject element)
        {
            bool value = true;
            if (element != null)
            {
                value = (bool)element.GetValue(IsDefaultProperty);
            }

            return value;
        }

        /// <summary>
        /// Set sort column id.
        /// </summary>
        /// <param name="element">object to set.</param>
        /// <param name="value">Value to set.</param>
        public static void SetSortColumnID(DependencyObject element, string value)
        {
            if (element != null)
            {
                element.SetValue(SortColumnIDProperty, value);
            }
        }

        /// <summary>
        /// Get sort column id.
        /// </summary>
        /// <param name="element">column.</param>
        /// <returns>The id.</returns>
        public static string? GetSortColumnID(DependencyObject element)
        {
            string? value = null;
            if (element != null)
            {
                value = (string)element.GetValue(SortColumnIDProperty);
            }

            return value;
        }

        /// <summary>
        /// Set Current sort column.
        /// </summary>
        /// <param name="element">List view to set.</param>
        /// <param name="value">Column to set to.</param>
        public static void SetCurrentSortColumn(ListView element, GridViewColumnHeader value)
        {
            if (element != null)
            {
                if (value == null)
                {
                    RemoveSortAdorner(element);
                }

                element.SetValue(CurrentSortColumnProperty, value);
            }
        }

        /// <summary>
        /// Get Current sort column.
        /// </summary>
        /// <param name="element">element to check.</param>
        /// <returns>Column Header.</returns>
        public static GridViewColumnHeader? GetCurrentSortColumn(DependencyObject element)
        {
            GridViewColumnHeader? value = null;
            if (element != null)
            {
                value = (GridViewColumnHeader)element.GetValue(CurrentSortColumnProperty);
            }

            return value;
        }

        /// <summary>
        /// Disable sort.
        /// </summary>
        /// <param name="target">Object to disable sort on.</param>
        public static void DisableSort(ListView target)
        {
            if (target != null)
            {
                target.SetValue(SortEnabledProperty, false);
                RemoveSortAdorner(target);
            }
        }

        /// <summary>
        /// Enable sort.
        /// </summary>
        /// <param name="target">Object to enable sort on.</param>
        public static void EnableSort(DependencyObject target)
        {
            if (target != null)
            {
                target.SetValue(SortEnabledProperty, true);
                Sort(target);
            }
        }

        /// <summary>
        /// Get sort enabled.
        /// </summary>
        /// <param name="target">object to check.</param>
        /// <returns>true if sort is enabled.</returns>
        public static bool GetSortEnabled(DependencyObject target)
        {
            bool retVal = true;
            if (target != null)
            {
                retVal = (bool)target.GetValue(SortEnabledProperty);
            }

            return retVal;
        }

        /// <summary>
        /// Sort.
        /// </summary>
        /// <param name="target">Object to sort.</param>
        public static void Sort(DependencyObject target)
        {
            GridViewColumnHeader? sortColumn = GetCurrentSortColumn(target);
            if (sortColumn != null)
            {
                sortColumn.Sort();
            }
        }

        /// <summary>
        /// Sort.
        /// </summary>
        /// <param name="column">Column to sort on.</param>
        public static void Sort(this GridViewColumnHeader column)
        {
            if (column != null)
            {
                ListView? parent = column.ParentListView();
                if (parent != null)
                {
                    if (!GetIsSorting(parent) && GetSortEnabled(parent))
                    {
                        SetIsSorting(parent, true);
                        string? fieldxxx = GetSortColumnID(column);
                        string[]? fieldList = null;
                        string? field = null;
                        if (!string.IsNullOrEmpty(fieldxxx))
                        {
                            fieldList = fieldxxx.Split('|');
                            if (fieldList.Length == 1)
                            {
                                field = fieldList[0];
                            }
                        }

                        //TODO: Allow compound column fields.
                        GridViewColumnHeader? curSortCol = GetCurrentSortColumn(parent);

                        SortAdorner? curAdorner = GetCurrentSortAdorner(parent);
                        if (fieldList != null)
                        {
                            ListCollectionView? dataView = CollectionViewSource.GetDefaultView(parent.ItemsSource) as ListCollectionView;
                            if (dataView != null)
                            {
                                if (curSortCol != null)
                                {
                                    AdornerLayer.GetAdornerLayer(curSortCol).Remove(curAdorner);
                                    dataView.SortDescriptions.Clear();
                                    parent.Items.SortDescriptions.Clear();
                                }
                                else
                                {
                                    if (curAdorner != null)
                                    {
                                        AdornerLayer.GetAdornerLayer(parent).Remove(curAdorner);
                                        dataView.SortDescriptions.Clear();
                                        parent.Items.SortDescriptions.Clear();
                                    }
                                }

                                ListSortDirection newDir = GetSortDirection(column);

                                curAdorner = new SortAdorner(column, newDir);

                                AdornerLayer.GetAdornerLayer(column).Add(curAdorner);
                                SetCurrentSortColumn(parent, column);

                                SetCurrentSortAdorner(parent, curAdorner);

                                IComparer? sorter = null;

                                if (fieldList.Length > 1)
                                {
                                    if (MultipleColumnSorter.IsCandidate(dataView.ItemProperties, fieldList))
                                    {
                                        if (newDir == ListSortDirection.Ascending)
                                        {
                                            sorter = new MultipleColumnSorter(fieldList);
                                        }
                                        else
                                        {
                                            sorter = new ReverseMultipleColumnSorter(fieldList);
                                        }
                                    }
                                    else
                                    {
                                        foreach (string f in fieldList)
                                        {
                                            dataView.SortDescriptions.Add(new SortDescription(f, newDir));
                                        }
                                    }
                                }
                                else
                                {
                                    if (field != null)
                                    {
                                        foreach (ItemPropertyInfo p in dataView.ItemProperties)
                                        {
                                            if (p.Name.ToUpperInvariant() == field.ToUpperInvariant())
                                            {
                                                Type ptype = p.GetType();
                                                if (ptype == typeof(DateTime))
                                                {
                                                    if (newDir == ListSortDirection.Ascending)
                                                    {
                                                        sorter = new DateSorter();
                                                    }
                                                    else
                                                    {
                                                        sorter = new ReverseDateSorter();
                                                    }
                                                }
                                                else if (ptype == typeof(IComparable))
                                                {
                                                    if (newDir == ListSortDirection.Ascending)
                                                    {
                                                        sorter = new ComparableSorter();
                                                    }
                                                    else
                                                    {
                                                        sorter = new ReverseComparableSorter();
                                                    }
                                                }

                                                break;
                                            }
                                        }
                                    }

                                    if (sorter == null)
                                    {
                                        dataView.SortDescriptions.Add(new SortDescription(field, newDir));
                                    }
                                    else
                                    {
                                        dataView.CustomSort = sorter;
                                    }
                                }

                                dataView.Refresh();
                            }
                        }

                        SetIsSorting(parent, false);
                    }
                }
            }
        }

        private static void RemoveSortAdorner(ListView target)
        {
            GridViewColumnHeader? curSortCol = GetCurrentSortColumn(target);
            SortAdorner? curAdorner = GetCurrentSortAdorner(target);
            if (curSortCol != null)
            {
                AdornerLayer.GetAdornerLayer(curSortCol).Remove(curAdorner);
                if (CollectionViewSource.GetDefaultView(target.ItemsSource) is ListCollectionView dataView)
                {
                    dataView.SortDescriptions.Clear();
                }

                target.Items.SortDescriptions.Clear();
            }
        }

        private static void ItemContainerGenerator_ItemsChanged(object sender, System.Windows.Controls.Primitives.ItemsChangedEventArgs e, ListView? parent)
        {
            if (parent != null)
            {
                var column = GetCurrentSortColumn(parent);
                if (column != null)
                {
                    column.Sort();
                }
            }
        }

        private static void OnCurrentSortAdornerChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is ListView me)
            {
                //RemoveSortAdorner(me);
                //Sender will be a listview?
                //remove all sortadorners from all columns but one for e.newvalue.
            }
        }

        private static void SetCurrentSortAdorner(ListView element, SortAdorner value)
        {
            if (element != null)
            {
                element.SetValue(CurrentSortAdornerProperty, value);
            }
        }

        private static SortAdorner? GetCurrentSortAdorner(ListView? element)
        {
            SortAdorner? value = null;
            if (element != null)
            {
                value = (SortAdorner)element.GetValue(CurrentSortAdornerProperty);
            }

            return value;
        }

        private static void GridColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is GridViewColumnHeader headerClicked)
            {
                SetSortDirection(headerClicked, (GetSortDirection(headerClicked) == ListSortDirection.Ascending) ? ListSortDirection.Descending : ListSortDirection.Ascending);

                headerClicked.Sort();
            }
        }

        private static ListView? ParentListView(this GridViewColumnHeader me)
        {
            ListView? parent = GetParentListView(me);
            if (parent == null)
            {
                parent = GetAncestor<ListView>(me);
                if (parent != null)
                {
                    SetParentListView(me, parent);
                }
            }

            return parent;
        }

        private static void SetIsSorting(DependencyObject element, bool value)
        {
            if (element != null)
            {
                element.SetValue(IsSortingProperty, value);
            }
        }

        private static bool GetIsSorting(DependencyObject element)
        {
            bool value = false;
            if (element != null)
            {
                bool? val = element.GetValue(IsSortingProperty) as bool?;
                if (val == null)
                {
                    value = false;
                }
                else
                {
                    value = val.Value;
                }
            }

            return value;
        }

        private static void SetParentListView(DependencyObject element, ListView value)
        {
            if (element != null)
            {
                element.SetValue(ParentListViewProperty, value);
            }
        }

        private static ListView? GetParentListView(DependencyObject element)
        {
            ListView? value = null;
            if (element != null)
            {
                value = (ListView)element.GetValue(ParentListViewProperty);
            }

            return value;
        }

        private static void OnSortColumnIDChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is GridViewColumnHeader column)
            {
                if (e.NewValue != null && e.OldValue == null)
                {
                    column.Click += new RoutedEventHandler(GridColumnHeader_Click);
                    column.Loaded += new RoutedEventHandler(Column_Loaded);
                }

                if (e.NewValue == null && e.OldValue != null)
                {
                    column.Click -= new RoutedEventHandler(GridColumnHeader_Click);
                    column.Loaded -= new RoutedEventHandler(Column_Loaded);
                }
            }
        }

        private static void Column_Loaded(object sender, RoutedEventArgs e)
        {
            GridViewColumnHeader column = (GridViewColumnHeader)sender;
            if (GetIsDefault(column))
            {
                column.Sort();
            }
        }

        private static T? GetAncestor<T>(DependencyObject reference)
            where T : DependencyObject
        {
            if (reference != null)
            {
                DependencyObject parent = VisualTreeHelper.GetParent(reference);
                if (parent != null)
                {
                    while (!(parent is T))
                    {
                        parent = VisualTreeHelper.GetParent(parent);
                    }
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
            else
            {
                return default;
            }
        }

        private static void OnParentListViewChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is ListView newparent)
            {
                newparent.ItemContainerGenerator.ItemsChanged += (o, err) => ItemContainerGenerator_ItemsChanged(o, err, newparent);
            }

            if (e.OldValue is ListView oldparent)
            {
                oldparent.ItemContainerGenerator.ItemsChanged -= (o, err) => ItemContainerGenerator_ItemsChanged(o, err, oldparent);
            }
        }
    }
}