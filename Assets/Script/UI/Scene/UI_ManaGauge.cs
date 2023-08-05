using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_ManaGauge : UI_Scene
{
    [SerializeField] Image _bluemanaIMG;
    [SerializeField] TextMeshProUGUI _currentMana;


    //enum Objects
    //{
    //    Fill,
    //    ManaCostText,
    //}

    //private void Awake()
    //{
    //    Bind<GameObject>(typeof(Objects));
    //}
    
    public void DrawGauge(int max, int current)
    {
        _bluemanaIMG.fillAmount = (float)current / max;
        _currentMana.text = current.ToString();
    }
}
