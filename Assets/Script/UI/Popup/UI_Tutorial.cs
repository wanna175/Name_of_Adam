using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Tutorial : MonoBehaviour
{
    [SerializeField] List<GameObject> Tutorial;

    public void TutorialActive(int i)
    {
        Tutorial[i].SetActive(true);
        TutorialTimeStop();
    }

    public void TutorialTimeStop()
    {
        GameManager.Instance.isTutorialactive = true;
        Time.timeScale = 0;
    }

    public void TutorialTimeStart()
    {
        GameManager.Instance.isTutorialactive = false;
        Time.timeScale = 1;
    }
}
