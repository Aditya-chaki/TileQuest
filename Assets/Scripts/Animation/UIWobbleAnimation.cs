using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIWobbleAnimation : MonoBehaviour
{
    // Adjust these values to customize your wobble animation
    public float wobbleAngle = 10f; // Maximum angle of the wobble
    public float duration = 0.5f;   // Duration of one wobble cycle

    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        StartWobble();
    }

    void StartWobble()
    {
        // Create a sequence for the wobble animation
        Sequence wobbleSequence = DOTween.Sequence();

        // Add the wobble animation to the sequence
        wobbleSequence.Append(rectTransform.DORotate(new Vector3(0, 0, wobbleAngle), duration)
            .SetEase(Ease.InOutSine))
            .Append(rectTransform.DORotate(new Vector3(0, 0, -wobbleAngle), duration)
            .SetEase(Ease.InOutSine))
            .SetLoops(-1, LoopType.Yoyo); // Loop indefinitely with a Yoyo effect
    }
}
