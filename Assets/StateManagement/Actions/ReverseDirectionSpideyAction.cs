using Assets.SpideyActions.SpideyStates;
using UnityEngine;

namespace Assets.SpideyActions
{
    public class ReverseDirectionSpideyAction : MonoBehaviour, ISpideyAction
    {
        public GenericStateHandler<SpiderCrawly> StateHandlerFactory(GenericStateHandler<SpiderCrawly> returnToOnSuccess)
        {
            return new ReverseDirection(returnToOnSuccess);
        }
    }
}
