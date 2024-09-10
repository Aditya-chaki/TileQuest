using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SliderAnimation : MonoBehaviour
{
    public Slider slider; // Reference to the Slider component
    public float animationDuration = 1f; // Duration of the animation
    public Character CharacterID;

    void Start()
    {
       int opinionValue = CharacterID.GetOpinion();
        AnimateSlider(opinionValue); // Animates the slider to full value (1.0)
    }

    public void AnimateSlider(float targetValue)
    {
        slider.DOValue(targetValue, animationDuration).SetEase(Ease.Linear);
    }
}
