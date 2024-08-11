using UnityEngine;

public class Buff_Stigma_Charge : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Stigmata_Charge;

        _name = "Charge";

        _owner = owner;

        _stigmataBuff = true;
    }
}