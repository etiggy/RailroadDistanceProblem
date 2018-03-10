using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RailroadDistanceProblem.Model
{
    //Class representation of an edge between nodes in a graph
    class Edge
    {
        //Public properties with private setters
        internal int weightOfEdge { get; private set; }

        //Reference to the node the edge point towards to
        internal Node nodeB { get; private set; }

        //Default constructor with node to connect to and edge weight parameter
        public Edge(Node nodeB, int weightOfEdge)
        {
            this.nodeB = nodeB;
            this.weightOfEdge = weightOfEdge;
        }

        //Chained constructor with node to connect parameter only
        public Edge(Node nodeB) : this(nodeB, 0) { }
    }
}
