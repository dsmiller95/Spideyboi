using System.Threading.Tasks;

namespace Assets.SpideyActions.SpideyStates
{
    public class Waiting : GenericStateHandler<SpiderCrawly>
    {
        private GenericStateHandler<SpiderCrawly> returnToState;
        private int delay;
        public Waiting(GenericStateHandler<SpiderCrawly> returnToState, float time)
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