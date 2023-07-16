namespace Graphs
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Graph graph = new();

            graph.AddEdge(new(1, 3, 1));
            graph.AddEdge(new(2, 4, 1));
            graph.AddEdge(new(3, 4, 1));
            graph.AddEdge(new(3, 5, 1));
            graph.AddEdge(new(4, 6, 1));
            graph.AddEdge(new(5, 6, 1));
            graph.AddEdge(new(6, 11, 1));
            graph.AddEdge(new(9, 10, 1));
            graph.AddEdge(new(10, 11, 1));
            graph.AddEdge(new(11, 12, 1));
            graph.AddEdge(new(12, 13, 1));
            graph.AddEdge(new(6, 7, 1));
            graph.AddEdge(new(7, 8, 1));
            graph.AddEdge(new(8, 13, 1));
            graph.AddEdge(new(13, 14, 1));

            List<int> ints = graph.TopologicalSort();

            foreach (var vertex in ints)
                Console.WriteLine(vertex);
        }
    }
}