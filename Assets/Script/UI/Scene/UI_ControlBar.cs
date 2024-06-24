using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ControlBar : UI_Scene
{
    [SerializeField] public UI_Hands UI_Hands;
    [SerializeField] public UI_PlayerHP UI_PlayerHP;
    [SerializeField] public UI_PlayerSkill UI_PlayerSkill;
    [SerializeField] public UI_DarkEssence UI_DarkEssence;
    [SerializeField] public UI_ManaGauge UI_ManaGauge;
    [SerializeField] public Animator UI_Aniamtor;
    [SerializeField] private Image _active;
    private List<CanvasGroup> canvasGroups;

    private const float activeMaxAlpha = 1.0f;
    private const float fadeSpeed = 1.0f;
    private const float fadeMinAlpha = 0.5f;

    public void Init()
    {
        _active.color = new Color(1f, 1f, 1f, activeMaxAlpha);

        canvasGroups = new List<CanvasGroup>();
        canvasGroups.AddRange(GetComponentsInChildren<CanvasGroup>());
    }

    public void EndPlayerHit()
        => UI_Aniamtor.SetBool("isPlayerHit", false);

    public void ControlBarActive()
    {
        _active.gameObject.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(FadeInActive());
        StartCoroutine(FadeInCanvas());
    }

    private IEnumerator FadeInActive()
    {
        Color c = _active.color;

        while (c.a < activeMaxAlpha)
        {
            c.a += Time.deltaTime * fadeSpeed * 0.75f;
            _active.color = c;
            yield return null;
        }
    }

    public void ControlBarInactive()
    {
        StopAllCoroutines();
        StartCoroutine(FadeOutActive());
        StartCoroutine(FadeOutCanvas());
    }

    public void CreateSystemInfo(string info)
    {
        UI_SystemFadeInfo systemInfo = GameManager.Resource.Instantiate("UI/SystemFadeInfo", this.transform).GetComponent<UI_SystemFadeInfo>();
        systemInfo.Init(new Vector2(0f, -265f), info);
    }

    private IEnumerator FadeOutActive()
    {
        Color c = _active.color;

        while (c.a > 0)
        {
            c.a -= Time.deltaTime * fadeSpeed * 3.0f;
            _active.color = c;
            yield return null;
        }

        _active.gameObject.SetActive(false);
    }

    private void SetCanvasGroupAlpha(float alpha)
    {
        foreach (var canvasGroup in canvasGroups)
        {
            canvasGroup.alpha = alpha;
        }
    }

    private IEnumerator FadeInCanvas()
    {
        float alpha = canvasGroups[0].alpha;
        while (alpha < 1.0f)
        {
            alpha += Time.deltaTime * fadeSpeed;
            SetCanvasGroupAlpha(alpha);
            yield return null;
        }
        SetCanvasGroupAlpha(1.0f);
    }

    private IEnumerator FadeOutCanvas()
    {
        float alpha = canvasGroups[0].alpha;
        while (alpha > fadeMinAlpha)
        {
            alpha -= Time.deltaTime * fadeSpeed;
            SetCanvasGroupAlpha(alpha);
            yield return null;
        }
        SetCanvasGroupAlpha(fadeMinAlpha);
    }
}
