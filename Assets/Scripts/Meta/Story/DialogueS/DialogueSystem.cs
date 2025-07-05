// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using TMPro;
// using UnityEngine.UI;

// namespace VNGame
// {
//     public class DialogueSystem : MonoBehaviour
//     {
//         public TextMeshProUGUI characterNameText;
//         public TextMeshProUGUI dialogueText;
//         public TMP_InputField nameInputField;
//         public GameObject dialoguePanel;

//         [Header("Character Visuals")]
//         public Transform leftCharacterSlot;
//         public Transform rightCharacterSlot;
//         public GameObject fixedPlayerCharacter;
//         private GameObject currentRightCharacter;

//         [Header("Decision Card Template")]
//         public GameObject decisionCardTemplate;
//         public TMP_Text questionText;
//         public TMP_Text optionAButtonText;
//         public TMP_Text optionBButtonText;
//         public Button optionAButton;
//         public Button optionBButton;
//         public Image middlePortrait;

//         [Header("Data")]
//         public DialogueManager dialogueManager;
//         public string initialInvariant = "tut1";

//         private string currentInvariant;
//         private int currentDialogueIndex;
//         private List<Dialogue> currentDialogues;
//         private Dialogue currentDecision;
//         private bool isTyping = false;

//         void Start()
//         {
//             StartCoroutine(DelayedStart());
//         }

//         IEnumerator DelayedStart()
//         {
//             yield return new WaitForSeconds(0.1f);

//             string saved = PlayerPrefs.GetString("LastInvariant", initialInvariant);

//             if (!dialogueManager.dialogues.ContainsKey(saved))
//             {
//                 saved = initialInvariant;
//             }

//             DisplayDialogueByInvariant(saved);
//         }

//         public void DisplayDialogueByInvariant(string invariant)
//         {
//             if (!dialogueManager.dialogues.ContainsKey(invariant))
//             {
//                 invariant = initialInvariant;
//             }

//             currentInvariant = invariant;
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
//                 dialoguePanel.SetActive(false);
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
//                 UpdateRightCharacter(dialogue.Character);
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
//             if (currentDialogueIndex < currentDialogues.Count)
//             {
//                 Dialogue currentDialogue = currentDialogues[currentDialogueIndex];

//                 if (currentDialogue.Character != "DecisionCard" && !string.IsNullOrEmpty(currentDialogue.NextInvariant))
//                 {
//                     PlayerPrefs.SetString("LastInvariant", currentDialogue.NextInvariant);
//                     PlayerPrefs.Save();

//                     dialoguePanel.SetActive(false);
//                     decisionCardTemplate.SetActive(false);
//                     return;
//                 }
//             }

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

//             if (!string.IsNullOrEmpty(decision.PortraitName))
//             {
//                 Sprite portrait = Resources.Load<Sprite>("Portraits/" + decision.PortraitName);
//                 if (portrait != null)
//                 {
//                     middlePortrait.sprite = portrait;
//                     middlePortrait.enabled = true;
//                 }
//                 else
//                 {
//                     middlePortrait.enabled = false;
//                 }
//             }
//             else
//             {
//                 middlePortrait.enabled = false;
//             }
//         }

//         void ApplyDecision(bool choseA)
//         {
//             int goldChange = choseA ? currentDecision.GoldA : currentDecision.GoldB;
//             int opinionChange = choseA ? currentDecision.OpinionA : currentDecision.OpinionB;

//             Config.Gold += goldChange;

//             string nextBranch = choseA ? currentDecision.NextA : currentDecision.NextB;

//             decisionCardTemplate.SetActive(false);

//             if (!string.IsNullOrEmpty(nextBranch) && dialogueManager.dialogues.ContainsKey(nextBranch))
//             {
//                 DisplayDialogueByInvariant(nextBranch);
//             }
//             else
//             {
//                 dialoguePanel.SetActive(false);
//             }
//         }

//         void UpdateRightCharacter(string characterName)
//         {
//             if (currentRightCharacter != null)
//             {
//                 Destroy(currentRightCharacter);
//             }

//             GameObject prefab = Resources.Load<GameObject>("Characters/" + characterName);
//             if (prefab != null)
//             {
//                 currentRightCharacter = Instantiate(prefab, rightCharacterSlot);
//                 currentRightCharacter.transform.localPosition = Vector3.zero;
//             }
//         }

