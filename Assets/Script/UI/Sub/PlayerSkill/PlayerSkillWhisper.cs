using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillWhisper : PlayerSkill
{
    private string name = "Whisper";
    private int manaCost = 20;
    private int darkEssence = 1;
    private string description = "20 마나와 1 검은 정수를 지불하고 원하는 적 유닛에게 타락도 1을 부여합니다.";

    public override int GetDarkEssenceCost() => darkEssence;
    public override int GetManaCost() => manaCost;
    public override string GetName() => name;
    public override string GetDescription() => description;

    public override void CancelSelect()
    {
        BattleManager.PlayerSkillController.EnemyTargetPlayerSkillReady(FieldColorType.none);
    }

    public override void OnSelect()
    {
        BattleManager.PlayerSkillController.EnemyTargetPlayerSkillReady(FieldColorType.PlayerSkillWhisper);
    }
}