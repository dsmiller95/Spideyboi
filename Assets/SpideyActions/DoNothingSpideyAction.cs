
using UnityEngine;

namespace Assets.SpideyActions
{
    public class DoNothingSpideyAction : MonoBehaviour, ISpideyAction
    {
        public void DoAction(SpiderCrawly crawly, NodeBehavior currentNode, NodeBehavior lastNode, NodeBehavior tentativeNextNode)
        {
        }
    }
}
