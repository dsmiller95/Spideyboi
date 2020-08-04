using Assets.UI.Draggable;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.UI
{
    [RequireComponent(typeof(RectTransform))]
    public class DropSlot : MonoBehaviour
    {
        public UnityEvent AboutToRecieveDrop;
        public UnityEvent PendingDropCancelled;

        public DragItem lastDragRecieve;

        public void BecameDropCandidate(DragItem item)
        {
            AboutToRecieveDrop.Invoke();
        }
        public void LostDropCandidate(DragItem item)
        {
            PendingDropCancelled.Invoke();
        }

        protected void SetChildTransform(DragItem child)
        {
            child.transform.SetParent(transform, false);
            var droppedRect = child.GetComponent<RectTransform>();
            droppedRect.anchoredPosition = Vector3.zero;
        }

        protected void SetItemContents(DragItem item)
        {
            if (lastDragRecieve != null && item != lastDragRecieve && lastDragRecieve.transform.parent == transform)
            {
                Destroy(lastDragRecieve?.gameObject);
            }
            SetChildTransform(item);
            lastDragRecieve = item;
        }

        public void TakeItemContents(DropSlot other)
        {
            SetChildTransform(other.lastDragRecieve);

            lastDragRecieve = other.lastDragRecieve;
            other.lastDragRecieve = null;
        }

        public virtual void DropItemInto(DragItem item)
        {
            SetItemContents(item);
        }
    }
}
