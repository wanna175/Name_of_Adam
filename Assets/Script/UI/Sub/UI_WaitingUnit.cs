using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_WaitingUnit : MonoBehaviour
{
    private BattleUnit _unit;

    //public void SetUnit(Sprite s)
    //{
        
    //    _Image.enabled = true;
    //    _Image.sprite = s;
        
    //}

    public void SetUnit(BattleUnit unit)
    {
        _unit = unit;
        GetComponent<Image>().sprite = _unit.Data.Image;
    }

    public BattleUnit GetUnit()
    {
        return _unit;
    }
}
