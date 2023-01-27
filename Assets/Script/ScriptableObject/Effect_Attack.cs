using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Effect_Attack", menuName = "Scriptable Object/Effect_Attack", order = 3)]
public class Effect_Attack : EffectSO
{   
    // 공격 실행
    public override void Effect(BattleUnit caster, List<BattleUnit> battleUnits)
    {
        caster.Attack_OnAttack(battleUnits);
    }
}

// 23.01.25 김종석
// Effect_Attack에서 처리하던 타겟 서칭을 SkillSO로 이동