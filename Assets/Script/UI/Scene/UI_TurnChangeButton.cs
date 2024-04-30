using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;


public class UI_TurnChangeButton : UI_Scene, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Button _changeButton;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Image _buttonImage;
    [SerializeField] private Sprite buttonEnterSprite;
    [SerializeField] private Sprite buttonExitSprite;

    private bool isCanCtrl;

    public void OnPointerEnter(PointerEventData eventData)
    {
        _buttonImage.sprite = buttonEnterSprite;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _buttonImage.sprite = buttonExitSprite;
    }

    public void SetEnable(bool enable)
        => isCanCtrl = enable;

    public void TurnChange()
    {
        GameManager.Sound.Play("Stage_Transition/Engage/EngageEnter");
        
        if (isCanCtrl)
        {
            PhaseController _phase = BattleManager.Phase;
            isCanCtrl = false;

            if (_phase.CurrentPhaseCheck(_phase.Prepare))
                _phase.ChangePhase(_phase.Engage);
            else if (_phase.CurrentPhaseCheck(_phase.Move))
                _phase.ChangePhase(_phase.Action);
            else if (_phase.CurrentPhaseCheck(_phase.Action))
            {
                BattleManager.Data.BattleOrderRemove(BattleManager.Data.GetNowUnitOrder());
                _phase.ChangePhase(_phase.Engage);
            }
        }
    }
}
