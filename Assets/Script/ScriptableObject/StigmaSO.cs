using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum StatType
{
    HP,
    ATK,
    SPD,
}

[CreateAssetMenu(fileName = "Stigma", menuName = "Scriptable Object/Stigma", order = 6)]
public class StigmaSO : ScriptableObject
{
    [SerializeField] Sprite Icon;
    [SerializeField] List<EffectSO> Effects = new List<EffectSO>();
    [SerializeField] StatType Type;
    [SerializeField] int Value;

    public void Use(Character chara)
    {
        foreach (EffectSO effect in Effects)
            effect.Effect(chara);
    }

    public Stat Use(Stat stat)
    {
        switch (Type)
        {
            case StatType.HP:
                stat.HP += Value;
                break;
            case StatType.ATK:
                stat.ATK += Value;
                break;
            case StatType.SPD:
                stat.SPD += Value;
                break;
        }

        return stat;
    }
}