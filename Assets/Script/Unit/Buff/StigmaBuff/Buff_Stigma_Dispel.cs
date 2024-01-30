using UnityEngine;

public class Buff_Stigma_Dispel : Buff
{
    public override void Init( BattleUnit owner)
    {
        _buffEnum = BuffEnum.Dispel;

        _name = "����";

        _description = "����";

        _count = -1;

        _countDownTiming = ActiveTiming.NONE;

        _buffActiveTiming = ActiveTiming.BEFORE_ATTACK;

        _owner = owner;

        _statBuff = false;

        _dispellable = false;

        _stigmaBuff = true;
    }

    public override bool Active(BattleUnit caster)
    {
        if (caster != null)
            caster.Buff.DispelBuff();

        return false;
    }
}