using UnityEngine;
using System.Collections.Generic;

public class Buff_Stigma_Hook : Buff
{
    private readonly List<Vector2> UDLR = new() { Vector2.right, Vector2.up, Vector2.left, Vector2.down };

    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Stigmata_Hook;

        _name = "Hook";

        _buffActiveTiming = ActiveTiming.AFTER_ATTACK;

        _owner = owner;

        _stigmataBuff = true;
    }

    public override bool Active(BattleUnit caster)
    {
        if (caster == null)
            return false;

        Vector2 hookDir = (_owner.Location - caster.Location).normalized;
        Vector2Int hookDirInt = new Vector2Int(Mathf.RoundToInt(hookDir.x), Mathf.RoundToInt(hookDir.y));
        Vector2 vec = caster.Location + hookDirInt;

        if (BattleManager.Field.IsInRange(vec) && !BattleManager.Field.TileDict[vec].UnitExist)
            BattleManager.Instance.MoveUnit(caster, vec);

        return false;
    }
}