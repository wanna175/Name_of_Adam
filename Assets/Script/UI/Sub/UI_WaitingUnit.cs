using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_WaitingUnit : MonoBehaviour
{
    private BattleUnit _unit;
    [SerializeField] private Image _unitImage;
    [SerializeField] private Image _background;

    private Color32 _enemy = new(130, 123, 56, 60);
    private Color32 _player = new(48, 12, 69, 60);

    public void SetUnit(BattleUnit unit)
    {
        _unit = unit;
        if (unit.Team == Team.Player)
        {
            _unitImage.sprite = unit.Data.CorruptPortraitImage;
            _background.color = _player;
        }
        else
        {
            _unitImage.sprite = unit.Data.PortraitImage;
            _background.color = _enemy;

            _unitImage.transform.eulerAngles += new Vector3(0f, 180f, 0f);
        }
    }

    public BattleUnit GetUnit() => _unit;
}
