using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill_Cross : PlayerSkill
{
    public override void Use(Vector2 coord)
    {
        //GameManager.Sound.Play("UI/PlayerSkillSFX/Fall");
        //이팩트를 여기에 추가
        BattleManager.Field.GetUnit(coord).GetAttack(-15, null);
        List<BattleUnit> targetUnits = BattleManager.Field.GetCrossUnits(coord);

        foreach (BattleUnit enemyUnits in targetUnits)
        {
            if(enemyUnits.Team == Team.Enemy)
            {
                enemyUnits.GetAttack(-15, null);
            }
        }

    }
    public override void CancelSelect()
    {
        BattleManager.PlayerSkillController.PlayerSkillReady(FieldColorType.none);
    }

    public override void OnSelect()
    {
        BattleManager.PlayerSkillController.PlayerSkillReady(FieldColorType.PlayerSkill, PlayerSkillTargetType.Enemy);
    }
}
