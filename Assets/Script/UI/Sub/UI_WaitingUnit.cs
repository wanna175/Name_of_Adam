using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_WaitingUnit : MonoBehaviour
{
    private BattleUnit _unit;
    [SerializeField] private Image _unitImage;
    [SerializeField] private Image _background;

    private Color32 _enemy = new Color32(137, 25, 25, 60);
    private Color32 _player = new Color32(28, 62, 129, 60);

    public void SetUnit(BattleUnit unit)
    {
        _unit = unit;
        _unitImage.GetComponent<Image>().sprite = _unit.Data.Image;
        if (unit.Team == Team.Player)
            _background.GetComponent<Image>().color = _player;
        else
            _background.GetComponent<Image>().color = _enemy;
    }

    public BattleUnit GetUnit()
    {
        return _unit;
    }
}
