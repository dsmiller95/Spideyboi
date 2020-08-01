namespace Assets.SpideyActions
{
    public class CreateNodeSpideyAction : ISpideyAction
    {
        public void DoAction(SpiderCrawly crawly, NodeBehavior currentNode, NodeBehavior lastNode, NodeBehavior tentativeNextNode)
        {
            var graph = crawly.graphManager;
            var newNode = graph.CreateNewNodeBetweenTwoConnectionsAtBase(currentNode, lastNode, tentativeNextNode, graph.defaultConnectionLength, crawly.whichSide == TraversalSide.LEFTHAND);
            var newCon = new Connection<NodeBehavior>(currentNode, newNode);

            graph.Graph.AddVertex(newNode);
            graph.Graph.AddEdge(newCon);
        }
    }
}
