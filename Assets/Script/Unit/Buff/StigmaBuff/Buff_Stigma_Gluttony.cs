using System.Collections.Generic;
using UnityEngine;

public class Buff_Stigma_Gluttony : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Stigmata_Gluttony;

        _name = "Gluttony";

        _description = "Gluttony Info";

        _buffActiveTiming = ActiveTiming.UNIT_TURN_START;

        _owner = owner;

        _stigmataBuff = true;
    }

    public override bool Active(BattleUnit caster)
    {
        List<Buff> debuffList = new();

        foreach (Buff buff in _owner.Buff.BuffList)
        {
            if (buff.IsDebuff)
                debuffList.Add(buff);
        }

        if (debuffList.Count > 0)
        {
            _owner.Buff.DeleteBuff(debuffList[Random.Range(0, debuffList.Count)].BuffEnum);
            Debug.Log(debuffList[Random.Range(0, debuffList.Count)].BuffEnum);
            GameManager.VisualEffect.StartVisualEffect("Arts/EffectAnimation/PlayerSkill/Heal", BattleManager.Field.GetTilePosition(_owner.Location));
            _owner.GetHeal(10, _owner);
        }

        return false;
    }
}