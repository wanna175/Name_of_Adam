using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_WaitingUnit : MonoBehaviour
{
    private BattleUnit _unit;
    [SerializeField] private Image _unitImage;
    [SerializeField] private Image _background;

    public void SetUnit(BattleUnit unit)
    {
        _unit = unit;
        _unitImage.GetComponent<Image>().sprite = _unit.Data.Image;
        if (unit.Team == Team.Player)
            _background.GetComponent<Image>().color = Color.blue;
        else
            _background.GetComponent<Image>().color = Color.red;
    }

    public BattleUnit GetUnit()
    {
        return _unit;
    }
}
