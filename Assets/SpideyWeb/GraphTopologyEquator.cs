using QuikGraph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.SpideyWeb
{
    [Serializable]
    public class BasicConnection : IEdge<int>
    {
        public BasicConnection((int, int) IDs)
        {
            source = IDs.Item1;
            target = IDs.Item2;
        }
        public int source;
        public int target;
        public int Target => target;

        public int Source => source;
    }
    public class GraphTopologyEquator
    {

        private UndirectedGraph<int, BasicConnection> targetGraph;
        private int originVertexInGoal;

        public GraphTopologyEquator(IEnumerable<BasicConnection> targetGraph, int originVertexInGoal)
        {
            this.targetGraph = new UndirectedGraph<int, BasicConnection>();
            this.targetGraph.AddVerticesAndEdgeRange(targetGraph);
            this.originVertexInGoal = originVertexInGoal;
        }

        public bool GraphMatches(UndirectedGraph<NodeBehavior, Connection> graph, NodeBehavior itemInGraph)
        {
            Dictionary<int, NodeBehavior> nodePairing = new Dictionary<int, NodeBehavior>();
            nodePairing[originVertexInGoal] = itemInGraph;

            return GraphMatches(graph, nodePairing, originVertexInGoal);

            //var targetEdges = targetGraph.AdjacentEdges(originVertexInGoal);
            //var actualEdges = graph.AdjacentEdges(nodePairing[originVertexInGoal]);
            //if(targetEdges.Count() != actualEdges.Count())
            //{
            //    return false;
            //}

            //foreach(var targetEdge in targetEdges)
            //{
            //    var other = targetEdge.GetOtherVertex(originVertexInGoal);
            //    foreach(var actualEdge in actualEdges)
            //    {
            //        var actualOther = actualEdge.GetOtherVertex(nodePairing[originVertexInGoal]);
            //        nodePairing[other] = actualOther;
            //        if(GraphMatches(graph, nodePairing, other))
            //        {
            //            return true;
            //        }
            //        nodePairing.Remove(other);
            //    }
            //}

            //return false;
        }
        private bool GraphMatches(UndirectedGraph<NodeBehavior, Connection> graph, Dictionary<int, NodeBehavior> currentPairings, int currentNode)
        {
            var targetEdges = targetGraph.AdjacentEdges(currentNode);
            var actualEdges = graph.AdjacentEdges(currentPairings[currentNode]);

            var currentlyUnsetTargetNodes = targetEdges
                .Select(x => x.GetOtherVertex(currentNode))
                .Where(node => !currentPairings.ContainsKey(node))
                .ToList();

            var currentlyUnusedActualNodes = actualEdges
                .Select(x => x.GetOtherVertex(currentPairings[currentNode]))
                .Where(node => !currentPairings.ContainsValue(node))
                .ToList();

            if (currentlyUnsetTargetNodes.Count != currentlyUnusedActualNodes.Count)
            {
                return false;
            }

            foreach (var targetNode in currentlyUnsetTargetNodes)
            {
                foreach (var actualNode in currentlyUnusedActualNodes)
                {
                    currentPairings[targetNode] = actualNode;
                    if (GraphMatches(graph, currentPairings, targetNode))
                    {
                        return true;
                    }
                }
                currentPairings.Remove(targetNode);
            }

            return false;
        }
    }
}
