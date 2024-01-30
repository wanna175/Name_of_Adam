using UnityEngine;

public class Buff_Stigma_LegacyOfBabel : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.LegacyOfBabel;

        _name = "�ٺ��� ����";

        _description = "�ڽ��� �ϸ��� ���� Ÿ�Ͽ� ��������ũ�� ��ġ�մϴ�.";

        _count = -1;

        _countDownTiming = ActiveTiming.NONE;

        _buffActiveTiming = ActiveTiming.FALLED;

        _owner = owner;

        _statBuff = false;

        _dispellable = false;

        _stigmaBuff = true;
    }


}