using UnityEngine;

public class Buff_Stigma_BloodBlessing : Buff
{
    int heal;

    public override void Init(BattleUnit caster, BattleUnit owner)
    {
        _buffEnum = BuffEnum.BloodBlessing;

        _name = "축복";

        _description = "축복.";

        _count = -1;

        _countDownTiming = ActiveTiming.NONE;

        _buffActiveTiming = ActiveTiming.FIELD_UNIT_DEAD;

        _caster = caster;

        _owner = owner;

        _statBuff = false;

        _dispellable = false;

        _stigmaBuff = true;
    }

    public override bool Active(BattleUnit caster, BattleUnit receiver)
    {
        _owner.ChangeHP(heal);

        return false;
    }

    public override void SetValue(int num)
    {
        heal = num;
    }
}