using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 컷씬에 사용될 데이터를 모아둔 클래스
// 범위용이랑 타겟용으로 분리해야 할 듯
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
    // 맞은 유닛이 여러 유닛일 시 저장하는 공간
    public List<Vector3> HitPositions;
    // 유닛들이 어디로 이동해야 하는지
    public Vector3 MovePosition;
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
        
        SetCSData(AttackUnit, HitUnits[0], HitUnits);
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

        SetCSData(AttackUnit, _hitUnit: HitUnit);
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

    void SetCSData(BattleUnit _atkUnit, BattleUnit _hitUnit, List<BattleUnit> _hitUnits = null)
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

        CSData.ZoomLocation = _FieldMNG.GameField.transform.position; ;
        CSData.ZoomLocation.z = Camera.main.transform.position.z;

        CSData.DefaultZoomSize = Camera.main.fieldOfView;

        CSData.LeftUnit = LeftUnit;
        CSData.RightUnit = RightUnit;

        CSData.LeftPosition = LeftUnit.transform.position;
        CSData.RightPosition = RightUnit.transform.position;
        CSData.MovePosition = GetMoveLocation(_atkUnit, _hitUnits);

        CSData.HitPositions = new List<Vector3>();
        foreach (BattleUnit unit in _hitUnits)
        {
            CSData.HitPositions.Add(unit.transform.position);
        }

        CSData.AttackUnit = _atkUnit;

        // 줌 사이즈는 나중에 유동적으로 바뀌거나 개별적으로 할 수도 있을 것 같다.
        // 일단 타입에 따라 임의로 부여함
        if(CSData.ATKType == AttackType.rangeAttack)
            CSData.ZoomSize = 50;
        if (CSData.ATKType == AttackType.targeting)
            CSData.ZoomSize = 35;

        #endregion
    }
    Vector3 GetMoveLocation(BattleUnit atkUnit, List<BattleUnit> hitUnits)
    {
        if(atkUnit.BattleUnitSO.GetAttackType() == AttackType.rangeAttack)
        {
            RangeType range = atkUnit.BattleUnitSO.GetRangeType();

            if(range == RangeType.noneMove)
            {
                Vector3 vec = atkUnit.transform.position;
                vec.x += 2;

                return vec;
            }
            else if(range == RangeType.tracking)
            {
                Vector3 vec = default;

                foreach (BattleUnit unit in hitUnits)
                {
                    if(atkUnit.UnitMove.LocY == unit.UnitMove.LocY)
                    {
                        vec = unit.transform.position;
                        vec.x -= 2;

                    }
                    else if(atkUnit.UnitMove.LocX == unit.UnitMove.LocX)
                    {
                        vec = unit.transform.position;
                        vec.y -= 2;
                    }
                }
                return vec;
            }
            else if(range == RangeType.center)
            {
                return _FieldMNG.GameField.transform.position;
            }
        }
        return default;
    }

    // 컷씬 지점으로 이동
    public void MoveUnitZoomIn(CutSceneData CSData, float t)
    {
        Vector3 leftVec = CSData.MovePosition;
        Vector3 rightVec = CSData.MovePosition;
        leftVec.x += -2;
        rightVec.x += 2;

        if (CSData.ATKType == AttackType.targeting)
        {
            if (CSData.AttackUnit == CSData.LeftUnit)
            {
                Debug.Log(CSData.LeftUnit);
                CSData.RightUnit.transform.position = Vector3.Lerp(CSData.RightPosition, rightVec, t);
                CSData.LeftUnit.transform.position = Vector3.Lerp(CSData.LeftPosition, leftVec, t);
            }
        }
        else if(CSData.ATKType == AttackType.rangeAttack)
        {
            float r = 2;
            float hitUnitcount = CSData.HitUnits.Count - 1;

            float Yrange = hitUnitcount * r;
            Yrange = (Yrange * 0.5f) - Yrange;

            if (CSData.AttackUnit == CSData.LeftUnit)
            {
                CSData.AttackUnit.transform.position = Vector3.Lerp(CSData.LeftPosition, leftVec, t);

                for(int i = 0; i <= hitUnitcount; i++)
                {
                    Vector3 vec = rightVec;
                    vec.y = Yrange + (r * i);

                    CSData.HitUnits[i].transform.position = Vector3.Lerp(CSData.HitPositions[i], vec, t);
                }
            }
            else
            {
                CSData.AttackUnit.transform.position = Vector3.Lerp(CSData.RightPosition, rightVec, t);

                for (int i = 0; i <= hitUnitcount; i++)
                {
                    Vector3 vec = leftVec;
                    vec.y = Yrange + (r * i);

                    CSData.HitUnits[i].transform.position = Vector3.Lerp(CSData.HitPositions[i], vec, t);
                }
            }
        }
    }

    #endregion

    #region Attack & Animation

    // 확대 후 컷씬
    public IEnumerator RangeCutScene(CutSceneData CSData)
    {
        float CutSceneTime = 1;

        yield return new WaitForSeconds(0.1f);
        // 공격하고 모션바뀌고 기타 등등 여기서 처리
        CSData.AttackUnit.UnitRenderer.SetIsAttackAnim(true);

        foreach(BattleUnit unit in CSData.HitUnits)
            unit.UnitRenderer.HitAnim(true);

        StartCoroutine(CameraHandler.CameraLotate(CutSceneTime));

        yield return new WaitForSeconds(CutSceneTime);


        foreach (BattleUnit unit in CSData.HitUnits)
        {
            unit.UnitAction.GetDamage(CSData.AttackUnit.GetStat().ATK);
            unit.UnitRenderer.HitAnim(false);
        }
        
        CSData.AttackUnit.UnitRenderer.SetIsAttackAnim(false);

        StartCoroutine(CameraHandler.CutSceneZoomOut(CSData));
    }

    public IEnumerator TargetingCutScene(CutSceneData CSData)
    {
        float CutSceneTime = 1;

        yield return new WaitForSeconds(0.1f);
        // 공격하고 모션바뀌고 기타 등등 여기서 처리
        CSData.AttackUnit.UnitRenderer.SetIsAttackAnim(true);
        CSData.HitUnit.UnitRenderer.HitAnim(true);
        
        StartCoroutine(CameraHandler.CameraLotate(CutSceneTime));

        yield return new WaitForSeconds(CutSceneTime);

        CSData.HitUnit.UnitAction.GetDamage(CSData.AttackUnit.GetStat().ATK);
        CSData.AttackUnit.UnitRenderer.SetIsAttackAnim(false);
        CSData.HitUnit.UnitRenderer.HitAnim(false);

        StartCoroutine(CameraHandler.CutSceneZoomOut(CSData));
    }

    #endregion

    // 원래 위치로 이동
    public void MoveUnitZoomOut(CutSceneData CSData, float t)
    {
        Vector3 leftVec = CSData.MovePosition;
        Vector3 rightVec = CSData.MovePosition;
        leftVec.x -= 2;
        rightVec.x += 2;

        if (CSData.ATKType == AttackType.targeting)
        {
            CSData.RightUnit.transform.position = Vector3.Lerp(rightVec, CSData.RightPosition, t);
            CSData.LeftUnit.transform.position = Vector3.Lerp(leftVec, CSData.LeftPosition, t);
        }
        else if(CSData.ATKType == AttackType.rangeAttack)
        {
            float r = 2;
            float hitUnitcount = CSData.HitUnits.Count - 1;

            float Yrange = hitUnitcount * r;
            Yrange = (Yrange * 0.5f) - Yrange;

            if (CSData.AttackUnit == CSData.LeftUnit)
            {
                CSData.AttackUnit.transform.position = Vector3.Lerp(leftVec, CSData.LeftPosition, t);

                for (int i = 0; i < hitUnitcount + 1; i++) 
                {
                    Vector3 vec = rightVec;
                    vec.y = Yrange + (r * i);

                    CSData.HitUnits[i].transform.position = Vector3.Lerp(vec, CSData.HitPositions[i], t);
                }
            }
            else
            {
                CSData.AttackUnit.transform.position = Vector3.Lerp(rightVec, CSData.RightPosition, t);

                for (int i = 0; i <= hitUnitcount; i++)
                {
                    Vector3 vec = leftVec;
                    vec.y = Yrange + (r * i);

                    CSData.HitUnits[i].transform.position = Vector3.Lerp(vec, CSData.HitPositions[i], t);
                }
            }
        }
    }

    public void NextUnitSkill()
    {
        _EngageMNG.UseUnitSkill();
    }
}
