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

    

    
    [SerializeField] private int foodVariable;
    [SerializeField] private int strengthVariable;
    [SerializeField] private int healthVariable;
    [SerializeField] private int goldVariable;
    


   

    void Start()
    {
        optionALoss?.onClick.AddListener(() => UpdateOpinion(-1));
        optionBGain?.onClick.AddListener(() => UpdateOpinion(1));

    }

    private void UpdateOpinion(int changeDirection)
    {
        

        // Update other variables
        Config.Food += foodVariable * changeDirection;
        Config.Strength += strengthVariable * changeDirection;
        Config.Health += healthVariable * changeDirection;
        Config.Gold += goldVariable * changeDirection;

        card.SetActive(false);

        // Update the text display
        string changeType = changeDirection > 0 ? "Increased" : "Dropped";
       

        
         
    }

    
}
