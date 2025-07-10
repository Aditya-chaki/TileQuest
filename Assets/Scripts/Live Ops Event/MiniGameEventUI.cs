using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
public class MiniGameEventUI : MonoBehaviour
{
    [Header("Event Configuration")]
    [SerializeField] private int requiredWatersortLevel;
    [SerializeField] private int requiredOnetLevel;

    [Header("UI Elements")]
    [SerializeField] private Slider progressSliderWatersort;  // Slider for quest progress
    [SerializeField] private TextMeshProUGUI progressTextWatersort;  // Text to show progress
    [SerializeField] private Image completedIndicatorWatersort;  
    [SerializeField] private TextMeshProUGUI discriptionTxtWatersort; 

    [SerializeField] private Slider progressSliderOnet;  // Slider for quest progress
    [SerializeField] private TextMeshProUGUI progressTextOnet;  // Text to show progress
    [SerializeField] private Image completedIndicatorOnet;  
    [SerializeField] private TextMeshProUGUI discriptionTxtOnet; 
    // Start is called before the first frame update
    void Start()
    {
        requiredOnetLevel = PlayerPrefs.GetInt("Event_WaterSortLevelRequired");
        requiredOnetLevel = PlayerPrefs.GetInt("Event_OnetLevelRequired");
    }

    // Update is called once per frame
    void Update()
    {
        UpdateUI();
    }
    void UpdateUI()
    {
        int currentWatersortLevel = PlayerPrefs.GetInt("Event_WaterSortCurrentLevel");
        int currentOnetLevel = PlayerPrefs.GetInt("Event_OnetCurrentLevel");
        
        progressSliderWatersort.maxValue = requiredWatersortLevel;
        progressSliderWatersort.value = currentWatersortLevel;
        progressTextWatersort.text = $"{currentWatersortLevel}/{requiredWatersortLevel}";
        discriptionTxtWatersort.text = "Complete 3 level of water sort minigame";
        if(currentWatersortLevel==requiredWatersortLevel)
        {
            completedIndicatorWatersort.enabled = true;
        }

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
