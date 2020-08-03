using QuikGraph;
using System.Threading.Tasks;
using UnityEditor.Experimental.GraphView;

namespace Assets.SpideyActions.SpideyStates
{
    public class ReverseDirection : GenericStateHandler<SpiderCrawly>
    {
        private GenericStateHandler<SpiderCrawly> returnToOnsuccess;
        public ReverseDirection(GenericStateHandler<SpiderCrawly> returnToOnsuccess)
        {
            this.returnToOnsuccess = returnToOnsuccess;
        }

        public async Task<GenericStateHandler<SpiderCrawly>> HandleState(SpiderCrawly crawly)
        {
            var lastNode = crawly.lastNode;
            var currentNode = crawly.currentConnectionForInspector.GetOtherVertex(lastNode);

            crawly.lastNode = currentNode;
            crawly.SwitchSide();
            crawly.distanceAlongConnectionForInspector = 0;
            await Task.Delay(100);
            return returnToOnsuccess;
        }


        public void TransitionIntoState(SpiderCrawly data)
        {
        }

        public void TransitionOutOfState(SpiderCrawly data)
        {
        }
    }
}