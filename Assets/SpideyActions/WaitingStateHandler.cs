using System.Threading.Tasks;

namespace Assets.Utilities
{
    public class WaitingStateHandler : GenericStateHandler<SpiderCrawly>
    {
        private GenericStateHandler<SpiderCrawly> returnToState;
        private int delay;
        public WaitingStateHandler(GenericStateHandler<SpiderCrawly> returnToState, float time)
        {
            delay = (int)(time * 1000);
            this.returnToState = returnToState;
        }

        public async Task<GenericStateHandler<SpiderCrawly>> HandleState(SpiderCrawly data)
        {
            await Task.Delay(delay);
            return returnToState;
        }

        public void TransitionIntoState(SpiderCrawly data)
        {
        }
        public void TransitionOutOfState(SpiderCrawly data)
        {
        }
    }
}