using System;

namespace VagabondLib.Utils
{
    /// <summary>
    /// Utility class used to map Tuples to a specified 
    /// </summary>
    public static class TupleMap
    {
        /// <summary>
        /// Map the tuple to a referenced value
        /// </summary>
        /// <typeparam name="T1">Type of Item1 of the Tuple</typeparam>
        /// <param name="t">Tuple to map</param>
        /// <param name="value1">Reference to map Item1 of the Tuple to</param>
        public static void Map<T1>(this Tuple<T1> t,
                                    ref T1 value1)
        {
            value1 = t.Item1;
        }

        /// <summary>
        /// Map the tuple to referenced values
        /// </summary>
        /// <typeparam name="T1">Type of Item1 of the Tuple</typeparam>
        /// <typeparam name="T2">Type of Item2 of the Tuple</typeparam>
        /// <param name="t">Tuple to map</param>
        /// <param name="value1">Reference to map Item1 of the Tuple to</param>
        /// <param name="value2">Reference to map Item2 of the Tuple to</param>
        public static void Map<T1, T2>(this Tuple<T1, T2> t,
                                        ref T1 value1,
                                        ref T2 value2)
        {
            value1 = t.Item1;
            value2 = t.Item2;
        }

        /// <summary>
        /// Map the tuple to referenced values
        /// </summary>
        /// <typeparam name="T1">Type of Item1 of the Tuple</typeparam>
        /// <typeparam name="T2">Type of Item2 of the Tuple</typeparam>
        /// <typeparam name="T3">Type of Item3 of the Tuple</typeparam>
        /// <param name="t">Tuple to map</param>
        /// <param name="value1">Reference to map Item1 of the Tuple to</param>
        /// <param name="value2">Reference to map Item2 of the Tuple to</param>
        /// <param name="value3">Reference to map Item3 of the Tuple to</param>
        public static void Map<T1, T2, T3>(this Tuple<T1, T2, T3> t,
                                            ref T1 value1,
                                            ref T2 value2,
                                            ref T3 value3)
        {
            value1 = t.Item1;
            value2 = t.Item2;
            value3 = t.Item3;
        }

        /// <summary>
        /// Map the tuple to referenced values
        /// </summary>
        /// <typeparam name="T1">Type of Item1 of the Tuple</typeparam>
        /// <typeparam name="T2">Type of Item2 of the Tuple</typeparam>
        /// <typeparam name="T3">Type of Item3 of the Tuple</typeparam>
        /// <typeparam name="T4">Type of Item4 of the Tuple</typeparam>
        /// <param name="t">Tuple to map</param>
        /// <param name="value1">Reference to map Item1 of the Tuple to</param>
        /// <param name="value2">Reference to map Item2 of the Tuple to</param>
        /// <param name="value3">Reference to map Item3 of the Tuple to</param>
        /// <param name="value4">Reference to map Item4 of the Tuple to</param>
        public static void Map<T1, T2, T3, T4>(this Tuple<T1, T2, T3, T4> t,
                                                ref T1 value1,
                                                ref T2 value2,
                                                ref T3 value3,
                                                ref T4 value4)
        {
            value1 = t.Item1;
            value2 = t.Item2;
            value3 = t.Item3;
            value4 = t.Item4;
        }

        /// <summary>
        /// Map the tuple to referenced values
        /// </summary>
        /// <typeparam name="T1">Type of Item1 of the Tuple</typeparam>
        /// <typeparam name="T2">Type of Item2 of the Tuple</typeparam>
        /// <typeparam name="T3">Type of Item3 of the Tuple</typeparam>
        /// <typeparam name="T4">Type of Item4 of the Tuple</typeparam>
        /// <typeparam name="T5">Type of Item5 of the Tuple</typeparam>
        /// <param name="t">Tuple to map</param>
        /// <param name="value1">Reference to map Item1 of the Tuple to</param>
        /// <param name="value2">Reference to map Item2 of the Tuple to</param>
        /// <param name="value3">Reference to map Item3 of the Tuple to</param>
        /// <param name="value4">Reference to map Item4 of the Tuple to</param>
        /// <param name="value5">Reference to map Item5 of the Tuple to</param>
        public static void Map<T1, T2, T3, T4, T5>(this Tuple<T1, T2, T3, T4, T5> t,
                                                    ref T1 value1,
                                                    ref T2 value2,
                                                    ref T3 value3,
                                                    ref T4 value4,
                                                    ref T5 value5)
        {
            value1 = t.Item1;
            value2 = t.Item2;
            value3 = t.Item3;
            value4 = t.Item4;
            value5 = t.Item5;
        }

