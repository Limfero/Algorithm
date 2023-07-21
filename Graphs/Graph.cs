namespace Graphs
{
    public class Graph
    {
        public List<Edge> Edges { get; private set; }

        private List<int> AllVertices { get { return GetAllVertices(); } }

        private readonly string EdgeAlreadyExists = "Such a edge already exists!";

        public Graph()
        {
            Edges = new();
        }

        public void AddEdge(Edge edge)
        {
            if (Edges.Contains(edge))
                throw new Exception(EdgeAlreadyExists);

            Edges.Add(edge);
        }

        public int[] TopologicalSort()
        {
            Dictionary<int, int> inDegree = GetAllVertexRelationships();

            Stack<int> next = new();

            for (int i = inDegree.Count; i > 0; i--)
                if (inDegree[i] == 0)
                    next.Push(i);
            return GetLinearOrderedVertices(inDegree, next).ToArray();
        }

        public double[] DagShortestPath(int vertex)
        {
            int[] linearOrdered = TopologicalSort();
            InitializeShortestAndPred(vertex, out double[] shortest, out int[] pred);

            foreach (var v in linearOrdered)
                foreach (var edge in GetEdgesByVertex(v))
                    Relax(edge, shortest, pred);

            return shortest;
        }

        public double[] Dijkstra(int vertex)
        {
            InitializeShortestAndPred(vertex, out double[] shortest, out int[] pred);

            Dictionary<int, double> vertices = new();
            foreach (var item in AllVertices)
                vertices.Add(item, shortest[item - 1]);

            while (vertices.Count != 0)
            {
                KeyValuePair<int, double> shortestEdge = vertices.FirstOrDefault(x => x.Value == vertices.Values.Min());

                vertices.Remove(shortestEdge.Key);

                foreach (var edge in GetEdgesByVertex(shortestEdge.Key))
                    Relax(edge, shortest, pred);
            }

            return shortest;
        }

        public List<Array> BellmanFord(int vertex)
        {
            InitializeShortestAndPred(vertex, out double[] shortest, out int[] pred);

            for (int i = 0; i < AllVertices.Count - 1; i++)
                foreach (var edge in Edges)
                    Relax(edge, shortest, pred);

            return new() { shortest, pred };
        }

        public int[] FindNegativeWeightCycle()
        {
            double[] shortest = (double[])BellmanFord(AllVertices.First())[0];
            int[] pred = (int[])BellmanFord(AllVertices.First())[1];

            foreach (var edge in Edges)
                if (shortest[edge.StartVertex] + edge.Weight < shortest[edge.EndVertex])
                {
                    int cycleStartVertex = FindingCycleStartVertex(edge, pred);
                    return SearchAllVerticesInCycle(cycleStartVertex, pred);
                }

            return Array.Empty<int>();
        }

        public Dictionary<string, double> FloydWarshall()
        {
            InitializeShortest(out double[,] shortest);

            FindingShortestPathForEachVertex(shortest);

            return GetResult(shortest);
        }

        private Dictionary<string, double> GetResult(double[,] shortest)
        {
            Dictionary<string, double> result = new();

            for (int u = 0; u < AllVertices.Count; u++)
                for (int v = 0; v < AllVertices.Count; v++)
                    if(u != v)
                        result.Add(string.Format("{0} - {1}", u + 1, v + 1), shortest[u, v]);

            return result;
        }

        private void FindingShortestPathForEachVertex(double[,] shortest)
        {
            for (int k = 0; k < AllVertices.Count; k++)
                for (int i = 0; i < AllVertices.Count; i++)
                    for (int j = 0; j < AllVertices.Count; j++)
                        if (shortest[i, j] > shortest[i, k] + shortest[k, j])
                            shortest[i, j] = shortest[i, k] + shortest[k, j];
        }

        private void InitializeShortest(out double[,] shortest)
        {
            shortest = new double[AllVertices.Count, AllVertices.Count];

            foreach (var edge in Edges)
                shortest[edge.StartVertex - 1, edge.EndVertex - 1] = edge.Weight;

            for (int i = 0; i < AllVertices.Count; i++)
                for (int j = 0; j < AllVertices.Count; j++)
                    if (i != j && shortest[i, j] == 0)
                        shortest[i, j] = double.PositiveInfinity;
        }

        private int[] SearchAllVerticesInCycle(int cycleStartVertex, int[] pred)
        {
            int vertexInCycle = pred[cycleStartVertex];
            Stack<int> cycle = new();
            cycle.Push(cycleStartVertex);

            while (vertexInCycle != cycleStartVertex)
            {
                cycle.Push(vertexInCycle);
                vertexInCycle = pred[vertexInCycle];
            }

            return cycle.ToArray();
        }

        private int FindingCycleStartVertex(Edge edge, int[] pred)
        {
            bool[] visited = new bool[AllVertices.Count];
            int thisVertex = edge.EndVertex;

            while (visited[thisVertex] == false)
            {
                visited[thisVertex] = true;
                thisVertex = pred[thisVertex];
            }

            return thisVertex;
        }

        private void InitializeShortestAndPred(int vertex, out double[] shortest, out int[] pred)
        {
            shortest = new double[AllVertices.Count];
            pred = new int[AllVertices.Count];

            for (int i = 0; i < shortest.Length; i++)
                if (i != vertex - 1)
                    shortest[i] = double.PositiveInfinity;

            foreach (var item in AllVertices)
                pred[item - 1] = -1;
        }

        private List<int> GetLinearOrderedVertices(Dictionary<int, int> inDegree, Stack<int> next)
        {
            List<int> result = new();

            while (next.Count != 0)
            {
                int u = next.Pop();

                result.Add(u);

                foreach (var edge in GetEdgesByVertex(u))
                {
                    inDegree[edge.EndVertex]--;

                    if (inDegree[edge.EndVertex] == 0)
                        next.Push(edge.EndVertex);
                }
            }

            return result;
        }

        private Dictionary<int, int> GetAllVertexRelationships()
        {
            Dictionary<int, int> inDegree = new();

            foreach (var vertex in AllVertices)
            {
                foreach (var adjacentEdge in GetEdgesByVertex(vertex))
                {
                    if (inDegree.ContainsKey(adjacentEdge.EndVertex))
                        inDegree[adjacentEdge.EndVertex]++;
                    else
                        inDegree.Add(adjacentEdge.EndVertex, 1);

                    if (!inDegree.ContainsKey(adjacentEdge.StartVertex))
                        inDegree.Add(adjacentEdge.StartVertex, 0);
                }
            }

            return inDegree;
        }

        private List<int> GetAllVertices()
        {
            List<int> allVertices = new();

            foreach (var edge in Edges)
            {
                if (!allVertices.Contains(edge.StartVertex))
                    allVertices.Add(edge.StartVertex);

                if (!allVertices.Contains(edge.EndVertex))
                    allVertices.Add(edge.EndVertex);
            }

            return allVertices;
        }

        private List<Edge> GetEdgesByVertex(int vertex)
        {
            List<Edge> result = new();

            foreach (var edge in Edges)
                if (edge.StartVertex == vertex)
                    result.Add(edge);

            return result;
        }

        private void Relax(Edge edge, double[] shortest, int[] pred)
        {
            if (shortest[edge.StartVertex - 1] + edge.Weight < shortest[edge.EndVertex - 1])
            {
                shortest[edge.EndVertex - 1] = shortest[edge.StartVertex - 1] + edge.Weight;
                pred[edge.EndVertex - 1] = edge.StartVertex;
            }
        }
    }
}
