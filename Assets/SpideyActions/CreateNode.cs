
using UnityEngine;

namespace Assets.SpideyActions
{
    public class CreateNodeSpideyAction: ISpideyAction
    {
        public void DoAction(GraphManager graph, INode<NodeBehavior> currentNode, INode<NodeBehavior> lastNode)
        {
            var newNode = graph.CreateNewNode("", currentNode.GetData().transform.position);
            var newCon = new Connection<NodeBehavior>(currentNode, newNode);

            graph.Graph.AddVertex(newNode);
            graph.Graph.AddEdge(newCon);
        }
    }
}
