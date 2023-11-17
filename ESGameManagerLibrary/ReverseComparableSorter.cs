using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESGameManagerLibrary
{
    /// <summary>
    /// Reverse comparable sorter.
    /// </summary>
    public class ReverseComparableSorter : IComparer
    {
        /// <summary>
        /// Compare two objects.
        /// </summary>
        /// <param name="x">first object.</param>
        /// <param name="y">second object.</param>
        /// <returns>-1 if x lt y, 1 if x gt y.</returns>
        public int Compare(object? x, object? y)
        {
            IComparable? dtx = x as IComparable;
            IComparable? dty = y as IComparable;
            if (dtx != null && dty != null)
            {
                return dty.CompareTo(dtx);
            }
            else if (dtx == null && dty != null)
            {
                return -1;
            }
            else if (dtx != null && dty == null)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }
}