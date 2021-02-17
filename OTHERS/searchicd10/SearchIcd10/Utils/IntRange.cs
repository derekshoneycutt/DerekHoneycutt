using System;
using System.Collections.Generic;
using System.Linq;

namespace SearchIcd10.Utils
{
    /// <summary>
    /// Class representing a range of Integer Values
    /// </summary>
    internal class IntRange : IEnumerable<int>, IComparable<int>, IComparable<IntRange>, IComparable
    {
        /// <summary>
        /// Gets or Sets the Low integer value for the range
        /// </summary>
        public int Low { get; set; }
        /// <summary>
        /// Gets or Sets the High integer value for the range
        /// </summary>
        public int High { get; set; }

        /// <summary>
        /// Prepares an invalid Range of Integers
        /// </summary>
        public IntRange()
        {
            Low = 0;
            High = -1;
        }
        /// <summary>
        /// Prepares a Range including only One Single Integer value
        /// </summary>
        /// <param name="num">The number to represent</param>
        public IntRange(int num)
        {
            Low = num;
            High = num;
        }
        /// <summary>
        /// Prepares a range of integers given the low and high value
        /// </summary>
        /// <param name="low">The low value to use for the range</param>
        /// <param name="high">The high value to use for the range</param>
        public IntRange(int low, int high)
        {
            Low = low;
            High = high;
        }
        /// <summary>
        /// Copy an existing instance of an Integer Range
        /// </summary>
        /// <param name="cpy">IntRange object to copy</param>
        public IntRange(IntRange cpy)
        {
            Low = cpy.Low;
            High = cpy.High;
        }
        /// <summary>
        /// Prepares a range of integers according to an range of existing integer values
        /// <para>Will use the lowest value as the Low and the highest value as the High</para>
        /// </summary>
        /// <param name="values">The existing integer values to build the range from</param>
        public IntRange(IEnumerable<int> values)
        {
            var list = values.ToList();
            Low = list.Min();
            High = list.Max();
        }

        private IEnumerable<int> GetRangeAsEnumerable()
        {
            if (High >= Low)
            {
                for (int onValue = Low; onValue <= High; ++onValue)
                {
                    yield return onValue;
                }
            }
        }

        /// <summary>
        /// Gets the enumerator such that the full range can be iterated over
        /// </summary>
        /// <returns>A new enumerator for the integer range</returns>
        public IEnumerator<int> GetEnumerator()
        {
            return GetRangeAsEnumerable().GetEnumerator();
        }

        /// <summary>
        /// Gets the enumerator such that the full range can be iterated over
        /// </summary>
        /// <returns>A new enumerator for the integer range</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Determines whether an integer falls within the range
        /// </summary>
        /// <param name="value">Value to test</param>
        /// <returns>True if value falls within the range</returns>
        public bool Contains(int value)
        {
            return (value >= Low) && (value <= High);
        }

        /// <summary>
        /// Add a new value in the range
        /// <para>If lower than Low, will be set as the new Low</para>
        /// <para>If higher than High, will be set as the new High</para>
        /// <para>May technically add more than 1 number due to this effect</para>
        /// </summary>
        /// <param name="value">Value to add to the range</param>
        public void Add(int value)
        {
            if (High < Low)
            {
                Low = value;
                High = value;
            }
            else
            {
                if (value < Low)
                {
                    Low = value;
                }
                else if (value > High)
                {
                    High = value;
                }
            }
        }

        /// <summary>
        /// Attempts to add a range of new integer values
        /// <para>Will modify the Low and High of the range to include all values passed</para>
        /// </summary>
        /// <param name="values">New values to add to the range</param>
        public void AddRange(IEnumerable<int> values)
        {
            foreach (var val in values)
            {
                Add(val);
            }
        }

        /// <summary>
        /// Converts the range to a string representation
        /// </summary>
        /// <returns>String representation of the range</returns>
        public override string ToString()
        {
            return String.Format("{0}-{1}", Low, High);
        }

        /// <summary>
        /// Compare the range to an integer
        /// <para>Returns value based on where the int value passed is in relation to the range values</para>
        /// </summary>
        /// <param name="other">Integer to test against the range</param>
        /// <returns>-1 to 1 depending on location of integer compared to the range</returns>
        public int CompareTo(int other)
        {
            if (other > High)
            {
                return -1;
            }
            if (other < Low)
            {
                return 1;
            }
            return 0;
        }

        /// <summary>
        /// Compares another range to this range
        /// </summary>
        /// <param name="other">Range to compare against</param>
        /// <returns>-1 to 1 depending on location of other range compared to the range</returns>
        public int CompareTo(IntRange other)
        {
            if (Object.ReferenceEquals(this, other))
            {
                return 0;
            }

            if ((this.Low >= other.Low) && (this.High <= other.High))
            {
                return 0;
            }
            if ((other.Low >= this.Low) && (other.High <= this.High))
            {
                return 0;
            }

            if (this.Low < other.Low)
            {
                return -1;
            }
            if (this.High > other.High)
            {
                return 1;
            }
            return 0;
        }

        /// <summary>
        /// Compares an unknown object to the Range
        /// <para>Will throw exception if type is not Int32 or IntRange</para>
        /// </summary>
        /// <param name="obj">Object to compare to the range</param>
        /// <returns>-1 to 1 depending on location of object compared to the range</returns>
        public int CompareTo(object obj)
        {
            if (obj is IntRange)
            {
                return this.CompareTo((IntRange)obj);
            }
            else if (obj is int)
            {
                return this.CompareTo((int)obj);
            }
            else
            {
                throw new System.ArgumentException(String.Format("Object in IntRange.CompareTo must be an IntRange or an int. Value passed is {0}", obj.GetType().ToString()));
            }
        }
    }
}
