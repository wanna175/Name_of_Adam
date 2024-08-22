using System.Collections.Generic;
using UnityEngine;

public class Buff_Stigma_Fortification : Buff
{
    readonly List<Vector2> UDLR = new() { Vector2.right, Vector2.up, Vector2.left, Vector2.down };

    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Stigmata_Fortification;

        _name = "Fortification";

        _description = "Fortification Info";

        _buffActiveTiming = ActiveTiming.STIGMA;

        _owner = owner;

        _stigmataBuff = true;
    }

    public override bool Active(BattleUnit caster)
    {
        List<Vector2> nonUnitLocation = new();

        foreach (Vector2 vec in UDLR)
        {
            if (BattleManager.Field.IsInRange(_owner.Location + vec) && !BattleManager.Field.TileDict[_owner.Location + vec].UnitExist)
            {
                nonUnitLocation.Add(_owner.Location + vec);
            }
        }

        if (nonUnitLocation.Count > 0)
        {
            SpawnData sd = new();
            sd.unitData = GameManager.Resource.Load<UnitDataSO>($"ScriptableObject/UnitDataSO/오벨리스크");
            sd.location = nonUnitLocation[Random.Range(0, nonUnitLocation.Count)];
            sd.team = _owner.Team;

            BattleManager.Spawner.SpawnDataSpawn(sd);
        }

        return false;
    }
}