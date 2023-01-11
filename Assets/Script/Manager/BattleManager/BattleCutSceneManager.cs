using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//컷씬에 사용될 데이터를 모아둔 클래스
public class CutSceneData
{
    // 공격 타입
    public AttackType ATKType;
    // 확대하는 시간
    public float ZoomTime;
    // 확대 할 대상
    public Vector3 ZoomLocation;
    // 얼마나 줌할 것인지
    public float DefaultZoomSize = 65;
    public float ZoomSize;
    // 어느 유닛이 어느 위치에 있는지
    public BattleUnit LeftUnit;
    public BattleUnit RightUnit;
    // 유닛들이 어느 위치에 있었는지
    public Vector3 LeftPosition;
    public Vector3 RightPosition;
    // 어느 유니이 공격자인지
    public BattleUnit AttackUnit;
    public BattleUnit HitUnit;
    public List<BattleUnit> HitUnits;
}


public class BattleCutSceneManager : MonoBehaviour
{
    BattleEngageManager _EngageMNG;
    FieldManager _FieldMNG;
    public CameraHandler CameraHandler;

    CutSceneData CSData;
    // 줌 인, 줌 아웃하는데 들어가는 시간
    float _zoomTime = 0.2f;

    private void Start()
    {
        _EngageMNG = GameManager.Instance.BattleMNG.EngageMNG;
        _FieldMNG = GameManager.Instance.BattleMNG.BattleDataMNG.FieldMNG;
        CSData = new CutSceneData();
    }


    // 배틀 컷씬을 시작
    #region Start CutScene

    // 광역공격의 경우
    public void BattleCutScene(BattleUnit AttackUnit, List<BattleUnit> HitUnits)
    {
        if (HitUnits.Count == 0)
        {
            StartCoroutine(ExitCutScene());
            return;
        }

        // 일단 첫번째로 맞은 애를 대려온다.
        // 광역공격은 어떻게 보일까? 기획에게 물어볼 필요 있음
        SetCSData(AttackUnit, HitUnits[0]);
        CSData.HitUnits = HitUnits;

        CameraHandler.CutSceneZoomIn(CSData);
    }
    // 타겟팅의 경우
    public void BattleCutScene(BattleUnit AttackUnit, BattleUnit HitUnit)
    {
        if (HitUnit == null)
        {
            StartCoroutine(ExitCutScene());
            return;
        }

        SetCSData(AttackUnit, HitUnit);
        CSData.HitUnit = HitUnit;

        CameraHandler.CutSceneZoomIn(CSData);
    }

    IEnumerator ExitCutScene()
    {
        yield return new WaitForSeconds(0.5f);
        _FieldMNG.FieldClear();
        yield return new WaitForSeconds(0.2f);
        _EngageMNG.UseUnitSkill();
    }

    void SetCSData(BattleUnit _atkUnit, BattleUnit _hitUnit)
    {
        // 어느 캐릭터가 어느 방향에 있나 확인 후 각 위치에 할당
        BattleUnit LeftUnit, RightUnit;

        #region Set Char LR
        // 왼쪽에 배치될 캐릭터와 오른쪽에 배치될 캐릭터를 구분
        if (_atkUnit.UnitMove.LocX < _hitUnit.UnitMove.LocX)
        {
            LeftUnit = _atkUnit;
            RightUnit = _hitUnit;
        }
        else if (_hitUnit.UnitMove.LocX < _atkUnit.UnitMove.LocX)
        {
            LeftUnit = _hitUnit;
            RightUnit = _atkUnit;
        }
        else
        {
            // 둘이 x값이 같을 경우 플레이어쪽이 왼쪽으로
            if (_atkUnit.BattleUnitSO.team == Team.Player)
            {
                LeftUnit = _atkUnit;
                RightUnit = _hitUnit;
            }
            else
            {
                LeftUnit = _hitUnit;
                RightUnit = _atkUnit;
            }
        }
        #endregion

        #region Create CutSceneData

        CSData.ATKType = _atkUnit.BattleUnitSO.GetAttackType();
        CSData.ZoomTime = _zoomTime;
        CSData.ZoomLocation = _FieldMNG.GameField.transform.position;
        CSData.ZoomLocation.z = Camera.main.transform.position.z;
        CSData.DefaultZoomSize = Camera.main.fieldOfView;
        CSData.ZoomSize = 30; // 얘는 나중에 유동적으로 받기
        CSData.LeftUnit = LeftUnit;
        CSData.RightUnit = RightUnit;
        CSData.LeftPosition = LeftUnit.transform.position;
        CSData.RightPosition = RightUnit.transform.position;
        CSData.AttackUnit = _atkUnit;

        #endregion
    }

    // 컷씬 지점으로 이동
    public void MoveUnitZoomIn(CutSceneData CSData, float t)
    {
        Vector3 leftVec = new Vector3(-2, -1.4f, 0);
        Vector3 rightVec = new Vector3(2, -1.4f, 0);

        CSData.LeftUnit.transform.position = Vector3.Lerp(CSData.LeftPosition, leftVec, t);
        CSData.RightUnit.transform.position = Vector3.Lerp(CSData.RightPosition, rightVec, t);
    }

    #endregion

    #region Attack & Animation

    // 확대 후 컷씬
    public IEnumerator RangeCutScene(CutSceneData CSData)
    {
        foreach(BattleUnit unit in CSData.HitUnits)
        {
            Debug.Log(CSData.AttackUnit.GetStat().ATK);
            unit.UnitAction.GetDamage(CSData.AttackUnit.GetStat().ATK);
        }
        // 공격하고 모션바뀌고 기타 등등 여기서 처리

        yield return new WaitForSeconds(1);

        StartCoroutine(CameraHandler.CutSceneZoomOut(CSData));
    }
    public IEnumerator TargetingCutScene(CutSceneData CSData)
    {
        CSData.HitUnit.UnitAction.GetDamage(CSData.AttackUnit.GetStat().ATK);
        // 공격하고 모션바뀌고 기타 등등 여기서 처리

        yield return new WaitForSeconds(1);

        StartCoroutine(CameraHandler.CutSceneZoomOut(CSData));
    }

    #endregion

    // 컷씬 지점으로 이동
    public void MoveUnitZoomOut(CutSceneData CSData, float t)
    {
        Vector3 leftVec = new Vector3(-2, -1.4f, 0);
        Vector3 rightVec = new Vector3(2, -1.4f, 0);

        CSData.LeftUnit.transform.position = Vector3.Lerp(leftVec, CSData.LeftPosition, t);
        CSData.RightUnit.transform.position = Vector3.Lerp(rightVec, CSData.RightPosition, t);
    }

    public void NextUnitSkill()
    {
        _EngageMNG.UseUnitSkill();
    }
}
