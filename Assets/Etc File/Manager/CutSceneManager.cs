using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 컷씬에 사용될 데이터를 모아둔 클래스
// 사용되는 변수들 둘러보고 구조체로 바꿀지 봐보자
// 범위용이랑 타겟용으로 분리해야 할 듯
public struct CutSceneData
{
    // 공격 타입
    public TargetType TargetType;
    // 확대하는 시간
    public float ZoomTime;
    // 확대 할 대상
    public Vector3 ZoomLocation;
    // 얼마나 줌할 것인지
    public float DefaultZoomSize;
    public float ZoomSize;
    // 유닛들이 어디로 이동해야 하는지
    public Vector3 DefaultPosition;
    public Vector3 MovePosition;
    // 어느 유니이 공격자인지
    public BattleUnit AttackUnit;
    public BattleUnit HitUnit;
    public List<BattleUnit> HitUnits;
}


public class CutSceneManager : MonoBehaviour
{
    Field _field;
    #region CameraHandler
    [SerializeField] private CameraHandler _CameraHandler;
    public bool moving = true;
    public CameraHandler CameraHandler
    {
        get { return _CameraHandler; }
        set { _CameraHandler = value; }
    }
    #endregion
    public float ZoomSize;
    public Vector3 AttackLocationAdd;

    CutSceneData CSData;
    // 줌 인, 줌 아웃하는데 들어가는 시간
    float _zoomTime = 0.1f;

    private void Start()
    {
        _field = BattleManager.Field;
        CSData = new CutSceneData();

        // CameraHandler = GameObject.Find("Camera").GetComponent<CameraHandler>();
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

        AttackUnit.GetComponent<SpriteRenderer>().sortingOrder = 5;
        foreach (BattleUnit unit in HitUnits)
        {
            unit.GetComponent<SpriteRenderer>().sortingOrder = 6;
        }

        _CameraHandler.CutSceneZoomIn(CSData);
    }

    void SetCSData(BattleUnit _atkUnit, List<BattleUnit> _hitUnits)
    {
        CSData.AttackUnit = _atkUnit;
        CSData.HitUnits = _hitUnits;

        CSData.TargetType = _atkUnit.Data.TargetType;
        CSData.ZoomTime = _zoomTime;
        
        CSData.DefaultPosition = _atkUnit.transform.position;
        CSData.MovePosition = GetMoveLocation(_atkUnit, _hitUnits);

        CSData.ZoomLocation = GetZoomLocation(CSData);
        CSData.ZoomLocation.z = Camera.main.transform.position.z;

        CSData.DefaultZoomSize = Camera.main.fieldOfView;

        // 줌 사이즈는 나중에 유동적으로 바뀌거나 개별적으로 할 수도 있을 것 같다.
        // 일단 타입에 따라 임의로 부여함
        if (CSData.TargetType == TargetType.Range)
            CSData.ZoomSize = ZoomSize;
        if (CSData.TargetType == TargetType.Select)
            CSData.ZoomSize = ZoomSize;
    }

    // 공격자의 위치를 지정
    // 공격범위 내의 적을 추적하는 것보다 한 지점에서 공격을 하는 것이 좋지 않을까?
    Vector3 GetMoveLocation(BattleUnit atkUnit, List<BattleUnit> hitUnits)
    {
        if (!moving)
            return atkUnit.transform.position;

        CutSceneType range = atkUnit.GetCutSceneType();

        // 공격자가 적을 추적
        // y축을 우선으로 가까운 곳에 있는 적을 찾는다.
        Vector3 vec = new Vector3();
        BattleUnit target = null;
        int num = 999;

        foreach (BattleUnit unit in hitUnits)
        {
            int dump = 999;
            if (!atkUnit.GetFlipX())
            {
                dump = (int)unit.Location.x;
                dump += Mathf.Abs((int)atkUnit.Location.y - (int)unit.Location.y) * 100;
            }
            else
            {
                dump = (int)unit.Location.x * -1;
                dump += Mathf.Abs((int)atkUnit.Location.y - (int)unit.Location.y) * 100;
            }

            if (dump < num)
            {
                num = dump;
                target = unit;
            }
        }

        vec = target.transform.position;
        vec.x -= 3;

        return vec + AttackLocationAdd;

        //// 제자리에서 공격
        //return atkUnit.transform.position;

        //return new Vector3();
    }

    Vector3 GetZoomLocation(CutSceneData CSData)
    {
        if (CSData.TargetType == TargetType.Range)
            //return _field.FieldPosition;
            return new Vector3();

        Vector3 zoomLoc = Vector3.Lerp(CSData.MovePosition, CSData.HitUnits[0].transform.position, 0.5f);

        return zoomLoc;
    }

    // 컷씬 지점으로 이동
    public void MoveUnitZoomIn(CutSceneData CSData, float t)
    {
        Vector3 moveLoc = CSData.MovePosition;

        if (CSData.AttackUnit.GetCutSceneType() == CutSceneType.tracking)
        {
            if (CSData.AttackUnit.GetFlipX())
                moveLoc.x += 2;
            else
                moveLoc.x -= 2;
        }
        
        CSData.AttackUnit.transform.position = Vector3.Lerp(CSData.DefaultPosition, moveLoc, t);
    }

    #endregion

    #region Attack & Animation.

    // 확대 후 컷씬
    public IEnumerator AttackCutScene(CutSceneData CSData)
    {
        // 줌 시간 제외, 확대하는 시간은 총 1초
        float CutSceneTime = 1;

        yield return new WaitForSeconds(0.1f);


        StartCoroutine(_CameraHandler.CameraLotate(CutSceneTime));
        CSData.AttackUnit.GetComponent<Animator>().SetBool("isAttack", true);

        yield return new WaitForSeconds(CutSceneTime);
        CSData.AttackUnit.GetComponent<Animator>().SetBool("isAttack", false);


        foreach (BattleUnit unit in CSData.HitUnits)
            unit.ChangeHP(-CSData.AttackUnit.GetStat().ATK);

        StartCoroutine(_CameraHandler.CutSceneZoomOut(CSData));
    }

    #endregion

    #region End CutScene

    // 원래 위치로 이동
    public void MoveUnitZoomOut(CutSceneData CSData, float t)
    {
        Vector3 moveLoc = CSData.MovePosition;

        if (CSData.AttackUnit.GetCutSceneType() == CutSceneType.tracking)
        {
            if (CSData.AttackUnit.GetFlipX())
                moveLoc.x += 2;
            else
                moveLoc.x -= 2;
        }

        CSData.AttackUnit.transform.position = Vector3.Lerp(moveLoc, CSData.DefaultPosition, t);
    }

    public void EndAttack()
    {
        //_BattleMNG.UseNextUnit();
    }

    IEnumerator ExitCutScene()
    {
        yield return new WaitForSeconds(0.5f);
        _field.ClearAllColor();
        yield return new WaitForSeconds(0.2f);
        EndAttack();
    }

    #endregion
}