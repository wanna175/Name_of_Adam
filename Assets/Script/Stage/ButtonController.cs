using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonController : MonoBehaviour
{
    [SerializeField] GameObject[] NextStageButton = new GameObject[3];
    [SerializeField] GameObject[] AfterNextStageButton = new GameObject[5];

    StageManager _stageMNG;


    private void Start()
    {
        _stageMNG = GameManager.StageMNG;

        SetButtonText();
    }

    public int GetIndex(GameObject obj)
    {
        for (int i = 0; i < NextStageButton.Length; i++)
        {
            if (ReferenceEquals(NextStageButton[i], obj))
                return i;
        }

        return -1;
    }

    void SetButtonText()
    {
        for (int i = 0; i < NextStageButton.Length; i++)
        {
            NextStageButton[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().
                text = _stageMNG.GetNextStage[i];

            if (_stageMNG.GetNextStage[i] == null)
                NextStageButton[i].GetComponent<Image>().color = new Color(0, 0, 0, 0);
        }

        for(int i = 0; i < AfterNextStageButton.Length; i++)
        {
            AfterNextStageButton[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().
                text = _stageMNG.GetAfterNextStage[i];

            if (_stageMNG.GetAfterNextStage[i] == null)
                AfterNextStageButton[i].GetComponent<Image>().color = new Color(0, 0, 0, 0);
        }
    }


    public void ButtonClick(GameObject ClickObject)
    {
        int index = GetIndex(ClickObject);

        _stageMNG.MoveNextStage(index);

        SetButtonText();
    }

    public void PointerEnter(GameObject FocusObject)
    {
        int index = GetIndex(FocusObject);

        NextStageButton[index].GetComponent<Image>().color = Color.white;

        for(int i = 0; i < 3; i++)
            AfterNextStageButton[index + i].GetComponent<Image>().color = Color.white;
    }

    public void PointerExit(GameObject FocusObject)
    {
        int index = GetIndex(FocusObject);

        NextStageButton[index].GetComponent<Image>().color = Color.red;

        for (int i = 0; i < 3; i++)
            AfterNextStageButton[index + i].GetComponent<Image>().color = Color.red;
    }
}
