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

    
    public void ZoomIn(BattleCutSceneData CSData, float t)
    {
        CutSceneCamera.transform.localPosition = Vector3.Lerp(MainCamera.transform.localPosition, CSData.ZoomLocation, t);
        CutSceneCamera.fieldOfView = Mathf.Lerp(CSData.DefaultZoomSize, CSData.ZoomSize, t);
    }

    // 컷씬 중 카메라 회전
    public void AttackCameraLotate(float gradient, int AttackDir, float t)
    {
        t = t * 4;

        if (1 < t)
        {
            Vector3 v = CutSceneCamera.transform.rotation.eulerAngles + new Vector3(0, 0, 0.005f * AttackDir);
            CutSceneCamera.transform.eulerAngles = v;

            return;
        }
        
        float z = CutSceneCamera.transform.eulerAngles.z;
        z = (z > 180) ? -(360 - z) : z;

        float a = Mathf.Lerp(z, gradient * AttackDir, t);
        CutSceneCamera.transform.eulerAngles = new Vector3(0, 0, a);
    }

    public IEnumerator AttackEffect(float shakeCount, float shakeTime, float shakePower)
    {
        Vector3 originPos = CutSceneCamera.transform.position;

        float count = shakeCount;
        float time = shakeTime / shakeCount;

        Vector3 editVec = new Vector3(shakePower, 0, 0);

        while (count > 0)
        {
            editVec *= -1;
            CutSceneCamera.transform.position += editVec;
            count--;

            yield return new WaitForSeconds(time);
        }

        CutSceneCamera.transform.position = originPos;
    }


    public void CameraLotate(Vector3 origin, Vector3 change, float t)
    {
        Vector3 vec = Vector3.Lerp(origin, change, t);

        CutSceneCamera.transform.rotation = Quaternion.Euler(vec);
    }

    // 컷씬 후 화면 줌 아웃
    public void CutSceneZoomOut(BattleCutSceneData CSData, float t)
    {
        CutSceneCamera.transform.localPosition = Vector3.Lerp(CSData.ZoomLocation, MainCamera.transform.localPosition, t);
        CutSceneCamera.fieldOfView = Mathf.Lerp(CSData.ZoomSize, CSData.DefaultZoomSize, t);
    }
}