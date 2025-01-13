using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Savior_UI : MonoBehaviour
{
    [SerializeField] private GameObject _hover;
    
    private UI_Info _hoverInfo;
    private BattleUnit _unit;

    public void Init(BattleUnit unit)
    {
        _unit = unit;
    }

    private void OnMouseEnter()
    {
        _hoverInfo = BattleManager.BattleUI.ShowInfo();
        _hoverInfo.SetInfo(_unit);
    }

    private void OnMouseExit()
    {
        if (_hoverInfo != null)
        {
            BattleManager.BattleUI.CloseInfo(_hoverInfo);
        }
    }
}
