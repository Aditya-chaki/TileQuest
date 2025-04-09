using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTouch : MonoBehaviour
{

    private float width;
    private float height;
    public Camera cam;
    public GameObject ball;
    // Start is called before the first frame update
    void Start()
    {
        width = (float)Screen.width;
        height = (float)Screen.height;
    }

    // Update is called once per frame
    void Update()
    {
        Touch touch;
        if(Input.touchCount>0&&false){
            touch = Input.GetTouch(0);
            if(touch.phase == TouchPhase.Ended){    
                Vector2 pos = touch.position;
                pos.x = (pos.x) / width;
                pos.y = (pos.y) / height;
                Vector2 point = cam.ScreenToWorldPoint(pos);    
                RaycastHit2D hit = Physics2D.Raycast(pos,Vector2.zero);
                Debug.Log(hit.collider);
                if(hit.collider)
                {
                    Instantiate(ball,hit.point,ball.transform.rotation);
                }
                else
                    Instantiate(ball,point,ball.transform.rotation);   
            }
        }
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
                    Instantiate(ball,hit.point,ball.transform.rotation);
                }
                else
                    Instantiate(ball,pos2D,ball.transform.rotation);  
            }
        }
        
              
    }
}
