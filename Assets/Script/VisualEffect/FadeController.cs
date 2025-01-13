using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeController : MonoBehaviour
{
    private CanvasGroup cg;
    public float fadeTime = 1f; // 페이드 타임 
    float accumTime = 0f;
    private Coroutine fadeCor;
    [SerializeField] bool isButton;
    [SerializeField] bool auto;
    [SerializeField] string scenename = "none";

    private void Awake()
    {
        //여기의 Alpha 값을 조절
        cg = GetComponent<CanvasGroup>(); // 캔버스 그룹
        fadeCor = null;
        if(auto == true)
        {
            StartFadeIn();
        }
    }

    public void Init(float fadeTime)
    {
        this.fadeTime = fadeTime;
    }

    public void StartFadeIn() // 호출 함수 Fade In을 시작
    {
        if (fadeCor != null)
            StopCoroutine(fadeCor);
        
        fadeCor = StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn() // 코루틴을 통해 페이드 인 시간 조절
    {
        while (accumTime < fadeTime)
        {
            cg.alpha = Mathf.Lerp(0f, 1f, accumTime / fadeTime);
            accumTime += Time.deltaTime;
            yield return null;
        }
        cg.alpha = 1f;
        accumTime = fadeTime;
        if (auto == true)
        {
            StartFadeOut();
        }
    }

    public void StartFadeOut() // 호출 함수 Fadeout을 시작
    {
        if (fadeCor != null)
            StopCoroutine(fadeCor);

        fadeCor = StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        while (0 < accumTime)
        {
            cg.alpha = Mathf.Lerp(0, 1f, accumTime / fadeTime);
            accumTime -= Time.deltaTime;
            yield return null;
        }
        cg.alpha = 0f;
        accumTime = 0;

        SceneCheck();
    }

    public void SceneCheck()
    {
        if(scenename != "none")
        {
            SceneChanger.SceneChange(scenename);
        }
    }
    public void EndFade()
    {
        StopCoroutine(fadeCor);
        cg.alpha = 1f;
    }
}
