using UnityEngine;

public class Buff_Stigma_FragmentsOfBirth : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Stigmata_FragmentsOfBirth;

        _name = "FragmentsOfBirth";

        _owner = owner;

        _stigmataBuff = true;
    }
}