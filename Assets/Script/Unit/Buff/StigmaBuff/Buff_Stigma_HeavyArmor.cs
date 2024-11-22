using System.Collections.Generic;
using UnityEngine;

public class Buff_Stigma_HeavyArmor : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Stigmata_HeavyArmor;

        _name = "Heavy Armor";

        _buffActiveTiming = ActiveTiming.BEFORE_ATTACKED;

        _owner = owner;

        _stigmataBuff = true;
    }

    public override bool Active(BattleUnit caster)
    {
        List<Vector2> rangeVector = new() { Vector2.right, Vector2.up, Vector2.left, Vector2.down,
        Vector2.right + Vector2.up, Vector2.left + Vector2.up, Vector2.right+Vector2.down, Vector2.left+Vector2.down};

        if (!BattleManager.Field.GetUnitsInRange(_owner.Location, rangeVector).Contains(caster))
            _owner.ChangedDamage = (int)(_owner.ChangedDamage * 0.6f);

        return false;
    }
}