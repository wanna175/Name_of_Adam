using UnityEngine;

public class Buff_Stigma_Blessing : Buff
{
    public override void Init(BattleUnit caster, BattleUnit owner)
    {
        _buffEnum = BuffEnum.Blessing;

        _name = "축복";

        _description = "축복.";

        _count = -1;

        _countDownTiming = ActiveTiming.NONE;

        _buffActiveTiming = ActiveTiming.AFTER_ATTACK;

        _caster = caster;

        _owner = owner;

        _statBuff = false;

        _dispellable = false;

        _stigmaBuff = true;
    }

    public override bool Active(BattleUnit caster, BattleUnit receiver)
    {
        BattleManager.Mana.ChangeMana(5);

        return false;
    }
}