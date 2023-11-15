using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltimatePlayerSkill_Whisper : PlayerSkill
{
    public override void Use(Vector2 coord, out bool isNotOverYet)
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
        isNotOverYet = false;
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