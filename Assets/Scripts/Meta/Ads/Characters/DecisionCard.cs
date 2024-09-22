using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using  VNGame;


public class DecisionCard : MonoBehaviour
{
    [SerializeField] private Button optionALoss;
    [SerializeField] private Button optionBGain;

    [SerializeField] private GameObject card;

    [SerializeField] private DialogueSystem dialogueSystem;

    [SerializeField] private TextMeshProUGUI opinionChangeText;
    [SerializeField] private int opinionVariable;
    [SerializeField] private int foodVariable;
    [SerializeField] private int strengthVariable;
    [SerializeField] private int healthVariable;
    [SerializeField] private int goldVariable;
    [SerializeField] private string TutNumber;


    public Character characterID;

    void Start()
    {
        optionALoss?.onClick.AddListener(() => UpdateOpinion(-1));
        optionBGain?.onClick.AddListener(() => UpdateOpinion(1));

    }

    private void UpdateOpinion(int changeDirection)
    {
        int opinionValue = characterID.GetOpinion();
        characterID.SetOpinion(opinionValue + (opinionVariable * changeDirection));

        // Update other variables
        Config.Food += foodVariable * changeDirection;
        Config.Strength += strengthVariable * changeDirection;
        Config.Health += healthVariable * changeDirection;
        Config.Gold += goldVariable * changeDirection;

        card.SetActive(false);

        // Update the text display
        string changeType = changeDirection > 0 ? "Increased" : "Dropped";
        opinionChangeText.text = $"Opinion {changeType} by {opinionVariable}";

        StartCoroutine(HideOpinionTextAfterDelay(3f));
         if (dialogueSystem != null)
        {
            dialogueSystem.SetInitialInvariant(TutNumber); // Call the setter to change the invariant
        }
    }

    private IEnumerator HideOpinionTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        opinionChangeText.text = ""; // Clear the text after delay
    }
}
