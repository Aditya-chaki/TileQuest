
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using TMPro;
// using UnityEngine.UI;
// using System.Text.RegularExpressions;

// namespace VNGame
// {
//     public class DialogueSystem : MonoBehaviour
//     {
//         public TextMeshProUGUI characterNameText;
//         public TextMeshProUGUI dialogueText;
//         public TMP_InputField nameInputField;
//         public GameObject dialoguePanel;
//         public GameObject decisionCardTemplate;
//         public TMP_Text questionText;
//         public TMP_Text optionAButtonText;
//         public TMP_Text optionBButtonText;
//         public Button optionAButton;
//         public Button optionBButton;
//         public DialogueManager dialogueManager;
//         public string initialInvariant = "tut1";

//         private string currentInvariant;
//         private int currentDialogueIndex;
//         private List<Dialogue> currentDialogues;
//         private Dialogue currentDecision;
//         private bool isTyping = false;

//         void Start()
//         {
//             // Load last played invariant if exists, else start fresh
//             string saved = PlayerPrefs.GetString("LastInvariant", initialInvariant);
//             DisplayDialogueByInvariant(saved);
//         }

//         public void DisplayDialogueByInvariant(string invariant)
//         {
//             if (dialoguePanel == null) return;  // panel destroyed? bail out

//             if (!dialogueManager.dialogues.ContainsKey(invariant))
//             {
//                 Debug.LogWarning($"Dialogue block '{invariant}' not found. End of story.");
//                 dialoguePanel.SetActive(false);
//                 return;
//             }

//             currentInvariant = invariant;
//             PlayerPrefs.SetString("LastInvariant", currentInvariant);  // Save progress

//             currentDialogues = dialogueManager.dialogues[invariant];
//             currentDialogueIndex = 0;
//             dialoguePanel.SetActive(true);
//             decisionCardTemplate.SetActive(false);
//             ShowDialogueLine();
//         }

//         void Update()
//         {
//             if (!isTyping && dialoguePanel.activeSelf && Input.GetMouseButtonDown(0))
//             {
//                 OnNextDialogue();
//             }
//         }

//         void ShowDialogueLine()
//         {
//             if (currentDialogueIndex >= currentDialogues.Count)
//             {
//                 string next = GetNextTutInvariant(currentInvariant);
//                 if (dialogueManager.dialogues.ContainsKey(next))
//                 {
//                     DisplayDialogueByInvariant(next);
//                 }
//                 else
//                 {
//                     Debug.Log("No further dialogue. End of current path.");
//                     dialoguePanel.SetActive(false);
//                 }
//                 return;
//             }

//             Dialogue dialogue = currentDialogues[currentDialogueIndex];
//             characterNameText.text = dialogue.Character;

//             if (dialogue.Character == "System")
//             {
//                 nameInputField.gameObject.SetActive(true);
//                 nameInputField.onEndEdit.RemoveAllListeners();
//                 nameInputField.onEndEdit.AddListener(HandleNameInput);
//                 nameInputField.Select();
//                 nameInputField.ActivateInputField();
//             }
//             else if (dialogue.Character == "DecisionCard")
//             {
//                 ShowDecisionCard(dialogue);
//             }
//             else
//             {
//                 StartCoroutine(TypeText(dialogue.EN));
//             }
//         }

//         IEnumerator TypeText(string line)
//         {
//             isTyping = true;
//             dialogueText.text = "";
//             foreach (char c in line)
//             {
//                 dialogueText.text += c;
//                 yield return new WaitForSeconds(0.02f);
//             }
//             isTyping = false;
//         }

//         void OnNextDialogue()
//         {
//             currentDialogueIndex++;
//             ShowDialogueLine();
//         }

//         void HandleNameInput(string value)
//         {
//             PlayerPrefs.SetString("PlayerName", value);
//             nameInputField.text = "";
//             nameInputField.gameObject.SetActive(false);
//             nameInputField.onEndEdit.RemoveListener(HandleNameInput);
//             OnNextDialogue();
//         }

//         void ShowDecisionCard(Dialogue decision)
//         {
//             dialoguePanel.SetActive(false);
//             decisionCardTemplate.SetActive(true);
//             currentDecision = decision;

//             questionText.text = decision.EN;
//             optionAButtonText.text = decision.OptionA;
//             optionBButtonText.text = decision.OptionB;

//             optionAButton.onClick.RemoveAllListeners();
//             optionBButton.onClick.RemoveAllListeners();

//             optionAButton.onClick.AddListener(() => ApplyDecision(true));
//             optionBButton.onClick.AddListener(() => ApplyDecision(false));
//         }

//         void ApplyDecision(bool choseA)
//         {
//             int foodChange = choseA ? currentDecision.FoodA : currentDecision.FoodB;
//             int goldChange = choseA ? currentDecision.GoldA : currentDecision.GoldB;
//             int opinionChange = choseA ? currentDecision.OpinionA : currentDecision.OpinionB;

