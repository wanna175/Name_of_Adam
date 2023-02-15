using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Skill : MonoBehaviour
{
    [SerializeField] List<Effect> Effects;

    public void Use(BattleUnit caster, BattleUnit receiver)
    {
        foreach (Effect effect in Effects)
            effect.Use(caster, receiver);
    }
}