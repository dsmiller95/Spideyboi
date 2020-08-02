using Assets.SpideyActions.SpideyStates;
using UnityEngine;

namespace Assets.SpideyActions
{
    public class SwitchSideSpideyAction : MonoBehaviour, ISpideyAction
    {
        public GenericStateHandler<SpiderCrawly> StateHandlerFactory(GenericStateHandler<SpiderCrawly> returnToOnSuccess)
        {
            return new SwitchSide(returnToOnSuccess);
        }
    }
}
