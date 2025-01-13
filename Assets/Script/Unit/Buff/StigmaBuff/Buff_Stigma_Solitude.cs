using UnityEngine;
using System.Collections.Generic;

public class Buff_Stigma_Solitude : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Stigmata_Solitude;

        _name = "Solitude";

        _buffActiveTiming = ActiveTiming.ATTACK_TURN_END;

        _owner = owner;

        _stigmataBuff = true;
    }

    public override bool Active(BattleUnit caster)
    {
        List<Vector2> around8List = new() { Vector2.right, Vector2.up, Vector2.left, Vector2.down, 
                                                        Vector2.right+Vector2.up, Vector2.right + Vector2.down,
                                                        Vector2.left + Vector2.up, Vector2.left + Vector2.down, };

        if (BattleManager.Field.GetUnitsInRange(_owner.Location, around8List, _owner.Team).Count == 0)
        {
            _owner.SetBuff(new Buff_AttackBoost());
        }

        return false;
    }
}