using UnityEngine;

public class Buff_Stigma_BloodBlessing : Buff
{
    private int _heal;

    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Stigmata_BloodBlessing;

        _name = "BloodBlessing";

        _buffActiveTiming = ActiveTiming.FIELD_UNIT_DEAD;

        _owner = owner;

        _stigmataBuff = true;
    }

    public override bool Active(BattleUnit caster)
    {
        _owner.GetHeal(_heal, caster);
        GameManager.VisualEffect.StartVisualEffect("Arts/EffectAnimation/PlayerSkill/Heal", BattleManager.Field.GetTilePosition(_owner.Location));

        return false;
    }

    public override void SetValue(int num)
    {
        _heal = num;
    }
}