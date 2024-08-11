using UnityEngine;

public class Buff_Stigma_KillingSpree : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.KillingSpree;

        _name = "KillingSpree";

        _buffActiveTiming = ActiveTiming.UNIT_KILL;

        _owner = owner;

        _stigmataBuff = true;
    }

    public override bool Active(BattleUnit caster)
    {
        if (caster.Team != _owner.Team && !_owner.Buff.CheckBuff(BuffEnum.KillingSpree))
        {
            _owner.SetBuff(new Buff_KillingSpree());
            BattleManager.Data.BattleOrderInsert(0, _owner, _owner.BattleUnitTotalStat.SPD);
        }

        return false;
    }
}
