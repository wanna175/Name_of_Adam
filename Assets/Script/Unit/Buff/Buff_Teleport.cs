using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_Teleport : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Teleport;

        _name = "순간이동";

        _sprite = GameManager.Resource.Load<Sprite>($"Arts/Buff/Buff_Teleport_Sprite");

        _description = "Gains 1 Sacred Step upon defeating an enemy. Sacred Step Becomes able to move to any desired position.";

        _count = 1;

        _countDownTiming = ActiveTiming.MOVE;

        _buffActiveTiming = ActiveTiming.MOVE_TURN_START;

        _owner = owner;

        _statBuff = false;

        _dispellable = true;

        _stigmaBuff = false;
    }

    public override void Stack()
    {
        _count++;
    }

    public override bool Active(BattleUnit caster)
    {
        // BattleUnit._isTeleportOn으로 대체

        return false;
    }
}
