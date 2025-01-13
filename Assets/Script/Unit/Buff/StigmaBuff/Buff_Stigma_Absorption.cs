using UnityEngine;

public class Buff_Stigma_Absorption : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Stigmata_Absorption;

        _name = "Absorption";

        _buffActiveTiming = ActiveTiming.BEFORE_ATTACK;

        _owner = owner;

        _stigmataBuff = true;
    }

    public override bool Active(BattleUnit caster)
    {
        if (caster != null && !caster.Buff.CheckBuff(BuffEnum.Invincibility)) // ��ȿȭ ������ ���� ���� �ߵ�
            _owner.GetHeal((int)(_owner.BattleUnitTotalStat.ATK * 0.3), caster);

        return false;
    }
}