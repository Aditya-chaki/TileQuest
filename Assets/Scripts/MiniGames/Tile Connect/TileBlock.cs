using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBlock : MonoBehaviour
{
    public int id;
    public Vector2 gridPosition;
    private TileConnectController grid;
    private SpriteRenderer spriteRenderer;
    private Touch touch;
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

   public void Init(int _x, int _y, TileConnectController _grid)
    {
        gridPosition.x = _x;
        gridPosition.y = _y;
        grid = _grid;
    }

    public void SetPosition(int _x, int _y)
    {
        gridPosition.x = _x;
        gridPosition.y = _y;
        transform.position = new Vector3(gridPosition.x*grid.gridSize.x,gridPosition.y*grid.gridSize.y,0f);
    }
    void Update()
    {
        if(Input.touchCount>0){
            touch = Input.GetTouch(0);
            if(touch.phase == TouchPhase.Moved){    
                Vector2 pos = touch.position;
                pos.x = (pos.x) / grid.width;
                pos.y = (pos.y) / grid.height;
                float worldx = Mathf.Lerp(grid.cam.transform.position.x -grid.cam.orthographicSize*grid.cam.aspect,
                    grid.cam.transform.position.x+grid.cam.orthographicSize*grid.cam.aspect,pos.x);
                float worldy = Mathf.Lerp(grid.cam.transform.position.y -grid.cam.orthographicSize,
                    grid.cam.transform.position.y+grid.cam.orthographicSize,pos.y);
                Vector2 pos2D = new Vector2(worldx,worldy);
                RaycastHit2D hit = Physics2D.Raycast(pos2D,Vector2.zero);
                Debug.Log(hit.collider.gameObject.name);
                if (hit.collider != null && hit.collider.gameObject == gameObject)
                {
                    Debug.Log(this.transform.name+" hit");
                    OnTileSelected();
                }
            }
        } 
    }

    void OnTileSelected()
    {
        List<TileBlock> matches = FindMatches();
        if (matches.Count >= 3)
        {
             // Update score
            foreach (TileBlock tile in matches)
            {
                grid.RemoveTile((int)tile.gridPosition.x, (int)tile.gridPosition.y);
            }
            grid.ShiftTilesDown();
        }
    }

    List<TileBlock> FindMatches()
    {
        List<TileBlock> matches = new List<TileBlock> { this };
        FindAdjacentMatches((int)gridPosition.x,(int)gridPosition.y, matches);
        return matches;
    }

    void FindAdjacentMatches(int x, int y, List<TileBlock> matches)
    {
        TileBlock[] neighbors = GetAdjacentTiles(x, y);
        foreach (TileBlock neighbor in neighbors)
        {
            if (neighbor != null && neighbor.spriteRenderer.sprite == spriteRenderer.sprite && !matches.Contains(neighbor))
            {
                matches.Add(neighbor);
                neighbor.FindAdjacentMatches((int)neighbor.gridPosition.x, (int)neighbor.gridPosition.y, matches);
            }
        }
    }

    TileBlock[] GetAdjacentTiles(int x, int y)
    {
        TileBlock[] neighbors = new TileBlock[4];
        if (x > 0) neighbors[0] = grid.grid[x - 1, y]; // Left
        if (x < grid.width - 1) neighbors[1] = grid.grid[x + 1, y]; // Right
        if (y > 0) neighbors[2] = grid.grid[x, y - 1]; // Down
        if (y < grid.height - 1) neighbors[3] = grid.grid[x, y + 1]; // Up
        return neighbors;
    }
}   
