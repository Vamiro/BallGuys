using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderVisualizerComponent : MonoBehaviour
{
    [SerializeField] public Slider slider;
    [SerializeField] private TMP_Text valueText;

    private void Start()
    {
        slider.onValueChanged.AddListener(OnChangeSlider);
    }

    public float SetSliderValue(float value)
    {
        slider.value = value;
        valueText.text = value.ToString("F2");
        return value;
    }

    public void SetSliderValueText(string text)
    {
        valueText.text = text;
    }

    private void OnChangeSlider(float value)
    {
        valueText.text = value.ToString("F2");
    }

    private void OnDestroy()
    {
        slider.onValueChanged.RemoveListener(OnChangeSlider);
    }
}
