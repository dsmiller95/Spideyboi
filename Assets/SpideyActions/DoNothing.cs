
using UnityEngine;

namespace Assets.SpideyActions
{
    public class DoNothingSpideyAction : ISpideyAction
    {
        public void DoAction(GraphManager graph, INode<NodeBehavior> currentNode, INode<NodeBehavior> lastNode)
        {
        }
    }
}
