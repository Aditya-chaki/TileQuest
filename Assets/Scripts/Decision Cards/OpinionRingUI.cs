using UnityEngine;
using UnityEngine.UI;

public class OpinionRingUI : MonoBehaviour
{
    [SerializeField] private Image opinionRingImage;
    [SerializeField] private string characterId;

    private void OnEnable()
    {
        UpdateOpinion();
    }

    public void UpdateOpinion()
    {
        int opinionValue = Config.GetOpinionMeter(characterId); // Assumes your class is named OpinionMeter
        float fillAmount = opinionValue / 100f;
        opinionRingImage.fillAmount = fillAmount;
    }

    // Optional: call this externally when opinions are updated
    public void SetCharacter(string id)
    {
        characterId = id;
        UpdateOpinion();
    }
}
