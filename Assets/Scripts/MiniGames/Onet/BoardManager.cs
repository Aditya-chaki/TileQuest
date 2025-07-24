using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.SceneManagement;
public class BoardManager : MonoBehaviour
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
    public int rows = 6;
    public int cols = 10;
    public float time = 60;
    public Vector3 offset;
    public GameObject tilePrefab;
    public Transform boardParent;
    public Sprite[] tileSprites; // Assign in Unity Editor
    public LineRenderer lineRender;
    public TMP_Text timeText;
    public GameObject gameOverPanel;
    public GameObject WatchAdPopup;
    public PausePopup2 pausePopUp;
    public TMP_Text rewardText;
    public Image rewardSprite;
    public Sprite food,strength,health,Gold;
    public bool isEventLevel = false;
    public GameObject eventGameOverPanel;

    private OnetTile[,] grid;
    private List<Vector2Int> availablePositions = new List<Vector2Int>();
    private OnetTile firstTile, secondTile;
    private List<Vector2Int> points = new List<Vector2Int>();
    private Dictionary<Vector2Int,Vector3> allTilePosition = new Dictionary<Vector2Int,Vector3>();
    float nextTime = 0;
    float diffRewardFactor = 0f;
    int score = 0;

    void Start()
    {
        if(isEventLevel==false)
        {
        WeeklyQuest.UpdateMiniGamesPlayed();
        DailyQuest.UpdateMinigamesPlayed();
        SetDifficulty();
        }
        InitializeBoard();
    }

    void Update()
    {
        if(gameState == GAME_STATE.PLAYING)
            GameTime();

        if(pausePopUp.gameObject.active==false&&WatchAdPopup.gameObject.active==false)
            gameState = GAME_STATE.PLAYING;
    }
    void SetDifficulty()
    {
        int randTime = Random.Range(0,3);
        int randCol = Random.Range(0,3);
        switch(randTime)
        {
            case 0:time = 30;
                break;

            case 1:time = 20;
                break;

            case 2:time = 10;
                break;        
        }

        switch(randCol)
        {
            case 0:cols = 6;
                break;

            case 1:cols = 8;
                break;

            case 2:cols = 10;
                break;        
        }
        diffRewardFactor = randTime+randCol/2;
    }

    void InitializeBoard()
    {
        grid = new OnetTile[cols, rows];
        int totalTiles = rows * cols;
        if (totalTiles % 2 != 0)
        {
            Debug.LogError("Grid size must be even!");
            return;
        }
        // Create a list of pairs
        int uniqueTileCount = Mathf.Min(tileSprites.Length, totalTiles / 4); // Multiple of 2 or 4 per type
        List<int> tileIDs = new List<int>();

        // Generate multiple pairs for each tile type
        for (int i = 0; i < uniqueTileCount; i++)
        {
            int numPairs = totalTiles / uniqueTileCount; // Distribute evenly
            for (int j = 0; j < numPairs; j++)
            {
                tileIDs.Add(i);
            }
        }
        Shuffle(tileIDs);

        if(cols>4)
            offset.x = -250;

        if(cols>=6)
            offset.y = -200;    

        int index = 0;
        for (int c = 0; c < cols; c++)
        {
            for (int r = 0; r < rows; r++)
            {
                if(index>=tileIDs.Count)
                    break;
                Vector2Int pos = new Vector2Int(r, c);
                GameObject tileObj = Instantiate(tilePrefab, boardParent);
                OnetTile tile = tileObj.GetComponent<OnetTile>();
                tile.tileID = tileIDs[index];
                tile.gridPosition = pos;
                
                tileObj.GetComponent<Image>().sprite = tileSprites[tile.tileID];
                tileObj.GetComponent<RectTransform>().anchoredPosition = new Vector3(pos.x*100,pos.y*100,0)+offset;
                grid[c, r] = tile;
                availablePositions.Add(pos);
                allTilePosition[pos] = tileObj.GetComponent<RectTransform>().anchoredPosition; 
                index++;
                
            }
        }
    }

    void Shuffle(List<int> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }

    public void SelectTile(OnetTile tile)
    {
    if (firstTile == null)
    {
        firstTile = tile;
        firstTile.GetComponent<RectTransform>().localScale *=1.2f; 
    }
    else if (secondTile == null)
    {
        secondTile = tile;

        if (firstTile.tileID == secondTile.tileID)
        {
            if (IsValidMatch(firstTile.gridPosition, secondTile.gridPosition))
            {
               
                lineRender.enabled = true;
                int idx=0;
                firstTile.GetComponent<RectTransform>().DOShakePosition(0.5f,5f,10,90,false,true);
                secondTile.GetComponent<RectTransform>().DOShakePosition(0.5f,5f,10,90,false,true);
                score++;
                StartCoroutine(RemoveTiles(firstTile, secondTile));
                if(isEventLevel==true)
                {
                    time+=2f;
                }
            }
            lineRender.enabled = false;
            points.Clear();
        }
        firstTile.GetComponent<RectTransform>().localScale *=0.8f;
        firstTile = secondTile = null;
    }
    }

    

    bool IsValidMatch(Vector2Int start, Vector2Int end)
    {
    if(start==end)
        return false;

    Queue<(Vector2Int pos, int turns, Vector2Int lastDir)> queue = new Queue<(Vector2Int, int, Vector2Int)>();
    HashSet<Vector2Int> visited = new HashSet<Vector2Int>();

    queue.Enqueue((start, 0, Vector2Int.zero));

    while (queue.Count > 0)
    {
        var (pos, turns, lastDir) = queue.Dequeue();

        if (pos == end) return true;
        if (turns > 3) continue;
        if (visited.Contains(pos)) continue;

        visited.Add(pos);
        points.Add(pos);
        foreach (var dir in new Vector2Int[] { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right })
        {
            Vector2Int nextPos = pos + dir;
            if (IsInsideGrid(nextPos) && (grid[nextPos.y, nextPos.x] == null || nextPos == end))
            {
                int newTurns = (lastDir == Vector2Int.zero || lastDir == dir) ? turns : turns + 1;
                queue.Enqueue((nextPos, newTurns, dir));
            }
        }
    }
    return false;
    }

    bool IsInsideGrid(Vector2Int pos)
    {
        return pos.x >= 0 && pos.x < rows && pos.y >= 0 && pos.y < cols;
    }

    IEnumerator RemoveTiles(OnetTile t1, OnetTile t2)
    {  
        yield return new WaitForSeconds(0.4f); 
        grid[t1.gridPosition.y, t1.gridPosition.x] = null;
        grid[t2.gridPosition.y, t2.gridPosition.x] = null;

        Destroy(t1.gameObject);
        Destroy(t2.gameObject);
        bool flagEmpty = true;
        for(int i=0;i<cols;i++)
        {
            for(int j=0;j<rows;j++)
            {
                if(grid[i,j]!=null)
                {
                    flagEmpty = false;
                    break;
                }
            }
        }
        Debug.Log(flagEmpty);
        if(flagEmpty==true)
        {
            Debug.Log("Game Won");    
            gameState = GAME_STATE.WIN;
            GameWon();
        }
    }

    public void HintPressed()
    {
        gameState = GAME_STATE.PAUSE;
        if(gameover)
        {
            return;
        }
        WatchAdPopup.SetActive(true);
    }

   public void ShowHint()
    {
    
    foreach (var tile1 in grid)
    {
        foreach (var tile2 in grid)
        {
            if( tile1==null || tile2==null)
                continue;
            if (tile1 != tile2 && tile1.tileID == tile2.tileID && IsValidMatch(tile1.gridPosition, tile2.gridPosition))
            {
                tile1.GetComponent<Image>().color = Color.green;
                tile2.GetComponent<Image>().color = Color.green;
                tile1.GetComponent<RectTransform>().DOShakePosition(0.5f,5f,10,90,false,true);
                tile2.GetComponent<RectTransform>().DOShakePosition(0.5f,5f,10,90,false,true);
                
                return;
            }
        }
    }
    }
    IEnumerator RemoveEffect()
    {
        int idx=0;
        foreach(Vector2Int pos in points)
        {
            lineRender.SetPosition(idx,allTilePosition[pos]);
        }
           yield return new WaitForEndOfFrame();
    }

    bool gameover = false;
    
    void GameTime()
    {
        if(gameState==GAME_STATE.PAUSE)
        {
            return;
        }
                
        if(gameState==GAME_STATE.WIN)
        {
            return;
        }

       if(Time.time>nextTime&&time>0 && gameState==GAME_STATE.PLAYING)
       { nextTime = Time.time + 1;
       time--;
       }

        timeText.text = time.ToString();

       if(time<=0 && gameover==false)
       {

        string currentEvent = PlayerPrefs.GetString("ActiveEvent","none");
        if(currentEvent=="MinigamesEvent")
        {
           int current = PlayerPrefs.GetInt("Event_OnetCurrentLevel")+1;
            PlayerPrefs.SetInt("Event_OnetCurrentLevel",current);
        }
        if(isEventLevel==true)
        {
            eventGameOverPanel.SetActive(true);
            return;
        }

        gameover = true;
        gameOverPanel.SetActive(true);
        boardParent.gameObject.SetActive(false);
        Debug.Log("Game Over");
        int randReward = Random.Range(0,4);
        int rewardValue = score*(int)diffRewardFactor*20;
        switch(randReward)
        {
            case 0:Config.Magic = Config.Magic+rewardValue;
                    rewardSprite.sprite = food;
                    rewardText.text = "x"+rewardValue.ToString();
                     Debug.Log(Config.Magic+" Food Reward");
                     break;
            case 1:Config.Influence = Config.Influence+rewardValue;
                    rewardSprite.sprite = health;
                    rewardText.text = "x"+rewardValue.ToString();
                     Debug.Log(Config.Influence+" Health Reward");
                     break; 
            case 2:Config.Gold = Config.Gold+rewardValue;
                    rewardSprite.sprite = Gold;
                    WeeklyQuest.UpdateGoldEarned(rewardValue);
                    rewardText.text = "x"+rewardValue.ToString();
                     Debug.Log(Config.Gold+" Gold Reward");
                     break;                 
        }
       }
        

    }

    private void GameWon()
    {
        gameover = true;
        gameOverPanel.SetActive(true);
        boardParent.gameObject.SetActive(false);
        Debug.Log("Game Over");
        int randReward = Random.Range(0,4);
        int rewardValue = score*(int)diffRewardFactor*20+(int)time*(int)diffRewardFactor;
        switch(randReward)
        {
           case 0:Config.Magic = Config.Magic+rewardValue;
                    rewardSprite.sprite = food;
                    rewardText.text = "x"+rewardValue.ToString();
                     Debug.Log(Config.Magic+" Food Reward");
                     break;
            case 1:Config.Influence = Config.Influence+rewardValue/2;
                    rewardSprite.sprite = health;
                    rewardText.text = "x"+rewardValue.ToString();
                     Debug.Log(Config.Influence+" Health Reward");
                     break; 
            case 2:Config.Gold = Config.Gold+rewardValue;
                    rewardSprite.sprite = Gold;
                    WeeklyQuest.UpdateGoldEarned(rewardValue);
                    rewardText.text = "x"+rewardValue.ToString();
                     Debug.Log(Config.Gold+" Gold Reward");
                     break;                   
        }
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
        ShowHint();

    }

    public void Pause()
    {
        if(gameover)
        {
            return;
        }

        if(gameState==GAME_STATE.PLAYING){
        gameState = GAME_STATE.PAUSE;
        pausePopUp.OpenPausePopup(0);
        }
        else
        {
            gameState = GAME_STATE.PLAYING;
        }
    }

    public void Home()
    {
        SceneManager.LoadScene("Menu");
    }
    
}   
