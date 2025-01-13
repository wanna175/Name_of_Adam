using UnityEngine;

public class Buff_Stigma_Symbiosis : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Stigmata_Symbiosis;

        _name = "Symbiosis";

        _owner = owner;

        _stigmataBuff = true;
    }
}