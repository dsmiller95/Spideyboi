using Assets.SpideyActions.SpideyStates;

namespace Assets.SpideyActions
{
    public interface ISpideyAction
    {
        GenericStateHandler<SpiderCrawly> StateHandlerFactory(GenericStateHandler<SpiderCrawly> returnToOnSuccess);
    }
}
