namespace Assets.SpideyActions
{
    public interface ISpideyAction
    {
        void DoAction(SpiderCrawly crawly, NodeBehavior currentNode, NodeBehavior lastNode, NodeBehavior tentativeNextNode);
    }
}
