// using System.Collections.Generic;
// using UnityEngine;
// using TMPro;
// using UnityEngine.SceneManagement;

// namespace VNGame
// {
//     public class DialogueSystem : MonoBehaviour
//     {
//         public TextMeshProUGUI characterNameText;
//         public TextMeshProUGUI dialogueText;
//         public DialogueManager dialogueManager;
//         public string initialInvariant = "Tut1"; 
//         public string SceneName;
//         public GameObject DialoguePrefab;
//         public GameObject DecisionCard;

//         private string currentInvariant;
//         private int currentDialogueIndex;
//         private List<Dialogue> currentDialogues;

//         void Start()
//         {
//             DisplayDialogueByInvariant(initialInvariant);
//         }
//          public string GetInitialInvariant()
//         {
//             return initialInvariant;
//         }
//         public void SetInitialInvariant(string newInvariant)
//         {
//             initialInvariant = newInvariant;
//             Debug.Log("Initial Invariant has been set to: " + initialInvariant);
//             DisplayDialogueByInvariant(initialInvariant);
//         }

//         public void DisplayDialogueByInvariant(string invariant)
//         {
//             if (dialogueManager.dialogues.ContainsKey(invariant))
//             {
//                 currentDialogues = dialogueManager.dialogues[invariant];
//                 currentDialogueIndex = 0;
//                 currentInvariant = invariant;
//                 DisplayCurrentDialogue();
//             }
//             else
//             {
//                 Debug.LogWarning("Dialogue with invariant " + invariant + " not found.");
//             }
//         }

//         void DisplayCurrentDialogue()
//         {
//             if (currentDialogueIndex < currentDialogues.Count)
//             {
//                 Dialogue dialogue = currentDialogues[currentDialogueIndex];
//                 characterNameText.text = dialogue.Character;
//                 dialogueText.text = dialogue.EN;
//             }
//             else
//             {
//                 Debug.LogWarning("No more dialogues under invariant " + currentInvariant);
//             }
//         }

//         public void OnNextButton()
//         {
//             if (currentDialogues != null && currentDialogueIndex < currentDialogues.Count - 1)
//             {
//                 currentDialogueIndex++;
//                 DisplayCurrentDialogue();
//             }
//             else
//             {
//                 Debug.LogWarning("Reached the end of dialogues for invariant " + currentInvariant);
//                 DecisionCard.SetActive(true);
//             }
//         }
//         // public void OnNextAndContinueButton()
//         // {
//         //     if (currentDialogues != null && currentDialogueIndex < currentDialogues.Count - 1)
//         //     {
//         //         currentDialogueIndex++;
//         //         DisplayCurrentDialogue();
//         //     }
//         //     else
//         //     {
//         //         Debug.LogWarning("Reached the end of dialogues for invariant " + currentInvariant);
//         //         DecisionCard.SetActive(true);
//         //     }
//         // }
//     }
// }


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace VNGame
{
    public class DialogueSystem : MonoBehaviour
    {
        public TextMeshProUGUI characterNameText;
        public TextMeshProUGUI dialogueText;
        public TMP_InputField nameInputField;
        public GameObject dialoguePanel;
        public GameObject decisionCardParent;
        public DialogueManager dialogueManager;
        public string initialInvariant = "Tut1";

        private string currentInvariant;
        private int currentDialogueIndex;
        private List<Dialogue> currentDialogues;
        private bool isTyping = false;

        // 🟩 Store already-used decision cards
        private HashSet<string> shownCards = new HashSet<string>();

        void Start()
        {
            if (characterNameText == null) Debug.LogError("Missing: characterNameText");
            if (dialogueText == null) Debug.LogError("Missing: dialogueText");
            if (nameInputField == null) Debug.LogError("Missing: nameInputField");
            if (dialoguePanel == null) Debug.LogError("Missing: dialoguePanel");
            if (decisionCardParent == null) Debug.LogError("Missing: decisionCardParent");
            if (dialogueManager == null) Debug.LogError("Missing: dialogueManager");

            DisplayDialogueByInvariant(initialInvariant);
        }

        public void DisplayDialogueByInvariant(string invariant)
        {
            if (dialogueManager == null || dialogueManager.dialogues == null)
            {
                Debug.LogError("DialogueManager or dialogues is null.");
                return;
            }

            if (dialogueManager.dialogues.ContainsKey(invariant))
            {
                currentDialogues = dialogueManager.dialogues[invariant];
                currentDialogueIndex = 0;
                currentInvariant = invariant;
                dialoguePanel.SetActive(true);
                ShowDialogueLine();
            }
            else
            {
                Debug.LogError($"Dialogue block '{invariant}' not found in DialogueManager.");
            }
        }

        void Update()
        {
            if (!isTyping && dialoguePanel.activeSelf && Input.GetMouseButtonDown(0))
            {
                OnNextDialogue();
            }
        }

        void ShowDialogueLine()
        {
            if (currentDialogueIndex >= currentDialogues.Count)
            {
                dialoguePanel.SetActive(false);
                return;
            }

            Dialogue dialogue = currentDialogues[currentDialogueIndex];
            string speaker = dialogue.Character;
            characterNameText.text = speaker;

            if (speaker == "System")
            {
                nameInputField.gameObject.SetActive(true);
                nameInputField.onEndEdit.RemoveAllListeners();
                nameInputField.onEndEdit.AddListener(HandleNameInput);
                nameInputField.Select();
                nameInputField.ActivateInputField();
            }
            else if (speaker == "DecisionCard")
            {
                dialoguePanel.SetActive(false);
                // 🔄 Automatically use invariant to match card (e.g., Scene1Card for Tut1)
                string cardName = currentInvariant.Replace("Tut", "Scene") + "Card";
                SpawnDecisionCardFromHierarchy(cardName);
            }
            else
            {
                StartCoroutine(TypeText(dialogue.EN));
            }
        }

        IEnumerator TypeText(string line)
        {
            isTyping = true;
            dialogueText.text = "";
            foreach (char c in line)
            {
                dialogueText.text += c;
                yield return new WaitForSeconds(0.02f);
            }
            isTyping = false;
        }

        void OnNextDialogue()
        {
            currentDialogueIndex++;
            ShowDialogueLine();
        }

        void HandleNameInput(string value)
        {
            PlayerPrefs.SetString("PlayerName", value);
            nameInputField.text = "";
            nameInputField.gameObject.SetActive(false);
            nameInputField.onEndEdit.RemoveListener(HandleNameInput);
            OnNextDialogue();
        }

        void SpawnDecisionCardFromHierarchy(string cardName)
        {
            if (shownCards.Contains(cardName))
            {
                Debug.Log("Decision card already shown: " + cardName);
                return;
            }

            GameObject cardObject = GameObject.Find(cardName);
            if (cardObject != null)
            {
                shownCards.Add(cardName);
                cardObject.SetActive(true);
                var decisionCard = cardObject.GetComponent<DecisionCard>();
                if (decisionCard != null)
                {
                    decisionCard.OnDecisionMade += HandleDecisionMade;
                }
                else
                {
                    Debug.LogWarning("No DecisionCard script found on: " + cardName);
                }
            }
            else
            {
                Debug.LogError("Decision card not found in hierarchy: " + cardName);
            }
        }

        void HandleDecisionMade(string nextInvariant)
        {
            // 🔥 Destroy active cards under parent
            foreach (Transform child in decisionCardParent.transform)
            {
                if (child != null)
                    Destroy(child.gameObject);
            }

            DisplayDialogueByInvariant(nextInvariant);
        }
    }
}

