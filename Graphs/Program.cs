namespace Graphs
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Graph graph = new(14);

            graph.AddEdge(1, 3, 1);
            graph.AddEdge(2, 4, 1);
            graph.AddEdge(3, 4, 1);
            graph.AddEdge(3, 5, 1);
            graph.AddEdge(4, 6, 1);
            graph.AddEdge(5, 6, 1);
            graph.AddEdge(6, 11, 1);
            graph.AddEdge(9, 10, 1);
            graph.AddEdge(10, 11, 1);
            graph.AddEdge(11, 12, 1);
            graph.AddEdge(12, 13, 1);
            graph.AddEdge(6, 7, 1);
            graph.AddEdge(7, 8, 1);
            graph.AddEdge(8, 13, 1);
            graph.AddEdge(13, 14, 1);

            List<int> ints = graph.TopologicalSort();

            foreach (var vertex in ints)
                Console.WriteLine(vertex);
        }
    }
}