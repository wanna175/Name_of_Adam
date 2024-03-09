using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class DifficultySelectSceneController : MonoBehaviour
{
    [SerializeField] GameObject UI_Difficulty;
    [SerializeField] GameObject UI_IncarnaSelect;
    [SerializeField] GameObject UI_HallSelect;
    [SerializeField] GameObject UI_ConfirmBtn;

    public GameObject UI_Incarna_List;
    public List<GameObject> Incarna_Card;
    public List<GameObject> Incarna_Info;

    private Incarna incarnaData;
    private bool DifficultySelected;
    private Color cardColor;

    void Start()
    {
        Init();
        GameManager.Sound.Play("UI/ClickSFX/UIClick2");
        GameManager.Sound.SceneBGMPlay("DifficultySelectScene");
    }

    private void Init()
    {
        UI_Difficulty.SetActive(false);//���� ������� ����
        UI_IncarnaSelect.SetActive(true);
        UI_HallSelect.SetActive(false);
        UI_ConfirmBtn.SetActive(false);
        DifficultySelected = false;

        LockIncarna(61, 1);
        LockIncarna(71, 2);
    }

    public void Confirm()
    {
        if (UI_IncarnaSelect.activeSelf == true)
        {
            GameManager.Sound.Play("UI/ButtonSFX/UIButtonClickSFX");

            if (GameManager.OutGameData.IsUnlockedItem(6))
            {
                GameManager.Data.GameDataMain.DarkEssence = 10;
            }
            else if (GameManager.OutGameData.IsUnlockedItem(3))
            {
                GameManager.Data.GameDataMain.DarkEssence = 7;
            }

            GameManager.Data.GameDataMain.Incarna = incarnaData;
            UI_IncarnaSelect.SetActive(false);
            UI_HallSelect.SetActive(true);
        }
        else if (UI_HallSelect.activeSelf == true)
        {
            GameManager.Sound.Play("UI/ClickSFX/UIClick2");

            GameManager.Data.MainDeckSet();
            GameManager.Data.GameData.FallenUnits.Clear();
            GameManager.Data.GameData.FallenUnits.AddRange(GameManager.Data.GameDataMain.DeckUnits);

            if (GameManager.OutGameData.GetCutSceneData(CutSceneType.Elieus_Enter) == false)
            {
                if (GameManager.OutGameData.IsPhanuelClear())
                {
                    SceneChanger.SceneChangeToCutScene(CutSceneType.Elieus_Enter);
                    return;
                }
            }

            SceneChanger.SceneChange("StageSelectScene");
        }
    }

    public void OnIncarnaClick(int i)
    {
        if (i == 1 && !GameManager.OutGameData.IsUnlockedItem(61))
        {
            GameManager.Sound.Play("UI/ClickSFX/ClickFailSFX");
            return;
        }

        if(i == 2 && !GameManager.OutGameData.IsUnlockedItem(71))
        {
            GameManager.Sound.Play("UI/ClickSFX/ClickFailSFX");
            return;
        }
        GameManager.Sound.Play("UI/ClickSFX/UIClick2");
        UI_Incarna_List.SetActive(false);
        UI_ConfirmBtn.SetActive(true);
        Incarna_Info[i].SetActive(true);
        incarnaData = GameManager.Resource.Load<Incarna>($"ScriptableObject/Incarna/{Incarna_Info[i].name}");

        List<TMP_Text> texts = new List<TMP_Text>();
        for (int j = 0; j < Incarna_Info[i].transform.childCount; j++)
        {
            TMP_Text text = Incarna_Info[i].transform.GetChild(j).GetComponent<TMP_Text>();
            if (text != null)
                texts.Add(text);
        }

        for (int j = 2; j < texts.Count; j++)
        {
            texts[j].SetText(GameManager.Locale.GetLocalizedPlayerSkillInfo(3 * i + j - 1));
        }
    }

    public void LockIncarna(int progressID, int incarnaID)
    {
        if (!GameManager.OutGameData.IsUnlockedItem(progressID))
        {
            Incarna_Card[incarnaID].transform.Find("Blocker").gameObject.SetActive(true);
        }
    }

    public void Quit()
    {
        GameManager.Sound.Play("UI/ButtonSFX/BackButtonClickSFX");

        if (UI_IncarnaSelect.activeSelf == true)
        {
            if (UI_ConfirmBtn.activeSelf == false)
            {
                SceneChanger.SceneChange("MainScene");
            }
            else
            {
                Incarna_Info.ForEach(obj => { obj.SetActive(false); });
                UI_ConfirmBtn.SetActive(false);
                UI_Incarna_List.SetActive(true);
            }
        }
        else if (UI_HallSelect.activeSelf == true)
        {
            UI_HallSelect.SetActive(false);
            UI_IncarnaSelect.SetActive(true);
            Incarna_Info.ForEach(obj => { obj.SetActive(false); });
            UI_ConfirmBtn.SetActive(false);
            UI_Incarna_List.SetActive(true);
        }
    }
}
