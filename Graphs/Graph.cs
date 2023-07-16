namespace Graphs
{
    public class Graph
    {
        public Dictionary<int, Dictionary<int, double>> Edges { get; private set; }

        private readonly string VertexDoesNotExist = "There is no such vertex";
        private readonly string EdgeAlreadyExists = "Such a edge already exists!";

        public Graph(int vertexCount)
        {
            Edges = new();

            for (int i = 1; i <= vertexCount; i++)
                Edges.Add(i, new());

        }

        public void AddEdge(int numberVertexWhereStarts, int numberVertexWhereEnds, double weight)
        {
            if (!Edges.ContainsKey(numberVertexWhereStarts) || !Edges.ContainsKey(numberVertexWhereEnds))
                throw new Exception(VertexDoesNotExist);

            foreach (var vertex in Edges[numberVertexWhereStarts].Keys)
                if (vertex == numberVertexWhereEnds)
                    throw new Exception(EdgeAlreadyExists);

            Edges[numberVertexWhereStarts].Add(numberVertexWhereEnds, weight);
        }

        public List<int> TopologicalSort()
        {
            List<int> linearlyOrderedVertices = new();
            int[] inDegree = new int[Edges.Count + 1];

            for (int i = 0; i < Edges.Count; i++)
                foreach(var vertex in Edges.ElementAt(i).Value.Keys)
                    inDegree[vertex]++;

            Stack<int> next = new();

            for (int i = inDegree.Length - 1; i > 0; i--)
                if (inDegree[i] == 0)
                    next.Push(i);

            while(next.Count != 0)
            {
                int u = next.Pop();

                linearlyOrderedVertices.Add(u);

                foreach (var vertex in Edges.ElementAt(u - 1).Value.Keys)
                {
                    inDegree[vertex]--;

                    if (inDegree[vertex] == 0)
                        next.Push(vertex);
                }
            }

            return linearlyOrderedVertices;
        }
    }
}
