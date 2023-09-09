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

    public IEnumerator AttackCutScene(BattleCutSceneData CSData)
    {
        _cameraHandler.SetCutSceneCamera();
        yield return StartCoroutine(ZoomIn(CSData));

        CSData.AttackUnit.GetComponent<Animator>().SetBool("isAttack", true);

        yield return new WaitUntil(() => IsAttack);
        IsAttack = false;

        BattleManager.Instance.UnitAttackAction(CSData.HitUnits);

        yield return StartCoroutine(AfterEffect(CSData));
        yield return StartCoroutine(ZoomOut(CSData));

        _cameraHandler.SetMainCamera();
        ExitBattleCutScene(CSData);

        yield return new WaitUntil(() => FallCheck(CSData.HitUnits));

        yield return new WaitForSeconds(1);

        BattleManager.Instance.EndUnitAction();
    }

    private bool FallCheck(List<BattleUnit> units)
    {
        foreach (BattleUnit unit in units)
            if (unit.FallEvent)
                return false;

        return true;
    }

    public void InitBattleCutScene(BattleCutSceneData CSData)
    {
        if (CSData.HitUnits.Count == 0)
            return;

        BattleManager.Field.ClearAllColor();
        CameraHandler.SetCutSceneCamera();
        UnitFlip(CSData);
        SetUnitRayer(CSData.AttackUnit, CSData.HitUnits, 5);
    }

    public void ExitBattleCutScene(BattleCutSceneData CSData)
    {
        SetUnitRayer(CSData.AttackUnit, CSData.HitUnits, 2);
        CameraHandler.SetMainCamera();
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

    public IEnumerator AfterEffect(BattleCutSceneData CSData)
    {
        foreach (BattleUnit unit in CSData.HitUnits)
        {
            if (unit != null)
                unit.GetComponent<Animator>().SetBool("isHit", true);
        }

        yield return StartCoroutine(_cameraHandler.AttackEffect(_shakeInfo));

        CSData.AttackUnit.GetComponent<Animator>().SetBool("isAttack", false);
        foreach (BattleUnit unit in CSData.HitUnits)
        {
            if (unit != null)
                unit.GetComponent<Animator>().SetBool("isHit", false);
        }
    }

    public IEnumerator SkillHitEffect(BattleUnit unit)
    {
        if (unit != null)
            unit.GetComponent<Animator>().SetBool("isHit", true);

        yield return new WaitForSeconds(0.3f);

        if (unit != null)
            unit.GetComponent<Animator>().SetBool("isHit", false);
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
        StartCoroutine(CSData.AttackUnit.CutSceneMove(CSData.AttackPosition, ZoomTime));

        yield return new WaitForSeconds(ZoomTime);
    }

    private void SetUnitRayer(BattleUnit AttackUnit, List<BattleUnit> HitUnits, int rayer)
    {
        AttackUnit.GetComponent<SpriteRenderer>().sortingOrder = rayer;
        foreach (BattleUnit unit in HitUnits)
        {
            if(unit != null)
                unit.GetComponent<SpriteRenderer>().sortingOrder = rayer;
        }
    }
}