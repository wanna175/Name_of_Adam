using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ClickableSlider : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] public Slider Slider;
    public void OnPointerDown(PointerEventData eventData)
    {
        float sliderWidth = Slider.GetComponent<RectTransform>().rect.width;

        float minPos = Slider.GetComponent<RectTransform>().position.x - sliderWidth / 2f;
        float maxPos = minPos + sliderWidth;
        Slider.value = Mathf.Lerp(Slider.minValue, Slider.maxValue, Mathf.InverseLerp(minPos, maxPos, eventData.position.x));
    }
}
