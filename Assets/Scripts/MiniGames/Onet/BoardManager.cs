using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class BoardManager : MonoBehaviour
{
    public int rows = 6;
    public int cols = 10;
    public Vector3 offset;
    public GameObject tilePrefab;
    public Transform boardParent;
    public Sprite[] tileSprites; // Assign in Unity Editor

    private OnetTile[,] grid;
    private List<Vector2Int> availablePositions = new List<Vector2Int>();
    private OnetTile firstTile, secondTile;

    void Start()
    {
        InitializeBoard();
    }


    void InitializeBoard()
    {
        grid = new OnetTile[rows, cols];

        // Create a list of pairs
        List<int> tileIDs = new List<int>();
        for (int i = 0; i < (rows * cols) / 2; i++)
        {
            tileIDs.Add(i);
            tileIDs.Add(i); // Each tile appears twice
        }
        Debug.Log(tileIDs.Count+"tiles");
        Shuffle(tileIDs);

        int index = 0;
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                Vector2Int pos = new Vector2Int(r, c);
                availablePositions.Add(pos);

                GameObject tileObj = Instantiate(tilePrefab, boardParent);
                OnetTile tile = tileObj.GetComponent<OnetTile>();
                Debug.Log(index+" "+pos);
                tile.tileID = tileIDs[index++];
                tile.gridPosition = pos;
                tileObj.GetComponent<Image>().sprite = tileSprites[tile.tileID];
                tileObj.GetComponent<RectTransform>().anchoredPosition = new Vector3(pos.x*100,pos.y*100,0)+offset;
                grid[r, c] = tile;
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
                RemoveTiles(firstTile, secondTile);
            }
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


   public void ShuffleBoard()
    {
    List<OnetTile> tiles = new List<OnetTile>();
    foreach (var tile in grid)
    {
        if (tile != null) tiles.Add(tile);
    }

    //Shuffle(tiles);

    int index = 0;
    for (int r = 0; r < rows; r++)
    {
        for (int c = 0; c < cols; c++)
        {
            if (grid[r, c] != null)
            {
                grid[r, c] = tiles[index++];
                grid[r, c].gridPosition = new Vector2Int(r, c);
            }
        }
    }
    }

   public void ShowHint()
    {
    foreach (var tile1 in grid)
    {
        foreach (var tile2 in grid)
        {
            if (tile1 != tile2 && tile1.tileID == tile2.tileID && IsValidMatch(tile1.gridPosition, tile2.gridPosition))
            {
                tile1.GetComponent<Image>().color = Color.green;
                tile2.GetComponent<Image>().color = Color.green;
                return;
            }
        }
    }
    }


}   
