using UnityEngine;

public class Buff_Stigma_Trinity : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Stigmata_Trinity;

        _name = "Trinity";

        _owner = owner;

        _stigmataBuff = true;
    }
}