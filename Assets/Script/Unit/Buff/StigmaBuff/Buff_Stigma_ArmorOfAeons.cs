using UnityEngine;

public class Buff_Stigma_ArmorOfAeons : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Stigmata_ArmorOfAeons;

        _name = "Armor Of Aeons";

        _description = "Armor Of Aeons Info";

        _buffActiveTiming = ActiveTiming.STIGMA | ActiveTiming.MOVE_TURN_START;

        _owner = owner;

        _stigmataBuff = true;
    }

    public override bool Active(BattleUnit caster)
    {
        if (!IsActive)
        {
            _owner.SetBuff(new Buff_Invincibility());
            _owner.SetBuff(new Buff_Invincibility());
            IsActive = true;

            return false;
        }

        return _owner.Buff.CheckBuff(BuffEnum.Invincibility);
    }
}