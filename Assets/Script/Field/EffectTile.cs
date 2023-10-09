using System;
using UnityEngine;

public class EffectTile : MonoBehaviour
{
    private EffectTileType _effectType = EffectTileType.None;
    public void SetEffect(EffectTileType effectType) => _effectType = effectType;
    public EffectTileType GetEffect() => _effectType;
}
