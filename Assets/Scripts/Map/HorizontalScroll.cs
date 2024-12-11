using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HorizontalScroll : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public RectTransform content; // The RectTransform of the scrolling content.
    public float scrollSpeed = 10f; // Adjusts the speed of the scroll.

    private Vector2 lastDragPosition;
    private bool isDragging;

    private void Update()
    {
        // Optional: Smoothly decelerate the scrolling when the user stops dragging.
        if (!isDragging)
        {
            Vector2 velocity = content.anchoredPosition;
            velocity.x = Mathf.Lerp(velocity.x, Mathf.Round(velocity.x), Time.deltaTime * scrollSpeed);
            content.anchoredPosition = velocity;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        lastDragPosition = eventData.position;
        isDragging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 currentDragPosition = eventData.position;
        float delta = currentDragPosition.x - lastDragPosition.x;

        // Move the content horizontally based on drag delta.
        Vector2 newPosition = content.anchoredPosition;
        newPosition.x += delta;
        content.anchoredPosition = newPosition;

        lastDragPosition = currentDragPosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;
    }
}
