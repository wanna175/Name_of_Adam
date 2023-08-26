using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleCutSceneController : MonoBehaviour
{
    [SerializeField] private CameraHandler _CameraHandler;
    public CameraHandler CameraHandler => _CameraHandler;
    [SerializeField] GameObject Blur;
    [Space(20f)]

    public float ZoomTime = 1;
    public float CutSceneTime = 1;
    
    public float ShakePower = 0.5f;
    public float ShakeMinus = 0.1f;
    public float ShakeTime = 0.1f;
    public float ShakeCount = 5;

    [NonSerialized] public bool isAttack = false;


    public IEnumerator AttackCutScene(BattleCutSceneData CSData)
    {
        _CameraHandler.SetCutSceneCamera();
        yield return StartCoroutine(ZoomIn(CSData));

        CSData.AttackUnit.GetComponent<Animator>().SetBool("isAttack", true);

        yield return new WaitUntil(() => isAttack);
        isAttack = false;

        BattleManager.Instance.UnitAttackAction();

        yield return StartCoroutine(AfterEffect(CSData));
        yield return new WaitUntil(() => BattleManager.Data.CorruptUnits.Count == 0);
        yield return StartCoroutine(ZoomOut(CSData));

        _CameraHandler.SetMainCamera();
        ExitBattleCutScene(CSData);
        yield return new WaitForSeconds(1);

        BattleManager.Instance.EndUnitAction();
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
        Blur.SetActive(false);
    }

    private void UnitFlip(BattleCutSceneData CSData)
    {
        if(CSData.AttackUnitFlipX < 0)
        {
            CSData.AttackUnit.SetFlipX(false);
            foreach(BattleUnit unit in CSData.HitUnits)
                unit.SetFlipX(true);
        }
        else if (CSData.AttackUnitFlipX > 0)
        {
            CSData.AttackUnit.SetFlipX(true);
            foreach (BattleUnit unit in CSData.HitUnits)
                unit.SetFlipX(false);
        }
        else
        {
            bool attackDir = CSData.AttackUnit.GetFlipX();
            foreach (BattleUnit unit in CSData.HitUnits)
                unit.SetFlipX(!attackDir);
        }
    }

    public IEnumerator AfterEffect(BattleCutSceneData CSData)
    {
        foreach (BattleUnit unit in CSData.HitUnits)
        {
            if (unit != null)
                unit.GetComponent<Animator>().SetBool("isHit", true);
        }

        yield return StartCoroutine(_CameraHandler.AttackEffect(ShakeCount, ShakeTime, ShakePower, ShakeMinus));

        CSData.AttackUnit.GetComponent<Animator>().SetBool("isAttack", false);
        foreach (BattleUnit unit in CSData.HitUnits)
        {
            if (unit != null)
                unit.GetComponent<Animator>().SetBool("isHit", false);
        }
    }

    // 컷씬 지점으로 이동
    public IEnumerator ZoomIn(BattleCutSceneData CSData)
    {
        Blur.SetActive(true);

        StartCoroutine(_CameraHandler.CameraMove(CSData.ZoomLocation, ZoomTime));
        StartCoroutine(_CameraHandler.CameraZoom(CSData.ZoomSize, ZoomTime));
        StartCoroutine(CSData.AttackUnit.CutSceneMove(CSData.MovePosition, ZoomTime));

        yield return new WaitForSeconds(ZoomTime);
    }

    // 원래 위치로 이동
    public IEnumerator ZoomOut(BattleCutSceneData CSData)
    {
        Blur.SetActive(false);

        StartCoroutine(_CameraHandler.CameraMove(_CameraHandler.GetMainPosition(), ZoomTime));
        StartCoroutine(_CameraHandler.CameraZoom(_CameraHandler.GetMainFieldOfView(), ZoomTime));
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