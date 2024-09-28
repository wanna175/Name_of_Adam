using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Info : UI_Scene
{
    [SerializeField] private Image _unitImage;

    [SerializeField] private TextMeshProUGUI _name;

    [SerializeField] private Image _insignia;

    [SerializeField] private Image _apostleInsignia;
    [SerializeField] private Image _eliteInsignia;
    [SerializeField] private Image _bossInsignia;

    [SerializeField] private TextMeshProUGUI _statAttack;
    [SerializeField] private TextMeshProUGUI _statSpeed;
    [SerializeField] private TextMeshProUGUI _statCost;

    [SerializeField] private GameObject _statDarkEssence;
    [SerializeField] private TextMeshProUGUI _statDarkEssenceCost;

    [SerializeField] private TextMeshProUGUI _statAttackChange;
    [SerializeField] private TextMeshProUGUI _statSpeedChange;
    [SerializeField] private TextMeshProUGUI _statCostChange;

    [SerializeField] private UI_HPBar _hpBar;
    [SerializeField] private TextMeshProUGUI _hpText;

    [SerializeField] private Transform _rangeGrid;
    [SerializeField] private TextMeshProUGUI _attackType;
    [SerializeField] private GameObject _squarePrefab;

    [SerializeField] private Transform _unitInfoFallGrid;

    [SerializeField] private Transform _buffDescriptionGrid;
    [SerializeField] private GameObject _buffDescriptionPrefab;

    [SerializeField] private Transform _stigmaGrid;
    [SerializeField] private Transform _stigmaDescriptionGrid;
    [SerializeField] private GameObject _stigmaDescriptionPrefab;
    [SerializeField] private GameObject _stigmaDescriptionFirstImage;
    [SerializeField] private GameObject _stigmaDescriptionLastImage;

    //색상은 UI에서 정해주는대로
    readonly Color _upColor = new(1f, 0.22f, 0.22f);
    readonly Color _downColor = new(0.35f, 0.35f, 1f);

    readonly Color _attackRangeColor = new(0.6f, 0.05f, 0.05f);
    readonly Color _unitRangeColor = new(0.77f, 0.45f, 0.45f);

    readonly Color _playerInsigniaColor = new(0.35f, 0.09f, 0.05f);
    readonly Color _enemyInsigniaColor = new(0.54f, 0.5f, 0.34f);

    public void SetInfo(BattleUnit battleUnit)
    {
        //배틀 유닛의 경우
        DeckUnit unit = battleUnit.DeckUnit;

        _name.text = unit.Data.Name;
        Team team = battleUnit.Team;

        if (battleUnit.Team == Team.Player)
        {
            _unitImage.sprite = unit.Data.CorruptDiaPortraitImage;
            _insignia.color = _playerInsigniaColor;
        }
        else
        {
            _unitImage.sprite = unit.Data.DiaPortraitImage;
            _insignia.color = _enemyInsigniaColor;
        }

        _apostleInsignia.gameObject.SetActive(unit.Data.Rarity == Rarity.Original);
         _eliteInsignia.gameObject.SetActive(unit.Data.Rarity == Rarity.Elite);
        _bossInsignia.gameObject.SetActive(unit.Data.Rarity == Rarity.Boss);

        string hpText = battleUnit.BattleUnitTotalStat.CurrentHP.ToString() + "/" + battleUnit.BattleUnitTotalStat.MaxHP.ToString();

        _statDarkEssence.SetActive(battleUnit.Data.DarkEssenseCost > 0);
        _statDarkEssenceCost.text = battleUnit.Data.DarkEssenseCost.ToString();

        _statAttack.text = battleUnit.BattleUnitTotalStat.ATK.ToString();
        _statSpeed.text = battleUnit.BattleUnitTotalStat.SPD.ToString();
        _statCost.text = battleUnit.BattleUnitTotalStat.ManaCost.ToString();

        _statAttackChange.text = (unit.DeckUnitChangedStat.ATK + battleUnit.BattleUnitChangedStat.ATK > 0) ? "+" : "";
        if (unit.DeckUnitChangedStat.ATK + battleUnit.BattleUnitChangedStat.ATK != 0)
        {
            _statAttackChange.text += (unit.DeckUnitChangedStat.ATK + battleUnit.BattleUnitChangedStat.ATK).ToString();
            _statAttackChange.color = (unit.DeckUnitChangedStat.ATK + battleUnit.BattleUnitChangedStat.ATK > 0) ? _upColor : _downColor;
        }

        _statSpeedChange.text = (unit.DeckUnitChangedStat.SPD + battleUnit.BattleUnitChangedStat.SPD > 0) ? "+" : "";
        if (unit.DeckUnitChangedStat.SPD + battleUnit.BattleUnitChangedStat.SPD != 0)
        {
            _statSpeedChange.text += (unit.DeckUnitChangedStat.SPD + battleUnit.BattleUnitChangedStat.SPD).ToString();
            _statSpeedChange.color = (unit.DeckUnitChangedStat.SPD + battleUnit.BattleUnitChangedStat.SPD > 0) ? _upColor : _downColor;
        }

        _statCostChange.text = (unit.DeckUnitChangedStat.ManaCost + battleUnit.BattleUnitChangedStat.ManaCost > 0) ? "+" : "";
        if (unit.DeckUnitChangedStat.ManaCost + battleUnit.BattleUnitChangedStat.ManaCost != 0)
        {
            _statCostChange.text += (unit.DeckUnitChangedStat.ManaCost + battleUnit.BattleUnitChangedStat.ManaCost).ToString();
            _statCostChange.color = (unit.DeckUnitChangedStat.ManaCost + battleUnit.BattleUnitChangedStat.ManaCost > 0) ? _downColor : _upColor;
        }

        _hpText.text = hpText;

        _hpBar.SetHPBar(team);
        _hpBar.SetFallBar(unit);

        _hpBar.RefreshHPBar((float)battleUnit.BattleUnitTotalStat.CurrentHP / (float)battleUnit.BattleUnitTotalStat.MaxHP);
        _hpBar.RefreshFallBar(battleUnit.Fall.GetCurrentFallCount(), FallAnimMode.Off);

        foreach (UI_Buff ui_buff in battleUnit._hpBar.BuffBlockList)
        {
            UI_BuffDescription bd = GameObject.Instantiate(_buffDescriptionPrefab, _buffDescriptionGrid).GetComponent<UI_BuffDescription>();
            bd.SetBuff(ui_buff.BuffInBlock);
        }

        if (battleUnit.StigmaList.Count > 0)
        {
            foreach (Stigma stigma in battleUnit.StigmaList)
            {
                UI_StigmaDescription sd = GameObject.Instantiate(_stigmaDescriptionPrefab, _stigmaDescriptionGrid).GetComponent<UI_StigmaDescription>();
                sd.SetStigma(stigma);
            }

            _stigmaDescriptionFirstImage.SetActive(true);
            _stigmaDescriptionLastImage.SetActive(true);
            _stigmaDescriptionLastImage.transform.SetSiblingIndex(_stigmaDescriptionGrid.childCount - 1);
        }

        List<int> rangeIntegerList = new();

        foreach (Vector2 vec in battleUnit.GetAttackRange())
        {
            int rangeInteger = 27 + (int)vec.x + (int)vec.y * 11;
            rangeIntegerList.Add(rangeInteger);
        }

        for (int i = 0; i < unit.Data.AttackRange.Length; i++)
        {
            Image block = GameObject.Instantiate(_squarePrefab, _rangeGrid).GetComponent<Image>();
            if (i == 27)
                block.color = _unitRangeColor;
            else if (rangeIntegerList.Contains(i))
                block.color = _attackRangeColor;
            else
                block.color = Color.grey;
        }

        _attackType.text = GameManager.Locale.GetLocalizedBattleScene(unit.Data.UnitAttackType.ToString());

        StartCoroutine(nameof(UpdateUIWithDelay));
    }

    public void SetInfo(DeckUnit unit, Team team)
    {
        //덱 유닛의 경우
        _name.text = unit.Data.Name;

        _unitImage.sprite = unit.Data.Image;

        if (team == Team.Player)
        {
            _unitImage.sprite = unit.Data.CorruptDiaPortraitImage;
            _insignia.color = _playerInsigniaColor;
        }
        else
        {
            _unitImage.sprite = unit.Data.DiaPortraitImage;
            _insignia.color = _enemyInsigniaColor;
        }

        _apostleInsignia.gameObject.SetActive(unit.Data.Rarity == Rarity.Original);
        _eliteInsignia.gameObject.SetActive(unit.Data.Rarity == Rarity.Elite);
        _bossInsignia.gameObject.SetActive(unit.Data.Rarity == Rarity.Boss);

        string hpText = unit.DeckUnitTotalStat.CurrentHP.ToString() + "/" + unit.DeckUnitTotalStat.MaxHP.ToString();

        _statDarkEssence.SetActive(unit.Data.DarkEssenseCost > 0);
        _statDarkEssenceCost.text = unit.Data.DarkEssenseCost.ToString();

        _statAttack.text = unit.DeckUnitTotalStat.ATK.ToString();
        _statSpeed.text = unit.DeckUnitTotalStat.SPD.ToString();
        _statCost.text = unit.DeckUnitTotalStat.ManaCost.ToString();

        _statAttackChange.text = (unit.DeckUnitChangedStat.ATK > 0) ? "+" : "";
        if (unit.DeckUnitChangedStat.ATK != 0)
        {
            _statAttackChange.text += (unit.DeckUnitChangedStat.ATK).ToString();
            _statAttackChange.color = (unit.DeckUnitChangedStat.ATK > 0) ? _upColor : _downColor;
        }

        _statSpeedChange.text = (unit.DeckUnitChangedStat.SPD > 0) ? "+" : "";
        if (unit.DeckUnitChangedStat.SPD != 0)
        {
            _statSpeedChange.text += (unit.DeckUnitChangedStat.SPD).ToString();
            _statSpeedChange.color = (unit.DeckUnitChangedStat.SPD > 0) ? _upColor : _downColor;
        }

        _statCostChange.text = (unit.DeckUnitChangedStat.ManaCost > 0) ? "+" : "";
        if (unit.DeckUnitChangedStat.ManaCost != 0)
        {
            _statCostChange.text += (unit.DeckUnitChangedStat.ManaCost).ToString();
            _statCostChange.color = (unit.DeckUnitChangedStat.ManaCost > 0) ? _downColor : _upColor;
        }

        _hpText.text = hpText;

        _hpBar.SetHPBar(team);
        _hpBar.SetFallBar(unit);

        _hpBar.RefreshHPBar((float)unit.DeckUnitTotalStat.CurrentHP / (float)unit.DeckUnitTotalStat.MaxHP);
        _hpBar.RefreshFallBar(unit.DeckUnitTotalStat.FallCurrentCount, FallAnimMode.Off);

        if (unit.GetStigma().Count > 0)
        {
            foreach (Stigma stigma in unit.GetStigma())
            {
                UI_StigmaDescription sd = GameObject.Instantiate(_stigmaDescriptionPrefab, _stigmaDescriptionGrid).GetComponent<UI_StigmaDescription>();
                sd.SetStigma(stigma);
            }

            _stigmaDescriptionFirstImage.SetActive(true);
            _stigmaDescriptionLastImage.SetActive(true);
            _stigmaDescriptionLastImage.transform.SetSiblingIndex(_stigmaDescriptionGrid.childCount - 1);
        }

        for (int i = 0; i < unit.Data.AttackRange.Length; i++)
        {
            Image block = GameObject.Instantiate(_squarePrefab, _rangeGrid).GetComponent<Image>();
            if (i == 27)
                block.color = _unitRangeColor;
            else if (unit.Data.AttackRange[i])
                block.color = _attackRangeColor;
            else
                block.color = Color.grey;
        }

        _attackType.text = GameManager.Locale.GetLocalizedBattleScene(unit.Data.UnitAttackType.ToString());

        StartCoroutine(nameof(UpdateUIWithDelay));
    }

    IEnumerator UpdateUIWithDelay()
    {
        yield return null;

        VerticalLayoutGroup strigmaGroup = _stigmaGrid.GetComponent<VerticalLayoutGroup>();
        strigmaGroup.childForceExpandWidth = false;
        LayoutRebuilder.ForceRebuildLayoutImmediate(strigmaGroup.GetComponent<RectTransform>());

        VerticalLayoutGroup buffGroup = _buffDescriptionGrid.GetComponent<VerticalLayoutGroup>();
        buffGroup.childForceExpandWidth = false;
        LayoutRebuilder.ForceRebuildLayoutImmediate(buffGroup.GetComponent<RectTransform>());
    }

    public void InfoDestroy()
    {
        GameManager.Resource.Destroy(this.gameObject);
    }
}
