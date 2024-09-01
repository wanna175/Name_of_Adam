using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class UnitAction_Yohrn : UnitAction
{
    const float _moveSpeed = 0.3f;
    private bool _firstMoveCheck = false;
    private int _isEvenTileAttackTurn = 0;//0ÀÌ¸é È¦¼ö, 1ÀÌ¸é Â¦¼ö Ä­À» °ø°Ý
    private List<Vector2> _upDownCoord = new() { Vector2.down, Vector2.down * 2, Vector2.zero, Vector2.up, Vector2.up * 2 };

    private List<BattleUnit> _subUnitList = new();
    private Dictionary<Vector2, GameObject> _portalDict = new();

    private bool _isFall = false;

    private bool _actionBlock = false;

    public override void AIMove(BattleUnit attackUnit)
    {
        UnitMoveAction(attackUnit);
    }

    public override void AISkillUse(BattleUnit attackUnit)
    {
        if (_actionBlock)
        {
            BattleManager.Instance.PlayAfterCoroutine(() => {
                AISkillUse(attackUnit);
            }, 0.1f);
            return;
        }

        List<BattleUnit> targetUnits = new();
        List<BattleUnit> attackSubUnits = new();

        foreach (BattleUnit subUnit in _subUnitList)
        {
            if (subUnit.Location.x % 2 == _isEvenTileAttackTurn)
            {
                attackSubUnits.Add(subUnit);
                targetUnits.AddRange(BattleManager.Field.GetUnitsInRange(subUnit.Location, _upDownCoord,
                    (attackUnit.Team == Team.Enemy) ? Team.Player : Team.Enemy));
            }
        }

        _isEvenTileAttackTurn = (_isEvenTileAttackTurn + 1) % 2;

        if (targetUnits.Count > 0)
        {
            _actionBlock = true;
            foreach (BattleUnit subUnit in _subUnitList)
            {
                if (attackSubUnits.Contains(subUnit))
                {
                    subUnit.AnimatorSetBool("isAttack", true);
                    GameManager.VisualEffect.StartVisualEffect("Arts/EffectAnimation/AttackEffect/Yohrn_Attack",
                        BattleManager.Field.GetTilePosition(subUnit.Location) + new Vector3(0f, 1f, 0f));
                }

                subUnit.UnitRenderer.sortingOrder = 5;
            }

            BattleManager.Instance.AttackStart(attackUnit, targetUnits.Distinct().ToList(), true);
        }
        else
        {
            _actionBlock = false;
            BattleManager.Instance.EndUnitAction();
        }
    }

    private void UnitMoveAction(BattleUnit attackUnit)
    {
        if (_actionBlock)
        {
            BattleManager.Instance.PlayAfterCoroutine(() => {
                UnitMoveAction(attackUnit);
            }, 0.1f);
            return;
        }

        Vector2 moveDirectionVector = (attackUnit.Team == Team.Enemy) ? Vector2.left : Vector2.right;
        Vector2 moveOppositeVector = (attackUnit.Team == Team.Enemy) ? Vector2.right : Vector2.left;

        if (BattleManager.Field.IsInRange(attackUnit.Location + moveDirectionVector))
        {
            BattleUnit frontUnit = BattleManager.Field.TileDict[attackUnit.Location + moveDirectionVector].Unit;
            if (frontUnit != null)
            {
                frontUnit.UnitDiedEvent(false);
                attackUnit.AnimatorSetBool("isMoveAttack", true);
                BattleManager.Instance.PlayAfterCoroutine(() => {
                    attackUnit.AnimatorSetBool("isMoveAttack", false);
                }, 2f);
                //Á×ÀÌ±â
            }

            BattleManager.Instance.MoveUnit(attackUnit, attackUnit.Location + moveDirectionVector, _moveSpeed);
            foreach (BattleUnit subUnit in _subUnitList)
            {
                SubUnitMoveAction(subUnit, false);
            }

            SubUnitSpawn(attackUnit);

            if (!_firstMoveCheck)
            {
                //ÇÑ ¹ø ´õ Àç±ÍÀûÀ¸·Î ½ÇÇà
                _firstMoveCheck = true;
                BattleManager.Instance.PlayAfterCoroutine(() => {
                    UnitMoveAction(attackUnit);
                }, 3f);
            }
            else
            {
                _firstMoveCheck = false;
                BattleManager.Instance.PlayAfterCoroutine(() => {
                    BattleManager.Phase.ChangePhase(BattleManager.Phase.Action);
                }, 3f);
            }
        }
        else
        {
            UnitChangeRow(attackUnit, false);
        }
    }

    private void UnitChangeRow(BattleUnit attackUnit, bool isBackMove)
    {
        if (_actionBlock)
        {
            BattleManager.Instance.PlayAfterCoroutine(() => {
                UnitChangeRow(attackUnit, isBackMove);
            }, 0.1f);
            return;
        }

        Vector2 moveVector = isBackMove
            ? new Vector2(0, attackUnit.Location.y switch
            {
                0 => 2, 1 => 0, _ => -1
            })
            : new Vector2(5, attackUnit.Location.y switch
            {
                2 => 0, 0 => 1, _ => -1
            });

        if (moveVector.y == -1)
            return;

        _actionBlock = true;

        BattleUnit frontUnit = BattleManager.Field.TileDict[moveVector].Unit;
        if (frontUnit != null)
        {
            BattleManager.Field.ExitTile(frontUnit.Location);
            BattleManager.Instance.PlayAfterCoroutine(() => {
                frontUnit.UnitDiedEvent(false);
                attackUnit.AnimatorSetBool("isMoveAttack", true);
                BattleManager.Instance.PlayAfterCoroutine(() => {
                    attackUnit.AnimatorSetBool("isMoveAttack", false);
                }, 2f);
            }, 3f);
            //Á×ÀÌ±â
        }

        if (!_portalDict.ContainsKey(attackUnit.Location))
            CreatePortal(attackUnit.Location);

        BattleManager.Field.ExitTile(attackUnit.Location);
        BattleManager.Field.EnterTile(attackUnit, moveVector);
        
        BattleManager.Instance.StartCoroutine(UnitPortalMoveAnimation(attackUnit, moveVector));

        foreach (BattleUnit subUnit in _subUnitList)
        {
            SubUnitMoveAction(subUnit, false);
        }
        SubUnitSpawn(attackUnit);

        if (!_firstMoveCheck)
        {
            //ÇÑ ¹ø ´õ Àç±ÍÀûÀ¸·Î ½ÇÇà
            _firstMoveCheck = true;
            BattleManager.Instance.PlayAfterCoroutine(() => {
                UnitMoveAction(attackUnit);
            }, 2f);
        }
        else
        {
            _firstMoveCheck = false;
            BattleManager.Instance.PlayAfterCoroutine(() => {
                BattleManager.Phase.ChangePhase(BattleManager.Phase.Action);
            }, 2f);
        }
    }

    public BattleUnit SubUnitSpawn(BattleUnit caster)
    {
        SpawnData sd = new();
        sd.unitData = GameManager.Resource.Load<UnitDataSO>($"ScriptableObject/UnitDataSO/¿æ_¸öÃ¼");
        sd.location = (caster.Team == Team.Enemy) ? new(5, 2) : new(0, 2);
        sd.team = caster.Team;

        BattleUnit spawnUnit = BattleManager.Spawner.SpawnDataSpawn(sd);

        if (spawnUnit == null)
            return null;

        _subUnitList.Add(spawnUnit);
        _subUnitList = _subUnitList
            .OrderBy(unit =>
                unit.Location.y == 1 ? 0 :
                unit.Location.y == 0 ? 1 :
                unit.Location.y == 2 ? 2 : 3)  // y°ªÀ» Æ¯Á¤ ¼ø¼­·Î Á¤·Ä
            .ThenBy(unit => (unit.Team == Team.Enemy) ? unit.Location.x : -unit.Location.x)
            .ToList();

        BattleManager.Instance.StartCoroutine(UnitPortalMoveAnimationCoroutine(spawnUnit, false));

        return spawnUnit;
    }

    public void SubUnitMoveAction(BattleUnit unit, bool isBackMove)
    {
        Vector2 moveVector = (unit.Team == Team.Enemy)
            ? (isBackMove ? Vector2.right : Vector2.left)
            : (isBackMove ? Vector2.left : Vector2.right);

        if (BattleManager.Field.IsInRange(unit.Location + moveVector))
        {
            BattleManager.Instance.MoveUnit(unit, unit.Location + moveVector, _moveSpeed, isBackMove);
        }
        else
        {
            SubUnitChangeRow(unit, isBackMove);
        }
    }

    private void SubUnitChangeRow(BattleUnit unit, bool isBackMove)
    {
        Vector2 moveVector = isBackMove
            ? new Vector2(0, unit.Location.y switch
            {
                0 => 2, 1 => 0, _ => -1
            })
            : new Vector2(5, unit.Location.y switch
            {
                2 => 0, 0 => 1, _ => -1
            });

        if (moveVector.y == -1 || BattleManager.Field.GetUnit(moveVector) != null)
            return;

        BattleManager.Field.ExitTile(unit.Location);
        BattleManager.Field.EnterTile(unit, moveVector);
        unit.SetLocation(moveVector);
    }

    public IEnumerator UnitPortalMoveAnimation(BattleUnit unit, Vector2 moverLocation)
    {
        yield return BattleManager.Instance.StartCoroutine(UnitPortalMoveAnimationCoroutine(unit, true));
        unit.SetLocation(moverLocation);

        if (!_portalDict.ContainsKey(unit.Location))
            CreatePortal(unit.Location);

        yield return BattleManager.Instance.StartCoroutine(UnitPortalMoveAnimationCoroutine(unit, false));
        unit.SetLocation(moverLocation);

        _actionBlock = false;
    }

    public IEnumerator UnitPortalMoveAnimationCoroutine(BattleUnit unit, bool isPortalEnter)
    {
        Vector3 moveDirection = new(unit.Location.x == 0 ? -4 : 4, 0, 0);

        Vector3 moveStartPosition = unit.transform.position + (isPortalEnter ? Vector3.zero : moveDirection);
        Vector3 moveEndPosition = unit.transform.position + (isPortalEnter ? moveDirection : Vector3.zero);

        unit.transform.position = moveStartPosition;
        unit.UnitRenderer.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;

        yield return BattleManager.Instance.StartCoroutine(unit.MoveFieldPositionCoroutine(moveEndPosition, false, _moveSpeed));

        unit.UnitRenderer.maskInteraction = SpriteMaskInteraction.None;
        unit.SetLocation(unit.Location);
    }

    private void CreatePortal(Vector2 location)
    {
        Vector3 portalTransformPosition = BattleManager.Field.GetTilePosition(location);
        portalTransformPosition.x += location.x == 0 ? -2 : 2;
        portalTransformPosition.y += 1.5f;

        GameObject portal = GameManager.Resource.Instantiate("BattleUnits/Yohrn_Portal");
        portal.transform.position = portalTransformPosition;

        if (location.x == 0)
            portal.transform.rotation = new(0f, 180f, 0f, 0f);

        _portalDict.Add(location, portal);
    }


    public override bool ActionTimingCheck(ActiveTiming activeTiming, BattleUnit caster, BattleUnit receiver)
    {
        if ((activeTiming & ActiveTiming.SUMMON) == ActiveTiming.SUMMON)
        {
            CreatePortal(caster.Location);
            BattleManager.Instance.StartCoroutine(UnitPortalMoveAnimationCoroutine(caster, false));
        }
        else if ((activeTiming & ActiveTiming.BEFORE_CHANGE_FALL) == ActiveTiming.BEFORE_CHANGE_FALL)
        {
            return receiver != caster;
        }
        else if ((activeTiming & ActiveTiming.FIELD_UNIT_DEAD) == ActiveTiming.FIELD_UNIT_DEAD)
        {
            if (!_subUnitList.Contains(receiver))
                return false;

            _subUnitList.Remove(receiver);
            if (receiver.Team == caster.Team)
            {
                caster.GetAttack(-receiver.BattleUnitTotalStat.MaxHP, null);

                Vector2 moveDirectionVector = (caster.Team == Team.Enemy) ? Vector2.left : Vector2.right;
                Vector2 moveOppositeVector = (caster.Team == Team.Enemy) ? Vector2.right : Vector2.left;

                for (int i = _subUnitList.Count - 1; i >= 0; i--)
                {
                    SubUnitMoveAction(_subUnitList[i], true);
                    Debug.Log(_subUnitList[i].Location);
                }

                if (BattleManager.Field.IsInRange(caster.Location + moveOppositeVector))
                {
                    BattleManager.Instance.MoveUnit(caster, caster.Location + moveOppositeVector, _moveSpeed, true);
                }
                else
                {
                    UnitChangeRow(caster, true);
                }
            }
        }
        else if ((activeTiming & ActiveTiming.ATTACK_TURN_START) == ActiveTiming.ATTACK_TURN_START)
        {
            foreach (BattleUnit subUnit in _subUnitList)
            {
                Tile tile = BattleManager.Field.TileDict[subUnit.Location];
                tile.IsColored = true;
                tile.SetColor(BattleManager.Field.ColorList(FieldColorType.Attack));

                if (subUnit.Location.x % 2 != _isEvenTileAttackTurn)
                    continue;

                foreach (Vector2 vec in _upDownCoord)
                {
                    Vector2 targetLocation = vec + subUnit.Location;

                    if (BattleManager.Field.IsInRange(targetLocation))
                    {
                        tile = BattleManager.Field.TileDict[targetLocation];
                        tile.IsColored = true;
                        tile.SetColor(BattleManager.Field.ColorList(FieldColorType.Attack));
                    }
                }
            }
        }
        else if ((activeTiming & ActiveTiming.ATTACK_TURN_END) == ActiveTiming.ATTACK_TURN_END)
        {
            _actionBlock = false;
        }
        else if ((activeTiming & ActiveTiming.FIELD_UNIT_FALLED) == ActiveTiming.FIELD_UNIT_FALLED)
        {
            if (_subUnitList.Contains(receiver))
            {
                _subUnitList.Remove(receiver);
                caster.ChangeFall(1, caster, FallAnimMode.On);
            }
        }
        else if ((activeTiming & ActiveTiming.AFTER_UNIT_DEAD) == ActiveTiming.AFTER_UNIT_DEAD)
        {
            if (BattleManager.Data.BattleUnitList.Find(findUnit => findUnit.Data.ID == "¿æ" && findUnit != caster) != null)
            {
                while (true)
                {
                    BattleUnit remainUnit = BattleManager.Data.BattleUnitList.Find(findUnit => findUnit.Data.ID == "¿æ_¸öÃ¼" && findUnit.Team == caster.Team);
                    if (remainUnit == null)
                        break;

                    remainUnit.UnitDiedEvent(false);
                }
            }
        }
        else if ((activeTiming & ActiveTiming.FALLED) == ActiveTiming.FALLED)
        {
            if (!_isFall && caster.Team == Team.Enemy)
            {
                BattleManager.BattleUI.UI_TurnChangeButton.SetEnable(false);
                BattleManager.Instance.SetTlieClickCoolDown(4f);

                caster.AnimatorSetBool("isCorrupt", true);
                BattleManager.Instance.PlayAfterCoroutine(() =>
                {
                    caster.UnitFallEvent();
                }, 2f);

                _isFall = true;

                return true;
            }
            else
            {
                if (BattleManager.Data.BattleUnitList.Find(findUnit => findUnit.Data.ID == "¿æ" && findUnit != caster) != null)
                {
                    while (true)
                    {
                        BattleUnit remainUnit = BattleManager.Data.BattleUnitList.Find(findUnit => findUnit.Data.ID == "¿æ_¸öÃ¼" && findUnit.Team == caster.Team);
                        if (remainUnit == null)
                            break;

                        remainUnit.UnitFallEvent();
                    }
                }

                _isFall = false;

                return false;
            }
        }
        else if ((activeTiming & ActiveTiming.BEFORE_ATTACK) == ActiveTiming.BEFORE_ATTACK)
        {
            if (receiver != null)
            {
                receiver.ChangeFall(1, caster, FallAnimMode.On);
            }
        }
        else if ((activeTiming & ActiveTiming.ATTACK_MOTION_END) == ActiveTiming.ATTACK_MOTION_END)
        {
            foreach (BattleUnit subUnit in _subUnitList)
            {
                subUnit.AnimatorSetBool("isAttack", false);
                subUnit.UnitRenderer.sortingOrder = 2;
            }
        }

        return false;
    }
}