using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TutorialArrow : MonoBehaviour
{
    private const float MOVE_DISTANCE = 30f;
    private const float MOVE_SPEED = 30f;

    [SerializeField]
    private Image image;

    [SerializeField]
    private RectTransform rectTr;

    [SerializeField]
    private Vector2 moveDir;

    [SerializeField]
    private AnimationCurve speedCurve;

    public void StartEffect()
    {
        image.enabled = true;
        //StartCoroutine("HardEffect");
        StartCoroutine("SmoothEffect");
    }

    public void StopEffect()
    {
        image.enabled = false;
        //StopCoroutine("HardEffect");
        StartCoroutine("SmoothEffect");
    }

    IEnumerator HardEffect()
    {
        Vector2 goalPos = rectTr.anchoredPosition - moveDir * MOVE_DISTANCE;
        float curDis = 0f;

        while (curDis >= MOVE_DISTANCE)
        {
            rectTr.anchoredPosition -= moveDir * MOVE_SPEED * Time.deltaTime;
            curDis += MOVE_SPEED * Time.deltaTime;
            yield return null;
        }
        rectTr.anchoredPosition = goalPos;

        goalPos = rectTr.anchoredPosition + moveDir * MOVE_DISTANCE;
        curDis = 0f;
        while (curDis >= MOVE_DISTANCE)
        {
            rectTr.anchoredPosition += moveDir * MOVE_SPEED * Time.deltaTime;
            curDis += MOVE_SPEED * Time.deltaTime;
            yield return null;
        }
        rectTr.anchoredPosition = goalPos;

        StartCoroutine("HardEffect");
    }

    IEnumerator SmoothEffect()
    {
        Vector2 startPos = rectTr.anchoredPosition;
        Vector2 goalPos = rectTr.anchoredPosition - moveDir * MOVE_DISTANCE;
        Vector2 gapVec = goalPos - rectTr.anchoredPosition;
        float gap;

        // 뒤로 이동
        while (gapVec.normalized != moveDir.normalized)
        {
            gapVec = goalPos - rectTr.anchoredPosition;
            gap = Mathf.Lerp(0f, 1f, gapVec.magnitude / (goalPos - startPos).magnitude);
            rectTr.anchoredPosition -= moveDir * speedCurve.Evaluate(gap) * MOVE_SPEED * Time.deltaTime;
            yield return null;
        }
        rectTr.anchoredPosition = goalPos;

        // 터닝 포인트
        startPos = rectTr.anchoredPosition;
        goalPos = rectTr.anchoredPosition + moveDir * MOVE_DISTANCE;
        gapVec = goalPos - rectTr.anchoredPosition;

        // 앞으로 이동
        while (gapVec.normalized == moveDir.normalized)
        {
            gapVec = goalPos - rectTr.anchoredPosition;
            gap = Mathf.Lerp(0f, 1f, gapVec.magnitude / (goalPos - startPos).magnitude);
            rectTr.anchoredPosition += moveDir * speedCurve.Evaluate(gap) * MOVE_SPEED * Time.deltaTime;
            yield return null;
        }
        rectTr.anchoredPosition = goalPos;

        StartCoroutine("SmoothEffect");
    }
}
