using SearchIcd10.Utils;
using SearchIcd10.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;

namespace SearchIcd10.ValueConverters
{
    /// <summary>
    /// Converter Class used to get the part of a List Item's Title that should be "Dulled"
    /// <para>NOTE: First value should be "Dull" string and Second value should be Item's NumberedTitle</para>
    /// <para>Converts to List of WPF Run objects. Does not ConvertBack at all</para>
    /// </summary>
    public class ListItemTitleMultiConverter : IMultiValueConverter
    {
        private IEnumerable<IntRange> GetOverlappingRanges(SortedList<IntRange, bool> ranges, int low, int high)
        {
            foreach (var range in ranges)
            {
                if ((low >= range.Key.Low) && (low <= range.Key.High))
                {
                    yield return range.Key;
                }
                else if ((high >= range.Key.Low) && (high <= range.Key.High))
                {
                    yield return range.Key;
                }
                else if ((range.Key.Low >= low) && (range.Key.Low <= high))
                {
                    yield return range.Key;
                }
                else if ((range.Key.High >= low) && (range.Key.High <= high))
                {
                    yield return range.Key;
                }
            }
        }

        private SortedList<IntRange, bool> UpdateRangesToDullString(string dullString, string startStr, SortedList<IntRange, bool> ranges)
        {
            var ret = new SortedList<IntRange, bool>(ranges);

            if (!String.IsNullOrEmpty(dullString))
            {
                var parentPos = startStr.IndexOf(dullString, 0, StringComparison.CurrentCultureIgnoreCase);
                while (parentPos >= 0)
                {
                    var thisRange = new IntRange(parentPos, parentPos + dullString.Length - 1);

                    var overlapped = GetOverlappingRanges(ret, thisRange.Low, thisRange.High).ToList();

                    bool? useSplit = null;

                    var useOverlapped = new List<IntRange>();
                    if (overlapped.Count > 1)
                    {
                        useOverlapped.AddRange(overlapped);
                    }
                    else
                    {
                        var overlap = overlapped[0];
                        if ((overlap.Low == thisRange.Low) && (overlap.High == thisRange.High))
                        {
                            ret[overlap] = true;
                            return ret;
                        }
                        else
                        {
                            useSplit = ret[overlap];
                            var splitAt = overlap.Low + ((overlap.High - overlap.Low) / 2);
                            useOverlapped.Add(new IntRange(overlap.Low, splitAt));
                            useOverlapped.Add(new IntRange(splitAt + 1, overlap.High));
                        }
                    }

                    var first = new IntRange(useOverlapped.First());
                    if (first.Low < thisRange.Low)
                    {
                        bool useVal = (useSplit == null) ? ret[first] : useSplit.Value;
                        if (useVal)
                        {
                            thisRange.Low = first.Low;
                            first = null;
                        }
                        else
                        {
                            first.High = thisRange.Low - 1;
                        }
                    }
                    else
                    {
                        first = null;
                    }

                    var last = new IntRange(useOverlapped.Last());
                    if (last.High > thisRange.High)
                    {
                        bool useVal = (useSplit == null) ? ret[last] : useSplit.Value;
                        if (useVal)
                        {
                            thisRange.High = last.High;
                            last = null;
                        }
                        else
                        {
                            last.Low = thisRange.High + 1;
                        }
                    }
                    else
                    {
                        last = null;
                    }

                    foreach (var overlap in overlapped)
                    {
                        ret.Remove(overlap);
                    }

                    if (first != null)
                    {
                        ret.Add(first, false);
                    }
                    ret.Add(thisRange, true);
                    if (last != null)
                    {
                        ret.Add(last, false);
                    }

                    parentPos = startStr.IndexOf(dullString, parentPos + 1, StringComparison.CurrentCultureIgnoreCase);
                }
            }

            return ret;
        }

