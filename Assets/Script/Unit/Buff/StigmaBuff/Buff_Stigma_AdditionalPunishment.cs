using UnityEngine;

public class Buff_Stigma_AdditionalPunishment : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Stigmata_AdditionalPunishment;

        _name = "AdditionalPunishment";

        _buffActiveTiming = ActiveTiming.BEFORE_ATTACK;

        _owner = owner;

        _stigmataBuff = true;
    }

    public override bool Active(BattleUnit caster)
    {
        // BattleManager-ActionPhaseClick¿¡¼­ ±¸ÇöµÊ
        return false;
    }
}
