using UnityEngine;

public class Buff_Stigma_Blooming : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Blooming;

        _name = "��ȭ";

        _description = "����� ���� ��ȭ�ϸ� �������� �ɺ��� �˴ϴ�.";

        _count = -1;

        _countDownTiming = ActiveTiming.NONE;

        _buffActiveTiming = ActiveTiming.NONE;

        _owner = owner;

        _statBuff = false;

        _dispellable = false;

        _stigmaBuff = true;
    }
}