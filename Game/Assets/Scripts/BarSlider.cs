using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarSlider : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;

    public void SetValue(int value){
        slider.value = value;
        fill.color = gradient.Evaluate(slider.normalizedValue);

    }

    public void SetMaxValue(int value){
        slider.maxValue = value;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
