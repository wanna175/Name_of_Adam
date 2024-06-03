using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_TurnNotify : UI_Scene
{
    const float fadeTime = 0.75f;
    const float displayTime = 1.0f;

    private Sprite _playerTurn;
    private Sprite _unitTurn;

    private bool _displayFlag;
    private bool _isFirstTurn;

    [SerializeField] private Image _image;
    [SerializeField] private GameObject _firstPlayerTurnInfo;
    [SerializeField] private FadeController _fadeController;
    [SerializeField] private CanvasGroup _canvasGroup;

    private void Awake()
    {
        _playerTurn = GameManager.Resource.Load<Sprite>($"Arts/UI/Battle_UI/Text/PlayerTurnText");
        _unitTurn = GameManager.Resource.Load<Sprite>($"Arts/UI/Battle_UI/Text/UnitTurnText");

        Hide();
    }

    public void SetPlayerTurn(bool isFirst)
    {
        _isFirstTurn = isFirst;
        _firstPlayerTurnInfo.SetActive(_isFirstTurn);

        _displayFlag = true;

        FadeIn();
        Invoke(nameof(FadeOut), fadeTime + displayTime);

        _image.sprite = _playerTurn;
    }

    public void SetUnitTurn()
    {
        _firstPlayerTurnInfo.SetActive(false);

        if (_displayFlag) 
        {
            Invoke(nameof(SetUnitTurn), fadeTime + displayTime + fadeTime);

            return;
        }

        FadeIn();
        Invoke(nameof(FadeOut), fadeTime + displayTime);

        _image.sprite = _unitTurn;
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
        _isFirstTurn = false;
    }

    public void Off()
    {
        gameObject.SetActive(false);
    }
}
