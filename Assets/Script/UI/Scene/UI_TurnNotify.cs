using UnityEngine;
using UnityEngine.UI;

public class UI_TurnNotify : UI_Scene
{
    const float fadeTime = 0.5f;
    const float displayTime = 0.5f;

    private Sprite _playerTurn;
    private Sprite _unitTurn;

    private bool _displayFlag;

    private CanvasGroup _canvasGroup;

    private void Awake()
    {
        _playerTurn = GameManager.Resource.Load<Sprite>($"Arts/UI/Battle_UI/Text/PlayerTurnText");
        _unitTurn = GameManager.Resource.Load<Sprite>($"Arts/UI/Battle_UI/Text/UnitTurnText");

        _canvasGroup = GetComponent<CanvasGroup>();

        Hide();
    }

    public void SetPlayerTurn()
    {
        _displayFlag = true;

        FadeIn();
        Invoke(nameof(FadeOut), fadeTime + displayTime);

        GetComponent<Image>().sprite = _playerTurn;
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

        GetComponent<Image>().sprite = _unitTurn;
    }

    public void FadeIn()
    {
        if (!gameObject.activeSelf)
            return;

        GetComponent<FadeController>().StartFadeIn();
    }

    public void FadeOut()
    {
        if (!gameObject.activeSelf)
            return;

        GetComponent<FadeController>().StartFadeOut();
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
