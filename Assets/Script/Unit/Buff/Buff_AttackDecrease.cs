using UnityEngine;

public class Buff_AttackDecrease : Buff
{
    private int _attackDown;
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.AttackDecrease;

        _name = "Attack Decrease";

        _sprite = GameManager.Resource.Load<Sprite>($"Arts/Buff/Buff_AttackDecrease_Sprite");

        _description = "Attack Decrease Info";

        _count = 1;

        _countDownTiming = ActiveTiming.ATTACK_MOTION_END;

        _buffActiveTiming = ActiveTiming.NONE;

        _owner = owner;

        _statBuff = true;

        _dispellable = false;

        _stigmaBuff = false;

        _attackDown = owner.DeckUnit.DeckUnitTotalStat.ATK / 2;
    }

    public override void Stack()
    {
        _count += 1;
    }

    public override Stat GetBuffedStat()
    {
        Stat stat = new();  
        stat.ATK -= _attackDown;

        return stat;
    }
}