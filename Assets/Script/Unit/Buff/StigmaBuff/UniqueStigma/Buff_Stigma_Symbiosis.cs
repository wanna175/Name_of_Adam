using UnityEngine;

public class Buff_Stigma_Symbiosis : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.LegacyOfBabel;

        _name = "����";

        _description = "�� ���� �����ϴ� ����� ���ư� ���� ���� �����ϴ�.\n������ ���� �� ������ ������ ������ �ο��ϸ� ���� ���ķ� �̵��մϴ�.\n���ƴ� �̵��� �� ������ ���� ���� ���� �������� �����մϴ�.";

        _count = -1;

        _countDownTiming = ActiveTiming.NONE;

        _buffActiveTiming = ActiveTiming.NONE;

        _owner = owner;

        _statBuff = false;

        _dispellable = false;

        _stigmaBuff = true;
    }
}