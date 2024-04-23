using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainSceneController : MonoBehaviour
{
    [SerializeField] GameObject Canvas;
    [SerializeField] GameObject ContinueBox;

    private void Start()
    {
        if (GameManager.SaveManager.SaveFileCheck())
            ContinueBox.SetActive(true);
        else
            ContinueBox.SetActive(false);

        if (GameManager.OutGameData.IsPhanuelClear() && GameManager.OutGameData.GetIsOnMainTooltipForPhanuel() == false)
        {
            GameManager.OutGameData.SetIsOnMainTooltipForPhanuel(true);
            GameManager.OutGameData.SaveData();
            GameManager.UI.ShowPopup<UI_SystemInfo>().Init("PhanuelClear", "PhanuelTooltip");
        }

        if (GameManager.OutGameData.IsHorusClear() && GameManager.OutGameData.GetIsOnMainTooltipForHorus() == false)
        {
            GameManager.OutGameData.SetIsOnMainTooltipForHorus(true);
            GameManager.OutGameData.SaveData();
            GameManager.UI.ShowPopup<UI_SystemInfo>().Init("HorusClear", "HorusTooltip");
        }
    }

    public void StartButton()
    {
        if (GameManager.SaveManager.SaveFileCheck())
        {
            GameManager.Sound.Play("UI/ButtonSFX/UIButtonClickSFX");
            GameManager.UI.ShowPopup<UI_SystemSelect>().Init("Restart", ResetAlertYesButton);
        }
        else
        {
            GameManager.Data.DeckClear();
            GameManager.Data.HallDeckSet();
            GameManager.Data.GameData.FallenUnits.AddRange(GameManager.Data.GameData.DeckUnits);
            Destroy(GameManager.Instance.gameObject);

            GameManager.SaveManager.DeleteSaveData();
            GameManager.Sound.Play("UI/ButtonSFX/UIButtonClickSFX");

            /*if (GameManager.OutGameData.IsGameOverCheck()==false)
            {
                //npc관련 데이터 초기화
                GameManager.OutGameData.ResetNPCQuest();
            }
            else
            {
                GameManager.OutGameData.SetIsGameOverCheck(false);
            }*/

            if (GameManager.OutGameData.IsTutorialClear())
            {
                GameManager.Data.HallSelectedDeckSet();
                SceneChanger.SceneChange("DifficultySelectScene");
            }
            else
            {
                SceneChanger.SceneChangeToCutScene(CutSceneType.Main);
            }
        }
    }

    public void ContinueBotton()
    {
        GameManager.Sound.Play("UI/ButtonSFX/UIButtonClickSFX");
        if (GameManager.SaveManager.SaveFileCheck())
            SceneChanger.SceneChange("StageSelectScene");
    }
    public void ProgressButton()
    {
        GameManager.Sound.Play("UI/ButtonSFX/UIButtonClickSFX");
        SceneChanger.SceneChange("ProgressShopScene");
    }

    public void HallButton()
    {
        GameManager.Sound.Play("UI/ButtonSFX/UIButtonClickSFX");
        GameManager.Data.HallDeckSet();
        GameManager.UI.ShowPopup<UI_MyDeck>("UI_MyDeck").Init(false);
    }

    public void OptionButton()
    {
        GameManager.Sound.Play("UI/ButtonSFX/UIButtonClickSFX");
        UI_Option go = GameManager.UI.ShowPopup<UI_Option>();
        //GameObject go = Resources.Load<GameObject>("Prefabs/UI/Popup/UI_Option");
        //GameObject.Instantiate(go, Canvas.transform);
    }

    public void ResetAlertYesButton()
    {
        // 게임오브젝트를 생성해서 보내주기 & 생성한 오브젝트가 맵 선택 씬에 도달했을 때 활성화되서 튜토 이미지 띄우고 자신 삭제하기
        GameManager.Data.DeckClear();
        GameManager.Data.GameData.FallenUnits.AddRange(GameManager.Data.GameData.DeckUnits);
        Destroy(GameManager.Instance.gameObject);

        GameManager.SaveManager.DeleteSaveData();
        GameManager.Sound.Play("UI/ButtonSFX/UIButtonClickSFX");
        /*if (GameManager.OutGameData.IsGameOverCheck() == false)
        {
            //npc관련 데이터 초기화
            GameManager.OutGameData.ResetNPCQuest();
        }
        else
        {
            GameManager.OutGameData.SetIsGameOverCheck(false);
        }*/

        if (GameManager.OutGameData.IsTutorialClear())
        {
            GameManager.Data.HallDeckSet();
            GameManager.Data.HallSelectedDeckSet();
            SceneChanger.SceneChange("DifficultySelectScene");
        }
        else
        {
            SceneChanger.SceneChange("CutScene");
        }
    }

    public void ExitButton()
    {
        GameManager.Sound.Play("UI/ButtonSFX/UIButtonClickSFX");
        Application.Quit();
    }
}