using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

public class UI_BattleOver : UI_Scene
{
    [SerializeField] private Image _textImage;
    [SerializeField] private FadeController fc;
    [SerializeField] private UI_RewardScene _rewardScene;

    private string _result;

    public void SetImage(string result)
    {
        fc.GetComponent<FadeController>().StartFadeIn();

        GetComponent<Canvas>().sortingOrder = 100;
        _result = result;

        if (result == "win") 
        {
            _rewardScene.gameObject.SetActive(true);
            _rewardScene.Init(GameManager.Data.GetSortedDeck(SortMode.Hall));
            _textImage.gameObject.SetActive(false);

            GameManager.Sound.Clear();
            GameManager.Sound.Play("Result/WinBGM", Sounds.BGM);
        }
        else if (result == "elite win")
        {
            _textImage.sprite = GameManager.Resource.Load<Sprite>($"Arts/UI/Battle_UI/Text/EliteWinText");
            GameManager.Sound.Clear();
            GameManager.Sound.Play("Result/WinBGM", Sounds.BGM);
        }
        else if (result == "lose")
        {
            _textImage.sprite = GameManager.Resource.Load<Sprite>($"Arts/UI/Battle_UI/Text/LoseText");
            GameManager.Sound.Clear();
            GameManager.Sound.Play("Result/LoseBGM", Sounds.BGM);
        }
    }

    public void OnClick()
    {
        GameManager.Sound.Play("UI/UISFX/UIButtonSFX");

        if (_result == "win")
        {
            if (!GameManager.OutGameData.Data.TutorialClear && GameManager.Data.StageAct == 0 && GameManager.Data.Map.CurrentTileID == 3)
            {
                Debug.Log("Tutorial Clear!");

                GameManager.OutGameData.Data.TutorialClear = true;

                if (GameManager.OutGameData.CutScenePlayCheck(CutSceneType.Tubalcain_Enter)
                    && GameManager.Data.GameData.CurrentAct == 1)
                {
                    GameManager.OutGameData.SetCutSceneData(CutSceneType.Tubalcain_Enter, true);
                    SceneChanger.SceneChangeToCutScene(CutSceneType.Tubalcain_Enter);
                }
                else
                {
                    SceneChanger.SceneChange("StageSelectScene");
                }

                GameManager.OutGameData.SaveData();

                return;
            }

            if (_rewardScene.IsEndCreate)
            {
                GameObject.Find("RatterBoxCanvas").transform.Find("FadeOut").gameObject.SetActive(true);

                GameManager.Instance.PlayAfterCoroutine(() => {
                    SceneChanger.SceneChange("StageSelectScene");
                }, 1f);

            }
            else
            {
                _rewardScene.EndFadeIn();
            }
        }
        else if (_result == "elite win")
        {
            if(GameManager.Data.Map.GetCurrentStage().Name == StageName.BossBattle)
            {
                BattleOverDestroy();
                GameManager.UI.ShowPopup<UI_ProgressSummary>().Init("VICTORY");
            }
            else
            {
                BattleOverDestroy();
                GameManager.UI.ShowPopup<UI_EliteReward>("UI_EliteReward").SetRewardPanel();
            }
        }
        else if (_result == "lose")
        {
            BattleOverDestroy();

            if (GameManager.OutGameData.Data.TutorialClear)
            {
                GameManager.UI.ShowPopup<UI_ProgressSummary>().Init("DEFEAT");
            }
            else
            {
                SceneChanger.SceneChange("MainScene");
            }

        }
    }

    public void BattleOverDestroy()
    {
        GameManager.Resource.Destroy(this.gameObject);
    }
}