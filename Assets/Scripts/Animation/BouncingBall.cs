using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class BouncingBall : MonoBehaviour
{
    public float bounceHeight = 50f; // Height in pixels
    public float bounceDuration = 0.5f;
    public int bounceCount = -1; // -1 for infinite loop

    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        StartBouncing();
    }

    void StartBouncing()
    {
        // Initial position
        Vector3 startPos = rectTransform.anchoredPosition;

        // Bounce up
        rectTransform.DOAnchorPosY(startPos.y + bounceHeight, bounceDuration).SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                // Bounce down
                rectTransform.DOAnchorPosY(startPos.y, bounceDuration).SetEase(Ease.InQuad)
                    .OnComplete(() =>
                    {
                        // Repeat bounce
                        if (bounceCount == -1 || bounceCount > 0)
                        {
                            if (bounceCount > 0) bounceCount--;
                            StartBouncing();
                        }
                    });
            });
    }
}
