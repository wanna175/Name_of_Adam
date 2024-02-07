using UnityEngine;

public class Buff_Tailwind : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Tailwind;

        _name = "Tailwind";

        _sprite = GameManager.Resource.Load<Sprite>($"Arts/Buff/Buff_Tailwind_Sprite");

        _description = "All allies in the same row as the summoner gain a one-time speed increase upon summoning.";

        _count = 1;

        _countDownTiming = ActiveTiming.TURN_END;

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