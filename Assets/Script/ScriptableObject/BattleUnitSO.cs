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
    [SerializeField] public Team team;
    [SerializeField] public Sprite sprite;
    [SerializeField] public Stat stat;
    [SerializeField] public int FallGauge;
    [SerializeField] public bool Fall;
    [SerializeField] SkillSO skill;

    // 캐릭터의 스킬 사용
    public void use(BattleUnit ch)
    {
        skill.use(ch);
    }

    public AttackType GetAttackType()
    {
        foreach (EffectSO sk in skill.EffectList)
        {
            if (sk.GetType() == typeof(Effect_Attack))
            {
                Effect_Attack ea = sk as Effect_Attack;
                return ea.attackType;
            }
        }
        return AttackType.none;
    }

    // 타겟팅 스킬을 가진 경우, 범위를 반환한다.
    public List<Vector2> GetTargetingRange()
    {
        foreach(EffectSO sk in skill.EffectList)
        {
            if (sk.GetType() == typeof(Effect_Attack))
            {
                Effect_Attack ea = sk as Effect_Attack;
                return ea.GetRange();
            }
        }

        return null;
    }

    public int SkillLength() => skill.EffectList.Count;
}
