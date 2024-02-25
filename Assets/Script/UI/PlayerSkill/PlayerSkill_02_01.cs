using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill_02_01 : PlayerSkill
{
    private Vector2 _tilePos;

    public override bool Use(Vector2 coord)
    {
        _tilePos = coord;
        BattleUnit unit = BattleManager.Field.GetUnit(_tilePos);
        unit.ChangeHP(-25);

        GameManager.Sound.Play("UI/PlayerSkillSFX/Punishment");
        GameManager.VisualEffect.StartVisualEffect("Arts/EffectAnimation/PlayerSkill/DarkThunder", BattleManager.Field.GetTilePosition(coord));
                
        return true;
    }

    public override bool Action(ActiveTiming activeTiming, Vector2 coord)
    {
        return base.Action(activeTiming, coord);
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