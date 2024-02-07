using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill_02_01 : PlayerSkill
{
    private Vector2 tilePos;

    public override bool Use(Vector2 coord)
    {
        tilePos = coord;
        BattleUnit unit = BattleManager.Field.GetUnit(tilePos);
        unit.ChangeHP(-30);

        GameManager.Sound.Play("UI/PlayerSkillSFX/Fall");
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

    public override string GetDescription()
    {
        SetDescription();
        return base.GetDescription();
    }

    public void SetDescription()
    {
        string description = "";

        switch (GameManager.Locale.CurrentLocaleIndex)
        {
            case 0: description = "Deals 30 damage to the designated unit."; break;
            case 1: description = "지정한 유닛에게 데미지를 30 줍니다."; break;
        }

        base.SetDescription(description);
    }
}