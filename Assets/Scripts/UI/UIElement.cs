using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIElement : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public UnityEvent OnClick;
    public UnityEvent OnClickOutside;

    private RectTransform _rectTransform;
    private RectTransform RectTransform => _rectTransform != null ? _rectTransform : _rectTransform = GetComponent<RectTransform>();

    private Camera _mainCamera;
    private EventSystem _eventSystem;
    private bool _isClicked = false;
    private bool _wasLastClicked = false;

    private List<Selectable> selectables = new List<Selectable>();

    private void Awake()
    {
        _mainCamera = Camera.main;
        _eventSystem = EventSystem.current;
        if (_eventSystem == null)
        {
            Debug.LogError("No EventSystem found in the scene.");
        }
    }

    private void Start()
    {
        selectables = gameObject.GetComponentsInChildren<Selectable>().ToList();
    }

    private void Update()
    {
        if (OnClickOutside.GetPersistentEventCount() == 0) return;

        if (_eventSystem != null && (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)))
        {
            Vector2 pointerPosition = GetPointerPosition();

            if (RectTransformUtility.RectangleContainsScreenPoint(RectTransform, pointerPosition, _mainCamera) || _isClicked || IsSelectableSelected())
            {
                return;
            }

            if (_wasLastClicked)
            {
                OnClickOutside?.Invoke();
                _wasLastClicked = false;
            }
        }
    }

    private bool IsSelectableSelected()
    {
        foreach (Selectable selectable in selectables)
        {
            if (EventSystem.current.currentSelectedGameObject != null)
            {
                if (EventSystem.current.currentSelectedGameObject == selectable.gameObject)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnClick?.Invoke();
        _isClicked = true;
        _wasLastClicked = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _isClicked = false;
    }

    private Vector2 GetPointerPosition()
    {
        return Input.touchCount > 0 ? Input.GetTouch(0).position : (Vector2)Input.mousePosition;
    }
}
