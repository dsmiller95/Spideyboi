using Assets.UI.Draggable;
using UnityEngine;

namespace Assets.UI
{
    [RequireComponent(typeof(RectTransform))]
    public class EndDropSlot : DropSlot
    {
        public DropSlot dropSlotPrefab;
        public Vector2 translateAmount;

        public override void DropItemInto(DragItem item)
        {
            base.DropItemInto(item);

            var newSlot = Instantiate(dropSlotPrefab, transform.parent).GetComponent<DropSlot>();
            newSlot.TakeItemContents(this);

            newSlot.transform.position = transform.position;

            transform.position += (Vector3)translateAmount;
        }
    }
}
