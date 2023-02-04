using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Skill : MonoBehaviour
{
    enum TargetType
    {
        Select,
        Range,
    }

    [SerializeField] AttackType AttackType;
    [SerializeField] List<EffectSO> Effects;
    [SerializeField] RangeSO Range;

    public void Use(BattleUnit caster, BattleUnit receiver)
    {
        List<Vector2> rangeList = Range.GetRange(caster.Location);
    }
}