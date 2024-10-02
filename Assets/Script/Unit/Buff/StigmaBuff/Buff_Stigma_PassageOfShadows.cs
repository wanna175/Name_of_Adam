using UnityEngine;

public class Buff_Stigma_PassageOfShadows : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Stigmata_PassageOfShadows;

        _name = "PassageOfShadows";

        _buffActiveTiming = ActiveTiming.UNIT_KILL | ActiveTiming.STIGMA;

        _owner = owner;

        _stigmataBuff = true;
    }

    public override bool Active(BattleUnit caster)
    {
        _owner.SetBuff(new Buff_SacredStep());

        return false;
    }
}