using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESGameManagerLibrary
{
    /// <summary>
    /// Reverse date sorter.
    /// </summary>
    public class ReverseDateSorter : IComparer
    {
        /// <summary>
        /// Compare two date objects.
        /// </summary>
        /// <param name="x">first date object.</param>
        /// <param name="y">second date object.</param>
        /// <returns>-1 if x lt y, 1 if x gt y.</returns>
        public int Compare(object? x, object? y)
        {
            DateTime? dtx = x as DateTime?;
            DateTime? dty = y as DateTime?;
            if (dty != null && dtx != null)
            {
                return dty.Value.CompareTo(dtx.Value);
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