using UnityEngine;

public class Buff_Stigma_RepeatingSurge : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Stigmata_RepeatingSurge;

        _name = "RepeatingSurge";

        _owner = owner;

        _stigmataBuff = true;
    }
}