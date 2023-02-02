using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "Scriptable Object/Character", order = 0)]
public class BattleUnitSO : ScriptableObject
{
    [SerializeField] public Team Team;
    [SerializeField] public RangeType RType;
    [SerializeField] public Sprite sprite;
    [SerializeField] public Stat stat;
    [SerializeField] public int MoveDistance;
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
    public CutSceneType GetCutSceneType() => skill.CSType;

    public List<Vector2> GetRange() => skill.GetRange();

    public int SkillLength() => skill.EffectList.Count;
}
