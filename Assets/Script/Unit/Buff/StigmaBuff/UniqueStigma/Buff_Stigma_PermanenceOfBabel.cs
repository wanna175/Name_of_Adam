using UnityEngine;

public class Buff_Stigma_PermanenceOfBabel : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Stigmata_PermanenceOfBabel;

        _name = "PermanenceOfBabel";

        _owner = owner;

        _stigmataBuff = true;
    }
}