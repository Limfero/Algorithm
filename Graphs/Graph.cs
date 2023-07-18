namespace Graphs
{
    public class Graph
    {
        public List<Edge> Edges { get; private set; }

        private List<int> AllVertices { get { return GetAllVertices(); } }
        private double[] _shortest;
        private int[] _pred;

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

        public List<int> TopologicalSort()
        {
            Dictionary<int, int> inDegree = GetAllVertexRelationships();

            Stack<int> next = new();

            for (int i = inDegree.Count; i > 0; i--)
                if (inDegree[i] == 0)
                    next.Push(i);
            return GetLinearOrderedVertices(inDegree, next);
        }

        public double[] DagShortestPath(int vertex)
        {
            List<int> linearOrdered = TopologicalSort();
            InitializeShortestAndPred(vertex);

            foreach (var v in linearOrdered)
                foreach (var edge in GetEdgesByVertex(v))
                    Relax(edge);

            return _shortest;
        }

        public double[] Dijkstra(int vertex)
        {
            InitializeShortestAndPred(vertex);

            Dictionary<int, double> vertices = new();
            foreach (var item in AllVertices)
                vertices.Add(item, _shortest[item - 1]);

            while (vertices.Count != 0)
            {
                KeyValuePair<int, double> shortest = vertices.FirstOrDefault(x => x.Value == vertices.Values.Min());

                vertices.Remove(shortest.Key);

                foreach (var edge in GetEdgesByVertex(shortest.Key))
                    Relax(edge);
            }

            return _shortest;
        }

        public double[] BellmanFord(int vertex)
        {
            InitializeShortestAndPred(vertex);

            for (int i = 0; i < AllVertices.Count - 1; i++)
                foreach (var edge in Edges)
                    Relax(edge);

            return _shortest;
        }

        private void InitializeShortestAndPred(int vertex)
        {
            _shortest = new double[AllVertices.Count];
            _pred = new int[AllVertices.Count];

            for (int i = 0; i < _shortest.Length; i++)
                if (i != vertex - 1)
                    _shortest[i] = double.PositiveInfinity;

            foreach (var item in AllVertices)
                _pred[item - 1] = -1;
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

        private void Relax(Edge edge)
        {
            if (_shortest[edge.StartVertex - 1] + edge.Weight < _shortest[edge.EndVertex - 1])
            {
                _shortest[edge.EndVertex - 1] = _shortest[edge.StartVertex - 1] + edge.Weight;
                _pred[edge.EndVertex - 1] = edge.StartVertex;
            }
        }
    }
}
