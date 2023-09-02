using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

public class UI_UnitInfo : UI_Popup
{
    [SerializeField] private GameObject _selectButton;
    [SerializeField] private GameObject _fallGaugePrefab;
    [SerializeField] private GameObject _stigmaPrefab;
    [SerializeField] private GameObject _squarePrefab;

    [SerializeField] private TextMeshProUGUI _unitInfoName;
    [SerializeField] private Transform _unitInfoFallGrid;
    [SerializeField] private TextMeshProUGUI _unitInfoStat;
    [SerializeField] private Transform _unitInfoStigmaGrid;
    [SerializeField] private Transform _unitInfoSkillRangeGrid;
    [SerializeField] private Transform _unitAnimation;

    private DeckUnit _unit;
    private Action<DeckUnit> _onSelect;

    public void SetUnit(DeckUnit unit)
    {
        _unit = unit;
    }

    public void Init(Action<DeckUnit> onSelect=null)
    {
        _onSelect = onSelect;
        _selectButton.SetActive(onSelect != null);
        _unitInfoName.text = _unit.Data.Name;

        _unitInfoStat.text =    "HP:\t" + _unit.DeckUnitTotalStat.MaxHP.ToString() + "\n" +
                                    "Cost:\t" + _unit.DeckUnitTotalStat.ManaCost.ToString() + "\n" +
                                    "Attack:\t" + _unit.DeckUnitTotalStat.ATK.ToString() + "\n" +
                                    "Speed:\t" + _unit.DeckUnitTotalStat.SPD.ToString();

        for (int i = 0; i < _unit.DeckUnitTotalStat.FallMaxCount; i++)
        {
            UI_FallGauge fg = GameObject.Instantiate(_fallGaugePrefab, _unitInfoFallGrid).GetComponent<UI_FallGauge>();
            if (i < _unit.DeckUnitTotalStat.FallCurrentCount)
                fg.Set(true);
            else
                fg.Set(false);

            fg.Init();
        }

        foreach (Stigma sti in _unit.GetStigma())
        {
            GameObject.Instantiate(_stigmaPrefab, _unitInfoStigmaGrid).GetComponent<UI_HoverImageBlock>().Set(sti.Sprite, sti.Description);
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

    public void SetAnimation()
    {
        AnimationClip clip = Resources.Load<AnimationClip>("Arts/EffectAnimation/VisualEffect/UnitSpawnBackEffect");
        GameManager.VisualEffect.StartVisualEffect(clip, new Vector3(0f, 3.5f, 0f));
    }



    public void Quit()
    {
        GameManager.UI.ClosePopup();
    }

    public void Select()
    {
        _onSelect(_unit);
    }
}