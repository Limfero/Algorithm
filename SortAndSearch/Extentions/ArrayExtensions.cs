namespace SortAndSearch.Extentions
{
    internal static class ArrayExtensions
    {
        private readonly static int NotFound = -1;

        // Searching
        public static int LinearSearch(this int[] array, int seekingValue)
        {
            int answer = NotFound;
            int index = 0;

            while (index < array.Length)
            {
                if (array[index] == seekingValue)
                    answer = index;

                index++;
            }

            return answer;
        }
        
        public static int BetterLinearSearch(this int[] array, int seekingValue)
        {
            int index = 0;

            while (index < array.Length)
            {
                if (array[index] == seekingValue)
                    return index;

                index++;
            }

            return NotFound;
        }

        public static int SentinalLinearSearch(this int[] array, int seekingValue)
        {
            int length = array.Length;
            int last = array[length - 1];
            int index = 0;

            array[length - 1] = seekingValue;

            while (array[index] != seekingValue)
                index++;

            array[length - 1] = last;

            if (index < length - 1 || array[length - 1] == seekingValue)
                return index;

            return NotFound;
        }

        public static int RecursiveLinearSearch(this int[] array, int seekingValue, int startIndex)
        {
            int length = array.Length;

            if (startIndex > length - 1)
                return NotFound;
            else if (startIndex <= length - 1)
                if (array[startIndex] == seekingValue)
                    return startIndex;

            return RecursiveLinearSearch(array, seekingValue, startIndex + 1);
        }

        public static int BinarySearch(this int[] array, int seekingValue)
        {
            int left = 0; int right = array.Length;

            while (left <= right)
            {
                int middle = (left + right) / 2;

                if (array[middle] == seekingValue)
                    return middle;
                else if (array[middle] > seekingValue)
                    right = middle - 1;
                else if (array[middle] < seekingValue)
                    left = middle + 1;
            }

            return NotFound;
        }

        public static int RecursiveBinarySearch(this int[] array, int seekingValue, int left, int right)
        {
            if (left > right)
                return NotFound;

            int middle = (left + right) / 2;

            if (array[middle] == seekingValue)
                return middle;
            else if (array[middle] > seekingValue)
                return RecursiveBinarySearch(array, seekingValue, left, middle - 1);
            else
                return RecursiveBinarySearch(array, seekingValue, middle + 1, right);
        }

        //Sorting
        public static void SelectionSort(this int[] array)
        {
            for (int i = 0; i < array.Length - 1; i++)
            {
                int smalllest = i;

                for (int j = i + 1; j < array.Length; j++)
                    if (array[j] < array[smalllest])
                        smalllest = j;

                (array[i], array[smalllest]) = (array[smalllest], array[i]);
            }
        }

        public static void InsertionSort(this int[] array)
        {
            for (int i = 2; i < array.Length; i++)
            {
                int key = array[i]; int j = i - 1;

                while (j > 0 && array[j] > key)
                {
                    array[j + 1] = array[j];
                    j--;
                }

                array[j + 1] = key;
            }
        }

        public static void MergeSort(this int[] array, int left, int right)
        {
            if (left >= right)
                return;

            int middle = (left + right) / 2;

            MergeSort(array, left, middle);
            MergeSort(array, middle + 1, right);

            Merge(array, left, middle, right);
        }

        private static void Merge(int[] array, int left, int middle, int right)
        {
            int lengthFirst = middle - left + 1;
            int lengthSecond = right - middle;

            int[] first = new int[lengthFirst + 1];
            int[] second = new int[lengthSecond + 1];

            Array.Copy(array, left, first, 0, middle - left + 1);
            Array.Copy(array, middle + 1, second, 0, right - (middle + 1) + 1);

            first[lengthFirst] = int.MaxValue;
            second[lengthSecond] = int.MaxValue;

            int i = 0; int j = 0;
            for (int k = left; k <= right; k++)
                if (first[i] <= second[j])
                {
                    array[k] = first[i];
                    i++;
                }
                else
                {
                    array[k] = second[j];
                    j++;
                }
        }

        public static void QuickSort(this int[] array, int left, int right)
        {
            if (left >= right)
                return;

            int referenceIndex = Partition(array, left, right);

            QuickSort(array, left, referenceIndex - 1);
            QuickSort(array, referenceIndex + 1, right);
        }

        private static int Partition(int[] array, int left, int right)
        {
            int referenceIndex = left;

            for (int i = left; i < right; i++)
                if (array[i] <= array[right])
                {
                    (array[referenceIndex], array[i]) = (array[i], array[referenceIndex]);
                    referenceIndex++;
                }

            (array[referenceIndex], array[right]) = (array[right], array[referenceIndex]);

            return referenceIndex;
        }   

        public static void CoutingSort(this int[] array, int rangeValues)
        {
            int[] equals = CountKeysEquals(array, rangeValues);
            int[] less = CountKeysLess(equals, rangeValues);
            Rerrange(array, less, rangeValues);
        }

        private static int[] CountKeysEquals(int[] array, int rangeValues)
        {
            int[] equals = new int[rangeValues];

            for (int i = 0; i < array.Length; i++)
            {
                int key = array[i];

                equals[key]++;
            }

            return equals;
        }

        private static int[] CountKeysLess(int[] equals, int rangeValues)
        {
            int[] less = new int[rangeValues - 1];

            for (int i = 1; i < rangeValues - 1; i++)
                less[i] = less[i - 1] + equals[i - 1];

            return less;
        }

        private static void Rerrange(int[] array, int[] less, int rangeValues)
        {
            int[] result = new int[array.Length];
            int[] next = new int[rangeValues - 1];

            for (int i = 0; i < rangeValues - 1; i++)
                next[i] = less[i] + 1;

            for (int i = 0; i < array.Length; i++)
            {
                int key = array[i];
                int index = next[key] - 1;

                result[index] = array[i];

                next[key]++;
            }

            Array.Copy(result, array, result.Length);
        }
    }
}
