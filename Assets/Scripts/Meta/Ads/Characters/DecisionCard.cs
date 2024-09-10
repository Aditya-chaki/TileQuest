using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class DecisionCard : MonoBehaviour
{
    [SerializeField] private Button OptionA_Loss;
    [SerializeField] private Button OptionB_Gain;
    [SerializeField] private Button Okay;
    [SerializeField] private GameObject Reward;
  
    [SerializeField] private TextMeshProUGUI OpinionChangeText; 
    [SerializeField] private int OpinionVariable;
    [SerializeField] private int FoodVariable;
    [SerializeField] private int StrengthVariable;
    [SerializeField] private int HealthVariable;
    [SerializeField] private int GoldVariable;

    


    public Character CharacterID;

    void Start()
    {
        if (OptionA_Loss != null)
        {
            OptionA_Loss.onClick.AddListener(LoseOpinion);
        }
        if (OptionB_Gain != null)
        {
            OptionB_Gain.onClick.AddListener(GainOpinion);
        }
        if (Okay != null)
        {
            Okay.onClick.AddListener(CloseScene);
        }
    }

public void LoseOpinion()
{
    int opinionValue = CharacterID.GetOpinion();
    CharacterID.SetOpinion(opinionValue - OpinionVariable);

    

    // Update other variables
    Config.Food = Config.Food - FoodVariable;
    Config.Strength = Config.Strength - StrengthVariable;
    Config.Health = Config.Health - HealthVariable;
    Config.Gold = Config.Gold - GoldVariable;

    Reward.SetActive(true);
    OpinionChangeText.text = "Opinion Dropped by " + OpinionVariable;
}

public void GainOpinion()
{
    int opinionValue = CharacterID.GetOpinion();
    CharacterID.SetOpinion(opinionValue + OpinionVariable);

   
    // Update other variables
    Config.Food = Config.Food + FoodVariable;
    Config.Strength = Config.Strength + StrengthVariable;
    Config.Health = Config.Health + HealthVariable;
    Config.Gold = Config.Gold + GoldVariable;

    Reward.SetActive(true);
    OpinionChangeText.text = "Opinion Increased by " + OpinionVariable;
}

    public void CloseScene()
    {
        SceneManager.LoadScene("Menu");
    }
}
