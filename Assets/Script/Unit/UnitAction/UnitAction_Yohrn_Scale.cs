using System.Collections;
using UnityEngine;

public class UnitAction_Yohrn_Scale : UnitAction
{
    const float _moveSpeed = 1.2f;
    private IEnumerator _moveCoroutine;
    private BattleUnit _yohrnUnit;
    private UnitAction_Yohrn _yohrnAction;
    private BattleUnit _ownerUnit;

    public void Init(BattleUnit yohrn, UnitAction_Yohrn yohrnAction, BattleUnit owner)
    {
        _yohrnUnit = yohrn;
        _yohrnAction = yohrnAction;
        _ownerUnit = owner;
    }

    public void UnitMoveAction(bool isBackMove)
    {
        Vector2 moveVector = _ownerUnit.Location + ((_ownerUnit.Team == Team.Enemy)
            ? (isBackMove ? Vector2.right : Vector2.left)
            : (isBackMove ? Vector2.left : Vector2.right));

        if (BattleManager.Field.IsInRange(moveVector))
        {
            if (BattleManager.Field.GetUnit(moveVector) != null)
                return;

            BattleManager.Field.ExitTile(_ownerUnit.Location);
            BattleManager.Field.EnterTile(_ownerUnit, moveVector);
            if (_moveCoroutine != null)
                BattleManager.Instance.StopCoroutine(_moveCoroutine);
            _moveCoroutine = UnitMoveFieldPosition(BattleManager.Field.GetTilePosition(moveVector), moveVector);
            BattleManager.Instance.StartCoroutine(_moveCoroutine);
        }
        else
        {
            UnitChangeRow(isBackMove);
        }
    }

    private void UnitChangeRow(bool isBackMove)
    {
        int moveX = (_ownerUnit.Team == Team.Enemy) == isBackMove ? 0 : 5;
        int moveY = _ownerUnit.Location.y switch
        {
            0 => isBackMove ? 2 : 1,
            1 => 0,
            2 => isBackMove ? -1 : 0,
            _ => -1
        };

        Vector2 moveVector = new(moveX, moveY);

        if (moveVector.y == -1 || BattleManager.Field.GetUnit(moveVector) != null)
            return;

        BattleManager.Field.ExitTile(_ownerUnit.Location);
        BattleManager.Field.EnterTile(_ownerUnit, moveVector);

        if (_moveCoroutine != null)
            BattleManager.Instance.StopCoroutine(_moveCoroutine);

        _moveCoroutine = UnitPortalMoveAnimation(moveVector);
        BattleManager.Instance.StartCoroutine(_moveCoroutine);
    }

    public IEnumerator UnitMoveFieldPosition(Vector3 moveDestination, Vector2 coord)
    {
        _moveCoroutine = _yohrnAction.MoveFieldPositionCoroutine(_ownerUnit, moveDestination, _moveSpeed);
        yield return BattleManager.Instance.StartCoroutine(_moveCoroutine);
        _ownerUnit.SetLocation(coord);
        BattleManager.Instance.ActiveTimingCheck(ActiveTiming.MOVE, _ownerUnit);
    }

    public IEnumerator UnitPortalMoveAnimation(Vector2 moverLocation)
    {
        yield return BattleManager.Instance.StartCoroutine(UnitPortalMoveAnimationCoroutine(true));
        _ownerUnit.SetLocation(moverLocation);

        yield return BattleManager.Instance.StartCoroutine(UnitPortalMoveAnimationCoroutine(false));
        _ownerUnit.SetLocation(moverLocation);
    }

    public IEnumerator UnitPortalMoveAnimationCoroutine(bool isPortalEnter)
    {
        Vector3 moveDirection = new(_ownerUnit.Location.x == 0 ? -4 : 4, 0, 0);

        Vector3 moveStartPosition = _ownerUnit.transform.position + (isPortalEnter ? Vector3.zero : moveDirection);
        Vector3 moveEndPosition = _ownerUnit.transform.position + (isPortalEnter ? moveDirection : Vector3.zero);

        _ownerUnit.transform.position = moveStartPosition;
        _ownerUnit.UnitRenderer.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;

        _moveCoroutine = _yohrnAction.MoveFieldPositionCoroutine(_ownerUnit, moveEndPosition, _moveSpeed * 1.5f);
        yield return BattleManager.Instance.StartCoroutine(_moveCoroutine);

        _ownerUnit.UnitRenderer.maskInteraction = SpriteMaskInteraction.None;
        _ownerUnit.SetLocation(_ownerUnit.Location);
    }

}