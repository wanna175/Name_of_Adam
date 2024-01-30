using UnityEngine;

public class Buff_Stigma_Repetance : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Repetance;

        _name = "��ȸ";

        _description = "�ش� ������ ü���� ���� �����϶� �ǰ� ����� �ž��� 1 ����߸��ϴ�";

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
        if (caster != null && _owner.BattleUnitTotalStat.MaxHP / 2 >= _owner.HP.GetCurrentHP())
            caster.ChangeFall(1);

        return false;
    }
}