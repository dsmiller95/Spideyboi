using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.UI.Draggable
{
    public class DragItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public static GameObject itemBeingDragged;
        public DragZone parentDragZone;

        public void Start()
        {
            parentDragZone = parentDragZone ?? transform.parent.GetComponent<DragZone>();
        }

        #region IBeginDragHandler implementation

        public void OnBeginDrag(PointerEventData eventData)
        {
            itemBeingDragged = gameObject;
            var layout = GetComponent<LayoutElement>();
            if (layout != null)
            {
                layout.ignoreLayout = true;
            }
            // TODO: why was this here??
            //GetComponent<CanvasGroup>().blocksRaycasts = false;
        }

        #endregion

        #region IDragHandler implementation

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = new Vector2(eventData.position.x, eventData.position.y);
            parentDragZone?.ElementDragged(this, eventData);
        }

        #endregion

        #region IEndDragHandler implementation
        public void OnEndDrag(PointerEventData eventData)
        {
            itemBeingDragged = null;
            var layout = GetComponent<LayoutElement>();
            if (layout != null)
            {
                layout.ignoreLayout = false;
            }
            parentDragZone?.ItemDroppedOnto(this, eventData);
        }

        #endregion
    }
}
