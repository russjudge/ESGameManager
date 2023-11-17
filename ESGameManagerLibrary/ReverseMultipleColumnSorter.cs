using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ESGameManagerLibrary
{

    /// <summary>
    /// Reverse Multiple Column Sorter.
    /// </summary>
    public class ReverseMultipleColumnSorter : IComparer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReverseMultipleColumnSorter"/> class.
        /// </summary>
        /// <param name="propertyNames">property names.</param>
        public ReverseMultipleColumnSorter(string[] propertyNames)
        {
            this.PropertyNames = propertyNames;
        }

        private string[] PropertyNames { get; set; }

        /// <summary>
        /// Compare two comparable. objects.
        /// </summary>
        /// <param name="x">first object.</param>
        /// <param name="y">second object.</param>
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
                    var xtype = x.GetType();
                    var ytype = y.GetType();
                    var xProp = xtype.GetProperty(name);
                    var yProp = ytype.GetProperty(name);
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
                            IComparable? xComp = propertiesObjectX[i].GetValue(x, null) as IComparable;
                            IComparable? yComp = propertiesObjectY[i].GetValue(y, null) as IComparable;
                            if (xComp != null && yComp != null)
                            {
                                retVal = xComp.CompareTo(yComp);
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
                return 1;
            }
            else if (x != null && y == null)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }
    }
}