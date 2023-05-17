using UnityEngine;

public class 망령 : Passive
{
    public override void Use(BattleUnit caster, BattleUnit receiver)
    {
        Vector2 moveVec = receiver.Location + (receiver.Location - caster.Location);

        if (!BattleManager.Field.TileDict.ContainsKey(moveVec))
            return;

        if (!BattleManager.Field.TileDict[moveVec].UnitExist)
            BattleManager.Field.MoveUnit(caster.Location, moveVec);
    }
}