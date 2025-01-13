using UnityEngine;

public class Buff_Stigma_Birth : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Stigmata_Birth;

        _name = "Birth";

        _owner = owner;

        _stigmataBuff = true;
    }
}