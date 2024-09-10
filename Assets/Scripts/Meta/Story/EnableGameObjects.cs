using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Threading.Tasks;

public class EnableGameObjects : MonoBehaviour
{
    
    public GameObject targetGameObject;
    public GameObject Bg;
    public GameObject Main;
    
    [SerializeField] RectTransform objectRect;
    [SerializeField] float topPos, middlePos;
    [SerializeField] float tweenDuration;
    [SerializeField] CanvasGroup image;

   
    public Button ChatButton;

    public Button CloseButton;

    void Start()
    {
        // Ensure ChatButton is assigned and add a listener to call the ToggleGameObject method when the button is clicked
        if (ChatButton != null)
        {
            ChatButton.onClick.AddListener(ToggleGameObject);
        }

        // Ensure CloseButton is assigned and add a listener to call the CloseGameObject method when the button is clicked
        if (CloseButton != null)
        {
            CloseButton.onClick.AddListener(CloseGameObject);
        }
    }

    // Method to toggle the GameObject's active state
    void ToggleGameObject()
    {
        if (targetGameObject != null)
        {
            targetGameObject.SetActive(!targetGameObject.activeSelf);
        }
        
         ObjectIntro();
         Bg.SetActive(true);
         targetGameObject.transform.SetParent(Bg.transform);
    }

    // Method to disable the GameObject
    async void CloseGameObject()
    {
        if (targetGameObject != null)
        {
           await ObjectOutro();
            targetGameObject.SetActive(false);
            Bg.SetActive(false);
            targetGameObject.transform.SetParent(Main.transform);
        }
    }
    void ObjectIntro(){
        image.DOFade(1,tweenDuration);
        objectRect.DOAnchorPosY(middlePos,tweenDuration);
    }
    async Task ObjectOutro(){
        image.DOFade(0,tweenDuration);
         await objectRect.DOAnchorPosY(topPos,tweenDuration).AsyncWaitForCompletion();
    }
}
