using UnityEngine;

public class Buff_Stigma_Atonement : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Stigmata_Atonement;

        _name = "Atonement";

        _owner = owner;

        _stigmataBuff = true;
    }
}