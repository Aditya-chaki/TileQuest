using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSortController : MonoBehaviour
{
    [Range(1,3)]
    public int Difficulty = 1;

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
    Dictionary<Color,int> colorBottleFrq = new Dictionary<Color,int>();
    // Start is called before the first frame update
    void Start()
    {
        width = (float)Screen.width;
        height = (float)Screen.height;
        DifficultyBottlesHolder[Difficulty-1].SetActive(true);
        AllBottles = DifficultyBottlesHolder[Difficulty-1].GetComponentsInChildren<BottleController>();

        for(int i=0;i<Difficulty*2;i++){
            colorBottleFrq.Add(waterColors[i],0);
        }
        Debug.Log("Start Bottles"+AllBottles.Length);
        AssignColors();
    }

    // Update is called once per frame
    void Update()
    {

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
                Debug.Log(hit.collider);
                if(hit.collider!=null){
                    if(hit.collider.GetComponent<BottleController>()!=null)
                    {
                        if(FirstBottle==null){
                            FirstBottle = hit.collider.GetComponent<BottleController>();
                            if(FirstBottle.numberOfColorsInBottle==0)
                            {
                                FirstBottle = null;
                            }
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
                                    FirstBottle = null;
                                    SecondBottle = null;
                                }
                                else
                                {
                                    FirstBottle = null;
                                    SecondBottle = null;
                                }
                                
                            }
                        }
                    }
                }
            }
        }
    }

    
    void AssignColors()
    {   
        
        for(int i=0;i<AllBottles.Length-2;i++)
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
            //Debug.Log(j);
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
    }

    public void ResetBottle()
    {
          for(int i=0;i<Difficulty*2;i++){
            colorBottleFrq[waterColors[i]]=0;
        }
        for(int i=0;i<AllBottles.Length-2;i++){
            AllBottles[i].ResetBottle(4);
        }
        Debug.Log("ResetBottles "+AllBottles.Length);
        AllBottles[AllBottles.Length-1].ResetBottle(0);
        AllBottles[AllBottles.Length-2].ResetBottle(0);
        AssignColors();
    }
    
}
