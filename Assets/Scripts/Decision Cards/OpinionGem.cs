using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OpinionGem : MonoBehaviour
{
    public GameObject tooltipPanel;
    public TMP_Text tooltipText;

    public TMP_Text tooltipTitle;

    //testing purpose only
    void Start()
{
    Config.KingOpinion = 80;
    Config.QueenOpinion = 60;
    Config.AdvisorOpinion = 40;
}


    public void ShowOpinionTooltip()
    {
        tooltipPanel.SetActive(true);
        float avg = GetAverageOpinion();
        tooltipText.text = $"{avg:F1}";
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
