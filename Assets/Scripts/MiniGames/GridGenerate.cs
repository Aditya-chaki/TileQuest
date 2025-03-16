using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class GridGenerate : MonoBehaviour
{
    public Transform parentObj;
    public GameObject objPrefab;
    public Vector2 girdSize;
    public float spaceMultiple = 100;
    public bool generateTheGrid = false;
    // Start is called before the first frame update
    void Awake()
    {
     for(int i=0;i<girdSize.x;i++){
        for(int j=0;j<girdSize.y;j++)
        {
            GameObject block = Instantiate(objPrefab,parentObj);
            block.transform.position = new Vector3(i,j,0);
        }
     }   
    }

    // Update is called once per frame
    void Update()
    {
        if(generateTheGrid==true)
        {
        for(int i=0;i<girdSize.x;i++){
            for(int j=0;j<girdSize.y;j++)
            {
            GameObject block = Instantiate(objPrefab,parentObj);
            block.transform.position = new Vector3(j*spaceMultiple,-i*spaceMultiple,0);
            }
        }    
        generateTheGrid = false;
        }
    }
}
