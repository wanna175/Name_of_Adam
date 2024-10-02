using UnityEngine;
using System.Collections.Generic;

public class Buff_Stigma_Hook : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Stigmata_Hook;

        _name = "Hook";

        _buffActiveTiming = ActiveTiming.AFTER_ATTACK;

        _owner = owner;

        _stigmataBuff = true;
    }

    public override bool Active(BattleUnit caster)
    {
        if (caster == null)
            return false;

        if (caster.Data.ID == "¿æ_¸öÃ¼" || caster.Data.Rarity == Rarity.Boss || caster.Data.ID == "¸®ºñ¿¤")
            return false;

        Vector2 hookDir = (_owner.Location - caster.Location).normalized;
        Vector2Int hookDirInt = new Vector2Int(Mathf.RoundToInt(hookDir.x), Mathf.RoundToInt(hookDir.y));
        Vector2 vec = caster.Location + hookDirInt;

        if (BattleManager.Field.IsInRange(vec) && !BattleManager.Field.TileDict[vec].UnitExist)
            BattleManager.Instance.MoveUnit(caster, vec);

        return false;
    }
}