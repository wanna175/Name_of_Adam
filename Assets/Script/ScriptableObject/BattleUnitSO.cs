using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "Scriptable Object/Character", order = 0)]
public class BattleUnitSO : ScriptableObject
{
    [SerializeField] public Sprite sprite;
    [SerializeField] public Stat stat;
    [SerializeField] public int MoveDistance;
    [SerializeField] public int ManaCost;
    [SerializeField] public bool Fall;
    [SerializeField] SkillSO skill;

    // 캐릭터의 스킬 사용
    public void use(BattleUnit ch)
    {
        skill.use(ch);
    }

    public AttackType GetAttackType() => skill.attackType;
    public CutSceneType GetCutSceneType() => skill.CSType;

    // 타겟팅 스킬을 가진 경우, 범위를 반환한다.
    public List<Vector2> GetTargetingRange()
    {
        if (skill.attackType == AttackType.targeting)
            return null;

        return skill.GetRange();
    }

    public List<Vector2> GetRange() => skill.GetRange();

    public int SkillLength() => skill.EffectList.Count;
}
