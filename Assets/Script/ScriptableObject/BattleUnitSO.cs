using System;
using System.Collections.Generic;
using UnityEngine;

// 캐릭터의 스탯
[Serializable]
public struct Stat
{
    public float HP;
    public float ATK;
    public int SPD;
}

// 캐릭터의 팀
[Serializable]
public enum Team
{
    Player,
    Enemy
}

[CreateAssetMenu(fileName = "Character", menuName = "Scriptable Object/Character", order = 0)]
public class BattleUnitSO : ScriptableObject
{
    [SerializeField] public bool MyTeam;
    [SerializeField] public Sprite sprite;
    [SerializeField] public Stat stat;
    [SerializeField] public int ManaCost;
    [SerializeField] public int FallGauge;
    [SerializeField] public bool Fall;
    [SerializeField] SkillSO skill;

    // 캐릭터의 스킬 사용
    public void use(BattleUnit ch)
    {
        skill.use(ch);
    }

    public AttackType GetAttackType() => skill.attackType;
    public RangeType GetRangeType() => skill.rangeType;

    // 타겟팅 스킬을 가진 경우, 범위를 반환한다.
    public List<Vector2> GetTargetingRange()
    {
        if (skill.attackType == AttackType.targeting)
            return null;

        return skill.GetRange();
    }

    public int SkillLength() => skill.EffectList.Count;
}
