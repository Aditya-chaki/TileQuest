using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class BoardManager : MonoBehaviour
{
    public int rows = 6;
    public int cols = 10;
    public float time = 60;
    public Vector3 offset;
    public GameObject tilePrefab;
    public Transform boardParent;
    public Sprite[] tileSprites; // Assign in Unity Editor
    public LineRenderer lineRender;
    public GameObject[] allTiles;
    public TMP_Text timeText;
    private OnetTile[,] grid;
    private List<Vector2Int> availablePositions = new List<Vector2Int>();
    private OnetTile firstTile, secondTile;
    private List<Vector2Int> points = new List<Vector2Int>();
    private Dictionary<Vector2Int,Vector3> allTilePosition = new Dictionary<Vector2Int,Vector3>();
    float nextTime = 0;
    void Start()
    {
        InitializeBoard();
    }

    void Update()
    {
        GameTime();
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

        int index = 0;
        for (int c = 0; c < cols; c++)
        {
            for (int r = 0; r < rows; r++)
            {
                Vector2Int pos = new Vector2Int(r, c);
                allTiles[index].SetActive(true);
               //GameObject tileObj = Instantiate(tilePrefab, boardParent);
                GameObject tileObj = allTiles[index];
                OnetTile tile = tileObj.GetComponent<OnetTile>();
                tile.tileID = tileIDs[index++];
                tile.gridPosition = pos;
                if(tileSprites.Length>tile.tileID){
                tileObj.GetComponent<Image>().sprite = tileSprites[tile.tileID];
                //tileObj.GetComponent<RectTransform>().anchoredPosition = new Vector3(pos.x*100,pos.y*100,0)+offset;
                grid[c, r] = tile;
                availablePositions.Add(pos);
                allTilePosition[pos] = tileObj.GetComponent<RectTransform>().anchoredPosition; 
                }
                if(tileIDs.Count==index)
                    return;
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
                StartCoroutine(RemoveEffect());
                 RemoveTiles(firstTile, secondTile);
            }
            lineRender.enabled = false;
            points.Clear();
        }

        firstTile = secondTile = null;
    }
    }


    bool IsValidMatch(Vector2Int start, Vector2Int end)
    {
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
            if (IsInsideGrid(nextPos) && (grid[nextPos.x, nextPos.y] == null || nextPos == end))
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

    void RemoveTiles(OnetTile t1, OnetTile t2)
    {   
    grid[t1.gridPosition.x, t1.gridPosition.y] = null;
    grid[t2.gridPosition.x, t2.gridPosition.y] = null;

    Destroy(t1.gameObject);
    Destroy(t2.gameObject);
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

    void GameTime()
    {
       if(Time.time>nextTime&&time>0)
       { nextTime = Time.time + 1;
       time--;
       }
       if(time<=0)
       {
        Debug.Log("Game Over");
       }
        timeText.text = time.ToString();

    }
   
}   
