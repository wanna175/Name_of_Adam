using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainSceneController : MonoBehaviour
{
    [SerializeField] GameObject Canvas;
    [SerializeField] GameObject ContinueBox;
    [SerializeField] GameObject UI_ResetAlert;
    [SerializeField] GameObject SystemInfo;

    private TMP_Text systemInfoText;
    private Image systemInfoFrame;

    private void Start()
    {
        if (GameManager.SaveManager.SaveFileCheck())
            ContinueBox.SetActive(true);
        else
            ContinueBox.SetActive(false);

        systemInfoText = SystemInfo.GetComponentInChildren<TMP_Text>();
        systemInfoFrame = SystemInfo.GetComponentInChildren<Image>();
        SystemInfo.SetActive(false);

        if (GameManager.OutGameData.IsPhanuelClear() && GameManager.OutGameData.GetIsOnMainTooltipForPhanuel() == false)
        {
            GameManager.OutGameData.SetIsOnMainTooltipForPhanuel(true);
            GameManager.OutGameData.SaveData();
            SetSystemInfo("축하합니다, 어둠의 선지자여. \r\n\r\n그러나 당신의 여정은 아직 끝나지 않았습니다. \r\n<color=yellow>새로운 빛<color=white>이 지평선 너머에서 당신을 기다리고 있습니다.");
        }

        if (GameManager.OutGameData.IsHorusClear() && GameManager.OutGameData.GetIsOnMainTooltipForHorus() == false)
        {
            GameManager.OutGameData.SetIsOnMainTooltipForHorus(true);
            GameManager.OutGameData.SaveData();
            SetSystemInfo("당신은 모든 것을 이겨냈지만, 진정한 시험은 이제부터입니다. \r\n\r\n매 판 새로운 전투를 마주하며 당신의 한계를 시험해보세요.");
        }
    }

    public void StartButton()
    {
        if (GameManager.SaveManager.SaveFileCheck())
        {
            GameManager.Sound.Play("UI/ButtonSFX/UIButtonClickSFX");
            UI_ResetAlert.SetActive(true);
        }
        else
        {
            GameManager.Data.DeckClear();
            GameManager.Data.GameData.FallenUnits.AddRange(GameManager.Data.GameDataMain.DeckUnits);
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
                GameManager.Data.HallDeckSet();
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
        GameManager.Data.GameData.FallenUnits.AddRange(GameManager.Data.GameDataMain.DeckUnits);
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

    public void ResetAlertNoButton()
    {
        UI_ResetAlert.SetActive(false);
    }


    public void ExitButton()
    {
        GameManager.Sound.Play("UI/ButtonSFX/UIButtonClickSFX");
        Application.Quit();
    }

    public void SetSystemInfo(string info, float fadeTime = 2.0f, float idleTime = 4.0f)
    {
        SystemInfo.SetActive(true);
        systemInfoText.text = info;
        //StartCoroutine(FadeSystemInfo(fadeTime, idleTime));
    }

    public void OnSystemInfoClose()
    {
        GameManager.Sound.Play("UI/ButtonSFX/UIButtonClickSFX");
        SystemInfo.SetActive(false);
    }

    IEnumerator FadeSystemInfo(float fadeTime, float idleTime)
    {
        float alpha = 0.0f;
        SetSystemInfoAlpha(alpha);

        while (alpha < 1f)
        {
            alpha += Time.deltaTime / fadeTime;
            SetSystemInfoAlpha(alpha);
            yield return null;
        }

        alpha = 1.0f;
        SetSystemInfoAlpha(alpha);

        yield return new WaitForSeconds(idleTime);

        while (alpha > 0f)
        {
            alpha -= Time.deltaTime / fadeTime;
            SetSystemInfoAlpha(alpha);
            yield return null;
        }

        alpha = 0.0f;
        SetSystemInfoAlpha(alpha);
        SystemInfo.SetActive(false);
    }

    private void SetSystemInfoAlpha(float alpha)
    {
        systemInfoFrame.color = new Color(1f, 1f, 1f, alpha);
        systemInfoText.color = new Color(1f, 1f, 1f, alpha);
    }
}