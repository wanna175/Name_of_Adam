using System.Collections.Generic;
using UnityEngine;

public class UnitAction_Horus : UnitAction
{
    readonly List<Vector2> UDLR = new() { Vector2.right, Vector2.up, Vector2.left, Vector2.down };
    private int _summonOrder = 1;
    private List<BattleUnit> _summonedUnit = new();
    private bool _isSummon = false;
    private bool _isFall = false;

    public override void AISkillUse(BattleUnit attackUnit)
    {
        if (DirectAttackCheck())
        {
            BattleManager.Instance.EndUnitAction();
            return;
        }

        attackUnit.AnimatorSetBool("isAttack", true);
        GameManager.Instance.PlayAfterCoroutine(() =>
        {
            attackUnit.AnimatorSetBool("isAttack", false);
        }, 3f);

        GameManager.Instance.PlayAfterCoroutine(() =>
        {
            SpawnUnitNearEnemy(attackUnit);
            SpawnUnitNearEnemy(attackUnit);
            BattleManager.Instance.EndUnitAction();
        }, 1.5f);
    }

    public override bool ActionStart(BattleUnit attackUnit, List<BattleUnit> hits, Vector2 coord)
    {
        if (hits.Count != 0 || _isSummon)
            return false;

        _isSummon = true;

        attackUnit.AnimatorSetBool("isAttack", true);
        GameManager.Instance.PlayAfterCoroutine(() =>
        {
            attackUnit.AnimatorSetBool("isAttack", false);
        }, 3f);

        GameManager.Instance.PlayAfterCoroutine(() =>
        {
            SpawnUnit(coord, attackUnit);
            SpawnUnitNearEnemy(attackUnit);
            BattleManager.Instance.EndUnitAction();
        }, 1.5f);

        
        return true;
    }

    public override bool ActionTimingCheck(ActiveTiming activeTiming, BattleUnit caster, BattleUnit receiver)
    {
        if ((activeTiming & ActiveTiming.SUMMON) == ActiveTiming.SUMMON)
        {
            BattleManager.Field.TileDict[caster.Location].ExitTile();
            if (caster.Team == Team.Player)
            {
                SpawnUnit(caster.Location, caster);
                caster.SetLocation(new(-1, -1));
                caster.transform.position = new(-9f, 3f, 0f);
                _isSummon = false;
            }
            else
            {
                caster.SetLocation(new(-1, -1));
                caster.transform.position = new(9f, 3f, 0f);
            }

            GameManager.Resource.Instantiate("BattleUnits/Savior_UI", caster.transform).GetComponent<Savior_UI>().Init(caster);
        }
        else if ((activeTiming & ActiveTiming.ATTACK_TURN_END) == ActiveTiming.ATTACK_TURN_END)
        {
            if (!_isSummon)
            {
                SpawnUnitNearEnemy(caster);
                SpawnUnitNearEnemy(caster);
            }

            _isSummon = false;
        }
        else if ((activeTiming & ActiveTiming.FIELD_UNIT_DEAD) == ActiveTiming.FIELD_UNIT_DEAD)
        {
            if (_summonedUnit.Contains(receiver))
            {
                _summonedUnit.Remove(receiver);
                if (receiver.Team == caster.Team)
                {
                    GameManager.Instance.PlayAfterCoroutine(() =>
                    {
                        caster.AnimatorSetBool("isHit", true);
                        caster.GetAttack(-500, null);
                    }, 1.5f);

                    GameManager.Instance.PlayAfterCoroutine(() =>
                    {
                        caster.AnimatorSetBool("isHit", false);
                    }, 1.8f);
                }
            }
        }
        else if ((activeTiming & ActiveTiming.FIELD_UNIT_FALLED) == ActiveTiming.FIELD_UNIT_FALLED)
        {
            if (_summonedUnit.Contains(receiver))
            {
                _summonedUnit.Remove(receiver);
                caster.ChangeFall(1, caster, FallAnimMode.On);
            }
        }
        else if ((activeTiming & ActiveTiming.FALLED) == ActiveTiming.FALLED)
        {
            if (!_isFall && caster.Team == Team.Enemy)
            {
                BattleManager.BattleUI.UI_TurnChangeButton.SetEnable(false);
                BattleManager.Instance.SetTlieClickCoolDown(4f);

                caster.AnimatorSetBool("isCorrupt", true);
                GameManager.Instance.PlayAfterCoroutine(() =>
                {
                    caster.UnitFallEvent();
                }, 1.5f);

                _isFall = true;

                return true;
            }
            else
            {
                if (BattleManager.Data.BattleUnitList.Find(findUnit => findUnit.Data.ID == "������" && findUnit != caster) != null)
                {
                    while (true)
                    {
                        BattleUnit remainUnit = BattleManager.Data.BattleUnitList.Find(unit => _summonedUnit.Contains(unit));
                        if (remainUnit == null)
                            break;

                        remainUnit.UnitDiedEvent(false);
                    }
                }

                _isFall = false;

                return false;
            }
        }
        else if ((activeTiming & ActiveTiming.ATTACK_TURN_START) == ActiveTiming.ATTACK_TURN_START)
        {
            foreach (Tile tile in BattleManager.Field.TileDict.Values)
            {
                tile.IsColored = true;
                tile.SetColor(BattleManager.Field.ColorList(FieldColorType.Attack));
            }
        }

        return false;
    }

    private void SpawnUnitNearEnemy(BattleUnit unit)
    {
        List<Vector2> summonVectorList = new();

        foreach (BattleUnit listUnit in BattleManager.Data.BattleUnitList)
        {
            if (listUnit.Team != unit.Team)
            {
                foreach (Vector2 udrl in UDLR)
                {
                    Vector2 checkVec = listUnit.Location + udrl;

                    if (BattleManager.Field.GetUnit(checkVec) == null && BattleManager.Field.IsInRange(checkVec))
                    {
                        summonVectorList.Add(checkVec);
                    }
                }
            }
        }

        if (summonVectorList.Count > 0)
        {
            SpawnUnit(summonVectorList[Random.Range(0, summonVectorList.Count)], unit);
        }
        else
        {
            for (int i = 0; i < 100; i++) //������ ū ������ �õ� Ƚ���� 100
            {
                Vector2 randVec = new(Random.Range(0, 6), Random.Range(0, 3));

                if (BattleManager.Field.GetUnit(randVec) == null)
                {
                    SpawnUnit(randVec, unit);
                    break;
                }
            }
        }
    }

    public override void SetUnit(string sender, BattleUnit unit)
    {
        if (sender == "FlowerOfSacrifice")
        {
            if (unit.Data.ID == "�����_��")
            {
                _summonedUnit.Remove(unit);
            }
            else 
            {
                _summonedUnit.Add(unit);
            }
        }
    }

    private void SpawnUnit(Vector2 spawnLocation, BattleUnit unit)
    {
        _isSummon = true;

        SpawnData sd = new();
        sd.unitData = GameManager.Resource.Load<UnitDataSO>($"ScriptableObject/UnitDataSO/�����_��");
        sd.location = spawnLocation;
        sd.team = unit.Team;

        BattleUnit spawnUnit = BattleManager.Spawner.SpawnDataSpawn(sd);
        _summonedUnit.Add(spawnUnit);
        spawnUnit.Action.SetUnit("Savior", unit);
        spawnUnit.Action.SetValue("Savior", _summonOrder);

        _summonOrder = ((_summonOrder % 3) + 1);
    }
}