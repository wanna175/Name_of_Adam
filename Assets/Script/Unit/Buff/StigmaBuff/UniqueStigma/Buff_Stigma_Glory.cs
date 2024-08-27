using UnityEngine;

public class Buff_Stigma_Glory : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Stigmata_Glory;

        _name = "Glory";

        _owner = owner;

        _stigmataBuff = true;
    }
}