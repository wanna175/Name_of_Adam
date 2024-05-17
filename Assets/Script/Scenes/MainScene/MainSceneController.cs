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
            GameManager.Data.SetDeck(GameManager.OutGameData.SetHallDeck());
            GameManager.Sound.Play("UI/ButtonSFX/UIButtonClickSFX");

            if (GameManager.OutGameData.IsTutorialClear())
            {
                GameManager.Data.HallDeckSet();
                SceneChanger.SceneChange("DifficultySelectScene");
            }
            else
            {
                GameManager.Data.GameData.FallenUnits.AddRange(GameManager.Data.GameData.DeckUnits);
                SceneChanger.SceneChangeToCutScene(CutSceneType.Main);
            }

            GameManager.SaveManager.DeleteSaveData();
            Destroy(GameManager.Instance.gameObject);
        }
    }

    public void ContinueBotton()
    {
        GameManager.Sound.Play("UI/ButtonSFX/UIButtonClickSFX");
        if (GameManager.SaveManager.SaveFileCheck())
        {
            SceneChanger.SceneChange("StageSelectScene");
            GameManager.SaveManager.LoadGame();
        }
    }
    public void ProgressButton()
    {
        GameManager.Sound.Play("UI/ButtonSFX/UIButtonClickSFX");
        SceneChanger.SceneChange("ProgressShopScene");
    }

    public void HallButton()
    {
        GameManager.Sound.Play("UI/ButtonSFX/UIButtonClickSFX");
        GameManager.Data.SetDeck(GameManager.OutGameData.SetHallDeck());
        GameManager.UI.ShowPopup<UI_MyDeck>().Init();
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
        GameManager.Data.SetDeck(GameManager.OutGameData.SetHallDeck());
        GameManager.Sound.Play("UI/ButtonSFX/UIButtonClickSFX");

        if (GameManager.OutGameData.IsTutorialClear())
        {
            GameManager.Data.HallDeckSet();
            SceneChanger.SceneChange("DifficultySelectScene");
        }
        else
        {
            GameManager.Data.GameData.FallenUnits.AddRange(GameManager.Data.GameData.DeckUnits);
            SceneChanger.SceneChange("CutScene");
        }

        GameManager.SaveManager.DeleteSaveData();
        Destroy(GameManager.Instance.gameObject);
    }

    public void DiscordButton()
    {
        Application.OpenURL("https://discord.com/invite/DhN6RRYxy5");
    }

    public void XButton()
    {
        Application.OpenURL("https://x.com/Revelatio_");
    }

    public void ExitButton()
    {
        GameManager.Sound.Play("UI/ButtonSFX/UIButtonClickSFX");
        Application.Quit();
    }
}