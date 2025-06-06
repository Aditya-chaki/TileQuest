// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.SceneManagement;
// using UnityEngine.UI;
// using TMPro;
// using VNGame;

// public class DecisionCard : MonoBehaviour
// {
//     [SerializeField] private Button optionALoss;
//     [SerializeField] private Button optionBGain;

//     [SerializeField] private GameObject card;


//     [SerializeField] private int foodVariable;
//     [SerializeField] private int strengthVariable;
//     [SerializeField] private int healthVariable;
//     [SerializeField] private int goldVariable;

//     [SerializeField] private int energyVariable;

//     void Start()
//     {
//         optionALoss?.onClick.AddListener(() => UpdateOpinion(-1));
//         optionBGain?.onClick.AddListener(() => UpdateOpinion(1));
//     }

//     private void UpdateOpinion(int changeDirection)
//     {
//         // Update other variables
//         Config.Food += foodVariable * changeDirection;
//         Config.Strength += strengthVariable * changeDirection;
//         Config.Health += healthVariable * changeDirection;
//         Config.Gold += goldVariable * changeDirection;
//         Config.Energy += energyVariable * changeDirection;

     

//         // Destroy the decision card after 5 seconds
//         Destroy(gameObject, 2f);

//         // Update the text display
//         string changeType = changeDirection > 0 ? "Increased" : "Dropped";
//     }
// }


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VNGame;

public class DecisionCard : MonoBehaviour
{
    [SerializeField] private Button optionAButton;
    [SerializeField] private Button optionBButton;

    // Option A Effects
    [Header("Option A Effects")]
    [SerializeField] private int foodA;
    [SerializeField] private int goldA;
    [SerializeField] private int healthA;
    [SerializeField] private int energyA;
    [SerializeField] private int opinionA;

    // Option B Effects
    [Header("Option B Effects")]
    [SerializeField] private int foodB;
    [SerializeField] private int goldB;
    [SerializeField] private int healthB;
    [SerializeField] private int energyB;
    [SerializeField] private int opinionB;

    // [Header("Character ID")]
    // [SerializeField] private string characterId = "queen"; // default, can change per card
    [Header("Faction Opinion Effects")]
    [SerializeField] private List<FactionOpinionEffect> factionOpinionEffectsA;
    [SerializeField] private List<FactionOpinionEffect> factionOpinionEffectsB;


    void Start()
    {
        optionAButton?.onClick.AddListener(ApplyOptionA);
        optionBButton?.onClick.AddListener(ApplyOptionB);
    }

    [System.Serializable]
    public class FactionOpinionEffect
    {
        public string factionId; // "nobles", "peasants", etc.
        public int opinionChange;
    }


private void ApplyOptionA()
{
    ApplyChanges(foodA, goldA, healthA, energyA, factionOpinionEffectsA);
}

private void ApplyOptionB()
{
    ApplyChanges(foodB, goldB, healthB, energyB, factionOpinionEffectsB);
}


private void ApplyChanges(
    int food, int gold, int health, int energy,
    List<FactionOpinionEffect> opinionEffects
)
{
    Config.Food += food;
    Config.Gold += gold;
    Config.Health += health;
    Config.Energy += energy;

    foreach (var effect in opinionEffects)
    {
        int current = Config.GetFactionOpinion(effect.factionId);
        Config.SetFactionOpinion(effect.factionId, current + effect.opinionChange);
    }
    DailyQuest.UpdateDecisionsMade();
    Destroy(gameObject, 2f);
}

}


