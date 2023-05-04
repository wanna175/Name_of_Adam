using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

public class UI_UnitInfo : UI_Popup
{
    [SerializeField] private UI_UnitCard _unitCard;
    [SerializeField] private GameObject _selectButton;
    [SerializeField] private GameObject _fallGaugePrefab;
    [SerializeField] private GameObject _squarePrefab;

    [SerializeField] private TextMeshProUGUI _unitInfoName;
    [SerializeField] private TextMeshProUGUI _unitInfoCost;
    [SerializeField] private Transform _unitInfoFallGrid;
    [SerializeField] private TextMeshProUGUI _unitInfoStat;
    [SerializeField] private Transform _unitInfoStigmaGrid;
    [SerializeField] private Transform _unitInfoSkillRangeGrid;
    [SerializeField] private TextMeshProUGUI _unitInfoSkillDescrption;
    [SerializeField] private Transform _unitInfoSkillimage;

    private DeckUnit _unit;
    private Selectable _selectable;

    public void SetUnit(DeckUnit unit)
    {
        _unit = unit;
    }

    public void Init(bool select, Selectable selectable)
    {
        _selectable = selectable;
        _selectButton.SetActive(select);

        _unitCard.Set(_unit.Data.Image, _unit.Data.Name, _unit.Stat.ManaCost.ToString());

        _unitInfoName.text = _unit.Data.Name;
        _unitInfoCost.text = _unit.Stat.ManaCost.ToString();

        _unitInfoStat.text = "HP: " + _unit.Stat.HP.ToString() + "\n" +
                                 "Attack: " + _unit.Stat.ATK.ToString() + "\n" +
                                 "Speed: " + _unit.Stat.SPD.ToString();

        _unitInfoSkillDescrption.text = "-";

        for (int i = 0; i < _unit.Stat.FallMaxCount; i++)
        {
            UI_FallGauge fg = GameObject.Instantiate(_fallGaugePrefab, _unitInfoFallGrid).GetComponent<UI_FallGauge>();
            if (i < _unit.Stat.FallCurrentCount)
                fg.Set(true);
            else
                fg.Set(false);

            fg.Init();
        }

        foreach (bool range in _unit.Data.AttackRange)
        {
            Image block = GameObject.Instantiate(_squarePrefab, _unitInfoSkillRangeGrid).GetComponent<Image>();
            if (range)
                block.color = Color.red;
            else
                block.color = Color.grey;
        }
    }

    public void Quit()
    {
        GameManager.UI.ClosePopup();
    }

    public void Select()
    {
        _selectable.OnSelect(_unit);
    }
}