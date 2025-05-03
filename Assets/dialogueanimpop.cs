using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class DialogueBoxUI : MonoBehaviour
{
    public CanvasGroup dialogueGroup; // For fade
    public RectTransform dialogueTransform; // For scale animation
    public TextMeshProUGUI dialogueText;

    public float animationDuration = 0.4f;
    public float typeSpeed = 0.05f;

    [TextArea(3, 10)]
    public string fullText;

    private Coroutine typingCoroutine;

    void Start()
    {
        dialogueGroup.alpha = 0;
        dialogueTransform.localScale = Vector3.zero;
        dialogueGroup.gameObject.SetActive(false);
    }

    public void ShowDialogue(string text)
    {
        fullText = text;
        dialogueGroup.gameObject.SetActive(true);
        StartCoroutine(PopIn());
    }

    IEnumerator PopIn()
    {
        float time = 0;
        dialogueTransform.localScale = Vector3.zero;
        dialogueGroup.alpha = 0;

        while (time < animationDuration)
        {
            time += Time.deltaTime;
            float t = time / animationDuration;

            // Smooth pop using ease-out
            float scaleValue = Mathf.SmoothStep(0, 1, t);
            dialogueTransform.localScale = Vector3.one * scaleValue;
            dialogueGroup.alpha = t;
            yield return null;
        }

        dialogueTransform.localScale = Vector3.one;
        dialogueGroup.alpha = 1;

        StartTyping();
    }

    void StartTyping()
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeText(fullText));
    }

    IEnumerator TypeText(string text)
    {
        dialogueText.text = "";
        foreach (char c in text)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typeSpeed);
        }
    }

    public void HideDialogue()
    {
        StartCoroutine(PopOut());
    }

    IEnumerator PopOut()
    {
        float time = 0;
        Vector3 startScale = dialogueTransform.localScale;

        while (time < animationDuration)
        {
            time += Time.deltaTime;
            float t = time / animationDuration;

            float scaleValue = Mathf.SmoothStep(1, 0, t);
            dialogueTransform.localScale = Vector3.one * scaleValue;
            dialogueGroup.alpha = 1 - t;
            yield return null;
        }

        dialogueTransform.localScale = Vector3.zero;
        dialogueGroup.alpha = 0;
        dialogueGroup.gameObject.SetActive(false);
    }
}

