using UnityEngine;

public class Buff_Raise : Buff
{
    private int attackUp;
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Raise;

        _name = "Attack Increase";

        _sprite = GameManager.Resource.Load<Sprite>($"Arts/Buff/Buff_Raise_Sprite");

        _description = "Attack Increase Info";

        _count = 1;

        _countDownTiming = ActiveTiming.ATTACK_MOTION_END;

        _buffActiveTiming = ActiveTiming.NONE;

        _owner = owner;

        _statBuff = true;

        _dispellable = false;

        _stigmaBuff = false;

        attackUp = owner.DeckUnit.DeckUnitTotalStat.ATK / 2;
    }

    public override void Stack()
    {
        _count += 1;
    }

    public override Stat GetBuffedStat()
    {
        Stat stat = new();  
        stat.ATK += attackUp;

        return stat;
    }
}