using UnityEngine;

public class Buff_Stigma_Blessing : Buff
{
    int _mana = 0;

    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Stigmata_Blessing;

        _name = "Blessing";

        _buffActiveTiming = ActiveTiming.AFTER_ATTACK;

        _owner = owner;

        _stigmataBuff = true;
    }

    public override bool Active(BattleUnit caster)
    {
        BattleManager.Mana.ChangeMana(_mana);

        return false;
    }

    public override void SetValue(int num)
    {
        _mana += num;
    }
}