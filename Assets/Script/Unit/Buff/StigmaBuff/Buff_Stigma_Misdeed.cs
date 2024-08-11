using UnityEngine;

public class Buff_Stigma_Misdeed : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Stigmata_Misdeed;

        _name = "Misdeed";

        _buffActiveTiming = ActiveTiming.STIGMA;

        _owner = owner;

        _stigmataBuff = true;
    }

    public override bool Active(BattleUnit caster)
    {
        if (!_owner.Buff.CheckBuff(BuffEnum.Malevolence))
            for (int i = 0; i < 2; i++)
                _owner.SetBuff(new Buff_Malevolence());

        return false;
    }
}