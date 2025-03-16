using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileConnectController : MonoBehaviour
{
    public Vector2Int gridSize;
    public GameObject tile;
    public Color seletedTileColor;
    public Vector2 offset;
    private Touch touch;
    public float width;
    public float height;
    public Camera cam;
    public Sprite[] tileSprites;
    public TileBlock[,] grid;
    // Start is called before the first frame update
    void Start()
    {
        width = (float)Screen.width;
        height = (float)Screen.height;
       GenerateGrid(); 
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.touchCount>0 && false){
            touch = Input.GetTouch(0);
            if(touch.phase == TouchPhase.Moved){    
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
                    hit.collider.GetComponent<SpriteRenderer>().color = seletedTileColor;
                    Debug.Log(hit.transform.name);
                }
            }
        } 
    }

    void GenerateGrid()
    {
        grid = new TileBlock[gridSize.x, gridSize.y];
        int totalTiles = gridSize.x * gridSize.y;
        if (totalTiles % 2 != 0)
        {
            Debug.LogError("Grid size must be even!");
            return;
        }

        for(int x=0;x<gridSize.x;x++)
        {
            for(int y = 0;y<gridSize.y;y++)
            {
                SpawnTile(x,y);
            }
        }
    }
    void SpawnTile(int x, int y)
    {
        GameObject tileObj = Instantiate(tile);
        tileObj.transform.position = new Vector2(x,-y)+offset;  
        tileObj.GetComponent<SpriteRenderer>().sprite = tileSprites[Random.Range(0,tileSprites.Length)];   
        tileObj.transform.parent = transform;
        tileObj.GetComponent<TileBlock>().Init(x,y,this);
        grid[x,y] = tileObj.GetComponent<TileBlock>();
    }
    public void RemoveTile(int x, int y)
    {
        if (grid[x, y] != null)
        {
            Destroy(grid[x, y].gameObject);
            grid[x, y] = null;
        }
    }

    public void ShiftTilesDown()
    {
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y - 1; y++)
            {
                if (grid[x, y] == null)
                {
                    for (int aboveY = y + 1; aboveY < height; aboveY++)
                    {
                        if (grid[x, aboveY] != null)
                        {
                            grid[x, y] = grid[x, aboveY];
                            grid[x, y].SetPosition(x, y);
                            grid[x, aboveY] = null;
                            break;
                        }
                    }
                }
            }
        }

        // Refill missing tiles
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (grid[x, y] == null)
                {
                    SpawnTile(x, y);
                }
            }
        }
    }

}
