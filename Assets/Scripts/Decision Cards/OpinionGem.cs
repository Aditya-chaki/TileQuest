using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;


public class OpinionGem : MonoBehaviour
{
    public GameObject tooltipPanel;
    public TMP_Text tooltipText;

    public void ShowOpinionTooltip()
    {
        tooltipPanel.SetActive(true);
        float avg = GetAverageOpinion();
        tooltipText.text = $"Avg Opinion: {avg:F1}";
    }

    public void HideOpinionTooltip()
    {
        tooltipPanel.SetActive(false);
    }

private float GetAverageOpinion()
{
    List<string> factions = Config.InitialFactions;
    float total = 0f;

    foreach (var faction in factions)
    {
        total += Config.GetFactionOpinion(faction);
    }

    return total / factions.Count;
}

}
