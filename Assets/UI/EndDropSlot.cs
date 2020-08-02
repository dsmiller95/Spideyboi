using Assets.UI.Draggable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

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

            this.transform.position += (Vector3)translateAmount;
        }
    }
}
