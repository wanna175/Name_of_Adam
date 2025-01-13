using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_TurnNotify : UI_Scene
{
    [SerializeField] private GameObject _preparationPhase;
    [SerializeField] private GameObject _battlePhase;

    private bool _displayFlag;

    public void SetPreparationPhaseDisplay()
    {
        _displayFlag = true;

        _battlePhase.SetActive(false);
        _preparationPhase.SetActive(true);

        GameManager.Instance.PlayAfterCoroutine(() =>
        {
            _displayFlag = false;
        }, 2f);
    }

    public void SetBattlePhaseDisplay()
    {
        if (_displayFlag)
        {
            GameManager.Instance.PlayAfterCoroutine(() =>
            {
                SetBattlePhaseDisplay();
            }, 0.1f);
        }
        else
        {
            _preparationPhase.SetActive(false);
            _battlePhase.SetActive(true);
        }
    }
}
