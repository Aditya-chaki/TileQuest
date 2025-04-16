using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottleController : MonoBehaviour
{
    public Color[] bottlesColor;
    public SpriteRenderer bottleMask;
    public SpriteRenderer bottleSprite;
    public AnimationCurve ScaleRotationMulti;
    public AnimationCurve FillAlount;
    public AnimationCurve RotationSpeedMulti;

    public float[] fillAmounts;
    public float[] rotationValues;

    private int rotationIndex = 0;

    [Range(0,4)]
    public int numberOfColorsInBottle = 4;
    public Color topColor;
    public int numberOfTopColorLayers = 1;

    public BottleController bottleController;
    private int numberOfColorToTransfer = 0;

    public Transform leftRotationPoint;
    public Transform rightRotationPoint;
    private Transform choseRotationPoint;
    public float directionMulti = 1;

    public LineRenderer lineRenderer;

    Vector3 originalPosition;
    Vector3 startPosition;
    Vector3 endPosition;
    // Start is called before the first frame update
    void Start()
    {
        bottleMask.material.SetFloat("_FillAmount",fillAmounts[numberOfColorsInBottle]);
        UpdateColorsOnShader();
        UpdateTopColorValues();
        originalPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        

    }
    public void ResetBottle(int num)
    {
        numberOfColorsInBottle = num;
        bottleMask.material.SetFloat("_FillAmount",fillAmounts[numberOfColorsInBottle]);
        UpdateColorsOnShader();
        UpdateTopColorValues();
    }

    public void StartColorTransfer()
    {
        
        ChoseRotationPointAndDirection();
        numberOfColorToTransfer = Mathf.Min(numberOfTopColorLayers,4-bottleController.numberOfColorsInBottle);
        //numberOfColorToTransfer = 1;
        Debug.Log(numberOfColorToTransfer+" "+gameObject.name);
        Color currentTopColor = topColor;
        for (int i = 0; i < numberOfColorToTransfer; i++) 
        {
        if (!bottlesColor[numberOfColorsInBottle - 1 - i].Equals(currentTopColor)) {
        Debug.LogError("Attempted to transfer multiple layers of different colors!");
        return; // Prevent incorrect transfer
            }
        }
        for(int i=0;i<numberOfColorToTransfer;i++){
           bottleController.bottlesColor[bottleController.numberOfColorsInBottle+i] = topColor;
           Debug.Log("Transfer"+i);
           }
        bottleController.UpdateColorsOnShader();
        CalculateRotationIndex(4 - bottleController.numberOfColorsInBottle);
        transform.GetComponent<SpriteRenderer>().sortingOrder +=2;
        bottleMask.sortingOrder+=2;
        StartCoroutine(MoveBottle());

    }
  

    public void UpdateColorsOnShader(){
        bottleMask.material.SetColor("_Color1",bottlesColor[0]);
        bottleMask.material.SetColor("_Color2",bottlesColor[1]);
        bottleMask.material.SetColor("_Color3",bottlesColor[2]);
        bottleMask.material.SetColor("_Color4",bottlesColor[3]);
        
    }


    IEnumerator MoveBottle(){
        if(directionMulti==1)
            {
               bottleSprite.flipX = true; 
               bottleMask.flipX = true;
            }
        startPosition = originalPosition;
        if(choseRotationPoint==leftRotationPoint)
        {
            endPosition = bottleController.rightRotationPoint.position;
        }
        else
        {
            endPosition = bottleController.leftRotationPoint.position;
        }
        float t=0;
        while(t<=1)
        {
            transform.position = Vector3.Lerp(startPosition,endPosition,t);
            t += Time.deltaTime*2;
            yield return new WaitForEndOfFrame();
        }
        transform.position = endPosition;
        StartCoroutine(RotateBottle());
    }

    IEnumerator MoveBottleBAck(){
        startPosition = transform.position;
        endPosition = originalPosition;
        
        float t=0;
        while(t<=1)
        {
            transform.position = Vector3.Lerp(startPosition,endPosition,t);
            t += Time.deltaTime*2;
            yield return new WaitForEndOfFrame();
        }
        transform.position = endPosition;
        if(directionMulti==1)
        {
               bottleSprite.flipX = false; 
               bottleMask.flipX  = false;
        }
        transform.GetComponent<SpriteRenderer>().sortingOrder -=2;
        bottleMask.sortingOrder-=2;
    }

    public float timeToRotate = 1.0f;

    IEnumerator RotateBottle(){
        float t=0;
        float lerpValue;
        float angleValue;
        float lastAngleValue = 0.0f;
        while(t<timeToRotate){
            lerpValue = t/timeToRotate;
            angleValue = Mathf.Lerp(0.0f,rotationValues[rotationIndex],lerpValue);
            //transform.eulerAngles = new Vector3(0,0,angleValue);
            transform.RotateAround(choseRotationPoint.position,Vector3.forward*directionMulti,lastAngleValue-angleValue);

            bottleMask.material.SetFloat("_SRM",ScaleRotationMulti.Evaluate(angleValue));

            if(fillAmounts[numberOfColorsInBottle]>FillAlount.Evaluate(angleValue)+0.005f)
            {
                if(lineRenderer.enabled == false)
                {
                 
                    lineRenderer.enabled = true;
                    lineRenderer.startColor = topColor;
                    lineRenderer.endColor = topColor;
                    lineRenderer.SetPosition(0,choseRotationPoint.position);
                    lineRenderer.SetPosition(1,choseRotationPoint.position-Vector3.up*1.45f);
                    
                }
            bottleMask.material.SetFloat("_FillAmount",FillAlount.Evaluate(angleValue));
            bottleController.FillUp(FillAlount.Evaluate(lastAngleValue)- FillAlount.Evaluate(angleValue));
            }
            
            t += Time.deltaTime*RotationSpeedMulti.Evaluate(angleValue);
            lastAngleValue = angleValue;
            yield return new WaitForEndOfFrame();
        }
        
        angleValue = directionMulti*rotationValues[rotationIndex];
        //transform.eulerAngles = new Vector3(0,0,angleValue);
        bottleMask.material.SetFloat("_SRM",ScaleRotationMulti.Evaluate(angleValue));
        bottleMask.material.SetFloat("_FillAmount",FillAlount.Evaluate(angleValue));

        numberOfColorsInBottle-=numberOfColorToTransfer;
        bottleController.numberOfColorsInBottle += numberOfColorToTransfer;
        lineRenderer.enabled = false;
        StartCoroutine(RotateBottleBack());
    }

    IEnumerator RotateBottleBack(){
        float t=0;
        float lerpValue;
        float angleValue;
        float lastAngleValue = directionMulti*rotationValues[rotationIndex];
        while(t<timeToRotate){
            lerpValue = t/timeToRotate;
            angleValue = Mathf.Lerp(directionMulti*rotationValues[rotationIndex],0.0f,lerpValue);
            bottleMask.material.SetFloat("_SRM",ScaleRotationMulti.Evaluate(angleValue));
            //transform.eulerAngles = new Vector3(0,0,angleValue);
            transform.RotateAround(choseRotationPoint.position,Vector3.forward,lastAngleValue-angleValue);
            lastAngleValue = angleValue;
            t += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        UpdateTopColorValues();
        angleValue = 0.0f;
        transform.eulerAngles = new Vector3(0,0,angleValue);
        bottleMask.material.SetFloat("_SRM",ScaleRotationMulti.Evaluate(angleValue));

        StartCoroutine(MoveBottleBAck());
    }

   public void UpdateTopColorValues(){

        if(numberOfColorsInBottle!=0){
            numberOfTopColorLayers = 1;
            topColor = bottlesColor[numberOfColorsInBottle-1];

            if(numberOfColorsInBottle == 4){
                if(bottlesColor[3].Equals(bottlesColor[2])){
                    numberOfTopColorLayers = 2;
                    if(bottlesColor[2].Equals(bottlesColor[1])){
                    numberOfTopColorLayers = 3;    
                    
                    if(bottlesColor[1].Equals(bottlesColor[0])){
                    numberOfTopColorLayers = 4;    
                    }
                    }
                }
            }

            else if(numberOfColorsInBottle == 3){
                if(bottlesColor[2].Equals(bottlesColor[1])){
                    numberOfTopColorLayers = 2;
                    if(bottlesColor[1].Equals(bottlesColor[0])){
                    numberOfTopColorLayers = 3;    
                    }
                }
            }

            else if(numberOfColorsInBottle == 2){
                if(bottlesColor[1].Equals(bottlesColor[0])){
                    numberOfTopColorLayers = 2;
                    }
                }

            
   
             rotationIndex=3-(numberOfColorsInBottle-numberOfTopColorLayers);    
            }
        
    }

    public bool FillBottleCheck(Color colorToCheck)
    {
        if(numberOfColorsInBottle==0){
            return true;
        }
        else if(numberOfColorsInBottle==4)
        {
                return false;
        }
        else
        {
            if(numberOfColorsInBottle==4){
                return false;
            }
            else
            {
                if(topColor.Equals(colorToCheck))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }

    private void CalculateRotationIndex(int numberEmptySpaceInSecondBottle)
    {
        rotationIndex = 3-(numberOfColorsInBottle-Mathf.Min(numberEmptySpaceInSecondBottle,numberOfTopColorLayers));
    }

    private void FillUp(float fillAmountToAdd)
    {
        bottleMask.material.SetFloat("_FillAmount",bottleMask.material.GetFloat("_FillAmount")+fillAmountToAdd);
    }

    private void ChoseRotationPointAndDirection()
    {
       
        if(transform.position.x>bottleController.transform.position.x)
        {
            choseRotationPoint = leftRotationPoint;
            directionMulti = -1.0f;
        }
        else
        {
            choseRotationPoint = rightRotationPoint;
            directionMulti = 1.0f;
        }
    }

    bool isColorSorted = false;
    public bool IsSorted()
    {
        isColorSorted = isColorMatch();   
        if(numberOfTopColorLayers==4||numberOfTopColorLayers==0||numberOfColorsInBottle==0||isColorSorted)
        {   
            return true;
        }
         
        return false;    
    }

    bool isColorMatch()
    {
        Color temp = bottlesColor[0];
        foreach(Color color in bottlesColor)
        {
            if(temp!=color)
                return false;
    
        }
        return true;
    }
}
