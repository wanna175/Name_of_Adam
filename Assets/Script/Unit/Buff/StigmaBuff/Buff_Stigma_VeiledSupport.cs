using UnityEngine;

public class Buff_Stigma_VeiledSupport: Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Stigmata_VeiledSupport;

        _name = "VeiledSupport";

        _buffActiveTiming = ActiveTiming.AFTER_SWITCH;

        _owner = owner;

        _stigmataBuff = true;
    }

    public override bool Active(BattleUnit caster)
    {
        caster.GetHeal(15, _owner);
        GameManager.VisualEffect.StartVisualEffect("Arts/EffectAnimation/PlayerSkill/Heal", BattleManager.Field.GetTilePosition(caster.Location));

        return false;
    }
}