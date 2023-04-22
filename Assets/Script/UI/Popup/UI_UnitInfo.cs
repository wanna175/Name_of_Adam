using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UI_UnitInfo : UI_Popup
{
    [SerializeField] private Image _imageArea;
    [SerializeField] private GameObject _fallGaugePrefab;

    [SerializeField] private TextMeshProUGUI _unitInfoName;
    [SerializeField] private TextMeshProUGUI _unitInfoCost;
    [SerializeField] private Transform _unitInfoFallGrid;
    [SerializeField] private TextMeshProUGUI _unitInfoStat;
    [SerializeField] private Transform _unitInfoStigmaGrid;
    [SerializeField] private Transform _unitInfoSkillRangeGrid;
    [SerializeField] private TextMeshProUGUI _unitInfoSkillDescrption;
    [SerializeField] private Transform _unitInfoSkillimage;

    private DeckUnit _unit;

    private List<UI_Card> _cardList = new List<UI_Card>();

    public void SetUnit(DeckUnit unit)
    {
        _unit = unit;
    }

    public void init() 
    {
        _imageArea.sprite = _unit.Data.Image;

        _unitInfoName.text = _unit.Data.Name.ToString();
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
    }

    public void Quit()
    {
        GameManager.UI.ClosePopup();
    }
}
