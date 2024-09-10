using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UISliderAnimation : MonoBehaviour
{
    public Slider slider; // Reference to the Slider component
    public float animationDuration = 1f; // Duration of the slider move animation
    public float fadeDuration = 0.5f; // Duration of the fade-in animation
    public float targetYPosition = 200f; // The Y position where the slider should move to

    private RectTransform sliderRectTransform;
    private CanvasGroup canvasGroup;

    void Start()
    {
        sliderRectTransform = slider.GetComponent<RectTransform>();
        canvasGroup = slider.GetComponent<CanvasGroup>();

        // Start with the slider invisible and off-screen
        canvasGroup.alpha = 0;
        sliderRectTransform.anchoredPosition = new Vector2(0, -Screen.height / 2); // Off-screen

        // Start the animation
        StartSliderAnimation();
    }

    void StartSliderAnimation()
    {
        // Fade in the slider
        canvasGroup.DOFade(1, fadeDuration).OnComplete(() =>
        {
            // Move the slider to the target position
            sliderRectTransform.DOAnchorPosY(targetYPosition, animationDuration).SetEase(Ease.OutQuad);
        });
    }
}
