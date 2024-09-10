using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIFloatingAnimation : MonoBehaviour
{
    // Adjust these values to customize your floating animation
    public float floatDistance = 10f; // Distance to move up and down
    public float duration = 2f;       // Duration of one float cycle

    private RectTransform rectTransform;
    private Vector3 originalPosition;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        originalPosition = rectTransform.anchoredPosition;
        StartFloating();
    }

    void StartFloating()
    {
        // Create a sequence for the floating animation
        Sequence floatingSequence = DOTween.Sequence();

        // Add the floating animation to the sequence
        floatingSequence.Append(rectTransform.DOAnchorPosY(originalPosition.y + floatDistance, duration)
            .SetEase(Ease.InOutSine))
            .Append(rectTransform.DOAnchorPosY(originalPosition.y - floatDistance, duration)
            .SetEase(Ease.InOutSine))
            .SetLoops(-1, LoopType.Yoyo); // Loop indefinitely with a Yoyo effect
    }
}
