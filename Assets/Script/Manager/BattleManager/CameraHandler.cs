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
            
            CutSceneCamera.transform.position = Vector3.Lerp(new Vector3(0, 0, CutSceneCamera.transform.position.z), CSData.ZoomLocation, time / CSData.ZoomTime);
            CutSceneCamera.fieldOfView = Mathf.Lerp(CSData.DefaultZoomSize, CSData.ZoomSize, time / CSData.ZoomTime);
            yield return null;
        }

        if (CSData.ATKType == AttackType.rangeAttack)
            StartCoroutine(_CutSceneMNG.RangeCutScene(CSData));
        else if (CSData.ATKType == AttackType.targeting)
            StartCoroutine(_CutSceneMNG.TargetingCutScene(CSData));
    }

    // 컷씬 후 화면 줌 아웃
    public IEnumerator CutSceneZoomOut(CutSceneData CSData)
    {
        Debug.Log('a');
        float time = 0;

        while (time <= CSData.ZoomTime)
        {
            time += Time.deltaTime;

            CutSceneCamera.transform.position = Vector3.Lerp(CSData.ZoomLocation, new Vector3(0, 0, CutSceneCamera.transform.position.z), time / CSData.ZoomTime);
            CutSceneCamera.fieldOfView = Mathf.Lerp(CSData.ZoomSize, CSData.DefaultZoomSize, time / +CSData.ZoomTime);

            yield return null;
        }
        SetMainCamera();

        _FieldMNG.FieldClear();
        yield return new WaitForSeconds(0.2f);

        _CutSceneMNG.NextUnitSkill();
    }
}