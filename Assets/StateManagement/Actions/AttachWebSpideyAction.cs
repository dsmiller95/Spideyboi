using Assets.SpideyActions.SpideyStates;
using UnityEngine;

namespace Assets.SpideyActions
{
    public class AttachWebSpideyAction : MonoBehaviour, ISpideyAction
    {
        public GenericStateHandler<SpiderCrawly> StateHandlerFactory(GenericStateHandler<SpiderCrawly> returnToOnSuccess)
        {
            return new AttachWeb(returnToOnSuccess);
        }
    }
}
