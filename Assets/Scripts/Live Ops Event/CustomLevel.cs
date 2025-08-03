using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class CustomLevel : MonoBehaviour
{
    public static CustomLevel instance;
    public GameObject gameManager;
    public int[] setLevels;
    public float time = 60;
    public TMP_Text timeText;
    public GameObject winPopUp;
    public GameObject losePopUp;
    public Button homeButton;
    public Button nextButton;

    private float nextTime;
    private bool isGameOver = false;
    private bool gameWon = false;
        
    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        int idx = PlayerPrefs.GetInt("Event_CurrentLevel");
        gameManager.GetComponent<GamePlayManager>().level = setLevels[idx];
        gameManager.SetActive(true);
        homeButton.onClick.AddListener(Home);
        nextButton.onClick.AddListener(Next);
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time>nextTime && time>0f)
        {
            nextTime = Time.time+1f;
            time--;
            timeText.text = time.ToString();
        }
        if(time<=0f && isGameOver==false&& gameWon==false)
        {
            isGameOver = true;
            losePopUp.SetActive(true);
        }

        

    }
    public void LevelWin()
    {
        if(gameWon==false)
        {
        winPopUp.SetActive(true);
        int currentLevelCompleted=PlayerPrefs.GetInt("Event_CurrentLevel",0)+1; 
        PlayerPrefs.SetInt("Event_CurrentLevel",currentLevelCompleted); 
        gameWon = true;
        }
    }

    public void Home()
    {
        SceneManager.LoadScene("Menu");
    }

    public void Next()
    {
        SceneManager.LoadScene("CustomEventTilematch");
    }
}
