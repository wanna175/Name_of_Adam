using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Stigma", menuName = "Scriptable Object/Stigma", order = 6)]
public class StigmaSO : ScriptableObject
{
    [SerializeField] Sprite Icon;
    [SerializeField] List<EffectSO> Effects = new List<EffectSO>();

    public void Use(Character chara)
    {
        foreach (EffectSO effect in Effects)
            effect.Effect(chara);
    }
}