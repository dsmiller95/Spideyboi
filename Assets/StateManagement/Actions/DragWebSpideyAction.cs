using Assets.SpideyActions.SpideyStates;
using UnityEngine;

namespace Assets.SpideyActions
{
    public class DragWebSpideyAction : MonoBehaviour, ISpideyAction
    {
        public GenericStateHandler<SpiderCrawly> StateHandlerFactory(GenericStateHandler<SpiderCrawly> returnToOnSuccess)
        {
            return new DragWeb(returnToOnSuccess);
        }
    }
}
