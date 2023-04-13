using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonController : MonoBehaviour
{ 
    [SerializeField] StageManager StageMNG;

    [SerializeField] GameObject StageButtonContainer;
    [SerializeField] GameObject NextStageButtonContainer;

    GameObject[] StageButtons = new GameObject[3];
    GameObject[] NextStageButtons = new GameObject[5];

    private void Start()
    {
        CreateButton();
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

    private void CreateButton()
    {
        ResourceManager resource = GameManager.Resource;

        for(int i = 0; i < 3; i++)
        {
            GameObject StageButton = resource.Instantiate("UI/Stage/BTN_StageSelect", StageButtonContainer.transform);
            StageButton.AddComponent<StageButtonEventTrigger>().Init(this);

            StageButtons[i] = StageButton;
        }
        for (int i = 0; i < 5; i++)
        {
            GameObject NextStageButton = resource.Instantiate("UI/Stage/BTN_StageSelect", NextStageButtonContainer.transform);

            NextStageButtons[i] = NextStageButton;
        }

        SetButtons();
    }

    private void SetButtons()
    {
        for (int i = 0; i < StageButtons.Length; i++)
        {
            string StageText = (StageMNG.GetStageArray[i] != null) ? StageMNG.GetStageArray[i].Name.ToString() : "";

            StageButtons[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().
                text = StageText;

            if (StageMNG.GetStageArray[i] == null)
                StageButtons[i].GetComponent<Image>().color = Color.clear;
            else
                StageButtons[i].GetComponent<Image>().sprite = StageMNG.GetStageArray[i].Background;
        }

        for(int i = 0; i < NextStageButtons.Length; i++)
        {
            string StageText = (StageMNG.GetNextStageArray[i] != null) ? StageMNG.GetNextStageArray[i].Name.ToString() : "";
            
            NextStageButtons[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().
                text = StageText;

            if (StageMNG.GetNextStageArray[i] == null)
                NextStageButtons[i].GetComponent<Image>().color = Color.clear;
            else
                NextStageButtons[i].GetComponent<Image>().sprite = StageMNG.GetNextStageArray[i].Background;
        }
    }


    public void ButtonClick(GameObject ClickObject)
    {
        int index = GetIndex(ClickObject);

        StageMNG.MoveNextStage(index);
    }

    #region Hover 시 이미지를 바꾸는 버전
    /*
    public void HoverEnter(GameObject FocusObject)
    {
        int index = GetIndex(FocusObject);

        if (_stageMNG.GetStageArray[index] != null)
            StageButtons[index].GetComponent<Image>().sprite = null;

        for (int i = 0; i < 3; i++)
        {
            if (_stageMNG.GetNextStageArray[index + i] != null)
                NextStageButtons[index + i].GetComponent<Image>().sprite = null;
        }
    }

    public void HoverExit(GameObject FocusObject)
    {
        int index = GetIndex(FocusObject);

        if (_stageMNG.GetStageArray[index] != null)
            StageButtons[index].GetComponent<Image>().sprite = _stageMNG.GetStageArray[index].Background;

        for (int i = 0; i < 3; i++)
        {
            if (_stageMNG.GetNextStageArray[index + i] != null)
                NextStageButtons[index + i].GetComponent<Image>().sprite = _stageMNG.GetNextStageArray[index + i].Background;
        }
    }
    */
    #endregion

    #region Hover 시 색만 어둡게 하는 버전

    public void HoverEnter(GameObject FocusObject)
    {
        int index = GetIndex(FocusObject);

        if (StageMNG.GetStageArray[index] != null)
            StageButtons[index].GetComponent<Image>().color = Color.gray;

        for (int i = 0; i < 3; i++)
        {
            if (StageMNG.GetNextStageArray[index + i] != null)
                NextStageButtons[index + i].GetComponent<Image>().color = Color.gray;
        }
    }

    public void HoverExit(GameObject FocusObject)
    {
        int index = GetIndex(FocusObject);

        if (StageMNG.GetStageArray[index] != null)
            StageButtons[index].GetComponent<Image>().color = Color.white;

        for (int i = 0; i < 3; i++)
        {
            if (StageMNG.GetNextStageArray[index + i] != null)
                NextStageButtons[index + i].GetComponent<Image>().color = Color.white;
        }
    }

    #endregion
}
