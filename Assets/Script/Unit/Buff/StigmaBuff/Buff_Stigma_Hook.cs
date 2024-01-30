using UnityEngine;
using System.Collections.Generic;

public class Buff_Stigma_Hook : Buff
{
    private readonly List<Vector2> UDLR = new() { Vector2.right, Vector2.up, Vector2.left, Vector2.down };

    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Hook;

        _name = "갈고리";

        _description = "피격된 유닛을 한 칸 끌어옵니다.";

        _count = -1;

        _countDownTiming = ActiveTiming.NONE;

        _buffActiveTiming = ActiveTiming.AFTER_ATTACK;

        _owner = owner;

        _statBuff = false;

        _dispellable = false;

        _stigmaBuff = true;
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