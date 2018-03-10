using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RailroadDistanceProblem.Model
{
    //Class representation of a node in a graph
    class Node
    {
        //Public properties with private setters
        internal string nameOfNode { get; private set; }
        internal int valueOfNode { get; private set; }

        //Collection of directional edges starting out from the node
        internal List<Edge> listOfEdges { get; private set; }

        //Default constructor with node name and node value parameter
        public Node(string nameOfNode, int valueOfNode)
        {
            listOfEdges = new List<Edge>();
            this.nameOfNode = nameOfNode;
            this.valueOfNode = valueOfNode;
        }

        //Chained constructor with node name parameter only 
        public Node(string nameOfNode) : this(nameOfNode, 0) { }

        //Method to add a directional edge to a node without edge weight parameter
        internal void ConnectTo(Node nodeB)
        {
            ConnectTo(nodeB, 0);
        }

        //Method to add a directional edge to a node with edge weight parameter
        internal void ConnectTo(Node nodeB, int weightOfEdge)
        {
            listOfEdges.Add(new Edge(nodeB, weightOfEdge));
        }

        //Method with boolean return value to check if a node is connected to another node specified in the parameter
        internal bool IsConnectedTo(Node nodeB)
        {
            Edge selectedEdge = listOfEdges.SingleOrDefault(m => m.nodeB.nameOfNode.Equals(nodeB.nameOfNode));
            return (selectedEdge != null) ? true : false;
        }
    }
}
