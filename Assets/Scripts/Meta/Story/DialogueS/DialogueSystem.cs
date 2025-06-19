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
using UnityEngine.UI;

namespace VNGame
{
    public class DialogueSystem : MonoBehaviour
    {
        public TextMeshProUGUI characterNameText;
        public TextMeshProUGUI dialogueText;
        public TMP_InputField nameInputField;
        public GameObject dialoguePanel;
        public GameObject decisionCardTemplate; // assign the disabled prefab in scene
        public TMP_Text questionText;
        public TMP_Text optionAButtonText;
        public TMP_Text optionBButtonText;
        public Button optionAButton;
        public Button optionBButton;
        public DialogueManager dialogueManager;
        public string initialInvariant = "Tut1";

        private string currentInvariant;
        private int currentDialogueIndex;
        private List<Dialogue> currentDialogues;
        private Dialogue currentDecision;
        private bool isTyping = false;

        void Start()
        {
            DisplayDialogueByInvariant(initialInvariant);
        }

        public void DisplayDialogueByInvariant(string invariant)
        {
            if (!dialogueManager.dialogues.ContainsKey(invariant))
            {
                Debug.LogError($"Dialogue block '{invariant}' not found.");
                return;
            }

            currentInvariant = invariant;
            currentDialogues = dialogueManager.dialogues[invariant];
            currentDialogueIndex = 0;
            dialoguePanel.SetActive(true);
            ShowDialogueLine();
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
            characterNameText.text = dialogue.Character;

            if (dialogue.Character == "System")
            {
                nameInputField.gameObject.SetActive(true);
                nameInputField.onEndEdit.RemoveAllListeners();
                nameInputField.onEndEdit.AddListener(HandleNameInput);
                nameInputField.Select();
                nameInputField.ActivateInputField();
            }
            else if (dialogue.Character == "DecisionCard")
            {
                ShowDecisionCard(dialogue);
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

        void ShowDecisionCard(Dialogue decision)
        {
            dialoguePanel.SetActive(false);
            decisionCardTemplate.SetActive(true);
            currentDecision = decision;

            questionText.text = decision.EN;
            optionAButtonText.text = decision.OptionA;
            optionBButtonText.text = decision.OptionB;

            optionAButton.onClick.RemoveAllListeners();
            optionBButton.onClick.RemoveAllListeners();

            optionAButton.onClick.AddListener(() => ApplyDecision(true));
            optionBButton.onClick.AddListener(() => ApplyDecision(false));
        }

        void ApplyDecision(bool choseA)
        {
            int foodChange = choseA ? currentDecision.FoodA : currentDecision.FoodB;
            int goldChange = choseA ? currentDecision.GoldA : currentDecision.GoldB;

            Config.Food += foodChange;
            Config.Gold += goldChange;
            // Add health/energy etc. here

            string nextInvariant = choseA ? currentDecision.NextA : currentDecision.NextB;

            decisionCardTemplate.SetActive(false);
            DisplayDialogueByInvariant(nextInvariant);
        }
    }
}
