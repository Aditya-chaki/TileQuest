using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
        int king = Config.KingOpinion;
        int queen = Config.QueenOpinion;
        int advisor = Config.AdvisorOpinion;
        return (king + queen + advisor) / 3f;
    }
}
