using UnityEngine;

public class Buff_Stigma_Sacrifice : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Sacrifice;

        _name = "���";

        _description = "�����ڴ� �ڽ��� �ɺ� �Ǵ� ����� ���� ���������� �������� ������\nŸ���ɶ����� �ž��� �����մϴ�.";

        _count = -1;

        _countDownTiming = ActiveTiming.NONE;

        _buffActiveTiming = ActiveTiming.NONE;

        _owner = owner;

        _statBuff = false;

        _dispellable = false;

        _stigmaBuff = true;
    }
}