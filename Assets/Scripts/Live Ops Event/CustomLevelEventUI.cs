using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class CustomLevelEventUI : MonoBehaviour
{
    [Header("Event Configuration")]
    [SerializeField] private int requiredLevel;

    [Header("UI Elements")]
    [SerializeField] private Slider progressSlider;  // Slider for quest progress
    [SerializeField] private TextMeshProUGUI progressText;  // Text to show progress
    [SerializeField] private Image completedIndicator;  
    [SerializeField] private TextMeshProUGUI discriptionTxt; 

    // Start is called before the first frame update
    void Start()
    {
        requiredLevel = PlayerPrefs.GetInt("Event_LevelRequired");
        UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateUI();
    }

    void UpdateUI()
    {
        int currentLevel = PlayerPrefs.GetInt("Event_CurrentLevel");
        progressSlider.maxValue = requiredLevel;
        progressSlider.value = currentLevel;
        progressText.text = $"{currentLevel}/{requiredLevel}";
        discriptionTxt.text = "Complete 5 level of Event tile match";
        if(currentLevel==requiredLevel)
        {
            completedIndicator.enabled = true;
        }
    }
}
