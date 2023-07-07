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
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(_draggingObject, eventData.position,
                eventData.pressEventCamera, out var globalMousePosition))
        {
            _draggingObject.position = globalMousePosition;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(_draggingObject, eventData.position,
                eventData.pressEventCamera, out var globalMousePosition))
        {
            _draggingObject.position = globalMousePosition;
        }

        _draggingObject.localScale *= 1.1f;

        GameManager.Instance.CurrentDraggingObject = _draggingObject.gameObject;

        _canvasGroup.blocksRaycasts = false;
        
        DisableHighlightEvent?.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _draggingObject.localScale = Vector3.one;
        _draggingObject.localPosition = Vector3.zero;
        
        GameManager.Instance.CurrentDraggingObject = null;
        
        _canvasGroup.blocksRaycasts = true;
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
