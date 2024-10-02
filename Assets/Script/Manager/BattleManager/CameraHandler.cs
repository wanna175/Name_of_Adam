using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CameraHandler : MonoBehaviour
{
    [SerializeField] Camera MainCamera;
    [SerializeField] Camera CutSceneCamera;

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

    
    public IEnumerator CameraMove(Vector3 moveVec, float moveTime)
    {
        Vector3 originVec = CutSceneCamera.transform.localPosition;
        float time = 0;
        while (time <= moveTime)
        {
            time += Time.deltaTime;
            float t = time / moveTime;

            CutSceneCamera.transform.localPosition = Vector3.Lerp(originVec, moveVec, t);
            yield return null;
        }
    }

    public IEnumerator CameraZoom(float zoomSize, float zoomTime)
    {
        float originSize = CutSceneCamera.fieldOfView;
        float time = 0;

        while (time <= zoomTime)
        {
            time += Time.deltaTime;
            float t = time / zoomTime;

            CutSceneCamera.fieldOfView = Mathf.Lerp(originSize, zoomSize, t);
            yield return null;
        }
    }

    public IEnumerator AttackEffect(List<Vector3> shakeInfo)
    {
        Vector3 originPos = CutSceneCamera.transform.position;

        float shakeTime = 0.7f;
        float shakeCurrentTime = 0f;

        while (shakeTime > shakeCurrentTime)
        {
            CutSceneCamera.transform.position = originPos + new Vector3(0.3f - shakeCurrentTime / 2, 0, 0);

            yield return new WaitForSeconds(0.15f);

            CutSceneCamera.transform.position = originPos + new Vector3(-1 * (0.3f - shakeCurrentTime / 2), 0, 0);

            shakeCurrentTime += 0.3f;
            yield return new WaitForSeconds(0.15f);
        }
        /*
        foreach(Vector3 vec in shakeInfo)
        {
            Vector3 editVec = new Vector3(vec.x, vec.y, 0);

            CutSceneCamera.transform.position = originPos + editVec;
            yield return new WaitForSeconds(vec.z);
        }
        */

        CutSceneCamera.transform.position = originPos;
    }


    public void CameraLotate(Vector3 origin, Vector3 change, float t)
    {
        Vector3 vec = Vector3.Lerp(origin, change, t);

        CutSceneCamera.transform.rotation = Quaternion.Euler(vec);
    }

    public Vector3 GetMainPosition() => MainCamera.transform.localPosition;
    public float GetMainFieldOfView() => MainCamera.fieldOfView;
}