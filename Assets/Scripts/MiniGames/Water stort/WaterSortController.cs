using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSortController : MonoBehaviour
{
    public BottleController FirstBottle;
    public BottleController SecondBottle;
    private Touch touch;
    private float width;
    private float height;
    public Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        width = (float)Screen.width;
        height = (float)Screen.height;
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
}
