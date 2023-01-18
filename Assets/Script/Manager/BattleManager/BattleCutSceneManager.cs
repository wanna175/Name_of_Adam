using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 컷씬에 사용될 데이터를 모아둔 클래스
// 사용되는 변수들 둘러보고 구조체로 바꿀지 봐보자
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
    // 유닛들이 어디로 이동해야 하는지
    public Vector3 DefaultPosition;
    public Vector3 MovePosition;
    // 어느 유니이 공격자인지
    public BattleUnit AttackUnit;
    public BattleUnit HitUnit;
    public List<BattleUnit> HitUnits;
}


public class BattleCutSceneManager : MonoBehaviour
{
    BattleManager _BattleMNG;
    FieldManager _FieldMNG;
    public CameraHandler CameraHandler;

    CutSceneData CSData;
    // 줌 인, 줌 아웃하는데 들어가는 시간
    float _zoomTime = 0.2f;

    private void Start()
    {
        _BattleMNG = GameManager.Instance.BattleMNG;
        _FieldMNG = GameManager.Instance.BattleMNG.BattleDataMNG.FieldMNG;
        CSData = new CutSceneData();
    }


    // 배틀 컷씬을 시작
    #region Start CutScene
        
    public void BattleCutScene(BattleUnit AttackUnit, List<BattleUnit> HitUnits)
    {
        if (HitUnits.Count == 0)
        {
            StartCoroutine(ExitCutScene());
            return;
        }
        
        SetCSData(AttackUnit, HitUnits);
        CSData.HitUnits = HitUnits;

        CameraHandler.CutSceneZoomIn(CSData);
    }

    void SetCSData(BattleUnit _atkUnit, List<BattleUnit> _hitUnits)
    {
        #region Create CutSceneData

        CSData.ATKType = _atkUnit.BattleUnitSO.GetAttackType();
        CSData.ZoomTime = _zoomTime;

        CSData.ZoomLocation = _FieldMNG.GameField.transform.position;
        CSData.ZoomLocation.z = Camera.main.transform.position.z;

        CSData.DefaultZoomSize = Camera.main.fieldOfView;

        CSData.DefaultPosition = _atkUnit.transform.position;
        CSData.MovePosition = GetMoveLocation(_atkUnit, _hitUnits);

        CSData.AttackUnit = _atkUnit;

        // 줌 사이즈는 나중에 유동적으로 바뀌거나 개별적으로 할 수도 있을 것 같다.
        // 일단 타입에 따라 임의로 부여함
        if(CSData.ATKType == AttackType.rangeAttack)
            CSData.ZoomSize = 50;
        if (CSData.ATKType == AttackType.targeting)
            CSData.ZoomSize = 35;

        #endregion
    }
    
    // 공격자의 위치를 지정
    // 공격범위 내의 적을 추적하는 것보다 한 지점에서 공격을 하는 것이 좋지 않을까?
    Vector3 GetMoveLocation(BattleUnit atkUnit, List<BattleUnit> hitUnits)
    {
        RangeType range = atkUnit.BattleUnitSO.GetRangeType();

        // 공격자가 적을 추적
        if(range == RangeType.tracking)
        {
            // y축을 우선으로 가까운 곳에 있는 적을 찾는다.
            Vector3 vec = default;
            int num;
            BattleUnit target = null;

            if (!atkUnit.UnitRenderer.GetFlipX())
            {
                num = 999;

                foreach (BattleUnit unit in hitUnits)
                {
                    int dump = unit.UnitMove.LocX;
                    dump += Mathf.Abs(atkUnit.UnitMove.LocY - unit.UnitMove.LocY) * 100;

                    if (dump < num)
                    {
                        num = dump;
                        target = unit;
                    }
                }
            }
            else
            {
                num = 999;

                foreach (BattleUnit unit in hitUnits)
                {
                    int dump = -unit.UnitMove.LocX;
                    dump += Mathf.Abs(atkUnit.UnitMove.LocY - unit.UnitMove.LocY) * 100;

                    if (dump < num)
                    {
                        num = dump;
                        target = unit;
                    }
                }
            }

            vec = target.transform.position;

            return vec;
        }
        // 움직이지 않는 공격
        else if (range == RangeType.noneMove)
            return atkUnit.transform.position;

        return default;
    }

    // 컷씬 지점으로 이동
    public void MoveUnitZoomIn(CutSceneData CSData, float t)
    {
        Vector3 moveLoc = CSData.MovePosition;

        if (CSData.AttackUnit.BattleUnitSO.GetRangeType() == RangeType.tracking)
        {
            if (CSData.AttackUnit.UnitRenderer.GetFlipX())
                moveLoc.x += 2;
            else
                moveLoc.x -= 2;
        }
        
        CSData.AttackUnit.transform.position = Vector3.Lerp(CSData.DefaultPosition, moveLoc, t);
    }

    #endregion

    #region Attack & Animation

    // 확대 후 컷씬
    // 여기도 겹치는게 많음, 합칠 수 있을거같은데
    public IEnumerator AttackCutScene(CutSceneData CSData)
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

    #endregion

    #region End CutScene

    // 원래 위치로 이동
    public void MoveUnitZoomOut(CutSceneData CSData, float t)
    {
        Vector3 moveLoc = CSData.MovePosition;
        if (CSData.AttackUnit.BattleUnitSO.GetRangeType() == RangeType.tracking)
        {
            if (CSData.AttackUnit.UnitRenderer.GetFlipX())
                moveLoc.x += 2;
            else
                moveLoc.x -= 2;
        }

        CSData.AttackUnit.transform.position = Vector3.Lerp(moveLoc, CSData.DefaultPosition, t);
    }

    public void NextUnitSkill()
    {
        _BattleMNG.UseUnitSkill();
    }

    IEnumerator ExitCutScene()
    {
        yield return new WaitForSeconds(0.5f);
        _FieldMNG.FieldClear();
        yield return new WaitForSeconds(0.2f);
        _BattleMNG.UseUnitSkill();
    }

    #endregion
}
