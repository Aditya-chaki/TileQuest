using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSort : MonoBehaviour
{
    bool isFirstGlass;
    bool isSecondGlass;
    Glass firstBottle;
    Glass secondBottle;
    public Glass[] all_bottles;
    public Color[] waterColors;
    public GameObject extraBottle; 
    Dictionary<Color,int> colorBottleFrq = new Dictionary<Color,int>();

    // Start is called before the first frame update
    void Start()
    {
        isFirstGlass = false;
        isSecondGlass = false;
        for(int i=0;i<waterColors.Length;i++){
            colorBottleFrq.Add(waterColors[i],0);
        }
        RandomAddColor();

    }

    // Update is called once per frame
    void Update()
    {
        CheckWinCondition();
    }

    public void pickGlass(Glass bottle){
        if(isFirstGlass == false && bottle.IsSorted()==false){
            isFirstGlass = true;
            firstBottle = bottle;
        }
       else if(isFirstGlass == true){
            isSecondGlass = true;
            secondBottle = bottle;
            if(secondBottle.AddLiquid(firstBottle.liquidStack.Peek())){
                firstBottle.RemoveLiquid();
            }
            else{
            isFirstGlass = false;
            firstBottle = null;
            secondBottle = null;
            }
            isFirstGlass = false;
            firstBottle = null;
            secondBottle = null;
        }

    }

    private void CheckWinCondition()
    {
        foreach (Glass bottle in all_bottles)
        {
            if (!bottle.IsSorted()) return;
        }

        Debug.Log("Game Won!");
    }

    int loopBreak = 0;

    private void RandomAddColor(){
        loopBreak = 0;
        for(int i=0;i<all_bottles.Length-1;i++){
            int j=0;
            while(j<3){
                loopBreak++;
            if(loopBreak>1000){
                return;
            }

            int idx = Random.Range(0,waterColors.Length-1);
            if(colorBottleFrq[waterColors[idx]]<3){
            all_bottles[i].AddLiquidAtStart(waterColors[idx]);
            colorBottleFrq[waterColors[idx]]++;
            j++;
            }
            }
        }

    }

    public void extraBottlePowerUp(){
        extraBottle.SetActive(true);
    }

    public void resetBottle(){
        for(int i=0;i<all_bottles.Length;i++){
            all_bottles[i].ClearBottle();
        }
        for(int i=0;i<waterColors.Length;i++){
            colorBottleFrq[waterColors[i]]=0;
        }
        RandomAddColor();
    }
}
