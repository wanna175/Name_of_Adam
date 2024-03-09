using UnityEngine;

public class Buff_Stigma_Martyrdom : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Martyrdom;

        _name = "����";

        _description = "�ش� ������ ��ȭ�������� ���� �̻� ���ִ� ���¿��� ���� �� �ǰ� ������ �ž��� 1 ����߸��ϴ�";

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
        if (caster != null && _owner.BattleUnitTotalStat.FallMaxCount - _owner.Fall.GetCurrentFallCount() <= 2)
            caster.ChangeFall(1);

        return false;
    }
}