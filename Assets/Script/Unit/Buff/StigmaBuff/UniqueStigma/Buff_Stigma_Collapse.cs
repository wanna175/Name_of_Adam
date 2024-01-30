using UnityEngine;

public class Buff_Stigma_Collapse : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Collapse;

        _name = "�ر�";

        _description = "��������ũ�� �ƹ��� �ൿ�� ���� ���ϸ�, �ž��� ��� �������ų�, �ٴ����� ������� �ı��˴ϴ�.";

        _count = -1;

        _countDownTiming = ActiveTiming.NONE;

        _buffActiveTiming = ActiveTiming.FALLED;

        _owner = owner;

        _statBuff = false;

        _dispellable = false;

        _stigmaBuff = true;
    }

    public override bool Active(BattleUnit caster)
    {
        _owner.UnitDiedEvent();

        return true;
    }
}