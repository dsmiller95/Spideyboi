using UnityEngine;

namespace Assets.SpideyActions
{
    public class CreateNodeSpideyAction : MonoBehaviour, ISpideyAction
    {
        public void DoAction(SpiderCrawly crawly, NodeBehavior currentNode, NodeBehavior lastNode, NodeBehavior tentativeNextNode)
        {
            var graph = crawly.graphManager;
            var newNode = graph.CreateNewNodeWithConnectionBetweenTwoConnectionsAtBase(currentNode, lastNode, tentativeNextNode, graph.defaultConnectionLength, crawly.whichSide == TraversalSide.LEFTHAND);
            graph.CreateConnection(currentNode, newNode);

        }
    }
}
