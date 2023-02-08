using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill", menuName = "Scriptable Object/Skill", order = 1)]
public class SkillSO : ScriptableObject
{
    [SerializeField] public CutSceneType CSType;
    [SerializeField] RangeSO range;    // 공격 범위

    [SerializeField] public List<EffectSO> EffectList;


    public void Use()
    {

    }

    // 현재 알고리즘은 범위 내의 적을 찾는 알고리즘
    // 힐같이 아군을 찾는 알고리즘은 나중에 따로 설정해야한다
    public void use(BattleUnit caster)
    {
        Field _field = GameManager.Battle.Field;
        List<Vector2> rangeList = range.GetRange(caster.Location);
        List<BattleUnit> hitUnits = new List<BattleUnit>();

        if (caster.Data.TargetType == TargetType.Range)
        {
            // 공격범위 안에 있는 모든 대상을 리스트에 넣는다.
            foreach (Vector2 vec in rangeList)
            {
                BattleUnit unit = _field.GetUnit(vec);

                if (unit == null)
                    continue;

                if (unit.Team != caster.Team)
                    hitUnits.Add(unit);
            }
        }
        else if (caster.Data.TargetType == TargetType.Select)
        {
            BattleUnit unit = _field.GetUnit(caster.SelectTile);

            if (unit == null)
                return;

            if (unit.Team != caster.Team)
                hitUnits.Add(unit);
        }

        foreach (EffectSO es in EffectList)
        {
            es.Effect(caster, hitUnits);
        }
    }

    public List<Vector2> GetRange() => range.GetRange();
}