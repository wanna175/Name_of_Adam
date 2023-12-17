using UnityEngine;

public class Buff_Stigma_Absorption : Buff
{
    public override void Init(BattleUnit owner)
    {
        _name = "흡수";

        _description = "피해량의 30퍼센트를 회복.";

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
        if (!caster.Buff.CheckBuff(BuffEnum.Invincible)) // 무효화 버프가 없을 때만 발동
            _owner.GetHeal((int)(_owner.BattleUnitTotalStat.ATK * 0.3), caster);

        return false;
    }
}