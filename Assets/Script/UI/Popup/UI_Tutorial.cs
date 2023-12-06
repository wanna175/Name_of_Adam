using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Tutorial : MonoBehaviour
{
    [SerializeField] List<GameObject> Tutorial;

    public void TutorialActive(int i)
    {
        GameManager.Sound.Play("UI/TutorialSFX/TutorialPopupSFX"); 
        Tutorial[i].SetActive(true);
        TutorialTimeStop();
    }

    public void TutorialTimeStop()
    {
        GameManager.Data.isTutorialactive = true;
        Time.timeScale = 0;
    }

    public void TutorialTimeStart()
    {
        GameManager.Data.isTutorialactive = false;
        Time.timeScale = 1;
    }

    public void CloseButton()
    {
        GameManager.Sound.Play("UI/ButtonSFX/BackButtonClickSFX");
    }

    public void NextButton()
    {
        GameManager.Sound.Play("UI/ButtonSFX/UIButtonClickSFX");
    }
}
