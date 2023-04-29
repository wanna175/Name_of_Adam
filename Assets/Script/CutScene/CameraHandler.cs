using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    [SerializeField] Camera MainCamera;
    [SerializeField] Camera CutSceneCamera;

    GameObject Blur;

    private void Start()
    {
        SetMainCamera();
    }

    // 컷씬 전용 카메라로 전환
    public void SetCutSceneCamera()
    {
        CutSceneCamera.enabled = true;
        MainCamera.enabled = false;

        CutSceneCamera.transform.position = MainCamera.transform.position;
        CutSceneCamera.fieldOfView = MainCamera.fieldOfView;
    }
    // 메인카메라로 전환
    public void SetMainCamera()
    {
        MainCamera.enabled = true;
        CutSceneCamera.enabled = false;
    }

    
    public void ZoomIn(CutSceneData CSData, float t)
    {
        CutSceneCamera.transform.localPosition = Vector3.Lerp(MainCamera.transform.localPosition, CSData.ZoomLocation, t);
        CutSceneCamera.fieldOfView = Mathf.Lerp(CSData.DefaultZoomSize, CSData.ZoomSize, t);
    }

    // 컷씬 중 카메라 회전
    public void CameraLotate(Vector3 origin, Vector3 tilt, float t)
    {
        Vector3 vec = Vector3.Lerp(origin, tilt, t);
        CutSceneCamera.transform.rotation = Quaternion.Euler(vec);
    }

    // 컷씬 후 화면 줌 아웃
    public void CutSceneZoomOut(CutSceneData CSData, float t)
    {
        CutSceneCamera.transform.localPosition = Vector3.Lerp(CSData.ZoomLocation, MainCamera.transform.localPosition, t);
        CutSceneCamera.fieldOfView = Mathf.Lerp(CSData.ZoomSize, CSData.DefaultZoomSize, t);

        CameraLotate(new Vector3(0, 0, CSData.TiltPower), new Vector3(0, 0, 0), t);
    }
}