        private SortedList<IntRange, bool> FixRanges(string startStr, SortedList<IntRange, bool> ranges)
        {
            var ret = new SortedList<IntRange, bool>(ranges);

            for (int rangeOn = 0; rangeOn < ret.Count; ++rangeOn)
            {
                var range = ret.ElementAt(rangeOn);
                var partStr = startStr.Substring(range.Key.Low, range.Key.High - range.Key.Low + 1);
                if (String.IsNullOrWhiteSpace(partStr))
                {
                    if (rangeOn > 0)
                    {
                        var useVal = ret.ElementAt(--rangeOn);
                        ret.RemoveAt(rangeOn);
                        ret.RemoveAt(rangeOn);
                        var newRange = new IntRange(useVal.Key.Low, range.Key.High);
                        ret.Add(newRange, useVal.Value);
                    }
                    else if (rangeOn + 1 < ret.Count)
                    {
                        var useVal = ret.ElementAt(rangeOn + 1);
                        ret.RemoveAt(0);
                        ret.RemoveAt(0);
                        var newRange = new IntRange(0, useVal.Key.High);
                        ret.Add(newRange, useVal.Value);
                        --rangeOn;
                    }
                }
                else
                {
                    if (rangeOn > 0)
                    {
                        var useVal = ret.ElementAt(rangeOn - 1);
                        if (useVal.Value == range.Value)
                        {
                            --rangeOn;
                            ret.RemoveAt(rangeOn);
                            ret.RemoveAt(rangeOn);
                            var newRange = new IntRange(useVal.Key.Low, range.Key.High);
                            ret.Add(newRange, useVal.Value);
                        }
                    }
                }
            }

            return ret;
        }

        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var dullstrs = values[0] as IEnumerable<String>;
            var itemTitle = values[1] as NumberedTitle;
            if (itemTitle != null)
            {
                if (dullstrs == null)
                {
                    dullstrs = Enumerable.Empty<string>();
                }
                var runList = new List<Run>();
                runList.Add(new Run(String.Format("{0}. ", itemTitle.Number)));

                var runRanges = new SortedList<IntRange, bool>();
                runRanges.Add(new IntRange(0, itemTitle.Title.Length - 1), false);

                foreach (var dullstr in dullstrs)
                {
                    var words = dullstr.Split(new char[] { }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var word in words)
                    {
                        runRanges = UpdateRangesToDullString(word, itemTitle.Title, runRanges);
                    }
                }
                runRanges = FixRanges(itemTitle.Title, runRanges);

                var rangeList = runRanges.ToList();
                string lastStr = String.Empty;
                string nextStr = String.Empty;
                bool hasGoneNext = false;

                for (int rangeOn = 0; rangeOn < rangeList.Count; ++rangeOn)
                {
                    var range = rangeList[rangeOn];
                    var partStr = itemTitle.Title.Substring(range.Key.Low, range.Key.High - range.Key.Low + 1);
                    if (!hasGoneNext)
                    {
                        if (rangeOn < rangeList.Count - 1)
                        {
                            var nextRange = rangeList[rangeOn + 1];
                            nextStr = itemTitle.Title.Substring(nextRange.Key.Low, nextRange.Key.High - nextRange.Key.Low + 1);
                        }
                        else
                        {
                            nextStr = String.Empty;
                        }
                        if (partStr.StartsWith(" ") || lastStr.EndsWith(" ") || nextStr.StartsWith(" ") || partStr.StartsWith(","))
                        {
                            lastStr = partStr;
                            if (rangeOn > 0)
                            {
                                partStr = String.Format("\r\n         {0}", partStr);
                                hasGoneNext = true;
                            }
                        }
                        else
                        {
                            lastStr = partStr;
                        }
                    }
                    if (range.Value)
                    {
                        var newRun = new Run(partStr);
                        newRun.Foreground = new SolidColorBrush(Colors.Gray);
                        newRun.FontStyle = FontStyles.Italic;
                        runList.Add(newRun);
                    }
                    else
                    {
                        var newRun = new Run(partStr);
                        runList.Add(newRun);
                    }
                }

                return runList;
            }
            return String.Empty;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            return new[] { Binding.DoNothing, Binding.DoNothing };
        }
    }
}
