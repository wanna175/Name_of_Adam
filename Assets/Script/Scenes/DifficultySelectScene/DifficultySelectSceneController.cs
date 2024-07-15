using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DifficultySelectSceneController : MonoBehaviour
{
    [SerializeField] private GameObject _difficultyUI;
    [SerializeField] private GameObject _incarnaSelectUI;
    [SerializeField] private GameObject _hallSelectUI;
    [SerializeField] private GameObject _confirmButtonUI;
    [SerializeField] private GameObject _incarnaListUI;

    [SerializeField] private List<GameObject> _incarnaBlocker;
    [SerializeField] private List<GameObject> _incarnaInfo;
    [SerializeField] private UI_HallCard[] _hallCards;

    private Incarna _incarnaData;

    void Start()
    {
        Init();

        GameManager.Sound.Play("UI/ClickSFX/UIClick2");
        GameManager.Sound.SceneBGMPlay("DifficultySelectScene");
    }

    private void Init()
    {
        _difficultyUI.SetActive(false);//추후 순서대로 수정
        _incarnaSelectUI.SetActive(true);
        _hallSelectUI.SetActive(false);
        _confirmButtonUI.SetActive(false);

        _incarnaBlocker[0].SetActive(!GameManager.OutGameData.IsUnlockedItem(61));
        _incarnaBlocker[1].SetActive(!GameManager.OutGameData.IsUnlockedItem(71));

        foreach (UI_HallCard card in _hallCards)
        {
            card.Init();
        }
    }

    public void Confirm()
    {
        if (_incarnaSelectUI.activeSelf == true)
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

            GameManager.Data.GameDataMain.Incarna = _incarnaData;
            _incarnaSelectUI.SetActive(false);
            _hallSelectUI.SetActive(true);
        }
        else if (_hallSelectUI.activeSelf == true)
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

    public void OnIncarnaClick(int incarnaIndex)
    {
        if ((incarnaIndex == 1 && !GameManager.OutGameData.IsUnlockedItem(71)) || 
            (incarnaIndex == 2 && !GameManager.OutGameData.IsUnlockedItem(61)))
        {
            GameManager.Sound.Play("UI/ClickSFX/ClickFailSFX");
            return;
        }

        GameManager.Sound.Play("UI/ClickSFX/UIClick2");
        _incarnaListUI.SetActive(false);
        _confirmButtonUI.SetActive(true);
        _incarnaInfo[incarnaIndex].SetActive(true);
        _incarnaData = GameManager.Resource.Load<Incarna>($"ScriptableObject/Incarna/{_incarnaInfo[incarnaIndex].name}");

        List<TMP_Text> texts = new();
        for (int i = 0; i < _incarnaInfo[incarnaIndex].transform.childCount; i++)
        {
            TMP_Text text = _incarnaInfo[incarnaIndex].transform.GetChild(i).GetComponent<TMP_Text>();
            if (text != null)
                texts.Add(text);
        }

        for (int i = 2; i < texts.Count; i++)
        {
            texts[i].SetText(GameManager.Locale.GetLocalizedPlayerSkillInfo(3 * incarnaIndex + i - 1));
        }
    }

    public void Quit()
    {
        GameManager.Sound.Play("UI/ButtonSFX/BackButtonClickSFX");

        if (_incarnaSelectUI.activeSelf == true)
        {
            if (_confirmButtonUI.activeSelf == false)
            {
                SceneChanger.SceneChange("MainScene");
            }
            else
            {
                _incarnaInfo.ForEach(obj => { obj.SetActive(false); });
                _confirmButtonUI.SetActive(false);
                _incarnaListUI.SetActive(true);
            }
        }
        else if (_hallSelectUI.activeSelf == true)
        {
            _hallSelectUI.SetActive(false);
            _incarnaSelectUI.SetActive(true);
            _incarnaInfo.ForEach(obj => { obj.SetActive(false); });
            _confirmButtonUI.SetActive(false);
            _incarnaListUI.SetActive(true);
        }
    }
}
