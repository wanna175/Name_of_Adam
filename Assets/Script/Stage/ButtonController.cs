using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonController : MonoBehaviour
{
    [SerializeField] GameObject[] StageButtons = new GameObject[3];
    [SerializeField] GameObject[] NextStageButtons = new GameObject[5];

    StageManager _stageMNG;


    private void Start()
    {
        _stageMNG = GameManager.StageMNG;

        SetButtonText();
    }

    public int GetIndex(GameObject obj)
    {
        for (int i = 0; i < StageButtons.Length; i++)
        {
            if (ReferenceEquals(StageButtons[i], obj))
                return i;
        }

        return -1;
    }

    private void SetButtonText()
    {
        for (int i = 0; i < StageButtons.Length; i++)
        {
            string StageText = (_stageMNG.GetStageArray[i] != null) ? _stageMNG.GetStageArray[i].Name : "";

            StageButtons[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().
                text = StageText;

            if (_stageMNG.GetStageArray[i] == null)
                StageButtons[i].GetComponent<Image>().color = new Color(0, 0, 0, 0);
        }

        for(int i = 0; i < NextStageButtons.Length; i++)
        {
            string StageText = (_stageMNG.GetNextStageArray[i] != null) ? _stageMNG.GetNextStageArray[i].Name : "";
            
            NextStageButtons[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().
                text = StageText;

            if (_stageMNG.GetNextStageArray[i] == null)
                NextStageButtons[i].GetComponent<Image>().color = new Color(0, 0, 0, 0);
        }
    }


    public void ButtonClick(GameObject ClickObject)
    {
        int index = GetIndex(ClickObject);

        _stageMNG.MoveNextStage(index);

        SetButtonText();
    }

    public void HoverEnter(GameObject FocusObject)
    {
        int index = GetIndex(FocusObject);

        if (_stageMNG.GetStageArray[index] != null)
            StageButtons[index].GetComponent<Image>().color = Color.white;

        for (int i = 0; i < 3; i++)
        {
            if (_stageMNG.GetNextStageArray[index + i] != null)
               NextStageButtons[index + i].GetComponent<Image>().color = Color.white;
        }
    }

    public void HoverExit(GameObject FocusObject)
    {
        int index = GetIndex(FocusObject);

        if (_stageMNG.GetStageArray[index] != null)
            StageButtons[index].GetComponent<Image>().color = Color.red;

        for (int i = 0; i < 3; i++)
        {
            if (_stageMNG.GetNextStageArray[index + i] != null)
                NextStageButtons[index + i].GetComponent<Image>().color = Color.red;
        }
    }
}
