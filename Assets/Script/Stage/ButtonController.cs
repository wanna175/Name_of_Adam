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
    GameObject[] NextStageButtons = new GameObject[3];

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
        for (int i = 0; i < 3; i++)
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
            StageButtons[i].SetActive(true);

            // string StageText = (_stageMNG.GetStageArray[i] != null) ? _stageMNG.GetStageArray[i].Name.ToString() : ""; // 기존 시스템
            string StageText = (StageMNG.GetStageArray[i].Name != StageName.none) ? StageMNG.GetStageArray[i].Name.ToString() : ""; // 스마게용

            StageButtons[i].transform.GetChild(2).transform.GetChild(0).
                GetComponent<TextMeshProUGUI>().text = StageText;

            // if (_stageMNG.GetStageArray[i] == null)
            if (StageMNG.GetStageArray[i].Name == StageName.none) // 스마게용
                //StageButtons[i].GetComponent<Image>().color = Color.clear;
                StageButtons[i].SetActive(false);
            else
                StageButtons[i].transform.GetChild(1).GetComponent<Image>().sprite = StageMNG.GetStageArray[i].Background;
        }

        /* // 5개의 랜덤 스테이지를 가져오는 기존의 시스템
        for(int i = 0; i < NextStageButtons.Length; i++)
        {
            Stage nextStage = _stageMNG.GetNextStageArray[i + 1];
            string StageText = (nextStage != null) ? nextStage.Name.ToString() : "";
            
            NextStageButtons[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().
                text = StageText;

            if (nextStage == null)
                NextStageButtons[i].GetComponent<Image>().color = Color.clear;
            else
                NextStageButtons[i].GetComponent<Image>().sprite = nextStage.Background;
        }
        */
    }


    public void ButtonClick(GameObject ClickObject)
    {
        int index = GetIndex(ClickObject);

        StageMNG.MoveNextStage(index);
    }
    

    public void HoverEnter(GameObject FocusObject)
    {
        int index = GetIndex(FocusObject);

        FadeController FC = NextStageButtons[index].transform.GetChild(0).GetComponent<FadeController>();

        FC.StartFadeIn();
    }

    public void HoverExit(GameObject FocusObject)
    {
        int index = GetIndex(FocusObject);

        FadeController FC = NextStageButtons[index].transform.GetChild(0).GetComponent<FadeController>();

        FC.StartFadeOut();
    }
}
