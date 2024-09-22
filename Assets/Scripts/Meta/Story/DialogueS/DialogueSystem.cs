using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

namespace VNGame
{
    public class DialogueSystem : MonoBehaviour
    {
        public TextMeshProUGUI characterNameText;
        public TextMeshProUGUI dialogueText;
        public DialogueManager dialogueManager;
        public string initialInvariant = "Tut1"; 
        public string SceneName;
        public GameObject DecisionCard;

        private string currentInvariant;
        private int currentDialogueIndex;
        private List<Dialogue> currentDialogues;

        void Start()
        {
            DisplayDialogueByInvariant(initialInvariant);
        }
         public string GetInitialInvariant()
        {
            return initialInvariant;
        }
        public void SetInitialInvariant(string newInvariant)
        {
            initialInvariant = newInvariant;
            Debug.Log("Initial Invariant has been set to: " + initialInvariant);
            DisplayDialogueByInvariant(initialInvariant);
        }

        public void DisplayDialogueByInvariant(string invariant)
        {
            if (dialogueManager.dialogues.ContainsKey(invariant))
            {
                currentDialogues = dialogueManager.dialogues[invariant];
                currentDialogueIndex = 0;
                currentInvariant = invariant;
                DisplayCurrentDialogue();
            }
            else
            {
                Debug.LogWarning("Dialogue with invariant " + invariant + " not found.");
            }
        }

        void DisplayCurrentDialogue()
        {
            if (currentDialogueIndex < currentDialogues.Count)
            {
                Dialogue dialogue = currentDialogues[currentDialogueIndex];
                characterNameText.text = dialogue.Character;
                dialogueText.text = dialogue.EN;
            }
            else
            {
                Debug.LogWarning("No more dialogues under invariant " + currentInvariant);
            }
        }

        public void OnNextButton()
        {
            if (currentDialogues != null && currentDialogueIndex < currentDialogues.Count - 1)
            {
                currentDialogueIndex++;
                DisplayCurrentDialogue();
            }
            else
            {
                Debug.LogWarning("Reached the end of dialogues for invariant " + currentInvariant);
                SceneManager.LoadScene("Menu");
            }
        }
        public void OnNextAndContinueButton()
        {
            if (currentDialogues != null && currentDialogueIndex < currentDialogues.Count - 1)
            {
                currentDialogueIndex++;
                DisplayCurrentDialogue();
            }
            else
            {
                Debug.LogWarning("Reached the end of dialogues for invariant " + currentInvariant);
                DecisionCard.SetActive(true);
            }
        }
    }
}
