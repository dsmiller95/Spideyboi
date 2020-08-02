using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.UI.Draggable
{
    public class DropSlotDragZone : DragZone
    {

        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {

        }

        DropSlot lastDropCandidate;

        public override void ElementDragged(DragItem dragItem, PointerEventData eventData)
        {
            UpdateLastDropCandidate(dragItem, eventData);
        }

        private void UpdateLastDropCandidate(DragItem item, PointerEventData pointer)
        {
            var currentCandidate = GetFirstIntersectingDropSlot(pointer);

            if (currentCandidate != lastDropCandidate)
            {
                lastDropCandidate?.LostDropCandidate(item);
                currentCandidate?.BecameDropCandidate(item);
                lastDropCandidate = currentCandidate;
            }
        }

        private DropSlot GetFirstIntersectingDropSlot(PointerEventData pointerData)
        {
            int index = 0;
            foreach (var child in GetComponentsInChildren<DropSlot>())
            {
                var rectTransform = child.transform as RectTransform;
                var transformedPoint = rectTransform.InverseTransformPoint(pointerData.position);
                if (rectTransform.rect.Contains(transformedPoint))
                {
                    return child;
                }
                index++;
            }
            return null;
        }

        private bool IsValidDrap(DragItem drop, PointerEventData eventData)
        {
            UpdateLastDropCandidate(drop, eventData);
            return lastDropCandidate != null;
        }

        public override void ItemDroppedOnto(DragItem dropped, PointerEventData eventData)
        {
            if (IsValidDrap(dropped, eventData))
            {
                lastDropCandidate?.DropItemInto(dropped);
                lastDropCandidate?.LostDropCandidate(dropped);
            }
            else
            {
                Destroy(dropped.gameObject);
            }
        }
    }
}