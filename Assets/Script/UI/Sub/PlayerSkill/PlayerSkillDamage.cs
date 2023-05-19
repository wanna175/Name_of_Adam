using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillDamage : PlayerSkill
{
    private string name = "Damage";
    private int manaCost = 20;
    private int darkEssence = 0;

    public override int GetDarkEssenceCost() => darkEssence;
    public override int GetManaCost() => manaCost;
    public override string GetName() => name;

    public override void CancleSelect()
    {
        BattleManager.Instance.EnemyTargetPlayerSkillReady(BattleManager.FieldColorType.none);
    }

    public override void OnSelect()
    {
        BattleManager.Instance.EnemyTargetPlayerSkillReady(BattleManager.FieldColorType.PlayerSkill_1);
    }
}