using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleCutSceneController : MonoBehaviour
{
    [SerializeField] private CameraHandler _cameraHandler;
    public CameraHandler CameraHandler => _cameraHandler;
    [SerializeField] private GameObject _blur;
    [Space(20f)]

    readonly float ZoomTime = 0.5f;

    [Space(10f)]
    [Header("공격 이펙트(X, Y : 이동할 위치, Z : 머무를 시간")]
    [SerializeField] private List<Vector3> _shakeInfo;

    [NonSerialized] public bool IsAttack = false;

    public void InitBattleCutScene(BattleCutSceneData CSData)
    {
        if (CSData.HitUnits.Count == 0)
            return;

        BattleManager.Field.ClearAllColor();
        _cameraHandler.SetCutSceneCamera();

        SetUnitRayer(CSData.AttackUnit, CSData.HitUnits, 5);
        if (!CSData.isPlayerAttack)
            UnitFlip(CSData);
    }

    public IEnumerator AttackCutScene(BattleCutSceneData CSData)
    {
        yield return StartCoroutine(ZoomIn(CSData));

        CSData.AttackUnit.AnimatorSetBool("isAttack", true);

        yield return new WaitUntil(() => IsAttack);
        IsAttack = false;

        UnitAttackAction(CSData);

        yield return StartCoroutine(AttackedEffect(CSData));
        yield return StartCoroutine(ZoomOut(CSData));

        _cameraHandler.SetMainCamera();
        ExitBattleCutScene(CSData);

        yield return new WaitUntil(() => FallCheck(CSData.HitUnits));

        //yield return new WaitForSeconds(1);

        BattleManager.Instance.EndUnitAction();
    }

    private bool FallCheck(List<BattleUnit> units)
    {
        foreach (BattleUnit unit in units)
            if (unit.FallEvent)
                return false;

        return true;
    }

    public void UnitAttackAction(BattleCutSceneData CSData)
    {
        foreach (BattleUnit hit in CSData.HitUnits)
        {
            if (hit == null)
                continue;
            
            //이 함수의 위치 조정 필요
            if (!CSData.isPlayerAttack)
            {
                CSData.AttackUnit.Action.Action(CSData.AttackUnit, hit);
            }
            else
            {
                GameManager.Sound.Play("UI/PlayerSkillSFX/Fall");
                BattleManager.BattleUI.UI_playerHP.DecreaseHP(1);
                BattleManager.BattleUI.UI_playerHP.StartEffect();
                BattleManager.Instance.BattleOverCheck();
            }
            
            if (CSData.AttackUnit.SkillEffectAnim != null)
                GameManager.VisualEffect.StartVisualEffect(CSData.AttackUnit.SkillEffectAnim, hit.transform.position);
        }

        string unitname = CSData.AttackUnit.DeckUnit.Data.Name;
        GameManager.Sound.Play("Character/" + unitname + "/" + unitname + "_Attack");
    }

    public void ExitBattleCutScene(BattleCutSceneData CSData)
    {
        SetUnitRayer(CSData.AttackUnit, CSData.HitUnits, 2);
        _cameraHandler.SetMainCamera();
        BattleManager.Field.ClearAllColor();
        _blur.SetActive(false);
    }

    private void UnitFlip(BattleCutSceneData CSData)
    {
        bool flip = CSData.AttackUnitFlipX;

        CSData.AttackUnit.SetFlipX(!flip);
        foreach (BattleUnit unit in CSData.HitUnits)
            unit.SetFlipX(flip);
    }

    public IEnumerator AttackedEffect(BattleCutSceneData CSData)
    {
        foreach (BattleUnit unit in CSData.HitUnits)
        {
            if (unit != null && !CSData.isPlayerAttack)
                unit.AnimatorSetBool("isHit", true);
        }

        yield return StartCoroutine(_cameraHandler.AttackEffect(_shakeInfo));

        foreach (BattleUnit unit in CSData.HitUnits)
        {
            if (unit != null && !CSData.isPlayerAttack)
                unit.AnimatorSetBool("isHit", false);
        }

        yield return new WaitUntil(() => CSData.AttackUnit.AnimatorIsMotionEnd());

        CSData.AttackUnit.AnimatorSetBool("isAttack", false);
        BattleManager.Instance.ActiveTimingCheck(ActiveTiming.ATTACK_MOTION_END, CSData.AttackUnit);
    }

    public IEnumerator SkillHitEffect(BattleUnit unit)
    {
        if (unit != null)
            unit.AnimatorSetBool("isHit", true);

        yield return new WaitForSeconds(0.3f);

        if (unit != null)
            unit.AnimatorSetBool("isHit", false);
    }

    // 컷씬 지점으로 이동
    public IEnumerator ZoomIn(BattleCutSceneData CSData)
    {
        _blur.SetActive(true);

        StartCoroutine(_cameraHandler.CameraMove(CSData.ZoomLocation, ZoomTime));
        StartCoroutine(_cameraHandler.CameraZoom(CSData.ZoomSize, ZoomTime));
        StartCoroutine(CSData.AttackUnit.CutSceneMove(CSData.MovePosition, ZoomTime));

        yield return new WaitForSeconds(ZoomTime);
    }

    // 원래 위치로 이동
    public IEnumerator ZoomOut(BattleCutSceneData CSData)
    {
        _blur.SetActive(false);

        StartCoroutine(_cameraHandler.CameraMove(_cameraHandler.GetMainPosition(), ZoomTime));
        StartCoroutine(_cameraHandler.CameraZoom(_cameraHandler.GetMainFieldOfView(), ZoomTime));
        StartCoroutine(CSData.AttackUnit.CutSceneMove(BattleManager.Field.GetTilePosition(CSData.AttackUnit.Location), ZoomTime));

        yield return new WaitForSeconds(ZoomTime);
    }

    private void SetUnitRayer(BattleUnit AttackUnit, List<BattleUnit> HitUnits, int rayer)
    {
        AttackUnit.UnitRenderer.sortingOrder = rayer;
        foreach (BattleUnit unit in HitUnits)
        {
            if (unit != null)
            {
                //unit.UnitRenderer.sortingOrder = rayer;
                unit.GetComponent<SpriteRenderer>().sortingOrder = rayer;
            }
        }
    }
}