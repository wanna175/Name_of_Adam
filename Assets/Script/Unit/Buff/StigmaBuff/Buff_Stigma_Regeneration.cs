using UnityEngine;

public class Buff_Stigma_Regeneration : Buff
{
    private int _heal = 0;
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Stigmata_Regeneration;

        _name = "Regeneration";

        _buffActiveTiming = ActiveTiming.ATTACK_TURN_END;

        _owner = owner;

        _stigmataBuff = true;
    }

    public override bool Active(BattleUnit caster)
    {
        _owner.GetHeal(_heal, caster);

        return false;
    }

    public override void SetValue(int num)
    {
        _heal += num;
    }
}