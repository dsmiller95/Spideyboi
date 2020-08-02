using Assets.Utilities;
using UnityEngine;

namespace Assets.SpideyActions
{
    public class CreateNodeSpideyAction : MonoBehaviour, ISpideyAction
    {
        public GenericStateHandler<SpiderCrawly> StateHandlerFactory(GenericStateHandler<SpiderCrawly> returnToOnSuccess)
        {
            return new CreateNodeStateHandler(returnToOnSuccess);
        }
    }
}
