using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill_Cross : PlayerSkill
{
    public override void Use(Vector2 coord)
    {
        //GameManager.Sound.Play("UI/PlayerSkillSFX/Fall");
        //ÀÌÆÑÆ®¸¦ ¿©±â¿¡ Ãß°¡
        List<Vector2> targetCoords = BattleManager.Field.GetCrossCoord(coord);

        foreach (Vector2 target in targetCoords)
        {
            GameManager.VisualEffect.StartVisualEffect(
                Resources.Load<AnimationClip>("Arts/EffectAnimation/PlayerSkill/CrossThunder"),
                BattleManager.Field.GetTilePosition(target) + new Vector3(0f, 3.5f, 0f));
            BattleUnit targetUnit = BattleManager.Field.GetUnit(target);

            if (targetUnit != null && targetUnit.Team == Team.Enemy)
            {
                targetUnit.GetAttack(-15, null);
            }


        }

    }
    private void Attack(Vector2 coord)
    {
        BattleManager.Field.GetUnit(coord).GetAttack(-15, null);
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
