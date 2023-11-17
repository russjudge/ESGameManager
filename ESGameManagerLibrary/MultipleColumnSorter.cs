using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ESGameManagerLibrary
{
    /// <summary>
    /// Multiple column sorter.
    /// </summary>
    public class MultipleColumnSorter : IComparer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MultipleColumnSorter"/> class.
        /// </summary>
        /// <param name="propertyNames">property names.</param>
        public MultipleColumnSorter(string[] propertyNames)
        {
            this.PropertyNames = propertyNames;
        }

        private string[] PropertyNames { get; set; }

        /// <summary>
        /// Is Candidate.
        /// </summary>
        /// <param name="properties">Properties.</param>
        /// <param name="propertyNames">Property names.</param>
        /// <returns>true if is candidate.</returns>
        public static bool IsCandidate(ReadOnlyCollection<ItemPropertyInfo> properties, string[] propertyNames)
        {
            return false;

            //below not supported-not sure how to get this to work.
            //bool retVal = true;
            //foreach (string name in propertyNames)
            //{
            //    foreach (ItemPropertyInfo item in Properties)
            //    {
            //        if (item.Name.ToUpper() == name.ToUpper() && item.GetType() != typeof(IComparable))
            //        {
            //            retVal = false;
            //            break;
            //        }
            //    }

            //}
            //return retVal;
        }

        /// <summary>
        /// Compare two date objects.
        /// </summary>
        /// <param name="x">first date object.</param>
        /// <param name="y">second date object.</param>
        /// <returns>-1 if x lt y, 1 if x gt y.</returns>
        public int Compare(object? x, object? y)
        {
            if (x != null && y != null)
            {
                int retVal = 0;
                List<PropertyInfo> propertiesObjectX = new List<PropertyInfo>();
                List<PropertyInfo> propertiesObjectY = new List<PropertyInfo>();
                foreach (string name in this.PropertyNames)
                {
                    var xType = x.GetType();
                    var xProp = xType.GetProperty(name);

                    var yType = y.GetType();
                    var yProp = yType.GetProperty(name);

                    if (xProp != null)
                    {
                        propertiesObjectX.Add(xProp);
                    }

                    if (yProp != null)
                    {
                        propertiesObjectY.Add(yProp);
                    }
                }

                for (int i = 0; i < this.PropertyNames.Length; i++)
                {
                    if (propertiesObjectX[i] != null && propertiesObjectX[i].CanRead && propertiesObjectY[i] != null && propertiesObjectY[i].CanRead)
                    {
                        if (propertiesObjectX[i].GetType() == typeof(IComparable) && propertiesObjectY[i].GetType() == typeof(IComparable))
                        {
                            IComparable? yValue = propertiesObjectY[i].GetValue(y, null) as IComparable;
                            IComparable? xValue = propertiesObjectX[i].GetValue(x, null) as IComparable;
                            if (yValue != null)
                            {
                                retVal = yValue.CompareTo(xValue);
                            }
                        }
                        else
                        {
                            retVal = 0;
                        }
                    }

                    if (retVal != 0)
                    {
                        break;
                    }
                }

                return retVal;
            }
            else if (x == null && y != null)
            {
                return -1;
            }
            else if (x != null && y == null)
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