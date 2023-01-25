using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Effect_Attack", menuName = "Scriptable Object/Effect_Attack", order = 3)]
public class Effect_Attack : EffectSO
{

    // 공격 실행
    public override void Effect(BattleUnit caster, List<BattleUnit> battleUnits)
    {
        caster.UnitAction.OnAttack(battleUnits);
    }
}
