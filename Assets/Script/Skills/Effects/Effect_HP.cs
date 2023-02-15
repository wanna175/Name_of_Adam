using System.Collections;
using UnityEngine;

public class Effect_HP : Effect
{
    public override void Use(BattleUnit caster, BattleUnit receiver)
    {
        int damage = caster.Stat.ATK;

        receiver.ChangeHP(damage);
    }
}