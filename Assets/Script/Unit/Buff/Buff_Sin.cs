using UnityEngine;

public class Buff_Sin : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Sin;

        _name = "����";

        _description = "���� �� Ÿ���� 1 �ο��մϴ�.";

        _count = -1;

        _countDownTiming = ActiveTiming.NONE;

        _buffActiveTiming = ActiveTiming.BEFORE_ATTACK;

        _owner = owner;

        _statBuff = false;

        _dispellable = false;

        _stigmaBuff = false;
    }

    public override bool Active(BattleUnit caster)
    {
        if (caster != null)
            caster.ChangeFall(1);

        return false;
    }
}