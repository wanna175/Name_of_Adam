using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltimatePlayerSkill_Whisper : PlayerSkill
{
    public override bool Use(Vector2 coord)
    {
        foreach (BattleUnit unit in BattleManager.Data.BattleUnitList)
        {
            if (unit.Team == Team.Enemy)
            {
                GameManager.Sound.Play("UI/PlayerSkillSFX/Fall");
                //이팩트를 여기에 추가
                unit.ChangeFall(1);
            }
        }
        return false;
    }

    public override void CancelSelect()
    {
        BattleManager.PlayerSkillController.PlayerSkillReady(FieldColorType.none);
    }

    public override void OnSelect()
    {
        BattleManager.PlayerSkillController.PlayerSkillReady(FieldColorType.UltimatePlayerSkill, PlayerSkillTargetType.Enemy);
    }
}