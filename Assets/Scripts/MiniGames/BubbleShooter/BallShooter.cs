using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BallShooter : MonoBehaviour
{
    Transform rectTransform;
    private Touch theTouch;
    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
       
        if(Input.touchCount>0){
            theTouch = Input.GetTouch(0);
            if(theTouch.phase == TouchPhase.Moved){
            Vector2 pos = theTouch.position;
            float angle = Mathf.Atan2(pos.y - rectTransform.position.y,pos.x -rectTransform.position.x ) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
            rectTransform.rotation = Quaternion.Euler(0, 0, angle);
        }
        }
    }
}
