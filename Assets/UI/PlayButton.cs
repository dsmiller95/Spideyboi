using Assets.SpideyActions;
using UnityEngine;

namespace Assets.UI
{
    public class PlayButton : MonoBehaviour
    {
        public GameObject SpideyActionsParent;
        public void EngageSpideyBois()
        {
            var actions = SpideyActionsParent.GetComponentsInChildren<ISpideyAction>();
            var spideyBois = GameObject.FindObjectsOfType<SpiderCrawly>();

            foreach (var boi in spideyBois)
            {
                boi.StartMoving(actions);
            }
        }
    }
}
