using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

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
    }

    private void Init()
    {
        UI_Difficulty.SetActive(false);//추후 순서대로 수정
        UI_IncarnaSelect.SetActive(true);
        UI_HallSelect.SetActive(false);
        UI_ConfirmBtn.SetActive(false);
        DifficultySelected = false;

        LockIncarna(61, 1);
        LockIncarna(71, 2);
    }

    public void Confirm()
    {
        GameManager.Sound.Play("UI/ButtonSFX/UIButtonClickSFX");

        if (UI_IncarnaSelect.activeSelf == true)
        {
            if (GameManager.OutGameData.IsUnlockedItem(18))
            {
                GameManager.Data.GameDataMain.DarkEssence = 10;
            }
            else if (GameManager.OutGameData.IsUnlockedItem(6))
            {
                GameManager.Data.GameDataMain.DarkEssence = 7;
            }

            GameManager.Data.GameDataMain.Incarna = incarnaData;
            UI_IncarnaSelect.SetActive(false);
            UI_HallSelect.SetActive(true);
        }
        else if (UI_HallSelect.activeSelf == true)
        {
            GameManager.Data.MainDeckSet();
            GameManager.Data.GameData.FallenUnits.Clear();
            GameManager.Data.GameData.FallenUnits.AddRange(GameManager.Data.GameDataMain.DeckUnits);

            SceneChanger.SceneChange("StageSelectScene");
        }
    }

    public void OnIncarnaClick(int i)
    {
        if(i == 1 && !GameManager.OutGameData.IsUnlockedItem(61))
        {
            return;
        }

        if(i == 2 && !GameManager.OutGameData.IsUnlockedItem(71))
        {
            return;
        }

        UI_Incarna_List.SetActive(false);
        UI_ConfirmBtn.SetActive(true);
        Incarna_Info[i].SetActive(true);
        incarnaData = GameManager.Resource.Load<Incarna>($"ScriptableObject/Incarna/{Incarna_Info[i].name}");
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
