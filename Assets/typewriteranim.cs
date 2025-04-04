using System.Collections;
using UnityEngine;
using TMPro;

public class TypewriterEffect : MonoBehaviour
{
    public TMP_Text textComponent;          // Assign in Inspector
    public string fullText = "Would you like to trade 100 food for 50 Gold?";

    [Header("Timing (in seconds)")]
    public float startDelay = 1.5f;         // Wait before typing starts
    public float typeDuration = 1f;         // Duration for typing effect
    public float endDelay = 5f;           // Wait after typing ends

    private void Start()
    {
        StartCoroutine(PlayTypewriter());
    }

    IEnumerator PlayTypewriter()
    {
        textComponent.text = "";
        yield return new WaitForSeconds(startDelay);

        int totalChars = fullText.Length;
        float delay = typeDuration / totalChars;

        for (int i = 0; i < totalChars; i++)
        {
            textComponent.text += fullText[i];
            yield return new WaitForSeconds(delay);
        }

        yield return new WaitForSeconds(endDelay);
        // You can trigger more actions here after the typewriter effect completes
    }
}
