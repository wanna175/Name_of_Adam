using System.Collections.Generic;
using UnityEngine;

public class Stigma_Tailwind : Stigma
{
    public override void Use(BattleUnit caster)
    {
        base.Use(caster);

        List<Vector2> areaCoords = new();
        for (int i = -2; i < 3; i++)
            areaCoords.Add(new Vector2(0, i));

        List<BattleUnit> targetUnits = BattleManager.Field.GetUnitsInRange(caster.Location, areaCoords);
        
        GameManager.Sound.Play("UI/PlayerSkillSFX/Swiftness");
        foreach (BattleUnit unit in targetUnits)
        {
            if (unit.Team == caster.Team)
            {
                GameManager.VisualEffect.StartVisualEffect("Arts/EffectAnimation/PlayerSkill/Tailwind", BattleManager.Field.GetTilePosition(unit.Location));
                unit.SetBuff(new Buff_SpeedIncrease());
            }
        }
    }
}