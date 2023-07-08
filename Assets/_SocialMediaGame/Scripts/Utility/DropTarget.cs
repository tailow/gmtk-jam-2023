using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class DropTarget : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDropHandler
{
    public UnityEvent<GameObject> DropEvent;
    public UnityEvent MouseEnterEvent;
    public UnityEvent MouseExitEvent;
    
    [SerializeField] private bool _highlightWithoutDraggingObject;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_highlightWithoutDraggingObject || eventData.pointerDrag)
        {
            MouseEnterEvent?.Invoke();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_highlightWithoutDraggingObject || eventData.pointerDrag)
        {
            MouseExitEvent?.Invoke();
        }
    }

    // gets executed when dropping held object onto this target
    public void OnDrop(PointerEventData eventData)
    {
        DropEvent?.Invoke(eventData.pointerDrag); // pass dropped object as parameter
        MouseExitEvent?.Invoke();
    }
}
