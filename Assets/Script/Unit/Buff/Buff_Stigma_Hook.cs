using UnityEngine;
using System.Collections.Generic;

public class Buff_Stigma_Hook : Buff
{
    private readonly List<Vector2> UDLR = new() { Vector2.right, Vector2.up, Vector2.left, Vector2.down };

    public override void Init(BattleUnit caster, BattleUnit owner)
    {
        _buffEnum = BuffEnum.Hook;

        _name = "갈고리";

        _description = "갈고리.";

        _count = -1;

        _countDownTiming = ActiveTiming.NONE;

        _buffActiveTiming = ActiveTiming.AFTER_ATTACK;

        _caster = caster;

        _owner = owner;

        _statBuff = false;

        _dispellable = false;

        _stigmaBuff = true;
    }

    public override bool Active(BattleUnit caster, BattleUnit receiver)
    {
        float currntMin = 100f;
        Vector2 moveVector = receiver.Location;

        foreach (Vector2 direction in UDLR)
        {
            Vector2 vec = receiver.Location + direction;
            float sqr = (vec - caster.Location).sqrMagnitude;

            if (currntMin > sqr)
            {
                currntMin = sqr;
                if (BattleManager.Field.IsInRange(vec) && !BattleManager.Field.TileDict[vec].UnitExist)
                {
                    moveVector = vec;
                }
            }
            else if (currntMin == sqr)
            {
                if (direction.x != 0 && BattleManager.Field.IsInRange(vec) && !BattleManager.Field.TileDict[vec].UnitExist)
                {
                    moveVector = vec;
                }
            }
        }

        BattleManager.Field.MoveUnit(receiver.Location, moveVector);

        return false;
    }
}