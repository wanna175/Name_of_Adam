using UnityEngine;

public class Buff_Sin : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Sin;

        _name = "Malevolence";

        _description = "Decreases the enemy's faith by 1 upon attacking.";

        _count = -1;

        _countDownTiming = ActiveTiming.NONE;

        _buffActiveTiming = ActiveTiming.SUMMON;

        _owner = owner;

        _statBuff = false;

        _dispellable = false;

        _stigmaBuff = false;
    }

    public override bool Active(BattleUnit caster)
    {
        Buff_Vice buff = new Buff_Vice();
        for (int i = 0; i < 66; i++)
            _owner.SetBuff(buff);

        return false;
    }
}