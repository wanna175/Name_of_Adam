using UnityEngine;
using System.Collections.Generic;

public class Buff_Stigma_StepsOfShadow : Buff
{
    private readonly List<Vector2> UDLR = new() { Vector2.right, Vector2.up, Vector2.left, Vector2.down };

    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Stigmata_StepsOfShadow;

        _name = "Steps Of Shadow";

        _buffActiveTiming = ActiveTiming.BEFORE_ATTACK;

        _owner = owner;

        _stigmataBuff = true;
    }

    public override bool Active(BattleUnit caster)
    {
        if (caster == null)
            return false;

        if (_owner.AttackUnitNum == 1)
        {
            Vector2 vec = caster.Location + (caster.Location - _owner.Location).normalized;
            if (BattleManager.Field.IsInRange(vec) && !BattleManager.Field.TileDict[vec].UnitExist)
            {
                BattleManager.Instance.MoveUnit(_owner, vec);
                _owner.SetFlipX(!_owner.GetFlipX());
            }
        }

        return false;
    }
}