using UnityEngine;

public class Buff_AfterAttackBounce : Buff
{
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.AfterAttackBounce;

        _name = "°ø°Ý ÈÄ º¹±Í";

        _description = "°ø°Ý ÈÄ º¹±Í";

        _count = 1;

        _countDownTiming = ActiveTiming.ATTACK_TURN_END;

        _buffActiveTiming = ActiveTiming.ATTACK_TURN_END;

        _owner = owner;

        _statBuff = false;

        _dispellable = false;

        _stigmaBuff = false;
    }

    public override bool Active(BattleUnit caster)
    {
        BattleManager.Data.BattleUnitList.Remove(_owner);
        BattleManager.Data.BattleUnitRemoveFromOrder(_owner);
        BattleManager.Data.AddDeckUnit(_owner.DeckUnit);
        BattleManager.BattleUI.FillHand();
        BattleManager.Field.FieldCloseInfo(BattleManager.Field.TileDict[_owner.Location]);
        Destroy(_owner.gameObject);

        return false;
    }
}