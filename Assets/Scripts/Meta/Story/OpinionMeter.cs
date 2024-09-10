using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpinionMeter : MonoBehaviour
{
    public Slider opinionSlider;
    public Character Op;

    void Start()
    {

        opinionSlider.value = Op.GetOpinion();
        opinionSlider.interactable = false;
    }

    void Update()
    {
        
        if (opinionSlider.value != Op.GetOpinion())
        {
            opinionSlider.value = Op.GetOpinion();
        }
    }
}
