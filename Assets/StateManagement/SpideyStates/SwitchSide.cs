using QuikGraph;
using System.Threading.Tasks;
using UnityEditor.Experimental.GraphView;

namespace Assets.SpideyActions.SpideyStates
{
    public class SwitchSide : GenericStateHandler<SpiderCrawly>
    {
        private GenericStateHandler<SpiderCrawly> returnToOnsuccess;
        public SwitchSide(GenericStateHandler<SpiderCrawly> returnToOnsuccess)
        {
            this.returnToOnsuccess = returnToOnsuccess;
        }

        public async Task<GenericStateHandler<SpiderCrawly>> HandleState(SpiderCrawly crawly)
        {
            crawly.SwitchSide();
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