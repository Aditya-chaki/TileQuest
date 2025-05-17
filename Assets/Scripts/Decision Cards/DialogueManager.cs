using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public TMP_Text speakerText;
    public TMP_Text dialogueText;
    public GameObject dialoguePanel;
    public GameObject decisionCardPrefab;

    private DialogueSet currentSet;
    private int dialogueIndex = 0;
    private bool dialogueInProgress = false;

    void Update()
    {
        if (dialogueInProgress && Input.GetMouseButtonDown(0))
        {
            ShowNextDialogue();
        }
    }

    public void StartDialogue(string dialogueSetFilename)
    {
        currentSet = DialogueLoader.LoadDialogueSet(dialogueSetFilename);
        if (currentSet == null || currentSet.lines.Length == 0) return;

        dialogueIndex = 0;
        dialoguePanel.SetActive(true);
        dialogueInProgress = true;
        ShowDialogue(currentSet.lines[dialogueIndex]);
    }

    void ShowNextDialogue()
    {
        dialogueIndex++;
        if (dialogueIndex < currentSet.lines.Length)
        {
            ShowDialogue(currentSet.lines[dialogueIndex]);
        }
        else
        {
            EndDialogue();
        }
    }

    void ShowDialogue(DialogueLine line)
    {
        speakerText.text = line.speaker;
        dialogueText.text = line.text;
    }

    void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        dialogueInProgress = false;

        // Spawn decision card from Resources/DecisionCard
        GameObject card = Instantiate(Resources.Load<GameObject>("DecisionCard/YourDecisionCardPrefabs"), transform);
        card.transform.SetAsLastSibling(); // bring to front
    }
}
