using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
public class MiniGameEventUI : MonoBehaviour
{
    [Header("Event Configuration")]
    [SerializeField] private int requiredOnetLevel;

    [Header("UI Elements")]
    [SerializeField] private Slider progressSliderOnet;  // Slider for quest progress
    [SerializeField] private TextMeshProUGUI progressTextOnet;  // Text to show progress
    [SerializeField] private Image completedIndicatorOnet;  
    [SerializeField] private TextMeshProUGUI discriptionTxtOnet; 
    // Start is called before the first frame update
    void Start()
    {
        
        requiredOnetLevel = PlayerPrefs.GetInt("Event_OnetLevelRequired");
        Debug.Log(" " +requiredOnetLevel);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateUI();
    }
    void UpdateUI()
    {
        
        int currentOnetLevel = PlayerPrefs.GetInt("Event_OnetCurrentLevel");
        progressSliderOnet.maxValue = requiredOnetLevel;
        progressSliderOnet.value = currentOnetLevel;
        progressTextOnet.text = $"{currentOnetLevel}/{requiredOnetLevel}";
        discriptionTxtOnet.text = "Complete 5 level of onet minigame";
        if(currentOnetLevel==requiredOnetLevel)
        {
            completedIndicatorOnet.enabled = true;
        }
    }
}
