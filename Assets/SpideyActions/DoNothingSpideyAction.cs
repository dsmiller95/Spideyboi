
using Assets.Utilities;
using UnityEngine;

namespace Assets.SpideyActions
{
    public class DoNothingSpideyAction : MonoBehaviour, ISpideyAction
    {
        public GenericStateHandler<SpiderCrawly> StateHandlerFactory(GenericStateHandler<SpiderCrawly> returnToOnSuccess)
        {
            return new WaitingStateHandler(returnToOnSuccess, 0.1f);
        }
    }
}
