using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphs
{
    public class Edge
    {
        public int StartVertex { get; init; }
        
        public int EndVertex { get; init; }

        public double Weight { get; init; }

        public Edge(int startVertex, int endVertex, double weight)
        {
            StartVertex = startVertex;
            EndVertex = endVertex;
            Weight = weight;
        }
    }
}
