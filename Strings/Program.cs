namespace Strings
{
    public class Program
    {
        static void Main(string[] args)
        {
            string first = "CATCGA"; string second = "GTACCGTCA";

            int[,] LCSTable = ComputeLCSTable(first, second);

            string longestCommonSubsequence = AssembleLCS(first, second, LCSTable, first.Length - 1, second.Length - 1);

            WriteTable(LCSTable);

            Console.WriteLine(longestCommonSubsequence);
        }

        public static int[,] ComputeLCSTable(string first, string second)
        {
            int[,] result = new int[first.Length, second.Length];

            for (int i = 0; i < first.Length; i++)
                result[i, 0] = 0;

            for (int i = 0; i < second.Length; i++)
                result[0, i] = 0;

            for (int i = 1; i < first.Length; i++)
                for (int j = 1; j < second.Length; j++)
                    if (first[i] == second[j])
                        result[i, j] = result[i - 1, j - 1] + 1;
                    else
                        result[i, j] = (new int[2] { result[i, j - 1], result[i - 1, j] }).Max();

            return result;
        }

        public static string AssembleLCS(string first, string second, int[,] LCSTable, int i, int j)
        {
            if (LCSTable[i, j] == 0)
                return "";

            if (first[i] == second[j])
                return AssembleLCS(first, second, LCSTable, i - 1, j - 1) + first[i];
            else if (LCSTable[i, j - 1] > LCSTable[i - 1, j])
                return AssembleLCS(first, second, LCSTable, i, j - 1);
            else
                return AssembleLCS(first, second, LCSTable, i - 1, j);
        }

        private static void WriteTable(int[,] LCSTable)
        {
            for (int i = 0; i < LCSTable.GetLength(0); i++)
            {
                for (int j = 0; j < LCSTable.GetLength(1); j++)
                    Console.Write("{0} ", LCSTable[i, j]);

                Console.WriteLine();
            }
        }
    }
}