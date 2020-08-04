using Assets.UI.Draggable;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.UI
{
    public abstract class DragZone : MonoBehaviour
    {
        public abstract void ElementDragged(DragItem element, PointerEventData eventData);

        public abstract void ItemDroppedOnto(DragItem element, PointerEventData eventData);
    }
}