//         public void StartDialogue()
//         {
//             string saved = PlayerPrefs.GetString("LastInvariant", initialInvariant);
//             DisplayDialogueByInvariant(saved);
//         }

//         public void ResetProgress()
//         {
//             PlayerPrefs.DeleteKey("LastInvariant");
//             PlayerPrefs.Save();
//         }
//     }
// }
/////////////////full story mode working just sprites and all are not working in the above script////////////////////

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

        [Header("Character Visuals")]
        public Transform leftCharacterSlot;
        public Transform rightCharacterSlot;
        public GameObject fixedPlayerCharacter;
        
        [Header("Character Sprite Management")]
        public Image characterImageComponent; // Drag the Manager's Image component here
        public Sprite defaultSprite; // Optional: default sprite when no character
        
        private GameObject currentRightCharacter;

        [Header("Decision Card Template")]
        public GameObject decisionCardTemplate;
        public TMP_Text questionText;
        public TMP_Text optionAButtonText;
        public TMP_Text optionBButtonText;
        public Button optionAButton;
        public Button optionBButton;
        public Image middlePortrait; // This should be the Image component from tut1Card -> Image

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
            StartCoroutine(DelayedStart());
        }

        IEnumerator DelayedStart()
        {
            yield return new WaitForSeconds(0.1f);
            
            string saved = PlayerPrefs.GetString("LastInvariant", initialInvariant);
            
            if (!dialogueManager.dialogues.ContainsKey(saved))
            {
                saved = initialInvariant;
            }
            
            DisplayDialogueByInvariant(saved);
        }

        public void DisplayDialogueByInvariant(string invariant)
        {
            if (!dialogueManager.dialogues.ContainsKey(invariant))
            {
                invariant = initialInvariant;
            }

            currentInvariant = invariant;
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
                // Update the character sprite based on who's speaking
                UpdateCharacterSprite(dialogue.Character);
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
            if (currentDialogueIndex < currentDialogues.Count)
            {
                Dialogue currentDialogue = currentDialogues[currentDialogueIndex];
                
                if (currentDialogue.Character != "DecisionCard" && !string.IsNullOrEmpty(currentDialogue.NextInvariant))
                {
                    PlayerPrefs.SetString("LastInvariant", currentDialogue.NextInvariant);
                    PlayerPrefs.Save();
                    
                    dialoguePanel.SetActive(false);
                    decisionCardTemplate.SetActive(false);
                    return;
                }
            }
            
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
            Debug.Log($"=== SHOWING DECISION CARD ===");
            Debug.Log($"Question: {decision.EN}");
            Debug.Log($"Option A: {decision.OptionA} -> {decision.NextA}");
            Debug.Log($"Option B: {decision.OptionB} -> {decision.NextB}");
            Debug.Log($"PortraitName: '{decision.PortraitName}'");
            
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

            // Handle portrait loading - THIS IS THE UPDATED PART
            UpdateDecisionCardPortrait(decision.PortraitName);
        }

        void UpdateDecisionCardPortrait(string portraitName)
        {
            Debug.Log($"=== UpdateDecisionCardPortrait called with: '{portraitName}' ===");

            // Check if the portrait Image component is assigned
            if (middlePortrait == null)
            {
                Debug.LogError("Middle Portrait Image Component is not assigned! Please drag the tut1Card -> Image component to the DialogueSystem script.");
                return;
            }

            // If no portrait name provided, hide the portrait or use default
            if (string.IsNullOrEmpty(portraitName))
            {
                Debug.Log("No portrait name provided, hiding portrait");
                middlePortrait.enabled = false;
                return;
            }

            // Show the portrait image
            middlePortrait.enabled = true;

            Debug.Log($"Trying to load portrait: Portraits/{portraitName}");

            // Try to load portrait sprite from Resources/Portraits folder
            Sprite portraitSprite = Resources.Load<Sprite>("Portraits/" + portraitName);
            
            if (portraitSprite != null)
            {
                Debug.Log($"SUCCESS: Found portrait sprite for '{portraitName}'");
                middlePortrait.sprite = portraitSprite;
            }
            else
            {
                Debug.LogWarning($"Portrait sprite not found: Resources/Portraits/{portraitName}");
                
                // List all available portrait sprites
                Sprite[] allPortraitSprites = Resources.LoadAll<Sprite>("Portraits");
                Debug.Log($"Available portrait sprites: {string.Join(", ", System.Array.ConvertAll(allPortraitSprites, x => x.name))}");
                
                // Try alternative naming conventions
                TryAlternativePortraitNames(portraitName);
            }
        }

        void TryAlternativePortraitNames(string originalName)
        {
            Debug.Log($"Trying alternative portrait names for: '{originalName}'");
            
            // Try removing special characters and spaces
            string cleanName = originalName.Replace(" ", "").Replace(",", "").Replace(".", "");
            Debug.Log($"Trying clean portrait name: '{cleanName}'");
            
            Sprite portraitSprite = Resources.Load<Sprite>("Portraits/" + cleanName);
            
            if (portraitSprite != null)
            {
                Debug.Log($"SUCCESS: Found portrait with clean name '{cleanName}'");
                middlePortrait.sprite = portraitSprite;
                return;
            }

            // Try lowercase version
            string lowerName = originalName.ToLower();
            Debug.Log($"Trying lowercase portrait name: '{lowerName}'");
            
            portraitSprite = Resources.Load<Sprite>("Portraits/" + lowerName);
            
            if (portraitSprite != null)
            {
                Debug.Log($"SUCCESS: Found portrait with lowercase name '{lowerName}'");
                middlePortrait.sprite = portraitSprite;
                return;
            }

            Debug.LogError($"Could not find portrait sprite for: '{originalName}' with any naming convention");
            
            // Keep the portrait enabled but maybe use a default "unknown" portrait
            // Or you could hide it: middlePortrait.enabled = false;
        }

        void ApplyDecision(bool choseA)
        {
            int goldChange = choseA ? currentDecision.GoldA : currentDecision.GoldB;
            int opinionChange = choseA ? currentDecision.OpinionA : currentDecision.OpinionB;

            Config.Gold += goldChange;

            string nextBranch = choseA ? currentDecision.NextA : currentDecision.NextB;

            decisionCardTemplate.SetActive(false);
            
            if (!string.IsNullOrEmpty(nextBranch) && dialogueManager.dialogues.ContainsKey(nextBranch))
            {
                DisplayDialogueByInvariant(nextBranch);
            }
            else
            {
                dialoguePanel.SetActive(false);
            }
        }

        void UpdateCharacterSprite(string characterName)
        {
            // Check if the Image component is assigned
            if (characterImageComponent == null)
            {
                Debug.LogError("Character Image Component is not assigned! Please drag the Manager's Image component to the DialogueSystem script.");
                return;
            }

            // Special cases where no character should appear (hide the sprite)
            if (string.IsNullOrEmpty(characterName) || 
                characterName == "You" || 
                characterName == "Player's Thoughts" || 
                characterName == "System")
            {
                // Hide the character or use default sprite
                if (defaultSprite != null)
                {
                    characterImageComponent.sprite = defaultSprite;
                }
                else
                {
                    characterImageComponent.enabled = false; // Hide completely
                }
                return;
            }

            // Show the image component
            characterImageComponent.enabled = true;

            // Try to load character sprite from Resources/Characters folder
            Sprite characterSprite = Resources.Load<Sprite>("Characters/" + characterName);
            
            if (characterSprite != null)
            {
                characterImageComponent.sprite = characterSprite;
            }
            else
            {
                // Try alternative naming conventions
                TryAlternativeCharacterSprites(characterName);
            }
        }

        void TryAlternativeCharacterSprites(string originalName)
        {
            // Try removing special characters and spaces
            string cleanName = originalName.Replace(" ", "").Replace(",", "").Replace(".", "");
            Sprite characterSprite = Resources.Load<Sprite>("Characters/" + cleanName);
            
            if (characterSprite != null)
            {
                characterImageComponent.sprite = characterSprite;
                return;
            }

            // Try lowercase version
            string lowerName = originalName.ToLower();
            characterSprite = Resources.Load<Sprite>("Characters/" + lowerName);
            
            if (characterSprite != null)
            {
                characterImageComponent.sprite = characterSprite;
                return;
            }

            // Fallback to default sprite if available
            if (defaultSprite != null)
            {
                characterImageComponent.sprite = defaultSprite;
            }
        }

        public void StartDialogue()
        {
            string saved = PlayerPrefs.GetString("LastInvariant", initialInvariant);
            DisplayDialogueByInvariant(saved);
        }

        public void ResetProgress()
        {
            PlayerPrefs.DeleteKey("LastInvariant");
            PlayerPrefs.Save();
        }
    }
}