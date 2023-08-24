using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_WaitingUnit : MonoBehaviour
{
    private BattleUnit _unit;
    [SerializeField] private Image _unitImage;
    [SerializeField] private Image _background;

    private Color32 _enemy = new Color32(130, 123, 56, 60);
    private Color32 _player = new Color32(48, 12, 69, 60);

    public void SetUnit(BattleUnit unit, bool _turned)
    {
        _unit = unit;
        if (unit.Team == Team.Player)
        {
            _unitImage.GetComponent<Image>().sprite = GameManager.Resource.Load<Sprite>($"Arts/Units/Unit_Portrait/" + _unit.DeckUnit.Data.Name + "_Å¸¶ô");
            _background.GetComponent<Image>().color = _player;
        }
        else
        {
            _unitImage.GetComponent<Image>().sprite = GameManager.Resource.Load<Sprite>($"Arts/Units/Unit_Portrait/" + _unit.DeckUnit.Data.Name);
            _background.GetComponent<Image>().color = _enemy;
            _unitImage.transform.eulerAngles += new Vector3(0f, 180f, 0f);
        }

        if(_turned)
        {
            _unitImage.transform.eulerAngles += new Vector3(0f, 180f, 0f);
        }
    }

    public BattleUnit GetUnit()
    {
        return _unit;
    }
}
