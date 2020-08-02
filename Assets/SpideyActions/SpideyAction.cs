using Assets.Utilities;

namespace Assets.SpideyActions
{
    public interface ISpideyAction
    {
        GenericStateHandler<SpiderCrawly> StateHandlerFactory(GenericStateHandler<SpiderCrawly> returnToOnSuccess);
    }
}
