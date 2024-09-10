using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIFadeInOut : MonoBehaviour
{
    public float fadeDuration = 1f;  // Duration for the fade in/out

    private CanvasGroup canvasGroup;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        // Ensure the CanvasGroup component is set up correctly
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        StartFadeInOut();
    }

    void StartFadeInOut()
    {
        // Fade out to 0, then fade in to 1
        canvasGroup.DOFade(0, fadeDuration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo); // Loop indefinitely with Yoyo effect (fade out then fade in)
    }
}
