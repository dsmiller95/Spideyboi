using Assets.SpideyActions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.UI
{
    public class PlayButton: MonoBehaviour
    {
        public GameObject SpideyActionsParent;
        public GameObject SpideyBoiParent;
        public void EngageSpideyBois()
        {
            var actions = SpideyActionsParent.GetComponentsInChildren<ISpideyAction>();
            var spideyBois = SpideyBoiParent.GetComponentsInChildren<SpiderCrawly>();

            foreach(var boi in spideyBois)
            {
                boi.StartMoving(actions);
            }
        }
    }
}
