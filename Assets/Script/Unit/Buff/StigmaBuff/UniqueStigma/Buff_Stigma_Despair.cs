using System.Collections.Generic;
using UnityEngine;

public class Buff_Stigma_Despair : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Stigmata_Despair;

        _name = "Despair";

        _buffActiveTiming = ActiveTiming.STIGMA;

        _owner = owner;

        _stigmataBuff = true;

        SetStigmataBuff();
    }

    private void SetStigmataBuff()
    {
        int filedUnitCount = 0;
        foreach (BattleUnit unit in BattleManager.Data.BattleUnitList)
        {
            if (unit.Team == _owner.Team)
                filedUnitCount++;
        }

        List<Stigma> stigmataList = GameManager.Data.StigmaController.GetRandomStigmaList(_owner.DeckUnit, filedUnitCount);
        foreach (Stigma stigmata in stigmataList)
        {
            stigmata.Use(_owner);
            Buff_Despair despair = new();
            despair.SetStigmata(stigmata);
            Debug.Log(stigmata.Name);
            _owner.SetBuff(despair);
        }
    }
}