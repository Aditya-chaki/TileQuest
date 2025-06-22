using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdMovement : MonoBehaviour
{
    public float speed = 2f;
    public SpriteRenderer birdSprite;
    int direction = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime*direction);

        // Destroy bird if it goes far off screen
        if(direction==1)
        {
            if (transform.position.x > Camera.main.ScreenToWorldPoint(new Vector3(Screen.width + 100, 0, 0)).x)
            {
            Destroy(gameObject);
            }
        }
        else if(direction==-1)
        {
            if (transform.position.x < Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).x)
            {
            Destroy(gameObject);
            }
        }
    }

    public void SetDirection(int dir)
    {
        direction = dir;
        if(dir==-1)
        {
            birdSprite.flipX = true;
        }
    }
}
