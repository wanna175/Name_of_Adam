using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CutSceneController : MonoBehaviour
{
    [SerializeField] private CameraHandler _CameraHandler;
    public CameraHandler CameraHandler => _CameraHandler;

    CutSceneData CSData;
    GameObject Blur;

    public float ZoomTime = 1;
    public float CutSceneTime = 1;
    

    public void BattleCutScene(BattleUnit AttackUnit, List<BattleUnit> HitUnits)
    {
        if (HitUnits.Count == 0)
            return;

        CSData = new CutSceneData();
        CSData.SetData(AttackUnit, HitUnits);
        
        CameraHandler.SetCutSceneCamera();
        SetUnitRayer(5);

        StartCoroutine(ZoomIn());
    }

    public IEnumerator AfterAttack()
    {
        foreach (BattleUnit unit in CSData.HitUnits)
        {
            if(unit != null)
                unit.GetComponent<Animator>().SetBool("isHit", true);
        }

        yield return StartCoroutine(AttackTilt());

        CSData.AttackUnit.GetComponent<Animator>().SetBool("isAttack", false);
        foreach (BattleUnit unit in CSData.HitUnits)
        {
            if (unit != null)
                unit.GetComponent<Animator>().SetBool("isHit", false);
        }

        yield return StartCoroutine(ZoomOut());

        SetUnitRayer(2);
        CameraHandler.SetMainCamera();
        BattleManager.Field.ClearAllColor();
        Destroy(Blur);
    }

    // 확대 후 컷씬
    public IEnumerator AttackTilt()
    {
        float time = 0;

        while (time <= CutSceneTime)
        {
            time += Time.deltaTime;
            float t = time / CutSceneTime;
            _CameraHandler.CameraLotate(new Vector3(0, 0, 0), new Vector3(0, 0, CSData.TiltPower), t);

            yield return null;
        }
    }

    // 컷씬 지점으로 이동
    IEnumerator ZoomIn()
    {
        Blur = GameManager.Resource.Instantiate("TestBlur");

        float time = 0;
        Vector3 moveVec = CSData.MovePosition;

        while (time <= ZoomTime)
        {
            time += Time.deltaTime;
            float t = time / ZoomTime;

            CSData.AttackUnit.transform.position = Vector3.Lerp(CSData.AttackPosition, CSData.MovePosition, t);
            _CameraHandler.ZoomIn(CSData, t);
            yield return null;
        }

        yield return new WaitForSeconds(0.1f);

        CSData.AttackUnit.GetComponent<Animator>().SetBool("isAttack", true);
    }

    // 원래 위치로 이동
    IEnumerator ZoomOut()
    {
        float time = 0;

        while (time <= ZoomTime)
        {
            time += Time.deltaTime;
            float t = time / ZoomTime;

            CSData.AttackUnit.transform.position = Vector3.Lerp(CSData.MovePosition, CSData.AttackPosition, t);
            _CameraHandler.CutSceneZoomOut(CSData, t);
            yield return null;
        }
    }

    private void SetUnitRayer(int rayer)
    {
        CSData.AttackUnit.GetComponent<SpriteRenderer>().sortingOrder = rayer + 1;
        foreach (BattleUnit unit in CSData.HitUnits)
        {
            if(unit != null)
                unit.GetComponent<SpriteRenderer>().sortingOrder = rayer;
        }

    }
}