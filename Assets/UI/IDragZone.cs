using Assets.UI.Draggable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.UI
{
    public abstract class DragZone: MonoBehaviour
    {
        public abstract void ElementDragged(DragItem element, PointerEventData eventData);

        public abstract void ItemDroppedOnto(DragItem element, PointerEventData eventData);
    }
}
