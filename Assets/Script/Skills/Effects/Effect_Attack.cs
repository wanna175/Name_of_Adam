using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Effect_HP", menuName = "Scriptable Object/Effect_HP")]
public class Effect_Attack : Effect
{
    public override void Use(BattleUnit caster, BattleUnit receiver)
    {
        int damage = caster.BattleUnitTotalStat.ATK;

        receiver.ChangeHP(-damage);
    }
}