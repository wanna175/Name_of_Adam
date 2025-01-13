using UnityEngine;

public class Buff_Smite : Buff
{    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Smite;

        _sprite = GameManager.Resource.Load<Sprite>($"Arts/Buff/Buff_Smite_Sprite");

        _name = "Smite";

        _description = "Smite Info";

        _count = 1;

        _countDownTiming = ActiveTiming.AFTER_ATTACK;

        _buffActiveTiming = ActiveTiming.AFTER_ATTACK;

        _owner = owner;
    }

    public override bool Active(BattleUnit caster)
    {
        if (caster == null)
            return false;

        if (caster.Data.ID == "¿æ_¸öÃ¼" || caster.Data.Rarity == Rarity.Boss || caster.DeckUnit.GetUnitSize() > 1)
            return false;

        Vector2 hookDir = (caster.Location - _owner.Location).normalized;
        Vector2Int hookDirInt = new Vector2Int(Mathf.RoundToInt(hookDir.x), Mathf.RoundToInt(hookDir.y));
        Vector2 vec = caster.Location + hookDirInt;

        if (BattleManager.Field.IsInRange(vec) && !BattleManager.Field.TileDict[vec].UnitExist)
            BattleManager.Instance.MoveUnit(caster, vec, 2f, true);

        return false;
    }

    public override void Stack()
    {
        _count += 1;
    }
}