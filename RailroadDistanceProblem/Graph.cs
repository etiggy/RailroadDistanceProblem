using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RailroadDistanceProblem.Model
{
    //Class representation of a graph
    class Graph
    {
        //Collection of all the nodes in the graph
        private List<Node> listOfNodes { get; private set; }

        //Private collection to store all possible routes from onwards the traversal process
        private List<List<Node>> listOfPossibleRoutes { get; set; }

        //Default constructor to create graph
        public Graph()
        {
            listOfNodes = new List<Node>();
        }

        //Method to create a node in the graph with node name and node value parameter. Checks before creation for node name clashes to 
        //circumvent duplication.
        //Returns boolean true on success and false on failure.
        internal bool CreateNode(string nameOfNode, int valueOfNode)
        {
            bool createdOK = false;

            if (GetNode(nameOfNode) == null)
            {
                listOfNodes.Add(new Node(nameOfNode, valueOfNode));
                createdOK = true;
            }

            return createdOK;
        }

        //Overload for method to create a node in the graph with node name parameter. Checks before creation for node name clashes to circumvent 
        //duplication.
        //Returns boolean true on success and false on failure.
        internal bool CreateNode(string nameOfNode)
        {
            return CreateNode(nameOfNode, 0);
        }

        //Internal method to return reference of node from a custom list of nodes based on node name parameter 
        private Node GetNode(string nameOfNode, List<Node> listOfNode)
        {
            return listOfNode.SingleOrDefault(m => m.nameOfNode.Equals(nameOfNode));
        }

        //Overload for internal method to return reference of node from the graph based on a node name parameter
        private Node GetNode(string nameOfNode)
        {
            return GetNode(nameOfNode, listOfNodes);
        }

        //Method to connect two nodes with an edge. Extra parameters to add weight to edge and set up bidirectional edges.
        //Returns boolean true on success and false on failure.
        internal bool ConnectNodes(string nameOfNodeA, string nameOfNodeB, int weightOfEdge, bool unidirectionalEdge)
        {
            bool connectionSuccessful = false;

            CreateNode(nameOfNodeA);
            Node nodeA = GetNode(nameOfNodeA);

            CreateNode(nameOfNodeB);
            Node nodeB = GetNode(nameOfNodeB);

            if (unidirectionalEdge)
            {
                if (!nodeA.IsConnectedTo(nodeB) && !nodeB.IsConnectedTo(nodeB))
                {
                    nodeA.ConnectTo(nodeB, weightOfEdge);
                    nodeB.ConnectTo(nodeA, weightOfEdge);
                    connectionSuccessful = true;
                }
            }
            else
            {
                if (!nodeA.IsConnectedTo(nodeB))
                {
                    nodeA.ConnectTo(nodeB, weightOfEdge);
                    connectionSuccessful = true;
                }
            }

            return connectionSuccessful;
        }

        //Overload for method to connect two nodes with a directional edge. Extra parameter to add weight to edge.
        //Returns boolean true on success and false on failure.
        internal bool ConnectNodes(string nameOfNodeA, string nameOfNodeB, int weightOfEdge)
        {
            return ConnectNodes(nameOfNodeA, nameOfNodeB, weightOfEdge, false);
        }

        //Overload for method to connect two nodes with a directional edge. Extra parameter to add weight to edge in string format.
        //Returns boolean true on success and false on failure.
        internal bool ConnectNodes(string nameOfNodeA, string nameOfNodeB, string weightOfEdge)
        {
            int weightOfEdgeInt = 0;

            return (Int32.TryParse(weightOfEdge, out weightOfEdgeInt)) ? ConnectNodes(nameOfNodeA, nameOfNodeB, weightOfEdgeInt, false) : false;
        }

        //Method to initiate the traversal of the graph. Accepts string array as parameter with list of node names to include in the path search.
        //The first item from the array is selected as the starting point and the last as the destination. The inclusion of ntermediary nodes is 
        //checked by a separate method after the initial run of the traversal algorithm.
        //Returns boolean true if at least one possible route is found and false if none.
        internal bool TraverseNodes(string[] nodeNameArray)
        {
            listOfPossibleRoutes = new List<List<Node>>();

            Node nodeA = GetNode(nodeNameArray.First());
            Node nodeB = GetNode(nodeNameArray.Last());

            if (nodeA != null && nodeB != null)
            {
                List<Node> nodesOnRoute = new List<Node>();
                DepthFirstRecursive(nodesOnRoute, nodeA, nodeB);
            }

            FilterPossibleRoutesForIntermediaryNodeInclusionInOrder(nodeNameArray);

            return (listOfPossibleRoutes.Count > 0) ? true : false;
        }

        //Recursive method to traverse graph using a depth-first search algorithm. Takes a route tracing list, a starting and a destination 
        //node as parameters.
        private void DepthFirstRecursive(List<Node> nodesOnRoute, Node nodeA, Node nodeB)
        {
            nodesOnRoute.Add(nodeA);

            foreach (var edge in nodeA.listOfEdges)
            {
                Node nextNode = edge.nodeB;

                if (!nodesOnRoute.Contains(nextNode))
                {
                    if (nextNode == nodeB)
                    {
                        List<Node> tempNodesOnRoute = new List<Node>();

                        foreach (var node in nodesOnRoute)
                        {
                            tempNodesOnRoute.Add(node);
                        }

                        tempNodesOnRoute.Add(nextNode);
                        listOfPossibleRoutes.Add(tempNodesOnRoute);
                    }
                    else
                    {
                        DepthFirstRecursive(nodesOnRoute, nextNode, nodeB);
                    }
                }
            }

            nodesOnRoute.Remove(nodesOnRoute.Last());
        }

        //Method to filter out possible routes that do not contain the intermediary nodes. The order of the intermediary nodes in the parameter 
        //array is preserved.
        private void FilterPossibleRoutesForIntermediaryNodeInclusionInOrder(string[] nodeNameArray)
        {
            List<List<Node>> newListOfPossibleRoutes = new List<List<Node>>();

            foreach (var route in listOfPossibleRoutes)
            {
                int nameOfNodeCharIndexInCurrentRoute = 0;
                bool routeContainsAllIntermediaryNodes = true;

                foreach (var nameOfNode in nodeNameArray)
                {
                    int currentNodeIndex = route.IndexOf(GetNode(nameOfNode, route));

                    if (((currentNodeIndex == 0) && (currentNodeIndex == nameOfNodeCharIndexInCurrentRoute))
                        || (currentNodeIndex > nameOfNodeCharIndexInCurrentRoute))
                    {
                        nameOfNodeCharIndexInCurrentRoute = currentNodeIndex;
                    }
                    else
                    {
                        routeContainsAllIntermediaryNodes = false;
                        break;
                    }
                }

                if (routeContainsAllIntermediaryNodes)
                {
                    newListOfPossibleRoutes.Add(route);
                }
            }

            listOfPossibleRoutes = newListOfPossibleRoutes;
        }

        //Method to summarise the weight of edges in all possible routes found and return the lowest weighted route as a key-value pair
        internal KeyValuePair<string, int> GetSmallestEdgeWeightRoute()
        {
            Dictionary<string, int> dictionaryOfWeightedPossibleRoutes = new Dictionary<string, int>();

            foreach (var route in listOfPossibleRoutes)
            {
                StringBuilder currentRouteNodes = new StringBuilder();
                int currentRouteEdgeWeights = 0;

                for (int i = 0; i < route.Count - 1; i++)
                {
                    Node currentNode = route[i];
                    Node nextNode = route[i + 1];

                    currentRouteNodes.Append(currentNode.nameOfNode);
                    currentRouteNodes.Append('-');
                    currentRouteEdgeWeights += currentNode.listOfEdges.SingleOrDefault(m => m.nodeB.nameOfNode.Equals(nextNode.nameOfNode)).weightOfEdge;
                }

                currentRouteNodes.Append(route.Last().nameOfNode);
                dictionaryOfWeightedPossibleRoutes.Add(currentRouteNodes.ToString(), currentRouteEdgeWeights);
            }

            dictionaryOfWeightedPossibleRoutes = dictionaryOfWeightedPossibleRoutes.OrderBy(m => m.Value).ToDictionary(n => n.Key, n => n.Value);

            return dictionaryOfWeightedPossibleRoutes.FirstOrDefault();
        }
    }
}
