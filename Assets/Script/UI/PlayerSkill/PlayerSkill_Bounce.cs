using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill_Bounce : PlayerSkill
{
    public override bool Use(Vector2 coord)
    {
        GameManager.Sound.Play("UI/PlayerSkillSFX/Bounce");
        GameManager.VisualEffect.StartVisualEffect("Arts/EffectAnimation/PlayerSkill/Bounce", BattleManager.Field.GetTilePosition(coord));

        BattleUnit unit = BattleManager.Field.GetUnit(coord);
        unit.DeckUnit.DeckUnitChangedStat = new Stat();

        BattleManager.Data.BattleUnitList.Remove(unit);
        BattleManager.Data.BattleUnitRemoveFromOrder(unit);
        BattleManager.Data.AddDeckUnit(unit.DeckUnit);
        BattleManager.BattleUI.FillHand();
        BattleManager.Field.FieldCloseInfo(BattleManager.Field.TileDict[coord]);
        Destroy(unit.gameObject);

        return false;
    }

    public override void CancelSelect()
    {
        BattleManager.PlayerSkillController.PlayerSkillReady(FieldColorType.none);
    }

    public override void OnSelect()
    {
        BattleManager.PlayerSkillController.PlayerSkillReady(FieldColorType.PlayerSkill, PlayerSkillTargetType.NotBattleOnly);
    }
}