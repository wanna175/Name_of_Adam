using UnityEngine;

public class Buff_Stigma_ThornsOfOblivion : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Stigmata_ThornsOfOblivion;

        _name = "ThornsOfOblivion";

        _owner = owner;

        _stigmataBuff = true;
    }
}