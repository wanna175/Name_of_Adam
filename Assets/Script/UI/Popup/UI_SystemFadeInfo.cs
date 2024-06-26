using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_SystemFadeInfo : MonoBehaviour
{
    [SerializeField] private RectTransform _canvasRect;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private TMP_Text _infoText;

    private readonly float goalAlpha = 0.75f;
    private readonly float fadeSpeed = 1.5f;
    private readonly float delayTime = 1.0f;

    private Vector2 createPos;
    private Vector2 goalPos;

    public void Init(Vector2 createPos, string info)
    {
        this.createPos = createPos;
        this.goalPos = createPos + new Vector2(0.0f, 15.0f);

        _canvasRect.anchoredPosition = createPos;
        _infoText.SetText(info);

        StartCoroutine(nameof(MoveUp));
        StartCoroutine(nameof(Fade));
    }

    IEnumerator MoveUp()
    {
        float time = 0.0f;
        float goalTime = goalAlpha / fadeSpeed;

        while (time < goalTime)
        {
            time += Time.deltaTime;
            _canvasRect.anchoredPosition = Vector2.Lerp(createPos, goalPos, time / goalTime);
            yield return null;
        }

        _canvasRect.anchoredPosition = goalPos;
    }

    IEnumerator Fade()
    {
        _canvasGroup.alpha = 0.0f;

        while (_canvasGroup.alpha < goalAlpha)
        {
            _canvasGroup.alpha += Time.deltaTime * fadeSpeed;
            yield return null;
        }

        _canvasGroup.alpha = goalAlpha;
        yield return new WaitForSeconds(delayTime);

        while (_canvasGroup.alpha > 0.0f)
        {
            _canvasGroup.alpha -= Time.deltaTime * fadeSpeed;
            yield return null;
        }

        GameManager.Resource.Destroy(this.gameObject);
    }
}
