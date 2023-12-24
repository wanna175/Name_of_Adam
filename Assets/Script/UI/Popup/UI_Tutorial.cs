using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_Tutorial : MonoBehaviour
{
    [SerializeField] 
    private List<GameObject> Tutorial;

    [SerializeField]
    private GameObject tooltip;

    public bool isWorkableTooltip;

    private void Start()
    {
        CloseToolTip();
        SetWorkableToolTip(false);
    }

    public void TutorialActive(int i)
    {
        GameManager.Sound.Play("UI/TutorialSFX/TutorialPopupSFX"); 
        Tutorial[i].SetActive(true);
        TutorialTimeStop();
    }

    public void TutorialTimeStop()
    {
        TutorialManager.Instance.isTutorialactive = true;
        Time.timeScale = 0;
    }

    private void TutorialTimeStart()
    {
        TutorialManager.Instance.isTutorialactive = false;
        Time.timeScale = 1;
    }

    public void OnCloseButton()
    {
        GameManager.Sound.Play("UI/ButtonSFX/BackButtonClickSFX");
        TutorialManager.Instance.SetNextStep();
        TutorialManager.Instance.ShowTutorial();
        TutorialTimeStart();
    }

    public void NextButton()
    {
        GameManager.Sound.Play("UI/ButtonSFX/UIButtonClickSFX");
    }

    public void ShowTooltip(string text)
    {
        tooltip.SetActive(true);
        tooltip.GetComponentInChildren<TMP_Text>().SetText(text);
        SetWorkableToolTip(true);
    }

    public void CloseToolTip()
    {
        tooltip.SetActive(false);
        SetWorkableToolTip(false);
    }

    public void SetWorkableToolTip(bool isWorkable) => isWorkableTooltip = isWorkable;
}
