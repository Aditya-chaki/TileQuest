using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class OnetTile : MonoBehaviour
{
    public int tileID; // Unique identifier for matching tiles
    public Vector2Int gridPosition; // Position in the grid

    private Button button;
    private BoardManager boardManager;

    private void Start()
    {
        button = GetComponent<Button>();
        boardManager = FindObjectOfType<BoardManager>();
        button.onClick.AddListener(OnTileClicked);
    }

    void OnTileClicked()
    {
        boardManager.SelectTile(this);
    }
}

