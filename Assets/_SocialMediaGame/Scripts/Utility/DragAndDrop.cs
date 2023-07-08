using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class DragAndDrop : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    private RectTransform _draggingObject;
    private CanvasGroup _canvasGroup;
    
    public UnityEvent EnableHighlightEvent;
    public UnityEvent DisableHighlightEvent;

    private void Awake()
    {
        _draggingObject = transform as RectTransform;
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        // moves dragged object to cursor position
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(_draggingObject, eventData.position,
                eventData.pressEventCamera, out var globalMousePosition))
        {
            _draggingObject.position = globalMousePosition;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // start dragging object
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(_draggingObject, eventData.position,
                eventData.pressEventCamera, out var globalMousePosition))
        {
            _draggingObject.position = globalMousePosition;
        }

        _draggingObject.localScale *= 1.1f;

        GameManager.Instance.CurrentDraggingObject = _draggingObject.gameObject;
        
        _canvasGroup.blocksRaycasts = false; // otherwise dragged object blocks raycast for drop target
        
        DisableHighlightEvent?.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // stop dragging object, reset back to default position
        _draggingObject.localScale = Vector3.one;
        _draggingObject.localPosition = Vector3.zero;

        _canvasGroup.blocksRaycasts = true;

        //GameManager.Instance.CurrentDraggingObject = null;   // we should probably do this, but it gets executed before drop event
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        EnableHighlightEvent?.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        DisableHighlightEvent?.Invoke();
    }
}
