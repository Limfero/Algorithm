using System;

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

            int[] ints = graph.TopologicalSort();

            graph = new();

            graph.AddEdge(new(1, 2, 7));
            graph.AddEdge(new(1, 3, 6));
            graph.AddEdge(new(3, 2, 8));
            graph.AddEdge(new(3, 4, 5));
            graph.AddEdge(new(4, 3, -2));
            graph.AddEdge(new(2, 4, -3));
            graph.AddEdge(new(2, 5, 9));
            graph.AddEdge(new(5, 4, 7));
            graph.AddEdge(new(5, 1, 2));
            graph.AddEdge(new(3, 5, -4));

            double[] shortest = graph.Dijkstra(1);
            double[] shortestBF = graph.BellmanFord(1);
            int[] cycle = graph.FindNegativeWeightCycle();

            Write(ints);
            Write(shortest);
            Write(shortestBF);
            Write(cycle);
        }

        private static void Write(Array array)
        {
            foreach (var vertex in array)
                Console.Write("{0} ", vertex);

            Console.WriteLine();
        }
    }
}