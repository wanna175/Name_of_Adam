using UnityEngine;

public class Buff_Stigma_BloodOath: Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Stigmata_BloodOath;

        _name = "Blood Oath";

        _description = "Blood Oath Info";

        _buffActiveTiming = ActiveTiming.FIELD_UNIT_DEAD;

        _owner = owner;

        _stigmataBuff = true;
    }

    public override bool Active(BattleUnit caster)
    {
        if (caster.Team == _owner.Team && BattleManager.Field.GetArroundUnits(caster.Location).Contains(_owner))
        {
            _owner.SetBuff(new Buff_KillingSpree());
        }

        return false;
    }
}