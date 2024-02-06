using UnityEngine;

public class Buff_Tailwind : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Tailwind;

        _name = "순풍";

        _sprite = GameManager.Resource.Load<Sprite>($"Arts/Buff/Buff_Tailwind_Sprite");

        _description = "공격 턴에서 가장 먼저 공격 턴을 가집니다.";

        _count = 1;

        _countDownTiming = ActiveTiming.MOVE_TURN_END;

        _buffActiveTiming = ActiveTiming.NONE;

        _owner = owner;

        _statBuff = true;

        _dispellable = false;

        _stigmaBuff = false;
    }

    public override void Stack()
    {
        _count += 1;
    }

    public override Stat GetBuffedStat()
    {
        Stat stat = new();
        stat.SPD += 100;

        return stat;
    }
}