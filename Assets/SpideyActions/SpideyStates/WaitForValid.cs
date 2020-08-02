using System.Threading.Tasks;

namespace Assets.SpideyActions.SpideyStates
{
    public class WaitForValid : GenericStateHandler<SpiderCrawly>
    {
        private GenericStateHandler<SpiderCrawly> returnToState;
        public WaitForValid(GenericStateHandler<SpiderCrawly> returnToState)
        {
            this.returnToState = returnToState;
        }

        public Task<GenericStateHandler<SpiderCrawly>> HandleState(SpiderCrawly data)
        {
            if (data.lastNode == null || data.currentConnection == null)
            {
                return Task.FromResult<GenericStateHandler<SpiderCrawly>>(this);
            }
            return Task.FromResult(returnToState);
        }

        public void TransitionIntoState(SpiderCrawly data)
        {
        }
        public void TransitionOutOfState(SpiderCrawly data)
        {
        }
    }
}