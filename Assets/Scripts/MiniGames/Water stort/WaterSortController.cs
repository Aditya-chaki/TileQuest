using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSortController : MonoBehaviour
{
    public BottleController FirstBottle;
    public BottleController SecondBottle;
    public BottleController[] AllBottles;
    public GameObject extraBottle;
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
        for(int i=0;i<waterColors.Length;i++){
            colorBottleFrq.Add(waterColors[i],0);
        }
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
    {   int loopBreak = 0;
        for(int i=0;i<AllBottles.Length-1;i++)
        {
            Color[] newColors = new Color[4]; 
            int j=0;
            while(j<4){
                if(loopBreak>5000){
                    return;
                }
                loopBreak++;
            int idx = Random.Range(0,waterColors.Length-1);
            if(colorBottleFrq[waterColors[idx]]<4){
            newColors[j]=waterColors[idx];
            colorBottleFrq[waterColors[idx]]++;
            j++;
            }
            }
            AllBottles[i].bottlesColor = newColors;
            AllBottles[i].UpdateColorsOnShader();
            }
    }

    public void ExtraBottle()
    {
        extraBottle.SetActive(true);
    }

    public void ResetBottle()
    {
          for(int i=0;i<waterColors.Length;i++){
            colorBottleFrq[waterColors[i]]=0;
        }
        AssignColors();
    }
    
}
