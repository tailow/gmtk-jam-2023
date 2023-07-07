using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class DropTarget : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler
{
    public UnityEvent<GameObject> DropEvent;
    public UnityEvent MouseEnterEvent;
    public UnityEvent MouseExitEvent;
    
    [SerializeField] private bool _highlightWithoutDraggingObject;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_highlightWithoutDraggingObject || GameManager.Instance.CurrentDraggingObject != null)
        {
            MouseEnterEvent?.Invoke();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_highlightWithoutDraggingObject || GameManager.Instance.CurrentDraggingObject != null)
        {
            MouseExitEvent?.Invoke();
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (GameManager.Instance.CurrentDraggingObject != null)
        {
            DropEvent?.Invoke(GameManager.Instance.CurrentDraggingObject);
        }
    }
}
