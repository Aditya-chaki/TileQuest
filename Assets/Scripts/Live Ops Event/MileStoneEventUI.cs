using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class MileStoneEventUI : MonoBehaviour
{
    [Header("Event Configuration")]
    [SerializeField] private int requiredInfluence;
    [SerializeField] private int requiredGold;
    [SerializeField] private int requiredMagic;
    [SerializeField] private int questNumber;

    [Header("UI Elements")]
    [SerializeField] private Slider progressSliderFood;  // Slider for quest progress
    [SerializeField] private TextMeshProUGUI progressTextFood;  // Text to show progress
    
    [SerializeField] private Slider progressSliderGold;  // Slider for quest progress
    [SerializeField] private TextMeshProUGUI progressTextGold;  // Text to show progress
    
    [SerializeField] private Slider progressSliderMagic;  // Slider for quest progress
    [SerializeField] private TextMeshProUGUI progressTextMagic;  // Text to show progress
    
    [SerializeField] private Image completedIndicatorFood;  
    [SerializeField] private TextMeshProUGUI discriptionTxtFood; 
    
    [SerializeField] private Image completedIndicatorGold;  
    [SerializeField] private TextMeshProUGUI discriptionTxtGold; 
        
    [SerializeField] private Image completedIndicatorMagic;  
    [SerializeField] private TextMeshProUGUI discriptionTxtMagic; 
    
    // Start is called before the first frame update
    void Start()
    {
        requiredInfluence = PlayerPrefs.GetInt("Event_requiredInfluence");
        requiredGold = PlayerPrefs.GetInt("Event_requiredGold");
        requiredMagic = PlayerPrefs.GetInt("Event_requiredMagic");
        UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateUI();
    }

    void UpdateUI()
    {
       int currentInfluence = PlayerPrefs.GetInt("Event_currentInfluence");
       int currentGold = PlayerPrefs.GetInt("Event_currentGold");
       int currentMagic = PlayerPrefs.GetInt("Event_currentMagic");

        progressSliderGold.maxValue = requiredGold;
        progressSliderGold.value = currentGold;
        progressTextGold.text = $"{currentGold}/{requiredGold}";
        discriptionTxtGold.text = "Earn 1000 Gold";

        progressSliderFood.maxValue = requiredInfluence;
        progressSliderFood.value = currentInfluence;
        progressTextFood.text = $"{currentInfluence}/{requiredInfluence}";
        discriptionTxtFood.text = "Collect 1000 Influence";
        
        progressSliderMagic.maxValue = requiredMagic;
        progressSliderMagic.value = currentMagic;
        progressTextMagic.text = $"{currentMagic}/{requiredMagic}";
        discriptionTxtMagic.text = "Collect 1000 Magic";

        if(currentInfluence == requiredInfluence)
        {
            completedIndicatorFood.enabled = true;
        }
        else
        {
            completedIndicatorFood.enabled = false; 
        }

        if(currentGold==requiredGold)
        {
            completedIndicatorGold.enabled = true;
        }
        else
        {
            completedIndicatorGold.enabled = false;
        }

        if(currentMagic==requiredMagic)
        {
             completedIndicatorMagic.enabled = true;
        }
        else
        {
            completedIndicatorMagic.enabled = false;
        }
    }
}