        /// <summary>
        /// Map the tuple to referenced values
        /// </summary>
        /// <typeparam name="T1">Type of Item1 of the Tuple</typeparam>
        /// <typeparam name="T2">Type of Item2 of the Tuple</typeparam>
        /// <typeparam name="T3">Type of Item3 of the Tuple</typeparam>
        /// <typeparam name="T4">Type of Item4 of the Tuple</typeparam>
        /// <typeparam name="T5">Type of Item5 of the Tuple</typeparam>
        /// <typeparam name="T6">Type of Item6 of the Tuple</typeparam>
        /// <param name="t">Tuple to map</param>
        /// <param name="value1">Reference to map Item1 of the Tuple to</param>
        /// <param name="value2">Reference to map Item2 of the Tuple to</param>
        /// <param name="value3">Reference to map Item3 of the Tuple to</param>
        /// <param name="value4">Reference to map Item4 of the Tuple to</param>
        /// <param name="value5">Reference to map Item5 of the Tuple to</param>
        /// <param name="value6">Reference to map Item6 of the Tuple to</param>
        public static void Map<T1, T2, T3, T4, T5, T6>(this Tuple<T1, T2, T3, T4, T5, T6> t,
                                                        ref T1 value1,
                                                        ref T2 value2,
                                                        ref T3 value3,
                                                        ref T4 value4,
                                                        ref T5 value5,
                                                        ref T6 value6)
        {
            value1 = t.Item1;
            value2 = t.Item2;
            value3 = t.Item3;
            value4 = t.Item4;
            value5 = t.Item5;
            value6 = t.Item6;
        }

        /// <summary>
        /// Map the tuple to referenced values
        /// </summary>
        /// <typeparam name="T1">Type of Item1 of the Tuple</typeparam>
        /// <typeparam name="T2">Type of Item2 of the Tuple</typeparam>
        /// <typeparam name="T3">Type of Item3 of the Tuple</typeparam>
        /// <typeparam name="T4">Type of Item4 of the Tuple</typeparam>
        /// <typeparam name="T5">Type of Item5 of the Tuple</typeparam>
        /// <typeparam name="T6">Type of Item6 of the Tuple</typeparam>
        /// <typeparam name="T7">Type of Item7 of the Tuple</typeparam>
        /// <param name="t">Tuple to map</param>
        /// <param name="value1">Reference to map Item1 of the Tuple to</param>
        /// <param name="value2">Reference to map Item2 of the Tuple to</param>
        /// <param name="value3">Reference to map Item3 of the Tuple to</param>
        /// <param name="value4">Reference to map Item4 of the Tuple to</param>
        /// <param name="value5">Reference to map Item5 of the Tuple to</param>
        /// <param name="value6">Reference to map Item6 of the Tuple to</param>
        /// <param name="value7">Reference to map Item7 of the Tuple to</param>
        public static void Map<T1, T2, T3, T4, T5, T6, T7>(this Tuple<T1, T2, T3, T4, T5, T6, T7> t,
                                                            ref T1 value1,
                                                            ref T2 value2,
                                                            ref T3 value3,
                                                            ref T4 value4,
                                                            ref T5 value5,
                                                            ref T6 value6,
                                                            ref T7 value7)
        {
            value1 = t.Item1;
            value2 = t.Item2;
            value3 = t.Item3;
            value4 = t.Item4;
            value5 = t.Item5;
            value6 = t.Item6;
            value7 = t.Item7;
        }

