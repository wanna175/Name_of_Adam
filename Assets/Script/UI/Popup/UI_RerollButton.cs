using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class UI_RerollButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Button _button;
    [SerializeField] private Image _buttonImage;
    [SerializeField] private Image _light;

    private const float _fadeSpeed = 2.0f;

    private bool _isActive;

    public void Init(Action buttonAction)
    {
        _button.onClick.AddListener(() =>
        {
            if (_isActive)
                buttonAction();
            else
                GameManager.Sound.Play("UI/ClickSFX/ClickFailSFX");
        });
        SetActive(false);
    }


    public void SetActive(bool active)
    {
        StopAllCoroutines();

        _isActive = active;
        _buttonImage.color = (active) ? Color.white : Color.gray;
        _light.color = new Color(1, 1, 1, 0);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!_isActive)
            return;

        StopAllCoroutines();
        StartCoroutine(FadeIn());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!_isActive)
            return;

        StopAllCoroutines();
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeIn()
    {
        Color color = _light.color;

        while(color.a < 1.0f)
        {
            color.a += Time.deltaTime * _fadeSpeed;
            _light.color = color;
            yield return null;
        }
    }

    IEnumerator FadeOut()
    {
        Color color = _light.color;

        while(color.a > 0)
        {
            color.a -= Time.deltaTime * _fadeSpeed;
            _light.color = color;
            yield return null;
        }
    }
}