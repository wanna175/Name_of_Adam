using UnityEngine;

public class Buff_Stigma_AbyssalSteps : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Stigmata_AbyssalSteps;

        _name = "AbyssalSteps";

        _owner = owner;

        _stigmataBuff = true;
    }
}