using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class UI_EliteCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject _highlight;

    [SerializeField] private List<GameObject> _stigmaFrames;

    private List<Image> _stigmataImages;

    [SerializeField] private TMP_Text _nameText;

    public Image UnitImage;
    private DeckUnit _deckUnit;

    public void Init(DeckUnit deckUnit)
    {
        _deckUnit = deckUnit;

        _stigmataImages = new List<Image>();
        foreach (var frame in _stigmaFrames)
            _stigmataImages.Add(frame.GetComponentsInChildren<Image>()[1]);

        foreach (var frame in _stigmaFrames)
            frame.SetActive(true);

        UnitImage.sprite = _deckUnit.Data.CorruptImage;
        UnitImage.color = Color.white;
        _nameText.SetText(_deckUnit.Data.Name);

        List<Stigma> stigmas = _deckUnit.GetStigma();
        for (int i = 0; i < _stigmataImages.Count; i++)
        {
            if (i < stigmas.Count)
            {
                _stigmaFrames[i].GetComponent<UI_StigmaHover>().SetStigma(stigmas[i]);
                _stigmataImages[i].sprite = stigmas[i].Sprite_28;
                _stigmataImages[i].color = Color.white;
            }
            else
            {
                _stigmaFrames[i].GetComponent<UI_StigmaHover>().SetEnable(false);
                _stigmataImages[i].color = new Color(1f, 1f, 1f, 0f);
            }
        }

        _highlight.SetActive(false);
    }

    public void OnClick()
    {
        GameManager.Sound.Play("UI/UISFX/UISelectSFX");

        GameManager.UI.ShowPopup<UI_SystemSelect>().Init("CorfirmEliteReward", () =>
        {
            GameManager.Sound.Play("UI/UISFX/UIButtonSFX");

            GameManager.Data.GameData.DeckUnits.Add(_deckUnit);
            GameManager.Data.GameData.FallenUnits.Add(_deckUnit);
            GameManager.OutGameData.SaveData();
            GameManager.SaveManager.SaveGame();

            bool isGoToCutScene = CheckAndGoToCutScene();
            if (!isGoToCutScene)
                SceneChanger.SceneChange("StageSelectScene");
        });


    }

    public void OnInfoButton()
    {
        GameManager.Sound.Play("UI/UISFX/UIButtonSFX");
        UI_UnitInfo ui = GameManager.UI.ShowPopup<UI_UnitInfo>("UI_UnitInfo");

        ui.SetUnit(_deckUnit);
        ui.Init();
    }

    public void OnPointerEnter(PointerEventData eventData)
        => _highlight.SetActive(true);

    public void OnPointerExit(PointerEventData eventData)
        => _highlight.SetActive(false);

    private bool CheckAndGoToCutScene()
    {
        bool isGoToCutScene = false;
        StageData currentStageData = GameManager.Data.Map.GetCurrentStage();
        Debug.Log($"현재 스테이지 정보: {currentStageData.Name}/{currentStageData.StageLevel}/{currentStageData.StageID}");

        if (currentStageData.StageLevel % 100 > 90)
            return false;

        if (currentStageData.Name == StageName.EliteBattle ||
        currentStageData.Name == StageName.BossBattle)
        {
            string unitName = GameManager.Data.StageDatas[currentStageData.StageLevel][currentStageData.StageID].Units[0].Name;
            switch (unitName)
            {
                case "투발카인": // 투발카인 -> 라헬레아 넘어가기
                    if (GameManager.OutGameData.CutScenePlayCheck(CutSceneType.RahelLea_Enter))
                    {
                        isGoToCutScene = true;
                        GameManager.OutGameData.SetCutSceneData(CutSceneType.RahelLea_Enter, true);
                        SceneChanger.SceneChangeToCutScene(CutSceneType.RahelLea_Enter);
                    }
                    break;
                case "라헬&레아": // 라헬레아 -> 바누엘 넘어가기
                    if (GameManager.OutGameData.CutScenePlayCheck(CutSceneType.Phanuel_Enter))
                    {
                        isGoToCutScene = true;
                        GameManager.OutGameData.SetCutSceneData(CutSceneType.Phanuel_Enter, true);
                        SceneChanger.SceneChangeToCutScene(CutSceneType.Phanuel_Enter);
                    }
                    break;
                case "엘리우스": // 엘리우스 -> 압바임 넘어가기
                    if (GameManager.OutGameData.CutScenePlayCheck(CutSceneType.Appaim_Enter))
                    {
                        isGoToCutScene = true;
                        GameManager.OutGameData.SetCutSceneData(CutSceneType.Appaim_Enter, true);
                        SceneChanger.SceneChangeToCutScene(CutSceneType.Appaim_Enter);
                    }
                    break;
                case "압바임": // 압바임 -> 구원자 넘어가기
                    if (GameManager.OutGameData.CutScenePlayCheck(CutSceneType.TheSavior_Enter))
                    {
                        isGoToCutScene = true;
                        GameManager.OutGameData.SetCutSceneData(CutSceneType.TheSavior_Enter, true);
                        SceneChanger.SceneChangeToCutScene(CutSceneType.TheSavior_Enter);
                    }
                    break;
                case "리비엘": // 리비엘 -> 아라벨라 넘어가기
                    if (GameManager.OutGameData.CutScenePlayCheck(CutSceneType.Arabella_Enter))
                    {
                        isGoToCutScene = true;
                        GameManager.OutGameData.SetCutSceneData(CutSceneType.Arabella_Enter, true);
                        SceneChanger.SceneChangeToCutScene(CutSceneType.Arabella_Enter);
                    }
                    break;
                case "아라벨라": // 아라벨라 -> 욘 넘어가기
                    if (GameManager.OutGameData.CutScenePlayCheck(CutSceneType.Yohrn_Enter))
                    {
                        isGoToCutScene = true;
                        GameManager.OutGameData.SetCutSceneData(CutSceneType.Yohrn_Enter, true);
                        SceneChanger.SceneChangeToCutScene(CutSceneType.Yohrn_Enter);
                    }
                    break;
            }
        }

        return isGoToCutScene;
    }
}
