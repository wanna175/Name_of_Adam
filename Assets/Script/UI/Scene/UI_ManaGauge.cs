using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class UI_ManaGauge : UI_Scene, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Animator ManaAnimator;
    [SerializeField] TextMeshProUGUI _currentMana;
    [SerializeField] UI_CannotEffect cannotEffect;
    public UI_CannotEffect CannotEffect => cannotEffect;

    public void Init()
    {
        cannotEffect.Init(Vector3.one, Vector3.one * 1.2f, 1.5f);
    }

    public void DrawGauge(int max, int current)
    {
        //_bluemanaIMG.fillAmount = (float)current / max;
        SetGauge(current);
        _currentMana.text = current.ToString();
    }

    public void SetGauge(int mana)
    {
        int manaLevel = Mathf.Clamp(mana / 10 * 10, 0, 100);
        string triggerName = "Mana" + manaLevel;
        ManaAnimator.SetTrigger(triggerName);
    }

    private bool _isHover = false;
    private bool _isHoverMessegeOn = false;

    public void OnPointerEnter(PointerEventData eventData)
    {
        _isHover = true;
        BattleManager.Instance.PlayAfterCoroutine(() => {
            if (_isHover)
            {
                _isHoverMessegeOn = true;
                GameManager.UI.ShowHover<UI_TextHover>().SetText(
                    $"{GameManager.Locale.GetLocalizedBattleScene("ManaGauge UI Info")}", Input.mousePosition);
            }
        }, 0.5f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _isHover = false;

        if (_isHoverMessegeOn)
        {
            _isHoverMessegeOn = false;
            GameManager.UI.CloseHover();
        }
    }
}