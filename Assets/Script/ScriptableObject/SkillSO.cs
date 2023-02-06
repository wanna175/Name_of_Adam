using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Memo : Enum일 필요 없을듯 (bool)
public enum AttackType
{
    targeting,
    rangeAttack
}

[CreateAssetMenu(fileName = "Skill", menuName = "Scriptable Object/Skill", order = 1)]
public class SkillSO : ScriptableObject
{
    [SerializeField] public AttackType attackType;
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
        Field _field = GameManager.BattleMNG.Field;
        List<Vector2> rangeList = range.GetRange(caster.Location);
        List<BattleUnit> hitUnits = new List<BattleUnit>();

        if (attackType == AttackType.rangeAttack)
        {
            // 공격범위 안에 있는 모든 대상을 리스트에 넣는다.
            foreach (Vector2 vec in rangeList)
            {
                BattleUnit unit = _field.GetUnit(vec);

                if (unit == null)
                    return;

                if (unit.MyTeam != caster.MyTeam)
                    hitUnits.Add(unit);
            }
        }
        else if (attackType == AttackType.targeting)
        {

            int x = (int)caster.SelectTile.x;
            int y = (int)caster.SelectTile.y;

            BattleUnit unit = _field.GetUnit(new Vector2(x, y));

            if (unit == null)
                return;

            if (unit.MyTeam != caster.MyTeam)
                hitUnits.Add(unit);
        }

        foreach (EffectSO es in EffectList)
        {
            es.Effect(caster, hitUnits);
        }
    }

    public List<Vector2> GetRange() => range.GetRange();
}

// 23.01.25 김종석
// Effect_Attack에서 진행하던 범위 확인을 SkillSO에서 진행하도록 변경
// 그에 따른 attackType과 CSType을 SkillSO에 이동
// 지금은 공격처리에 대한 타겟 설정만 되어있으므로
// 힐러의 타겟 서칭은 따로 해야함