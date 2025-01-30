using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Glass : MonoBehaviour
{
    public Stack<Color> liquidStack = new Stack<Color>();
    public int maxCapacity = 3;
    public Image[] liquidSegments;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void AddLiquidAtStart(Color color){
        if (liquidStack.Count < maxCapacity)
        {
            liquidStack.Push(color);
            UpdateVisual();
            return;
        }
        return;
    }

    // Adds a color to the bottle
    public bool AddLiquid(Color color)
    {
        if (liquidStack.Count < maxCapacity && (liquidStack.Count == 0 || liquidStack.Peek() == color))
        {
            liquidStack.Push(color);
            int j=0;
            for (int i = liquidSegments.Length-1; i>=maxCapacity-liquidStack.Count; i--)
            {
                liquidSegments[i].color = liquidStack.ToArray()[j];
                j++;
            }
            return true;
        }
        return false;
    }

    // Removes a color from the bottle
    public Color RemoveLiquid()
    {
        if (liquidStack.Count > 0)
        {
            Color color = liquidStack.Pop();
            //UpdateVisual();
            liquidSegments[maxCapacity-liquidStack.Count-1].color = Color.clear;
            return color;
        }
        return Color.clear; // Empty bottle
    }
    // Updates the visual representation of liquids
    private void UpdateVisual()
    {
        for (int i = 0; i < liquidSegments.Length; i++)
        {
            if (i < liquidStack.Count)
                liquidSegments[i].color = liquidStack.ToArray()[i];
            else
                liquidSegments[i].color = Color.clear; // Clear segment
        }
    }

    // Checks if the bottle is sorted
    public bool IsSorted()
    {
        if (liquidStack.Count == 0) return true;

        Color topColor = liquidStack.Peek();
        foreach (Color color in liquidStack)
        {
            if (color != topColor) return false;
        }
        return liquidStack.Count == maxCapacity;
    }

    public void ClearBottle()
    {
        for(int i=0;i<liquidSegments.Length;i++){
            liquidSegments[i].color = Color.clear;
        }
        liquidStack.Clear();
    }
}
