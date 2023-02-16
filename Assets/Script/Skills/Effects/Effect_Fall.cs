using System.Collections;
using UnityEngine;

public class Effect_Fall : Effect
{
    private int _fallValue = 1;

    public override void Use(BattleUnit caster, BattleUnit receiver)
    {
        receiver.ChangeFall(_fallValue);
    }
}