using UnityEngine;

public class Buff_Stigma_ChainOfFate : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Stigmata_ChainOfFate;

        _name = "ChainOfFate";

        _buffActiveTiming = ActiveTiming.BEFORE_ATTACK;

        _owner = owner;

        _stigmataBuff = true;
    }

    public override bool Active(BattleUnit caster)
    {
        if (RandomManager.GetFlag(0.3f))
        {
            caster.SetBuff(new Buff_Bind());
        }

        return false;
    }
}