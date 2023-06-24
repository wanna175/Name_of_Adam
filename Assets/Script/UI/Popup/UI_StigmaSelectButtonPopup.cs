using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class UI_StigmaSelectButtonPopup : UI_Popup
{
    private Action<Passive> _action;
    [SerializeField] private UI_StigmaSelectButton buttonPrefab;
    [SerializeField] private Transform Grid;

    public void init(Action<Passive> action,  List<Passive> stigmaList)
    {
        _action = action;

        for (int i = 0; i < stigmaList.Count; i++)
        { 
            GameObject.Instantiate(buttonPrefab, Grid).GetComponent<UI_StigmaSelectButton>().init(this, stigmaList[i]);
        }
    }

    public void OnClick(Passive stigma)
    {
        _action.Invoke(stigma);
    }
}
