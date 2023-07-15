using Algorith.Extentions;
using System.Diagnostics;

namespace Algorith
{
    internal class Program
    {
        static readonly int SeekingValue = -4;
        static readonly int Length = 10000;
        static int[] Array = GenerateArray(Length);
        static readonly SortedDictionary<double, string> SearchingResults = new();
        static readonly SortedDictionary<double, string> SortingResults = new();

        static void Main()
        {
            GetTime(ArrayExtensions.LinearSearch, SearchingResults);
            GetTime(ArrayExtensions.BetterLinearSearch, SearchingResults);
            GetTime(ArrayExtensions.SentinalLinearSearch, SearchingResults);
            GetTime(ArrayExtensions.RecursiveLinearSearch, 0, SearchingResults);

            List<int> sortArray = Array.ToList();
            sortArray.Sort();
            Array = sortArray.ToArray();

            GetTime(ArrayExtensions.BinarySearch, SearchingResults);
            GetTime(ArrayExtensions.RecursiveBinarySearch, 0, Array.Length, SearchingResults);

            Console.WriteLine("Searching");
            WriteResults(SearchingResults);

            Array = GenerateArray(Length);

            GetTime(ArrayExtensions.SelectionSort, SortingResults);
            GetTime(ArrayExtensions.InsertionSort, SortingResults);
            GetTime(ArrayExtensions.MergeSort, 0, Array.Length - 1, SortingResults);
            GetTime(ArrayExtensions.QuickSort, 0, Array.Length - 1, SortingResults);
            GetTime(ArrayExtensions.CoutingSort, 101, SortingResults);

            Console.WriteLine("\nSorting");
            WriteResults(SortingResults);
        }

        public static void GetTime(Func<int[], int, int> func, SortedDictionary<double, string> results)
        {
            Stopwatch timer = new();

            timer.Restart();
            func(Array, SeekingValue);
            timer.Stop();

            results.Add(timer.Elapsed.TotalMilliseconds, func.Method.Name);
        }

        public static void GetTime(Func<int[], int, int, int> func, int startIndex, SortedDictionary<double, string> results)
        {
            Stopwatch timer = new();

            timer.Restart();
            func(Array, SeekingValue, startIndex);
            timer.Stop();

            results.Add(timer.Elapsed.TotalMilliseconds, func.Method.Name);
        }

        public static void GetTime(Func<int[], int, int, int, int> func, int left, int right, SortedDictionary<double, string> results)
        {
            Stopwatch timer = new();

            timer.Restart();
            func(Array, SeekingValue, left, right);
            timer.Stop();

            results.Add(timer.Elapsed.TotalMilliseconds, func.Method.Name);
        }

        public static void GetTime(Action<int[]> act, SortedDictionary<double, string> results)
        {
            Stopwatch timer = new();

            timer.Restart();
            act((int[])Array.Clone());
            timer.Stop();

            results.Add(timer.Elapsed.TotalMilliseconds, act.Method.Name);
        }

        public static void GetTime(Action<int[], int, int> act, int left, int right, SortedDictionary<double, string> results)
        {
            Stopwatch timer = new();

            timer.Restart();
            act((int[])Array.Clone(), left, right);
            timer.Stop();

            results.Add(timer.Elapsed.TotalMilliseconds, act.Method.Name);
        }

        public static void GetTime(Action<int[], int> act, int rangeValues, SortedDictionary<double, string> results)
        {
            Stopwatch timer = new();

            int[] array = (int[])Array.Clone();

            timer.Restart();
            act(array, rangeValues);
            timer.Stop();

            results.Add(timer.Elapsed.TotalMilliseconds, act.Method.Name);
        }


        public static int[] GenerateArray(int length)
        {
            int[] array = new int[length];
            Random random = new();

            for (int i = 0; i < length; i++)
                array[i] = random.Next(100);

            return array;
        }

        public static void WriteResults(SortedDictionary<double, string> results)
        {
            for (int i = 0; i < results.Count; i++)
                Console.WriteLine("{0})Algorith: {1}\n  Time:     {2}", i + 1, results.Values.ToArray()[i], results.Keys.ToArray()[i]);
        }
    }
}
