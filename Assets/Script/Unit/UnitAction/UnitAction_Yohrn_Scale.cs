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
        _moveCoroutine = MoveFieldPositionCoroutine(_ownerUnit, moveDestination, _moveSpeed);
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

        _moveCoroutine = MoveFieldPositionCoroutine(_ownerUnit, moveEndPosition, _moveSpeed * 1.5f);
        yield return BattleManager.Instance.StartCoroutine(_moveCoroutine);

        _ownerUnit.UnitRenderer.maskInteraction = SpriteMaskInteraction.None;
        _ownerUnit.SetLocation(_ownerUnit.Location);
    }


    public IEnumerator MoveFieldPosition(BattleUnit unit, Vector3 moveDestination, Vector2 coord)
    {
        _moveCoroutine = MoveFieldPositionCoroutine(unit, moveDestination, _moveSpeed);
        yield return BattleManager.Instance.StartCoroutine(_moveCoroutine);
        unit.SetLocation(coord);
        BattleManager.Instance.ActiveTimingCheck(ActiveTiming.MOVE, unit);
    }

    public IEnumerator MoveFieldPositionCoroutine(BattleUnit unit, Vector3 moveDestination, float moveSpeed)
    {
        //잠재적 버그 가능성(스케일 안 바뀌기 확인해보기)
        float addScale = unit.transform.localScale.x;

        unit.SetFlipX(((unit.Team == Team.Enemy) ? Vector2.left : Vector2.right) == Vector2.left);

        Vector3 pVel = Vector3.zero;
        Vector3 sVel = Vector3.zero;

        float moveTime = 1f / moveSpeed;

        while (Vector3.Distance(moveDestination, unit.transform.position) >= 0.03f)
        {
            unit.transform.position = Vector3.SmoothDamp(unit.transform.position, moveDestination, ref pVel, moveTime);
            unit.transform.localScale = Vector3.SmoothDamp(unit.transform.localScale, new(addScale, addScale, 1), ref sVel, moveTime);

            yield return null;
        }

        pVel = Vector3.zero;
    }

    public override bool ActionTimingCheck(ActiveTiming activeTiming, BattleUnit caster, BattleUnit receiver)
    {
        if ((activeTiming & ActiveTiming.TURN_END) == ActiveTiming.TURN_END)
        {
            if (caster.Buff.CheckBuff(BuffEnum.Scale))
            {
                caster.DeleteBuff(BuffEnum.Scale);
                caster.AnimatorSetBool("isClose", true);
            }
            else
            {
                caster.SetBuff(new Buff_Scale());
                caster.AnimatorSetBool("isClose", false);
            }
        }

            return false;
    }
}