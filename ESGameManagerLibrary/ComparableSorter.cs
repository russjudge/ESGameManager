using System;
using System.Collections;

namespace ESGameManagerLibrary
{
    /// <summary>
    /// Comparable sorter.
    /// </summary>
    public class ComparableSorter : IComparer
    {
        /// <summary>
        /// Compare two comparable. objects.
        /// </summary>
        /// <param name="x">first object.</param>
        /// <param name="y">second object.</param>
        /// <returns>-1 if x lt y, 1 if x gt y.</returns>
        public int Compare(object? x, object? y)
        {
            IComparable? dtx = x as IComparable;
            IComparable? dty = y as IComparable;
            if (dtx == null && dty == null)
            {
                return 0;
            }
            else if (dtx == null && dty != null)
            {
                return 1;
            }
            else if (dtx != null && dty == null)
            {
                return -1;
            }
            else
            {
                if (dtx != null)
                {
                    return dtx.CompareTo(dty);
                }
                else
                {
                    return 1;
                }
            }
        }
    }
}