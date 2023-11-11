using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill_02_01 : PlayerSkill
{
    private Vector2 tilePos;

    public override void Use(Vector2 coord, out bool isSkillOn)
    {
        tilePos = coord;
        //GameManager.Sound.Play("UI/PlayerSkillSFX/Fall");
        //GameManager.VisualEffect.StartVisualEffect("Arts/EffectAnimation/PlayerSkill/DarkThunder", BattleManager.Field.GetTilePosition(coord));
        isSkillOn = true;
    }

    public override void Action(Vector2 coord, ActiveTiming activeTiming, out bool isNotOverYet)
    {
        base.Action(coord, activeTiming, out isNotOverYet);
        if (!isNotOverYet)
            return;

        
    }

    public override void CancelSelect()
    {
        BattleManager.PlayerSkillController.PlayerSkillReady(FieldColorType.none);
    }

    public override void OnSelect()
    {
        BattleManager.PlayerSkillController.PlayerSkillReady(FieldColorType.PlayerSkill, PlayerSkillTargetType.all);
    }
}