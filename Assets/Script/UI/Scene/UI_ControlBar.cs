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
    [SerializeField] private CanvasGroup canvasGroup;

    private const float fadeSpeed = 1.0f;
    private const float fadeMinAlpha = 0.5f;

    public void EndPlayerHit()
        => UI_Aniamtor.SetBool("isPlayerHit", false);

    public void ControlBarActive()
    {
        _active.gameObject.SetActive(true);
        StartCoroutine(FadeInActive());
        StartCoroutine(FadeInCanvas());
        UI_ManaGauge.SetColorManaText(new Color(0.4f, 0.4f, 0.4f));
    }

    private IEnumerator FadeInActive()
    {
        Color c = _active.color;

        while (c.a < 1)
        {
            c.a += 0.01f;

            _active.color = c;

            yield return null;
        }
    }

    public void ControlBarInactive()
    {
        StartCoroutine(FadeOutActive());
        StartCoroutine(FadeOutCanvas());
        UI_ManaGauge.SetColorManaText(Color.black);
    }

    private IEnumerator FadeOutActive()
    {
        Color c = _active.color;

        while (c.a > 0)
        {
            c.a -= 0.01f;

            _active.color = c;

            yield return null;
        }

        _active.gameObject.SetActive(false);
    }

    private IEnumerator FadeInCanvas()
    {
        float alpha = canvasGroup.alpha;
        while (alpha < 1.0f)
        {
            alpha += Time.deltaTime * fadeSpeed;
            canvasGroup.alpha = alpha;
            yield return null;
        }
        canvasGroup.alpha = 1.0f;
    }

    private IEnumerator FadeOutCanvas()
    {
        float alpha = canvasGroup.alpha;
        while (alpha > fadeMinAlpha)
        {
            alpha -= Time.deltaTime * fadeSpeed;
            canvasGroup.alpha = alpha;
            yield return null;
        }
        canvasGroup.alpha = fadeMinAlpha;
    }
}
