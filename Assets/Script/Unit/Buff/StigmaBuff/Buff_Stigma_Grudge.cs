using UnityEngine;

public class Buff_Stigma_Grudge : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Grudge;

        _name = "Grudge";

        _description = "Grudge Info";

        _count = -1;

        _countDownTiming = ActiveTiming.NONE;

        _buffActiveTiming = ActiveTiming.FIELD_UNIT_FALLED;

        _owner = owner;

        _statBuff = false;

        _dispellable = false;

        _stigmaBuff = true;
    }

    public override bool Active(BattleUnit caster)
    {
        if (caster.Team == _owner.Team && caster != _owner)
        {
            Buff_Grudge grudge = new();
            grudge.SetValue(20);
            _owner.SetBuff(grudge);
        }

        return false;
    }
}