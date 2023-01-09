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

    public List<Vector2> GetTargetingRange()
    {
        foreach(EffectSO sk in skill.EffectList)
        {
            if (sk.GetType() == typeof(Effect_Attack))
                return sk.GetRange();
        }

        return null;
    }

    public int SkillLength() => skill.EffectList.Count;
}
