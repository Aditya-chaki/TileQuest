using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIPopupAnimation : MonoBehaviour
{
    public float popupDuration = 0.5f; // Duration of the popup animation
    public float popupScale = 1.2f;    // Scale to animate to

    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        rectTransform.localScale = Vector3.zero; // Start scale is zero (hidden)
        StartPopup();
    }
    public void PressStartPopup(){
        
        StartPopup();
    }

    void StartPopup()
    {
        // Animate the scale to create a popup effect
        rectTransform.DOScale(popupScale, popupDuration)
            .SetEase(Ease.OutBack)
            .OnComplete(() => rectTransform.DOScale(1f, 0.2f).SetEase(Ease.InOutSine));
    }
}
