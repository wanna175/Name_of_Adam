using UnityEngine;
using System.Collections.Generic;

public class Buff_Stigma_ShadowStep: Buff
{
    private readonly List<Vector2> UDLR = new() { Vector2.right, Vector2.up, Vector2.left, Vector2.down };

    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.ShadowStep;

        _name = "그림자 밟기";

        _description = "피격 대상이 한명일 경우, 공격 후 피격 대상의 배후로 넘어갑니다.";

        _count = -1;

        _countDownTiming = ActiveTiming.NONE;

        _buffActiveTiming = ActiveTiming.BEFORE_ATTACK;

        _owner = owner;

        _statBuff = false;

        _dispellable = false;

        _stigmaBuff = true;
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