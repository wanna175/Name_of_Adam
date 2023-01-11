using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    [SerializeField] Camera MainCamera;
    [SerializeField] Camera CutSceneCamera;

    BattleCutSceneManager _CutSceneMNG;
    FieldManager _FieldMNG;

    private void Start()
    {
        //SetMainCamera();

        _CutSceneMNG = GameManager.Instance.BattleMNG.CutSceneMNG;
        _FieldMNG = GameManager.Instance.BattleMNG.BattleDataMNG.FieldMNG;
        _CutSceneMNG.CameraHandler = this;

        SetMainCamera();
    }

    void SetCutSceneCamera()
    {
        CutSceneCamera.enabled = true;
        MainCamera.enabled = false;

        CutSceneCamera.transform.position = MainCamera.transform.position;
        CutSceneCamera.fieldOfView = MainCamera.fieldOfView;
    }
    void SetMainCamera()
    {
        MainCamera.enabled = true;
        CutSceneCamera.enabled = false;
    }

    public void CutSceneZoomIn(CutSceneData CSData)
    {
        SetCutSceneCamera();

        StartCoroutine(ZoomIn(CSData));
    }
    // 화면 줌 인
    IEnumerator ZoomIn(CutSceneData CSData)
    {
        float time = 0;

        while (time <= CSData.ZoomTime)
        {
            time += Time.deltaTime;
            float t = time / CSData.ZoomTime;

            CutSceneCamera.transform.position = Vector3.Lerp(MainCamera.transform.position, CSData.ZoomLocation, t);
            CutSceneCamera.fieldOfView = Mathf.Lerp(CSData.DefaultZoomSize, CSData.ZoomSize, t);
            _CutSceneMNG.MoveUnitZoomIn(CSData, t);
            yield return null;
        }
        Debug.Log(CSData.ATKType);
        if (CSData.ATKType == AttackType.rangeAttack)
            StartCoroutine(_CutSceneMNG.RangeCutScene(CSData));
        else if (CSData.ATKType == AttackType.targeting)
            StartCoroutine(_CutSceneMNG.TargetingCutScene(CSData));
    }

    // 컷씬 후 화면 줌 아웃
    public IEnumerator CutSceneZoomOut(CutSceneData CSData)
    {
        float time = 0;

        while (time <= CSData.ZoomTime)
        {
            time += Time.deltaTime;
            float t = time / CSData.ZoomTime;

            CutSceneCamera.transform.position = Vector3.Lerp(CSData.ZoomLocation, MainCamera.transform.position, t);
            CutSceneCamera.fieldOfView = Mathf.Lerp(CSData.ZoomSize, CSData.DefaultZoomSize, t);
            _CutSceneMNG.MoveUnitZoomOut(CSData, t);
            yield return null;
        }
        SetMainCamera();

        _FieldMNG.FieldClear();
        yield return new WaitForSeconds(0.2f);

        _CutSceneMNG.NextUnitSkill();
    }
}