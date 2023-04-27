using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_FallGauge : UI_Base
{
    [SerializeField] Sprite FallGauge_Full;
    [SerializeField] Sprite FallGauge_Empty;
    public bool IsOn = false;
    
    private void Start()
    {
        Init();
    }

    public void Init()
    {
        if (IsOn)
        {
            GetComponent<Image>().sprite = FallGauge_Full;
        }
        else
        {
            GetComponent<Image>().sprite = FallGauge_Empty;
        }
    }

    public void Set(bool bo)
    {
        IsOn = bo;
    }
}
