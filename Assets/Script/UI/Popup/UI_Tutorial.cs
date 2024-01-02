using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using Mono.Cecil;

public class UI_Tutorial : MonoBehaviour
{
    [SerializeField]
    private Transform UIPageParent; 

    private List<GameObject> UIPages;

    [SerializeField]
    private Transform UIMaskParent;

    private List<GameObject> UIMasks;

    [SerializeField]
    private GameObject tooltip;

    public bool ValidToPassTooltip;

    private int currentIndexToTooltip;

    private void Start()
    {
        UIPages = new List<GameObject>();
        for (int i = 0; i < UIPageParent.childCount; i++)
            UIPages.Add(UIPageParent.GetChild(i).gameObject);

        UIMasks = new List<GameObject>();
        for (int i = 0; i < UIMaskParent.childCount; i++)
            UIMasks.Add(UIMaskParent.GetChild(i).gameObject);

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
        Time.timeScale = 1;
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
        tooltip.SetActive(true);
        tooltip.GetComponentInChildren<TMP_Text>().SetText(text);
        SetCurrentIndexToTooltip(indexToTooltip);
    }

    public IEnumerator ShowTooltip(string text, int indexToTooltip, float openTime)
    {
        yield return new WaitForSeconds(openTime);
        ShowTooltip(text, indexToTooltip);
    }

    public void SetCurrentIndexToTooltip(int index) => currentIndexToTooltip = index;

    public void CloseToolTip()
    {
        tooltip.SetActive(false);
        SetValidToPassToolTip(false);
    }

    public void SetValidToPassToolTip(bool isValidToPass)
    {
        ValidToPassTooltip = isValidToPass;
        UIMasks[currentIndexToTooltip].GetComponent<AlphaClicker>().SetEnable(!isValidToPass);
    }

    public void SetUIPage(int index)
    {
        foreach (GameObject go in UIPages)
            go.SetActive(false);

        if (index >= 0)
            UIPages[index].SetActive(true);
    }

    public void SetUIMask(int index)
    {
        foreach (GameObject go in UIMasks)
            go.SetActive(false);

        if (index >= 0)
            UIMasks[index].SetActive(true);
    }
}
