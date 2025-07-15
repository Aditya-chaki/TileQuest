using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VNGame;

public class DecisionCard : MonoBehaviour
{
    [SerializeField] private Button optionAButton;
    [SerializeField] private Button optionBButton;

    [Header("Option A Effects")]
    [SerializeField] private int influenceA;
    [SerializeField] private int goldA;
    [SerializeField] private int magicA;
    [SerializeField] private List<FactionOpinionEffect> factionOpinionEffectsA;
    [SerializeField] private string nextInvariantA = "Tut2";

    [Header("Option B Effects")]
    [SerializeField] private int influenceB;
    [SerializeField] private int goldB;
    [SerializeField] private int magicB;
    [SerializeField] private List<FactionOpinionEffect> factionOpinionEffectsB;
    [SerializeField] private string nextInvariantB = "Tut2";

    public Action<string> OnDecisionMade;

    [System.Serializable]
    public class FactionOpinionEffect
    {
        public string factionId;
        public int opinionChange;
    }

    private void Start()
    {
        optionAButton?.onClick.AddListener(() => ApplyOption(influenceA, goldA, magicA, factionOpinionEffectsA, nextInvariantA));
        optionBButton?.onClick.AddListener(() => ApplyOption(influenceB, goldB, magicB, factionOpinionEffectsB, nextInvariantB));
    }

    private void ApplyOption(int influence, int gold, int magic, List<FactionOpinionEffect> opinionEffects, string nextInvariant)
    {
        Config.Influence += influence;
        Config.Gold += gold;
        Config.Magic += magic;

        foreach (var effect in opinionEffects)
        {
            int current = Config.GetFactionOpinion(effect.factionId);
            Config.SetFactionOpinion(effect.factionId, current + effect.opinionChange);
        }

        DailyQuest.UpdateDecisionsMade(); // Optional hook

        OnDecisionMade?.Invoke(nextInvariant);
        Destroy(gameObject, 0.5f);
    }
}