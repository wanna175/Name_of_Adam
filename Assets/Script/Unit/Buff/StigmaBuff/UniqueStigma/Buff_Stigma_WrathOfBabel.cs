using UnityEngine;

public class Buff_Stigma_WrathOfBabel : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Stigmata_WrathOfBabel;

        _name = "WrathOfBabel";

        _owner = owner;

        _stigmataBuff = true;
    }
}