using UnityEngine;
using UnityEngine.UI;

public class RandomSliderLerp : MonoBehaviour
{
    public Slider slider;    // The slider UI element
    public float lerpSpeed = 1f;  // Speed of the lerp
    public float minValue = 0f;  // Minimum slider value
    public float maxValue = 1f;  // Maximum slider value

    private float targetValue;   // The value to lerp towards

    void Start()
    {
        SetRandomTargetValue();
    }

    void Update()
    {
        // Smoothly interpolate the slider's value towards the target value
        slider.value = Mathf.Lerp(slider.value, targetValue, Time.deltaTime * lerpSpeed);

        // Check if the slider is close enough to the target value, then set a new target
        if (Mathf.Abs(slider.value - targetValue) < 0.01f)
        {
            SetRandomTargetValue();
        }
    }

    void SetRandomTargetValue()
    {
        targetValue = Random.Range(minValue, maxValue); // Generate a new random target value
    }
}
