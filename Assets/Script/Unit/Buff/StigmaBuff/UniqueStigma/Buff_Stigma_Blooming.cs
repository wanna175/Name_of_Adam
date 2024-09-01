using UnityEngine;

public class Buff_Stigma_Blooming : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Stigmata_Blooming;

        _name = "Blooming";

        _owner = owner;

        _stigmataBuff = true;
    }
}