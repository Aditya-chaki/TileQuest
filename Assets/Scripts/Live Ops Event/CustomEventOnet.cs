using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomEventOnet : MonoBehaviour
{
    public List<int> setGridRow = new List<int>();
    public List<int> setGridCol = new List<int>();
    public List<float> setTime = new List<float>();
    public BoardManager boardManager;
    public GameObject winPopUp;
    bool isComplete = false;
    // Start is called before the first frame update
    void Start()
    {
        int levelIdx = PlayerPrefs.GetInt("EventOnetCurrentLevel");
        boardManager.rows = setGridRow[levelIdx];
        boardManager.cols = setGridCol[levelIdx];
        boardManager.time = setTime[levelIdx];
    }

    // Update is called once per frame
    void Update()
    {

        if(winPopUp.activeInHierarchy && isComplete==false)
        {
            int levelIdx = PlayerPrefs.GetInt("EventOnetCurrentLevel")+1;
            PlayerPrefs.SetInt("EventOnetCurrentLevel",levelIdx);
            isComplete = true;
        }
        
    }
}
