using UnityEngine;
using System.Collections.Generic;

public class Buff_Stigma_ShadowStep: Buff
{
    private readonly List<Vector2> UDLR = new() { Vector2.right, Vector2.up, Vector2.left, Vector2.down };

    public override void Init(BattleUnit caster, BattleUnit owner)
    {
        _buffEnum = BuffEnum.ShadowStep;

        _name = "±×¸²ÀÚ ¹â±â";

        _description = "±×¸²ÀÚ ¹â±â.";

        _count = -1;

        _countDownTiming = ActiveTiming.NONE;

        _buffActiveTiming = ActiveTiming.BEFORE_ATTACK;

        _caster = caster;

        _owner = owner;

        _statBuff = false;

        _dispellable = false;

        _stigmaBuff = true;
    }

    public override bool Active(BattleUnit caster, BattleUnit receiver)
    {
        float currntMax = 0f;
        Vector2 moveVector = caster.Location;

        foreach (Vector2 direction in UDLR)
        {
            Vector2 vec = receiver.Location + direction;
            float sqr = (vec - caster.Location).sqrMagnitude;

            if (currntMax < sqr)
            {
                currntMax = sqr;
                if (BattleManager.Field.IsInRange(vec) && !BattleManager.Field.TileDict[vec].UnitExist)
                {
                    moveVector = vec;
                }
            }
            else if (currntMax == sqr)
            {
                if (direction.x != 0 && BattleManager.Field.IsInRange(vec) && !BattleManager.Field.TileDict[vec].UnitExist)
                {
                    moveVector = vec;
                }
            }
        }

        BattleManager.Instance.MoveUnit(caster, moveVector);

        return false;
    }
}