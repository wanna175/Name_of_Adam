using UnityEngine;

public class Buff_Stigma_BlindFaith : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Stigmata_BlindFaith;

        _name = "BlindFaith";

        _description = "BlindFaith Info";

        _buffActiveTiming = ActiveTiming.BEFORE_BUFFED;

        _owner = owner;

        _stigmataBuff = true;
    }

    public override bool Active(BattleUnit caster)
    {
        return true;
    }

    public override bool Active(Buff buff)
    {
        if (buff.BuffEnum == BuffEnum.Malevolence || buff.StigmataBuff || buff.IsSystemBuff)
        {
            return false;
        }
        else
        {
            _owner.SetBuff(new Buff_Malevolence());
            return true;
        }
    }
}