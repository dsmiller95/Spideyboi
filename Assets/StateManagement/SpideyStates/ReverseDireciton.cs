using QuikGraph;

namespace Assets.SpideyActions.SpideyStates
{
    public class ReverseDirection : GenericStateHandler<SpiderCrawly>
    {
        private GenericStateHandler<SpiderCrawly> returnToOnsuccess;
        public ReverseDirection(GenericStateHandler<SpiderCrawly> returnToOnsuccess)
        {
            this.returnToOnsuccess = returnToOnsuccess;
        }

        public GenericStateHandler<SpiderCrawly> HandleState(SpiderCrawly crawly)
        {
            var lastNode = crawly.lastNode;
            var currentNode = crawly.currentConnectionForInspector.GetOtherVertex(lastNode);

            crawly.lastNode = currentNode;
            crawly.SwitchSide();
            crawly.distanceAlongConnectionForInspector = 0;

            return new Waiting(returnToOnsuccess, .2f);
        }


        public void TransitionIntoState(SpiderCrawly data)
        {
        }

        public void TransitionOutOfState(SpiderCrawly data)
        {
        }
    }
}