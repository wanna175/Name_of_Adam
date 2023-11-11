using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill_Move : PlayerSkill
{
    private BattleUnit selectedUnit;

    public override void Use(Vector2 coord, out bool isSkillOn)
    {
        selectedUnit = BattleManager.Field.GetUnit(coord);
        BattleManager.Field.SetNextActionTileColor(selectedUnit, FieldColorType.Move);
        isSkillOn = true;
        Debug.Log("A");
    }

    public override void Action(Vector2 coord, ActiveTiming activeTiming, out bool isSkillOn)
    {
        base.Action(coord, activeTiming, out isSkillOn);
        if (!isSkillOn)
            return;

        switch (activeTiming)
        {
            case ActiveTiming.NONE:
                BattleManager.Instance.MoveUnit(selectedUnit, coord);
                BattleManager.Field.ClearAllColor();
                BattleManager.PlayerSkillController.SetSkillDone();
                break;
        }
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
