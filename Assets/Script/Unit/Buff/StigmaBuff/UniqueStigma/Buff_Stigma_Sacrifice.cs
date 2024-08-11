using UnityEngine;

public class Buff_Stigma_Sacrifice : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Stigmata_Sacrifice;

        _name = "Sacrifice";

        _owner = owner;

        _stigmataBuff = true;
    }
}