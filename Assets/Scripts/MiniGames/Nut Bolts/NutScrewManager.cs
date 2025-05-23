using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NutScrewManager : MonoBehaviour
{
    private Touch touch;
    private float width;
    private float height;
    public Camera cam;
    public LayerMask screwLayer;
    public GameObject ball;
    public HingeJoint2D joint;
    GameObject screw;
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
                if(hit.collider)
                {
                    if(screw==null&&hit.collider.tag=="Screw")
                    {
                        screw = hit.collider.gameObject;
                        //screw.SetActive(false);
                    }

                    if(screw && hit.collider.tag=="DropArea")
                    {
                        screw.transform.position = hit.collider.transform.position;
                        joint.enabled = false;
                        screw =null;
                    }
                }                  
                
            }
        }
        
              
    }
}
