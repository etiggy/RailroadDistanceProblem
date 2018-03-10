using RailroadDistanceProblem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RailroadDistanceProblem
{
    class Program
    {
        //Input string regular expression to help parsing (only letters and '-' character allowed)
        private readonly static string inputStringRegex = @"[a-zA-Z-]*";

        //Initialisation data for railroad network
        private readonly static List<string[]> railwayStationsData = new List<string[]>() {
            new string[] {"A", "B", "5" },
            new string[] {"B", "C", "2" },
            new string[] {"C", "D", "3" },
            new string[] {"B", "E", "4" }
        };

        // Test non-circular graph
        //private readonly static List<string[]> railwayStationsData = new List<string[]>() {
        //    new string[] {"A", "C", "1" },
        //    new string[] {"A", "B", "3" },
        //    new string[] {"A", "D", "1" },
        //    new string[] {"B", "E", "1" },
        //    new string[] {"B", "D", "1" },
        //    new string[] {"C", "F", "4" },
        //    new string[] {"C", "B", "1" },
        //    new string[] {"E", "F", "1" }
        //};

        // Test circular graph
        //private readonly static List<string[]> railwayStationsData = new List<string[]>() {
        //    new string[] {"A", "C", "1" },
        //    new string[] {"A", "B", "5" },
        //    new string[] {"B", "C", "1" },
        //    new string[] {"B", "D", "1" },
        //    new string[] {"C", "F", "1" },
        //    new string[] {"D", "A", "1" },
        //    new string[] {"E", "B", "1" },
        //    new string[] {"F", "E", "1" }
        //};

        //Graph to hold railroad network data
        private static Graph railroadGraph;

        //Main method
        static void Main(string[] args)
        {
            Initialise();

            string[] inputString = ReadInput();
            while (!(inputString.Length == 1 && inputString.First().Equals("Q")))
            {
                PrintOutput(inputString);
                inputString = ReadInput();
            }

            Environment.Exit(0);
        }

        //Set up graph and populate it with railroad network data
        private static void Initialise()
        {
            railroadGraph = new Graph();

            foreach (var distanceData in railwayStationsData)
            {
                railroadGraph.ConnectNodes(distanceData[0], distanceData[1], distanceData[2]);
            }
        }

        //Method to read and parse input data
        private static string[] ReadInput()
        {
            string rawInputString;
            bool isValidInput;

            do
            {
                Console.WriteLine("\nPlease type in the desired route between towns (eg.: A-B-C) or \"Q\" to quit:");
                rawInputString = Console.ReadLine().Trim().ToUpper();

                Match regexResult = Regex.Match(rawInputString, inputStringRegex);

                isValidInput = (regexResult.Success && (rawInputString.Split('-').Count() >= 2 ||
                    (rawInputString.ToCharArray().First().Equals('Q') && rawInputString.ToCharArray().Count() == 1)));

                if (!isValidInput)
                {
                    Console.WriteLine("\nIncorrect input string, please try again");
                }

            } while (!isValidInput);

            return rawInputString.Split('-');
        }

        //Method to initiate graph traversal and print outpot results
        private static void PrintOutput(string[] nodeNameArray)
        {
            bool routeExist = railroadGraph.TraverseNodes(nodeNameArray);

            StringBuilder sb = new StringBuilder();

            foreach (var node in nodeNameArray)
            {
                sb.Append(node);
                sb.Append("-");
            }

            sb.Remove(sb.Length - 1, 1);

            if (routeExist)
            {
                KeyValuePair<string, int> optimalRoute = railroadGraph.GetSmallestEdgeWeightRoute();
                Console.WriteLine("The shortest route for {0} is through {2}: {1} units", sb.ToString(), optimalRoute.Value, optimalRoute.Key);
            }
            else
            {
                Console.WriteLine("The route {0} doesn't exist", sb.ToString());
            }
        }
    }
}
