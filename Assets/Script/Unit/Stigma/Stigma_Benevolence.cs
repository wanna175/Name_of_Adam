using System.Collections.Generic;
using UnityEngine;

public class Stigma_Benevolence : Stigma
{
    public override void Use(BattleUnit caster)
    {
        base.Use(caster);

        List<BattleUnit> targetUnits = BattleManager.Field.GetArroundUnits(caster.Location);

        GameManager.Sound.Play("UI/PlayerSkillSFX/Recovery");
        foreach (BattleUnit unit in targetUnits)
        {
            if (unit.Team == caster.Team)
            {
                GameManager.VisualEffect.StartVisualEffect("Arts/EffectAnimation/PlayerSkill/Heal", BattleManager.Field.GetTilePosition(unit.Location));
                unit.GetHeal(30, caster);
            }
        }
    }
}