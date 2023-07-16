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

        public List<int> TopologicalSort()
        {
            Dictionary<int, int> inDegree = GetAllVertexRelationships();

            Stack<int> next = new();

            for (int i = inDegree.Count; i > 0; i--)
                if (inDegree[i] == 0)
                    next.Push(i);
            return GetLinearOrderedVertices(inDegree, next);
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
    }
}
