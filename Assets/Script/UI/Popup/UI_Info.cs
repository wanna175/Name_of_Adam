using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Info : UI_Scene
{
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private TextMeshProUGUI _cost;
    [SerializeField] private TextMeshProUGUI _stat;

    [SerializeField] private UI_HPBar _hpBar;
    [SerializeField] private UI_HoverImageBlock _stigama;
    [SerializeField] private Transform _stigamaGrid;

    [SerializeField] private UI_HoverImageBlock _SkillImage;

    [SerializeField] private Transform _rangeGrid;
    [SerializeField] private GameObject _squarePrefab;

    //색상은 UI에서 정해주는대로
    readonly Color goodColor = Color.yellow;
    readonly Color badColor = Color.red;
    readonly string goodColorStr = "yellow";
    readonly string badColorStr = "red";
    readonly string normalColorStr = "white";

    public void SetInfo(BattleUnit battleUnit)
    {
        //배틀 유닛의 경우
        DeckUnit unit = battleUnit.DeckUnit;

        _name.text = unit.Data.Name;
        Team team = battleUnit.Team;

        if (unit.DeckUnitChangedStat.ManaCost > 0)
            _cost.color = badColor;
        else if (unit.DeckUnitChangedStat.ManaCost < 0)
            _cost.color = goodColor;

        _cost.text = unit.DeckUnitTotalStat.ManaCost.ToString();

        //"<color=\"black\"></color>"

        string statText = "HP:     " + "<color=\"currnetHPColor\">" + battleUnit.BattleUnitTotalStat.CurrentHP.ToString() +
                                "</color>" + " / " + battleUnit.BattleUnitTotalStat.MaxHP.ToString() + "\n" +
                                "Attack: " + "<color=\"AttackColor\">" + battleUnit.BattleUnitTotalStat.ATK.ToString() + "</color>" + "\n" +
                                "Speed:  " + "<color=\"SpeedColor\">" + battleUnit.BattleUnitTotalStat.SPD.ToString() + "</color>";
        //일단 작동은 하는데 수정필요 6/28
        //함수화 시켜서 정리하던가 해야함

        if (battleUnit.BattleUnitTotalStat.CurrentHP < battleUnit.BattleUnitTotalStat.MaxHP)
            statText = statText.Replace("currnetHPColor", badColorStr);
        else
            statText = statText.Replace("currnetHPColor", normalColorStr);

        /*
        if (unit.DeckUnitChangedStat.MaxHP > 0 || battleUnit.BattleUnitChangedStat.MaxHP > 0)
            statText = statText.Replace("HPColor", goodColorStr);
        else if (unit.DeckUnitChangedStat.MaxHP < 0 || battleUnit.BattleUnitChangedStat.MaxHP < 0)
            statText = statText.Replace("HPColor", badColorStr);
        else
            statText = statText.Replace("HPColor", normalColorStr);
        */

        if (unit.DeckUnitChangedStat.ATK > 0 || battleUnit.BattleUnitChangedStat.ATK > 0)
            statText = statText.Replace("AttackColor", goodColorStr);
        else if (unit.DeckUnitChangedStat.ATK < 0 || battleUnit.BattleUnitChangedStat.ATK < 0)
            statText = statText.Replace("AttackColor", badColorStr);
        else
            statText = statText.Replace("AttackColor", normalColorStr);

        if (unit.DeckUnitChangedStat.SPD > 0 || battleUnit.BattleUnitChangedStat.SPD > 0)
            statText = statText.Replace("SpeedColor", goodColorStr);
        else if (unit.DeckUnitChangedStat.SPD < 0 || battleUnit.BattleUnitChangedStat.SPD < 0)
            statText = statText.Replace("SpeedColor", badColorStr);
        else
            statText = statText.Replace("SpeedColor", normalColorStr);

        _stat.text = statText;

        _hpBar.SetHPBar(team, null);
        _hpBar.SetFallBar(unit);

        _hpBar.RefreshHPBar((float)battleUnit.BattleUnitTotalStat.CurrentHP / (float)battleUnit.BattleUnitTotalStat.MaxHP);
        _hpBar.RefreshFallGauge(battleUnit.BattleUnitTotalStat.FallCurrentCount);

        unit.SetStigma();
        foreach (Stigma sti in unit.Stigma)
        {
            GameObject.Instantiate(_stigama, _stigamaGrid).GetComponent<UI_HoverImageBlock>().Set(sti.Sprite, sti.Description);
        }

        Sprite attackType;

        if (unit.Data.BehaviorType == BehaviorType.근거리)
            attackType = GameManager.Resource.Load<Sprite>($"Arts/UI/Battle_UI/근거리_아이콘");
        else
            attackType = GameManager.Resource.Load<Sprite>($"Arts/UI/Battle_UI/원거리_아이콘");

        _SkillImage.Set(attackType, unit.Data.Description.Replace("(ATK)", unit.DeckUnitTotalStat.ATK.ToString()));

        foreach (bool range in unit.Data.AttackRange)
        {
            Image block = GameObject.Instantiate(_squarePrefab, _rangeGrid).GetComponent<Image>();
            if (range)
                block.color = Color.red;
            else
                block.color = Color.grey;
        }
    }

    public void SetInfo(DeckUnit unit, Team team)
    {
        //덱 유닛의 경우
        _name.text = unit.Data.Name;

        if (unit.DeckUnitChangedStat.ManaCost > 0)
            _cost.color = badColor;
        else if (unit.DeckUnitChangedStat.ManaCost < 0)
            _cost.color = goodColor;

        _cost.text = unit.DeckUnitTotalStat.ManaCost.ToString();

        //"<color=\"black\"></color>"

        string statText =   "HP:     " + "<color=\"currnetHPColor\">" + unit.DeckUnitTotalStat.CurrentHP.ToString() + 
                                "</color>" + " / " + unit.DeckUnitTotalStat.MaxHP.ToString() + "\n" +
                                "Attack: " + "<color=\"AttackColor\">" + unit.DeckUnitTotalStat.ATK.ToString() + "</color>" + "\n" +
                                "Speed:  " + "<color=\"SpeedColor\">" + unit.DeckUnitTotalStat.SPD.ToString() + "</color>";
        //일단 작동은 하는데 수정필요 6/28
        //함수화 시켜서 정리하던가 해야함

        if (unit.DeckUnitTotalStat.CurrentHP < unit.DeckUnitTotalStat.MaxHP)
            statText = statText.Replace("currnetHPColor", badColorStr);
        else
            statText = statText.Replace("currnetHPColor", normalColorStr);

        /*
        if (unit.DeckUnitChangedStat.MaxHP > 0)
            statText = statText.Replace("HPColor", goodColorStr);
        else if (unit.DeckUnitChangedStat.MaxHP < 0)
            statText = statText.Replace("HPColor", badColorStr);
        else
            statText = statText.Replace("HPColor", normalColorStr);
        */

        if (unit.DeckUnitChangedStat.ATK > 0)
            statText = statText.Replace("AttackColor", goodColorStr);
        else if (unit.DeckUnitChangedStat.ATK < 0)
            statText = statText.Replace("AttackColor", badColorStr);
        else
            statText = statText.Replace("AttackColor", normalColorStr);

        if (unit.DeckUnitChangedStat.SPD > 0)
            statText = statText.Replace("SpeedColor", goodColorStr);
        else if (unit.DeckUnitChangedStat.SPD < 0)
            statText = statText.Replace("SpeedColor", badColorStr);
        else
            statText = statText.Replace("SpeedColor", normalColorStr);

        _stat.text = statText;

        _hpBar.SetHPBar(team, null);
        _hpBar.SetFallBar(unit);

        _hpBar.RefreshHPBar((float)unit.DeckUnitTotalStat.CurrentHP / (float)unit.DeckUnitTotalStat.MaxHP);
        _hpBar.RefreshFallGauge(unit.DeckUnitTotalStat.FallCurrentCount);

        unit.SetStigma();
        foreach (Stigma sti in unit.Stigma)
        {
            GameObject.Instantiate(_stigama, _stigamaGrid).GetComponent<UI_HoverImageBlock>().Set(sti.Sprite, sti.Description);
        }

        Sprite attackType;

        if (unit.Data.BehaviorType == BehaviorType.근거리)
            attackType = GameManager.Resource.Load<Sprite>($"Arts/UI/Battle_UI/근거리_아이콘");
        else
            attackType = GameManager.Resource.Load<Sprite>($"Arts/UI/Battle_UI/원거리_아이콘");

        _SkillImage.Set(attackType, unit.Data.Description.Replace("(ATK)", unit.DeckUnitTotalStat.ATK.ToString()));

        foreach (bool range in unit.Data.AttackRange)
        {
            Image block = GameObject.Instantiate(_squarePrefab, _rangeGrid).GetComponent<Image>();
            if (range)
                block.color = Color.red;
            else
                block.color = Color.grey;
        }
    }

    public void InfoDestroy()
    {
        GameManager.Resource.Destroy(this.gameObject);
    }
}
