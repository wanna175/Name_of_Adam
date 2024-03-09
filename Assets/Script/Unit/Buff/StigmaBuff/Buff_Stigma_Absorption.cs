using UnityEngine;

public class Buff_Stigma_Absorption : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Absorption;

        _name = "���";

        _description = "���ط��� 30�ۼ�Ʈ�� ȸ��.";

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
        if (caster != null && !caster.Buff.CheckBuff(BuffEnum.Invincible)) // ��ȿȭ ������ ���� ���� �ߵ�
            _owner.GetHeal((int)(_owner.BattleUnitTotalStat.ATK * 0.3), caster);

        return false;
    }
}