//             Config.Food += foodChange;
//             Config.Gold += goldChange;
//             // Add more stat changes if needed

//             string nextBranch = choseA ? currentDecision.NextA : currentDecision.NextB;

//             decisionCardTemplate.SetActive(false);
//             DisplayDialogueByInvariant(nextBranch);
//         }

//         string GetNextTutInvariant(string current)
//         {
//             Match match = Regex.Match(current, @"tut(\d+)");
//             if (match.Success)
//             {
//                 int num = int.Parse(match.Groups[1].Value);
//                 return "tut" + (num + 1);
//             }
//             return current;
//         }
//     }
// }


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Text.RegularExpressions;

namespace VNGame
{
    public class DialogueSystem : MonoBehaviour
    {
        public TextMeshProUGUI characterNameText;
        public TextMeshProUGUI dialogueText;
        public TMP_InputField nameInputField;
        public GameObject dialoguePanel;

        [Header("Character Visuals")]
        public Transform leftCharacterSlot;
        public GameObject currentLeftCharacter;

        [Header("Decision Card Template")]
        public GameObject decisionCardTemplate;
        public TMP_Text questionText;
        public TMP_Text optionAButtonText;
        public TMP_Text optionBButtonText;
        public Button optionAButton;
        public Button optionBButton;
        public Image middlePortrait;  // For middle character portrait on card

        [Header("Data")]
        public DialogueManager dialogueManager;
        public string initialInvariant = "tut1";

        private string currentInvariant;
        private int currentDialogueIndex;
        private List<Dialogue> currentDialogues;
        private Dialogue currentDecision;
        private bool isTyping = false;

        void Start()
        {
            string saved = PlayerPrefs.GetString("LastInvariant", initialInvariant);
            DisplayDialogueByInvariant(saved);
        }

        public void DisplayDialogueByInvariant(string invariant)
        {
            if (!dialogueManager.dialogues.ContainsKey(invariant))
            {
                Debug.LogWarning($"Dialogue block '{invariant}' not found. End of story.");
                dialoguePanel.SetActive(false);
                return;
            }

            currentInvariant = invariant;
            PlayerPrefs.SetString("LastInvariant", currentInvariant);

            currentDialogues = dialogueManager.dialogues[invariant];
            currentDialogueIndex = 0;
            dialoguePanel.SetActive(true);
            decisionCardTemplate.SetActive(false);
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
                string next = GetNextTutInvariant(currentInvariant);
                if (dialogueManager.dialogues.ContainsKey(next))
                {
                    DisplayDialogueByInvariant(next);
                }
                else
                {
                    dialoguePanel.SetActive(false);
                }
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
                LoadLeftCharacter(dialogue.Character);
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

            // 🧠 Load portrait if specified
            if (!string.IsNullOrEmpty(decision.PortraitName))
            {
                Sprite portrait = Resources.Load<Sprite>("Portraits/" + decision.PortraitName);
                if (portrait != null)
                {
                    middlePortrait.sprite = portrait;
                    middlePortrait.enabled = true;
                }
                else
                {
                    Debug.LogWarning("Portrait not found: " + decision.PortraitName);
                    middlePortrait.enabled = false;
                }
            }
            else
            {
                middlePortrait.enabled = false;
            }
        }

        void ApplyDecision(bool choseA)
        {
            int foodChange = choseA ? currentDecision.FoodA : currentDecision.FoodB;
            int goldChange = choseA ? currentDecision.GoldA : currentDecision.GoldB;
            int opinionChange = choseA ? currentDecision.OpinionA : currentDecision.OpinionB;

            Config.Food += foodChange;
            Config.Gold += goldChange;

            string nextBranch = choseA ? currentDecision.NextA : currentDecision.NextB;

            decisionCardTemplate.SetActive(false);
            DisplayDialogueByInvariant(nextBranch);
        }

        void LoadLeftCharacter(string characterName)
        {
            if (currentLeftCharacter != null)
            {
                Destroy(currentLeftCharacter);
            }

            GameObject prefab = Resources.Load<GameObject>("Characters/" + characterName);
            if (prefab != null)
            {
                currentLeftCharacter = Instantiate(prefab, leftCharacterSlot);
                currentLeftCharacter.transform.localPosition = Vector3.zero;
            }
            else
            {
                Debug.LogWarning("Character prefab not found: " + characterName);
            }
        }

        string GetNextTutInvariant(string current)
        {
            Match match = Regex.Match(current, @"tut(\d+)");
            if (match.Success)
            {
                int num = int.Parse(match.Groups[1].Value);
                return "tut" + (num + 1);
            }
            return current;
        }
    }
}
