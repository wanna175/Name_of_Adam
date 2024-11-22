using System.Collections.Generic;
using UnityEngine;

public class Buff_Stigma_Distrust : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Stigmata_Distrust;

        _name = "Distrust";

        _buffActiveTiming = ActiveTiming.STIGMA;

        _owner = owner;

        _stigmataBuff = true;

        _owner.SetBuff(new Buff_Distrust());
    }
}