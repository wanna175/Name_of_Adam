using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Tutorial : MonoBehaviour
{
    [SerializeField] List<GameObject> Tutorial;

    public void TutorialActive(int i)
    {
        Tutorial[i].SetActive(true);
    }
}
