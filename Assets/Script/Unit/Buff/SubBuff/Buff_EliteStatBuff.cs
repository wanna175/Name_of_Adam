using UnityEngine;

public class Buff_EliteStatBuff : Buff
{
    private Stat _buffedStat = new();
    private int _bossNum = 0;
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.EliteStatBuff;

        _description = "EliteStatBuff Info";

        _owner = owner;

        _statBuff = true;

        _isSystemBuff = true;
    }

    public override void SetValue(int num)
    {
        _bossNum = num;
        if (_bossNum == 1)
        {
            _name = "Blessing Of Phanuel";
            _sprite = GameManager.Resource.Load<Sprite>($"Arts/Buff/Buff_BlessingOfPhanuel_Sprite");
        }
        else if (_bossNum == 2)
        {
            _name = "Blessing Of Savior";
            _sprite = GameManager.Resource.Load<Sprite>($"Arts/Buff/Buff_BlessingOfSavior_Sprite");
        }
        else if (_bossNum == 3)
        {
            _name = "Blessing Of Yorhn";
            _sprite = GameManager.Resource.Load<Sprite>($"Arts/Buff/Buff_BlessingOfYohrn_Sprite");
        }
    }

    public override Stat GetBuffedStat() => _buffedStat;

    public override void SetStat(Stat stat) => _buffedStat = stat;

    public override string GetDescription(int spacing = 1)
    {
        string desc = "<color=#FF9696><size=110%><b>" + Name + "</b><color=white></size>";
        for (int i = 0; i < spacing; i++)
            desc += "\n";
        desc += GameManager.Locale.GetLocalizedBuffInfo(_description);
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