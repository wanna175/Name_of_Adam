using System.Collections;
using UnityEngine;

public abstract class Effect : MonoBehaviour
{
    public abstract void Use(BattleUnit caster, BattleUnit receiver);
}