        /// <summary>
        /// Map the tuple to referenced values
        /// </summary>
        /// <typeparam name="T1">Type of Item1 of the Tuple</typeparam>
        /// <typeparam name="T2">Type of Item2 of the Tuple</typeparam>
        /// <typeparam name="T3">Type of Item3 of the Tuple</typeparam>
        /// <typeparam name="T4">Type of Item4 of the Tuple</typeparam>
        /// <typeparam name="T5">Type of Item5 of the Tuple</typeparam>
        /// <typeparam name="T6">Type of Item6 of the Tuple</typeparam>
        /// <typeparam name="T7">Type of Item7 of the Tuple</typeparam>
        /// <typeparam name="TRest">Tuple Type of the 8+ values of the tuple</typeparam>
        /// <param name="t">Tuple to map</param>
        /// <param name="value1">Reference to map Item1 of the Tuple to</param>
        /// <param name="value2">Reference to map Item2 of the Tuple to</param>
        /// <param name="value3">Reference to map Item3 of the Tuple to</param>
        /// <param name="value4">Reference to map Item4 of the Tuple to</param>
        /// <param name="value5">Reference to map Item5 of the Tuple to</param>
        /// <param name="value6">Reference to map Item6 of the Tuple to</param>
        /// <param name="value7">Reference to map Item7 of the Tuple to</param>
        /// <param name="valueRest">Reference to Tuple to map the 8+ values of the Tuple to</param>
        public static void Map<T1, T2, T3, T4, T5, T6, T7, TRest>(this Tuple<T1, T2, T3, T4, T5, T6, T7, TRest> t,
                                                                    ref T1 value1,
                                                                    ref T2 value2,
                                                                    ref T3 value3,
                                                                    ref T4 value4,
                                                                    ref T5 value5,
                                                                    ref T6 value6,
                                                                    ref T7 value7,
                                                                    ref TRest valueRest)
        {
            value1 = t.Item1;
            value2 = t.Item2;
            value3 = t.Item3;
            value4 = t.Item4;
            value5 = t.Item5;
            value6 = t.Item6;
            value7 = t.Item7;
            valueRest = t.Rest;
        }

        /// <summary>
        /// Map the tuple to referenced values
        /// </summary>
        /// <typeparam name="T1">Type of Item1 of the Tuple</typeparam>
        /// <typeparam name="T2">Type of Item2 of the Tuple</typeparam>
        /// <typeparam name="T3">Type of Item3 of the Tuple</typeparam>
        /// <typeparam name="T4">Type of Item4 of the Tuple</typeparam>
        /// <typeparam name="T5">Type of Item5 of the Tuple</typeparam>
        /// <typeparam name="T6">Type of Item6 of the Tuple</typeparam>
        /// <typeparam name="T7">Type of Item7 of the Tuple</typeparam>
        /// <typeparam name="TRest">Tuple Type of the 8+ values of the tuple</typeparam>
        /// <param name="t">Tuple to map</param>
        /// <param name="value1">Reference to map Item1 of the Tuple to</param>
        /// <param name="value2">Reference to map Item2 of the Tuple to</param>
        /// <param name="value3">Reference to map Item3 of the Tuple to</param>
        /// <param name="value4">Reference to map Item4 of the Tuple to</param>
        /// <param name="value5">Reference to map Item5 of the Tuple to</param>
        /// <param name="value6">Reference to map Item6 of the Tuple to</param>
        /// <param name="value7">Reference to map Item7 of the Tuple to</param>
        /// <param name="MapRest">Action to perform to map the 8th+ values of the Tuple</param>
        public static void Map<T1, T2, T3, T4, T5, T6, T7, TRest>(this Tuple<T1, T2, T3, T4, T5, T6, T7, TRest> t,
                                                                    ref T1 value1,
                                                                    ref T2 value2,
                                                                    ref T3 value3,
                                                                    ref T4 value4,
                                                                    ref T5 value5,
                                                                    ref T6 value6,
                                                                    ref T7 value7,
                                                                    Action<TRest> MapRest)
        {
            value1 = t.Item1;
            value2 = t.Item2;
            value3 = t.Item3;
            value4 = t.Item4;
            value5 = t.Item5;
            value6 = t.Item6;
            value7 = t.Item7;
            MapRest(t.Rest);
        }
    }
}
