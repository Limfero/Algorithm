namespace Strings
{
    public class Program
    {
        private readonly static string Delete = "del {0}";
        private readonly static string Insert = "ins {0}";
        private readonly static string Replace = "rep {0} by {1}";
        private readonly static string Copy = "copy {0}";

        static void Main(string[] args)
        {
            string first = "CATCGA"; string second = "GTACCGTCA";

            int[,] LCSTable = ComputeLCSTable(first, second);

            string longestCommonSubsequence = AssembleLCS(first, second, LCSTable, first.Length, second.Length);

            first = "ACAAGC"; second = "CCGT";

            List<Array> tables = ComputeTransformTables(first, second, -1, 1, 2, 2);

            WriteLCSTable(LCSTable);

            Console.WriteLine(longestCommonSubsequence);

            WriteTransformTables(tables);

            Console.WriteLine("\n{0}", AssembleTransformation((string[,])tables[1], first.Length, second.Length));
        }

        private static void WriteTransformTables(List<Array> tables)
        {
            Console.WriteLine();
            for (int i = 0; i < tables[0].GetLength(0); i++)
            {
                for (int j = 0; j < tables[0].GetLength(1); j++)
                    Console.Write("{0, -2} - {1, -13}", ((int[,])tables[0])[i, j], ((string[,])tables[1])[i, j]);

                Console.WriteLine();
            }
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

        public static List<Array> ComputeTransformTables(string first, string second, int copyingCost, int replacementCost, int removalСost, int insertCost)
        {
            InitializationCostAndOpArrays(first, second, removalСost, insertCost, out int[,] cost, out string[,] op);

            SetMinimizedValueOfAllowableOperations(first, second, copyingCost, replacementCost, removalСost, insertCost, cost, op);

            return new List<Array>() { cost, op};
        }

        public static string AssembleTransformation(string[,] op, int i, int j)
        {
            if (i == 0 && j == 0)
                return string.Empty;

            if (op[i, j].Split(" ")[0] == Copy.Split(" ")[0] || op[i, j].Split(" ")[0] == Replace.Split(" ")[0])
                return AssembleTransformation(op, i - 1, j - 1) + op[i, j] + "\n";
            else if (op[i, j].Split(" ")[0] == Delete.Split(" ")[0])
                return AssembleTransformation(op, i - 1, j) + op[i, j] + "\n";
            else
                return AssembleTransformation(op, i, j - 1) + op[i, j] + "\n";
        }

        private static void InitializationCostAndOpArrays(string first, string second, int removalСost, int insertCost, out int[,] cost, out string[,] op)
        {
            cost = new int[first.Length + 1, second.Length + 1];
            op = new string[first.Length + 1, second.Length + 1];

            for (int i = 1; i <= first.Length; i++)
                ChangingArrayValues(cost, op, i, 0, i * removalСost, string.Format(Delete, first[i - 1]));

            for (int i = 1; i <= second.Length; i++)
                ChangingArrayValues(cost, op, 0, i, i * insertCost, string.Format(Insert, second[i - 1]));
        }

        private static void SetMinimizedValueOfAllowableOperations(string first, string second, int copyingCost, int replacementCost, int removalСost, int insertCost, int[,] cost, string[,] op)
        {
            for (int i = 1; i <= first.Length; i++)
            {
                for (int j = 1; j <= second.Length; j++)
                {
                    if (first[i - 1] == second[j - 1])
                        ChangingArrayValues(cost, op, i, j, cost[i - 1, j - 1] + copyingCost, string.Format(Copy, first[i - 1]));
                    else
                        ChangingArrayValues(cost, op, i, j, cost[i - 1, j - 1] + replacementCost, string.Format(Replace, first[i - 1], second[j - 1]));

                    if (cost[i - 1, j] + removalСost < cost[i, j])
                        ChangingArrayValues(cost, op, i, j, cost[i - 1, j] + removalСost, string.Format(Delete, first[i - 1]));

                    if (cost[i, j - 1] + insertCost < cost[i, j])
                        ChangingArrayValues(cost, op, i, j, cost[i, j - 1] + insertCost, string.Format(Insert, second[j - 1]));
                }
            }
        }

        private static void ChangingArrayValues(int[,] cost, string[,] op, int startIndex, int endIndex, int newCost, string operation)
        {
            cost[startIndex, endIndex] = newCost;
            op[startIndex, endIndex] = operation;
        }

        private static void WriteLCSTable(int[,] LCSTable)
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