using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public UIConfig uiConfig; // Assign the correct UIConfig for the scene in the Inspector

    public RectTransform character1;
    public RectTransform character2;
    public RectTransform background;

    private Image character1Image;
    private Image character2Image;
    private Image backgroundImage;

    private void Awake()
    {
        // Get Image components from UI elements
        character1Image = character1.GetComponent<Image>();
        character2Image = character2.GetComponent<Image>();
        backgroundImage = background.GetComponent<Image>();
    }

    private void Start()
    {
        if (uiConfig != null)
        {
            ApplyUIConfig();
        }
    }

    public void ApplyUIConfig()
    {
        // Apply positions and scales
        if (character1 != null)
        {
            character1.anchoredPosition = uiConfig.character1Position;
            character1.localScale = uiConfig.character1Scale;
            character1Image.sprite = uiConfig.character1Sprite; // Set the sprite
        }

        if (character2 != null)
        {
            character2.anchoredPosition = uiConfig.character2Position;
            character2.localScale = uiConfig.character2Scale;
            character2Image.sprite = uiConfig.character2Sprite; // Set the sprite
        }

        if (background != null)
        {
            background.anchoredPosition = uiConfig.backgroundPosition;
            background.sizeDelta = uiConfig.backgroundSize;
            backgroundImage.sprite = uiConfig.backgroundSprite; // Set the sprite
        }
    }
}
