using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_TurnNotify : UI_Scene
{
    const float fadeTime = 0.75f;
    const float displayTime = 1.0f;

    [SerializeField] private Sprite _preparationPhase;
    [SerializeField] private Sprite _battlePhase;

    private bool _displayFlag;

    [SerializeField] private Image _image;
    [SerializeField] private FadeController _fadeController;
    [SerializeField] private CanvasGroup _canvasGroup;

    private void Awake()
    {
        Hide();
    }

    public void SetPlayerTurn()
    {
        _displayFlag = true;

        FadeIn();
        Invoke(nameof(FadeOut), fadeTime + displayTime);

        _image.sprite = _preparationPhase;
    }

    public void SetUnitTurn()
    {
        if (_displayFlag) 
        {
            Invoke(nameof(SetUnitTurn), fadeTime + displayTime + fadeTime);

            return;
        }

        FadeIn();
        Invoke(nameof(FadeOut), fadeTime + displayTime);

        _image.sprite = _battlePhase;
    }

    public void FadeIn()
    {
        if (!gameObject.activeSelf)
            return;

        _fadeController.StartFadeIn();
    }

    public void FadeOut()
    {
        if (!gameObject.activeSelf)
            return;

        _fadeController.StartFadeOut();
        Invoke(nameof(Hide), fadeTime);
    }

    private void Hide()
    {
        _canvasGroup.alpha = 0;
        _displayFlag = false;
    }

    public void Off()
    {
        gameObject.SetActive(false);
    }
}
