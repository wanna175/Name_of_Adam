using UnityEngine;

public class Buff_SpeedIncrease : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.SpeedIncrease;

        _name = "Speed Increase";

        _sprite = GameManager.Resource.Load<Sprite>($"Arts/Buff/Buff_SpeedIncrease_Sprite");

        _description = "Speed Increase Info";

        _count = 1;

        _countDownTiming = ActiveTiming.MOVE_TURN_END;

        _owner = owner;

        _statBuff = true;
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