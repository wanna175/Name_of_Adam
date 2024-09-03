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
    [SerializeField] private TextMeshProUGUI _chapterText;

    private void Start()
    {
        if (GameManager.SaveManager.SaveFileCheck())
            ContinueBox.SetActive(true);
        else
            ContinueBox.SetActive(false);

        if (GameManager.OutGameData.IsYohrnClear())
        {
            _chapterText.text = "Endless";
        }
        else if (GameManager.OutGameData.IsHorusClear())
        {
            _chapterText.text = "Chapter 3";
        }
        else if (GameManager.OutGameData.IsPhanuelClear())
        {
            _chapterText.text = "Chapter 2";
        }
        else
        {
            _chapterText.text = "Chapter 1";
        }

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

    public void NewGameButton()
    {
        GameManager.Sound.Play("UI/UISFX/UIImportantButtonSFX");

        if (GameManager.SaveManager.SaveFileCheck())
        {
            GameManager.UI.ShowPopup<UI_SystemSelect>().Init("Restart", ResetAlertYesButton);
        }
        else
        {
            GameManager.Data.DeckClear();
            GameManager.Data.SetDeck(GameManager.OutGameData.SetHallDeck());

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
            GameManager.Data.Init();
        }
    }

    public void ContinueBotton()
    {
        GameManager.Sound.Play("UI/UISFX/UIImportantButtonSFX");

        if (GameManager.SaveManager.SaveFileCheck())
        {
            SceneChanger.SceneChange("StageSelectScene");
            GameManager.SaveManager.LoadGame();
        }
    }

    public void DivineHallButton()
    {
        GameManager.Sound.Play("UI/UISFX/UIButtonSFX");

        GameManager.Data.SetDeck(GameManager.OutGameData.SetHallDeck());
        UI_MyDeck UI_MyDeck = GameManager.UI.ShowPopup<UI_MyDeck>();
        UI_MyDeck.Init();
        UI_MyDeck.EventInit((DeckUnit) =>
        {
            if (DeckUnit.PrivateKey.Contains("Origin"))
            {
                UI_SystemInfo UI_SystemInfo = GameManager.UI.ShowPopup<UI_SystemInfo>();
                UI_SystemInfo.Init("HallDeleteCannotInfo", "");
            }
            else
            {
                UI_SystemSelect UI_SystemSelect = GameManager.UI.ShowPopup<UI_SystemSelect>();
                UI_SystemSelect.Init("HallDeleteInfo", () =>
                {
                    GameManager.OutGameData.RemoveHallUnit(DeckUnit.PrivateKey);
                    GameManager.OutGameData.SaveData();
                    GameManager.UI.ClosePopup();

                    GameManager.Data.GetDeck().Remove(DeckUnit);
                    UI_MyDeck.Init();

                    GameManager.Sound.Play("UI/UISFX/UIButtonSFX");
                });
            }
        }, CurrentEvent.Hall_Delete);
    }

    public void SanctumButton()
    {
        GameManager.Sound.Play("UI/UISFX/UIButtonSFX");

        SceneChanger.SceneChange("ProgressShopScene");
    }

    public void OptionButton()
    {
        GameManager.Sound.Play("UI/UISFX/UIButtonSFX");

        GameManager.UI.ShowPopup<UI_Option>();
    }

    public void ResetAlertYesButton()
    {
        // 게임오브젝트를 생성해서 보내주기 & 생성한 오브젝트가 맵 선택 씬에 도달했을 때 활성화되서 튜토 이미지 띄우고 자신 삭제하기
        GameManager.Sound.Play("UI/UISFX/UIImportantButtonSFX");

        GameManager.Data.DeckClear();
        GameManager.Data.SetDeck(GameManager.OutGameData.SetHallDeck());

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
        GameManager.Data.Init();
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
        GameManager.Sound.Play("UI/UISFX/UIButtonSFX");
        Application.Quit();
    }

    public void ButtonHover()
    {
        GameManager.Sound.Play("UI/UISFX/UIHoverSFX");
    }
}