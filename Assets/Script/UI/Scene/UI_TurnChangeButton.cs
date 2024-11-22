using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class UI_TurnChangeButton : UI_Scene, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Button _changeButton;
    [SerializeField] private Image _buttonImage;
    [SerializeField] private Image _lightImage;

    [SerializeField] private Sprite _endPhaseSprite;
    [SerializeField] private Sprite _skipAttackSprite;
    [SerializeField] private Sprite _skipMoveSprite;
    [SerializeField] private Sprite _enemyTurnSprite;

    private bool _isCanCtrl;

    public void SetEnable(bool enable)
    {
        _isCanCtrl = enable;
        //Debug.Log("Turn Change Button : " + enable);
    }

    public void TurnChange()
    {
        GameManager.Sound.Play("UI/UISFX/UIButtonSFX");

        if (_isCanCtrl)
        {
            PhaseController phase = BattleManager.Phase;
            _isCanCtrl = false;

            if (phase.CurrentPhaseCheck(phase.Prepare))
            {
                phase.ChangePhase(phase.Engage);
                _buttonImage.sprite = _skipMoveSprite;
            }
            else if (phase.CurrentPhaseCheck(phase.Move))
            {
                phase.ChangePhase(phase.Action);
                _buttonImage.sprite = _skipAttackSprite;
            }
            else if (phase.CurrentPhaseCheck(phase.Action))
            {
                BattleManager.Data.BattleOrderRemove(BattleManager.Data.GetNowUnitOrder());
                phase.ChangePhase(phase.Engage);
            }
        }
    }

    public void SetButtonSprite()
    {
        if (BattleManager.Phase.CurrentPhaseCheck(BattleManager.Phase.Prepare))
        {
            _buttonImage.sprite = _endPhaseSprite;
        }
        else if (BattleManager.Phase.CurrentPhaseCheck(BattleManager.Phase.Action) || BattleManager.Phase.CurrentPhaseCheck(BattleManager.Phase.Move))
        {
            if (BattleManager.Data.GetNowUnit().Team == Team.Player)
            {
                _buttonImage.sprite = BattleManager.Phase.CurrentPhaseCheck(BattleManager.Phase.Move)
                                      ? _skipMoveSprite : _skipAttackSprite;
            }
            else
            {
                _buttonImage.sprite = _enemyTurnSprite;
                _changeButton.interactable = false;
                return;
            }
        }

        _changeButton.interactable = true;
    }

    //private bool _isHover = false;
    //private bool _isHoverMessegeOn = false;

    public void OnPointerEnter(PointerEventData eventData)
    {
        return;
        /*
        _isHover = true;
        GameManager.Instance.PlayAfterCoroutine(() => {
            if (_isHover && !_isHoverMessegeOn)
            {
                _isHoverMessegeOn = true;
                GameManager.UI.ShowHover<UI_TextHover>().SetText(
                    $"{GameManager.Locale.GetLocalizedBattleScene("TurnChange UI Info")}", Input.mousePosition);
            }
        }, 0.5f);
        */
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        return;
        /*
        _isHover = false;

        if (_isHoverMessegeOn)
        {
            _isHoverMessegeOn = false;
            GameManager.UI.CloseHover();
        }
        */
    }
}
