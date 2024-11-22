using UnityEngine;

public class Buff_Distrust : Buff
{
    private Stat _buffedStat = new();
    private string _buffedStatOwner;

    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Distrust;

        _name = "Distrust";

        _sprite = GameManager.Resource.Load<Sprite>($"Arts/Buff/Buff_Distrust_Sprite");

        _description = "Distrust Info";

        _owner = owner;

        _isSystemBuff = true;

        _statBuff = true;

        SetGainStat();
    }

    public override Stat GetBuffedStat()
    {
        return _buffedStat;
    }

    private void SetGainStat()
    {
        int statTotal = 0;

        foreach (BattleUnit unit in BattleManager.Data.BattleUnitList)
        {
            if (unit.Team == _owner.Team && unit != _owner &&
                unit.DeckUnit.DeckUnitTotalStat.ATK + unit.DeckUnit.DeckUnitTotalStat.MaxHP > statTotal)
            {
                _buffedStat = unit.DeckUnit.DeckUnitTotalStat;
                _buffedStat.CurrentHP = _buffedStat.MaxHP;
                statTotal = unit.DeckUnit.DeckUnitTotalStat.ATK + unit.DeckUnit.DeckUnitTotalStat.MaxHP;
                _buffedStatOwner = unit.Data.Name;
            }
        }
        _buffedStat.ManaCost = 0;
    }

    public override string GetDescription(int spacing = 1)
    {
        string desc = "<color=#FF9696><size=110%><b>" + Name + "</b><color=white></size>";
        for (int i = 0; i < spacing; i++)
            desc += "\n";
        desc += GameManager.Locale.GetLocalizedBuffInfo(_description);
        desc += _buffedStatOwner;
        desc += "\n";
        if (_buffedStat.ATK != 0)
            desc += GameManager.Locale.GetLocalizedBuffInfo("ATK") + _buffedStat.ATK + "\n";

        if (_buffedStat.SPD != 0)
            desc += GameManager.Locale.GetLocalizedBuffInfo("SPD") + _buffedStat.SPD + "\n";

        if (_buffedStat.MaxHP != 0)
            desc += GameManager.Locale.GetLocalizedBuffInfo("HP") + _buffedStat.MaxHP + "\n";

        return desc;
    }
}