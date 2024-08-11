using UnityEngine;

public class Buff_AttackBoost : Buff
{
    private int _attackUp;

    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.AttackBoost;

        _name = "Attack Boost";

        _sprite = GameManager.Resource.Load<Sprite>($"Arts/Buff/Buff_AttackBoost_Sprite");

        _description = "Attack Boost Info";

        _count = 1;

        _countDownTiming = ActiveTiming.ATTACK_MOTION_END;

        _owner = owner;

        _statBuff = true;

        _attackUp = owner.DeckUnit.DeckUnitTotalStat.ATK / 2;
    }

    public override void Stack()
    {
        _count += 1;
    }

    public override Stat GetBuffedStat()
    {
        Stat stat = new();  
        stat.ATK += _attackUp;

        return stat;
    }
}