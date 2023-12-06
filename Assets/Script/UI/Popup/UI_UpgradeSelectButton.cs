using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UI_UpgradeSelectButton : UI_Popup
{
    private UpgradeSceneController _uc;
    public void Init(UpgradeSceneController uc)
    {
        _uc = uc; 
    }

    public void OnClick(int select)
    {
        _uc.OnUpgradeSelect(select);
    }
    public void QuitBtn()
    {
        this.transform.SetAsFirstSibling();
        this.gameObject.SetActive(false);
    }

}
