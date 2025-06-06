using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class WaterSortController : MonoBehaviour
{
     enum GAME_STATE
    {
        PLAYING,
        PAUSE,
        END,
        WIN,
        LOSE,
        REWARD
    }
    GAME_STATE gameState = GAME_STATE.PLAYING;
    [Range(1,3)]
    public int Difficulty = 1;
    public bool isDebug = false;
    public GameObject[] DifficultyBottlesHolder;
    public BottleController FirstBottle;
    public BottleController SecondBottle;
    BottleController[] AllBottles;
    public GameObject[] extraBottle;
    public Color[] waterColors;
    private Touch touch;
    private float width;
    private float height;
    public Camera cam;
    public PausePopup2 pausePopUp;
    public GameObject gameOverPanel;
    public TMP_Text rewardText;
    public Image rewardSprite;
    public Sprite food,strength,health,Gold;
    public GameObject WatchAdPopup;
    Dictionary<Color,int> colorBottleFrq = new Dictionary<Color,int>();
    int NumerOfEmptyBottles = 1;
    bool extraBottleActive = false;

    float diffRewardFactor = 0f;


    // Start is called before the first frame update
    void Start()
    {   
        WeeklyQuest.UpdateMiniGamesPlayed();
        DailyQuest.UpdateMinigamesPlayed();
        if(isDebug==false)
             Difficulty = Random.Range(1,4);
        width = (float)Screen.width;
        height = (float)Screen.height;
        DifficultyBottlesHolder[Difficulty-1].SetActive(true);
        AllBottles = DifficultyBottlesHolder[Difficulty-1].GetComponentsInChildren<BottleController>();

        for(int i=0;i<Difficulty*2;i++){
            colorBottleFrq.Add(waterColors[i],0);
        }
        if(Difficulty==3)
        {
            NumerOfEmptyBottles = 2;
        }
        AssignColors();
    }

    // Update is called once per frame
    void Update()
    {
        if(gameState == GAME_STATE.PLAYING)
            GameWin();

        if(Input.touchCount>0){
            touch = Input.GetTouch(0);
            if(touch.phase == TouchPhase.Ended){    
                Vector2 pos = touch.position;
                pos.x = (pos.x) / width;
                pos.y = (pos.y) / height;
                float worldx = Mathf.Lerp(cam.transform.position.x -cam.orthographicSize*cam.aspect,
                    cam.transform.position.x+cam.orthographicSize*cam.aspect,pos.x);
                float worldy = Mathf.Lerp(cam.transform.position.y -cam.orthographicSize,
                    cam.transform.position.y+cam.orthographicSize,pos.y);
                Vector2 pos2D = new Vector2(worldx,worldy);
                RaycastHit2D hit = Physics2D.Raycast(pos2D,Vector2.zero);
                
                if(hit.collider!=null){
                    if(hit.collider.GetComponent<BottleController>()!=null)
                    {
                        if(FirstBottle==null){
                            FirstBottle = hit.collider.GetComponent<BottleController>();
                            if(FirstBottle.numberOfColorsInBottle==0)
                            {
                                FirstBottle = null;
                            }
                            else
                                FirstBottle.transform.localScale *=1.1f;
                        }
                        else
                        {
                            if(FirstBottle==hit.collider.GetComponent<BottleController>()){
                                FirstBottle =null;
                            }
                            else
                            {
                                SecondBottle = hit.collider.GetComponent<BottleController>();
                                FirstBottle.bottleController = SecondBottle;
                                FirstBottle.UpdateTopColorValues();
                                SecondBottle.UpdateTopColorValues();
                                if(SecondBottle.FillBottleCheck(FirstBottle.topColor)==true)
                                {
                                    FirstBottle.StartColorTransfer();
                                    FirstBottle.UpdateTopColorValues();
                                    SecondBottle.UpdateTopColorValues();
                                    FirstBottle.transform.localScale *=0.9f;
                                    FirstBottle = null;
                                    SecondBottle = null;
                                }
                                else
                                {
                                    FirstBottle.transform.localScale *=0.9f;
                                    FirstBottle = null;
                                    SecondBottle = null;
                                   
                                }
                                
                            }
                        }
                    }
                }
                else if(FirstBottle!=null)
                    {
                        FirstBottle.transform.localScale *=0.9f;
                        FirstBottle = null;
                    }
                    
            }
        }
    }

    
    void AssignColors()
    {   
        
        for(int i=0;i<AllBottles.Length-NumerOfEmptyBottles;i++)
        {   int loopBreak = 0;
            Color[] newColors = new Color[4]; 
            int j=0;
            while(j<4){
                if(loopBreak>5000){
                    Debug.Log("Loop reached limit");
                    break;
                }
                loopBreak++;
            int idx = Random.Range(0,waterColors.Length);
            if(colorBottleFrq.ContainsKey(waterColors[idx]) && colorBottleFrq[waterColors[idx]]<4){
            newColors[j]=waterColors[idx];
            colorBottleFrq[waterColors[idx]]++;
            j++;
           
            }
            }
            AllBottles[i].bottlesColor = newColors;
            AllBottles[i].UpdateColorsOnShader();
            AllBottles[i].UpdateTopColorValues();
            }
    }

    public void ExtraBottle()
    {
        extraBottle[Difficulty-1].SetActive(true);
        extraBottleActive = true;
    }

    public void ResetBottle()
    {
          for(int i=0;i<Difficulty*2;i++){
            colorBottleFrq[waterColors[i]]=0;
        }
        for(int i=0;i<AllBottles.Length-NumerOfEmptyBottles;i++){
            AllBottles[i].ResetBottle(4);
        }
        AllBottles[AllBottles.Length-1].ResetBottle(0);
        if(Difficulty==3)
        {
            AllBottles[AllBottles.Length-2].ResetBottle(0);
        }
        if(extraBottleActive==true)
        {
            extraBottle[Difficulty-1].GetComponent<BottleController>().ResetBottle(0);
        }
        AssignColors();
    }
    
    void GameWin()
    {
        foreach(BottleController bottle in AllBottles)
        {   
            if(bottle.IsSorted()==false)
                return;    
        }
        Debug.Log("Game Win");
        StartCoroutine(ShowWinPanel());
        gameState  = GAME_STATE.WIN;
    }
    
     IEnumerator ShowWinPanel() 
    {
        yield return new WaitForEndOfFrame();
        gameOverPanel.SetActive(true);
        gameState = GAME_STATE.WIN;
        diffRewardFactor = Difficulty;
        int randReward = Random.Range(0,3);
        int rewardValue = (int)diffRewardFactor*20;
        switch(randReward)
        {
            case 0:Config.Food = Config.Food+rewardValue;
                    rewardSprite.sprite = food;
                    rewardText.text = "x"+rewardValue.ToString();
                     Debug.Log(Config.Food+" Food Reward");
                     break;
            case 1:Config.Health = Config.Health+rewardValue;
                    rewardSprite.sprite = health;
                    rewardText.text = "x"+rewardValue.ToString();
                     Debug.Log(Config.Health+" Health Reward");
                     break; 
            case 2:Config.Gold = Config.Gold+rewardValue;
                    rewardSprite.sprite = Gold;
                    rewardText.text = "x"+rewardValue.ToString();
                     Debug.Log(Config.Gold+" Gold Reward");
                     WeeklyQuest.UpdateGoldEarned(rewardValue);
                     break;                 
        }
       
        
    }

    public void Pause()
    {
        
        pausePopUp.OpenPausePopup(1);
    }

    public void CloseWatchAdPopUp()
    {
        WatchAdPopup.SetActive(false);
        gameState = GAME_STATE.PLAYING;
    }    
    
    public void WatchAd()
    {
        Debug.Log("PlayAd");
        WatchAdPopup.SetActive(false);
        gameState = GAME_STATE.PLAYING;
        ExtraBottle();

    }

    public void WatchAdPressed()
    {
        WatchAdPopup.SetActive(true);
    }

    public void Home()
    {
        SceneManager.LoadScene("Menu");
    }
}
