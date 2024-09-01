using UnityEngine;

public class Buff_Stigma_LegacyOfBabel : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Stigmata_LegacyOfBabel;

        _name = "LegacyOfBabel";

        _owner = owner;

        _stigmataBuff = true;
    }
}