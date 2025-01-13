using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_Stigma_Assasination : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Stigmata_Assasination;

        _name = "Assasination";

        _owner = owner;

        _stigmataBuff = true;
    }

    public override bool Active(BattleUnit caster)
    {
        // 이 버프는 BattleUnit이 아닌, BattleManager에서 직접 수행됩니다.

        return false;
    }
}
