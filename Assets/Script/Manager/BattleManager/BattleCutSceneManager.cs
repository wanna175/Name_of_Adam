using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//컷씬에 사용될 데이터를 모아둔 클래스
class CutSceneData
{
    // 확대 할 대상
    public Vector3 ZoomLocation;
    // 얼마나 줌할 것인지
    public float DefaultZoomSize = 60;
    public float ZoomSize;
    // 어느 캐릭터가 어느 위치에 있는지
    public BattleUnit LeftChar;
    public BattleUnit RightChar;
    // 어느 캐릭터가 공격자인지
    public BattleUnit AttackChar;
    public BattleUnit HitChar;
}


public class BattleCutSceneManager : MonoBehaviour
{
    // 컷씬이 실행중인가?
    bool _isCutScene = false;
    // 줌 인, 줌 아웃하는데 들어가는 시간
    float _zoomTime = 0.2f;

    // 배틀 컷씬을 시작
    public void BattleCutScene(Transform ZoomLocation, BattleUnit AttackChar, BattleUnit HitChar)
    {
        // 어느 캐릭터가 어느 방향에 있나 확인 후 각 위치에 할당
        BattleUnit LeftChar, RightChar;

        #region Set Char LR
        // 왼쪽에 배치될 캐릭터와 오른쪽에 배치될 캐릭터를 구분
        if (AttackChar.UnitMove.LocX < HitChar.UnitMove.LocX)
        {
            LeftChar = AttackChar;
            RightChar = HitChar;
        }
        else if (HitChar.UnitMove.LocX < AttackChar.UnitMove.LocX)
        {
            LeftChar = HitChar;
            RightChar = AttackChar;
        }
        else
        {
            // 둘이 x값이 같을 경우 플레이어쪽이 왼쪽으로
            if (AttackChar.BattleUnitSO.team == Team.Player)
            {
                LeftChar = AttackChar;
                RightChar = HitChar;
            }
            else
            {
                LeftChar = HitChar;
                RightChar = AttackChar;
            }
        }
        #endregion

        #region Create CutSceneData

        CutSceneData CSData = new CutSceneData();

        CSData.ZoomLocation = ZoomLocation.position;
        CSData.ZoomLocation.z = Camera.main.transform.position.z;
        CSData.DefaultZoomSize = Camera.main.fieldOfView;
        CSData.ZoomSize = 30; // 얘는 나중에 유동적으로 받기
        CSData.LeftChar = LeftChar;
        CSData.RightChar = RightChar;
        CSData.AttackChar = AttackChar;
        CSData.HitChar = HitChar;

        #endregion

        StartCoroutine(ZoomIn(CSData));
    }

    // 화면 줌 인
    IEnumerator ZoomIn(CutSceneData CSData)
    {
        Debug.Log('a');
        float time = 0;

        while (time <= _zoomTime)
        {
            time += Time.deltaTime;

            Camera.main.transform.position = Vector3.Lerp(new Vector3(0, 0, Camera.main.transform.position.z), CSData.ZoomLocation, time / _zoomTime);
            Camera.main.fieldOfView = Mathf.Lerp(CSData.DefaultZoomSize, CSData.ZoomSize, time / _zoomTime);
            yield return null;
        }
        StartCoroutine(PlayCutScene(CSData));
    }

    // 확대 후 컷씬
    IEnumerator PlayCutScene(CutSceneData CSData)
    {
        Debug.Log('b');
        // 공격하고 모션바뀌고 기타 등등 여기서 처리

        yield return new WaitForSeconds(1);

        StartCoroutine(ZoomOut(CSData));

    }

    // 컷씬 후 화면 줌 아웃
    IEnumerator ZoomOut(CutSceneData CSData)
    {
        Debug.Log('c');
        float time = 0;

        while (time <= _zoomTime)
        {
            time += Time.deltaTime;

            Camera.main.transform.position = Vector3.Lerp(CSData.ZoomLocation, new Vector3(0, 0, Camera.main.transform.position.z), time / _zoomTime);
            Camera.main.fieldOfView = Mathf.Lerp(CSData.ZoomSize, CSData.DefaultZoomSize, time / +_zoomTime);

            yield return null;
        }
    }
}
