namespace Strings
{
    public class Program
    {
        static void Main(string[] args)
        {
            string first = "CATCGA"; string second = "GTACCGTCA";

            int[,] LCSTable = ComputeLCSTable(first, second);

            string longestCommonSubsequence = AssembleLCS(first, second, LCSTable, first.Length, second.Length);

            WriteTable(LCSTable);

            Console.WriteLine(longestCommonSubsequence);
        }

        public static int[,] ComputeLCSTable(string first, string second)
        {
            int[,] result = new int[first.Length + 1, second.Length + 1];

            for (int i = 1; i <= first.Length; i++)
                for (int j = 1; j <= second.Length; j++)
                    if (first[i - 1] == second[j - 1])
                        result[i, j] = result[i - 1, j - 1] + 1;
                    else
                        result[i, j] = (new int[2] { result[i, j - 1], result[i - 1, j] }).Max();

            return result;
        }

        public static string AssembleLCS(string first, string second, int[,] LCSTable, int lengthFirst, int lengthSecond)
        {
            if (LCSTable[lengthFirst, lengthSecond] == 0)
                return "";

            if (first[lengthFirst - 1] == second[lengthSecond - 1])
                return AssembleLCS(first, second, LCSTable, lengthFirst - 1, lengthSecond - 1) + first[lengthFirst - 1];
            else if (LCSTable[lengthFirst, lengthSecond - 1] > LCSTable[lengthFirst - 1, lengthSecond])
                return AssembleLCS(first, second, LCSTable, lengthFirst, lengthSecond - 1);
            else
                return AssembleLCS(first, second, LCSTable, lengthFirst - 1, lengthSecond);
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