using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_Stigma_Additional_Punishment : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Additional_Punishment;

        _name = "���� ó��";

        _description = "���� �� ������ ����� �ƴ� ���� ���� �� �ϳ��� �߰��� �����մϴ�.";

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
        // BattleManager-ActionPhaseClick���� ������
        return false;
    }
}
