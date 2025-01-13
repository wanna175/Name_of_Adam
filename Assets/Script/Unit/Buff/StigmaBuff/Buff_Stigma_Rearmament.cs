using UnityEngine;

public class Buff_Stigma_Rearmament : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Stigmata_Rearmament;

        _name = "Rearmament";

        _buffActiveTiming = ActiveTiming.ATTACK_TURN_END;

        _owner = owner;

        _stigmataBuff = true;
    }

    public override bool Active(BattleUnit caster)
    {
        if (!_owner.IsDoneAttack)
        {
            _owner.SetBuff(new Buff_AttackBoost());
            _owner.SetBuff(new Buff_SpeedIncrease());
        }

        return false;
    }
}