using UnityEngine;

public class Buff_Stigma_Regeneration : Buff
{
    int hpUp = 5;
    public override void Init(BattleUnit caster, BattleUnit owner)
    {
        _buffEnum = BuffEnum.Regeneration;

        _name = "재생력";

        _description = "해당 유닛의 턴이 끝날때마다 체력을 5 회복합니다.";

        _count = -1;

        _countDownTiming = ActiveTiming.NONE;

        _buffActiveTiming = ActiveTiming.ATTACK_TURN_END;

        _caster = caster;

        _owner = owner;

        _statBuff = false;

        _dispellable = false;

        _stigmaBuff = true;
    }

    public override bool Active(BattleUnit caster, BattleUnit receiver)
    {
        _owner.GetHeal(hpUp, caster);

        return false;
    }

    public override void SetValue(int num)
    {
        hpUp = num;
    }
}