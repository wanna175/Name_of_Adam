using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using Mono.Cecil;

public class UI_Tutorial : MonoBehaviour
{
    [SerializeField] private List<GameObject> _uiPages;
    [SerializeField] private List<GameObject> _uiMasks;
    [SerializeField] private GameObject _tooltip;

    public bool ValidToPassTooltip;
    private int _currentIndexToTooltip;

    private void Start()
    {
        SetUIPage(-1);
        SetUIMask(-1);
        CloseToolTip();
        SetValidToPassToolTip(false);
    }

    public void TutorialActive(int i)
    {
        GameManager.Sound.Play("UI/TutorialSFX/TutorialPopupSFX");
        SetUIPage(i);
        TutorialTimeStop();
    }

    private void TutorialTimeStop()
    {
        TutorialManager.Instance.IsTutorialactive = true;
        Time.timeScale = 0;
    }

    private void TutorialTimeStart()
    {
        TutorialManager.Instance.IsTutorialactive = false;
        Time.timeScale = GameManager.OutGameData.GetBattleSpeed();
    }

    public void OnLastCloseButton()
    {
        GameManager.Sound.Play("UI/ButtonSFX/BackButtonClickSFX");
        TutorialManager.Instance.SetNextStep();
        TutorialTimeStart();
    }

    public void OnCloseButton()
    {
        GameManager.Sound.Play("UI/ButtonSFX/BackButtonClickSFX");
        TutorialManager.Instance.ShowNextTutorial();
        TutorialTimeStart();
    }

    public void NextButton()
    {
        GameManager.Sound.Play("UI/ButtonSFX/UIButtonClickSFX");
        TutorialManager.Instance.ShowNextTutorial();
    }

    public void PreviousButton()
    {
        GameManager.Sound.Play("UI/ButtonSFX/UIButtonClickSFX");
        TutorialManager.Instance.ShowPreviousTutorial();
    }

    public void ShowTooltip(string text, int indexToTooltip)
    {
        _tooltip.SetActive(true);
        _tooltip.GetComponentInChildren<TMP_Text>().SetText(text);
        SetCurrentIndexToTooltip(indexToTooltip);
    }

    public IEnumerator ShowTooltip(string text, int indexToTooltip, float openTime)
    {
        yield return new WaitForSeconds(openTime);
        ShowTooltip(text, indexToTooltip);
    }

    public void SetCurrentIndexToTooltip(int index) => _currentIndexToTooltip = index;

    public void CloseToolTip()
    {
        _tooltip.SetActive(false);
        SetValidToPassToolTip(false);
    }

    public void SetValidToPassToolTip(bool isValidToPass)
    {
        ValidToPassTooltip = isValidToPass;
        _uiMasks[_currentIndexToTooltip].GetComponentInChildren<AlphaClicker>().SetEnable(!isValidToPass);
    }

    public void SetUIPage(int index)
    {
        foreach (GameObject go in _uiPages)
            go.SetActive(false);

        if (index >= 0)
            _uiPages[index].SetActive(true);
    }

    public void SetUIMask(int index)
    {
        foreach (GameObject go in _uiMasks)
            go.SetActive(false);

        if (index >= 0)
        {
            var effect = _uiMasks[index].GetComponentInChildren<TutorialArrow>();
            _uiMasks[index].SetActive(true);
            effect.StartEffect();
        }
    }
}
