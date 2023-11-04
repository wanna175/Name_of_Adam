using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillMove : PlayerSkill
{
    public override void Use(Vector2 coord)
    {

    }
    public override void CancelSelect()
    {
        BattleManager.PlayerSkillController.PlayerSkillReady(FieldColorType.none);
    }

    public override void OnSelect()
    {
        BattleManager.PlayerSkillController.PlayerSkillReady(FieldColorType.PlayerSkill, PlayerSkillTargetType.Friendly);
    }
}
