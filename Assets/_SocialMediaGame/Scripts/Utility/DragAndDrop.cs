using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class DragAndDrop : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private float _dampingSpeed = .05f;
    
    private RectTransform _draggingObject;
    private CanvasGroup _canvasGroup;
    
    public UnityEvent EnableHighlightEvent;
    public UnityEvent DisableHighlightEvent;

    private Vector3 _desiredPosition;
    private Vector3 _velocity;

    private void Awake()
    {
        _draggingObject = transform as RectTransform;
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        _desiredPosition = transform.parent.position;
    }

    private void Update()
    {
        _draggingObject.position =
            Vector3.SmoothDamp(_draggingObject.position, _desiredPosition, ref _velocity, _dampingSpeed);
    }

    public void OnDrag(PointerEventData eventData)
    {
        // moves dragged object to cursor position
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(_draggingObject, eventData.position,
                eventData.pressEventCamera, out var globalMousePosition))
        {
            _desiredPosition = globalMousePosition;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // start dragging object
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(_draggingObject, eventData.position,
                eventData.pressEventCamera, out var globalMousePosition))
        {
            _desiredPosition = globalMousePosition;
        }

        _draggingObject.localScale *= 1.1f;

        _canvasGroup.blocksRaycasts = false; // otherwise dragged object blocks raycast for drop target
        
        DisableHighlightEvent?.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // stop dragging object, reset back to default position
        _draggingObject.localScale = Vector3.one;

        _desiredPosition = transform.parent.position;
        
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
