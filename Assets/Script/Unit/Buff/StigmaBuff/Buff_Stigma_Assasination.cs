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
        // �� ������ BattleUnit�� �ƴ�, BattleManager���� ���� ����˴ϴ�.

        return false;
    }
}
