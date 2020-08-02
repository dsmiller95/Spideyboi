using Assets.UI.Draggable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragSource : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public DragItem dragItemPrefab;
    public DragZone targetDragZone;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private DragItem currentlySpawnedDragItem;

    private DragItem SpawnDragItemAsDragging()
    {
        var newDragItem = Instantiate(dragItemPrefab, targetDragZone.transform);
        return newDragItem.GetComponent<DragItem>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        currentlySpawnedDragItem = SpawnDragItemAsDragging();
        currentlySpawnedDragItem.OnBeginDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        currentlySpawnedDragItem?.OnDrag(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        currentlySpawnedDragItem?.OnEndDrag(eventData);
    }
}
