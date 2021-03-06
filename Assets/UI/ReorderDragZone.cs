﻿using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.UI.Draggable
{
    public delegate void OrderChanged();
    public class ReorderDragZone : DragZone
    {
        public GameObject divider;
        public event OrderChanged orderingChanged;

        // Start is called before the first frame update
        void Start()
        {
            divider.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {

        }

        public override void ElementDragged(DragItem element, PointerEventData eventData)
        {
            int newDividerIndex = GetNewDividerIndex(eventData.position);
            if (newDividerIndex < 0)
            {
                if (divider.activeSelf)
                {
                    divider.SetActive(false);
                }
                return;
            }

            if (!divider.activeSelf)
            {
                divider.SetActive(true);
            }
            if (newDividerIndex != divider.transform.GetSiblingIndex())
            {
                divider.transform.SetSiblingIndex(newDividerIndex);
            }
        }

        private int GetNewDividerIndex(Vector2 dragPositionWorldSpace)
        {
            var validDraggableElements = transform.Cast<RectTransform>()
                .Where(x => x.gameObject != divider && x.gameObject != DragItem.itemBeingDragged)
                .ToList();

            var firstDraggableElement = validDraggableElements.First();
            var worldSpaceTopRange =
                firstDraggableElement.TransformPoint(new Vector3(0, firstDraggableElement.rect.yMax));
            if (dragPositionWorldSpace.y >= worldSpaceTopRange.y)
            {
                return 0;
            }

            var lastIndex = transform.childCount - 1;
            var lastDraggableElement = validDraggableElements.Last();
            var worldSpaceBottomRange =
                lastDraggableElement.TransformPoint(new Vector3(0, lastDraggableElement.rect.yMin));
            if (dragPositionWorldSpace.y <= worldSpaceBottomRange.y)
            {
                return transform.childCount - 1;
            }

            int draggedOverIndex = GetChildIndexOfFirstIntersection(dragPositionWorldSpace);
            Debug.Log($"Dragged over item {draggedOverIndex}");
            if (draggedOverIndex < 0)
            {
                if (divider.activeSelf)
                {
                    divider.SetActive(false);
                }
                return -1;
            }

            var currentDividerIndex = divider.transform.GetSiblingIndex();
            if (draggedOverIndex == currentDividerIndex)
            {
                return draggedOverIndex;
            }
            else
            {
                Debug.Log($"Setting index to {draggedOverIndex + 1}");
                return draggedOverIndex + 1;
            }
        }

        private int GetChildIndexOfFirstIntersection(Vector2 point)
        {
            int index = 0;
            foreach (RectTransform child in transform)
            {
                var transformedPoint = child.InverseTransformPoint(point);
                if (child.rect.Contains(transformedPoint))
                {
                    return index;
                }
                index++;
            }
            return -1;
        }

        private bool IsValidDrap(DragItem drop)
        {
            var dropRect = (drop.transform as RectTransform).rect;
            dropRect.position += (Vector2)drop.transform.localPosition;
            var thisRect = transform as RectTransform;
            return dropRect.Overlaps(thisRect.rect);
        }

        public override void ItemDroppedOnto(DragItem dropped, PointerEventData eventData)
        {
            if (IsValidDrap(dropped))
            {
                dropped.transform.SetSiblingIndex(divider.transform.GetSiblingIndex());
            }
            else
            {
                Destroy(dropped.gameObject);
            }
            divider.SetActive(false);

            orderingChanged?.Invoke();
        }
    }
}