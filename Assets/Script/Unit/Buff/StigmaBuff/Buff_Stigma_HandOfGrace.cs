using UnityEngine;

public class Buff_Stigma_HandOfGrace : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Stigmata_HandOfGrace;

        _name = "HandOfGrace";

        _buffActiveTiming = ActiveTiming.BEFORE_ATTACK;

        _owner = owner;

        _stigmataBuff = true;
    }

    public override bool Active(BattleUnit caster)
    {
        foreach (BattleUnit unit in BattleManager.Field.GetUnitsInRange(_owner.Location, _owner.GetAttackRange(), _owner.Team))
        {
            if (unit != _owner)
            {
                GameManager.VisualEffect.StartVisualEffect("Arts/EffectAnimation/PlayerSkill/Heal", BattleManager.Field.GetTilePosition(unit.Location));
                unit.GetHeal(10, _owner);
            }
        }

        return false;
    }
}