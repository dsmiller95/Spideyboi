namespace Assets.SpideyActions.SpideyStates
{
    public class WaitForValid : GenericStateHandler<SpiderCrawly>
    {
        private GenericStateHandler<SpiderCrawly> returnToState;
        public WaitForValid(GenericStateHandler<SpiderCrawly> returnToState)
        {
            this.returnToState = returnToState;
        }

        public GenericStateHandler<SpiderCrawly> HandleState(SpiderCrawly data)
        {
            if (data.lastNode == null || data.currentConnectionForInspector == null)
            {
                return this;
            }
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