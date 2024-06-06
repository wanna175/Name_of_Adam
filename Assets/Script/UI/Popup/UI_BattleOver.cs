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
            _rewardScene.Init(GameManager.Data.GetSortedDeck(SortMode.Default));
            _textImage.gameObject.SetActive(false);
            //_textImage.sprite = GameManager.Resource.Load<Sprite>($"Arts/UI/Battle_UI/Text/WinText");
            GameManager.Sound.Clear();
            GameManager.Sound.Play("Win/WinBGM", Sounds.BGM);
        }
        else if (result == "elite win")
        {
            _textImage.sprite = GameManager.Resource.Load<Sprite>($"Arts/UI/Battle_UI/Text/EliteWinText");
            GameManager.Sound.Clear();
            GameManager.Sound.Play("Win/WinBGM", Sounds.BGM);
        }
        else if (result == "lose")
        {
            _textImage.sprite = GameManager.Resource.Load<Sprite>($"Arts/UI/Battle_UI/Text/LoseText");
            GameManager.Sound.Clear();
            GameManager.Sound.Play("Lose/LoseBGM", Sounds.BGM);
        }
    }

    public void OnClick()
    {
        GameManager.Sound.Play("UI/ButtonSFX/UIButtonClickSFX");

        if (_result == "win")
        {
            if (!GameManager.OutGameData.IsTutorialClear() && GameManager.Data.StageAct == 0 && GameManager.Data.Map.CurrentTileID == 3)
            {
                GameManager.OutGameData.DoneTutorial(true);
                if (GameManager.OutGameData.GetCutSceneData(CutSceneType.Tutorial) == false)
                    SceneChanger.SceneChangeToCutScene(CutSceneType.Tutorial);
                else
                    SceneChanger.SceneChange("StageSelectScene");
                Debug.Log("Tutorial Clear!");
                return;
            }

            if (_rewardScene.IsEndCreate)
            {
                SceneChanger.SceneChange("StageSelectScene");
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
                GameObject.Find("@UI_Root").transform.Find("UI_ProgressSummary").gameObject.SetActive(true);
                GameObject.Find("Result List").GetComponent<UI_ProgressSummary>().Title.text = "Victory";
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

            if (GameManager.OutGameData.IsTutorialClear())
            {
                GameObject.Find("@UI_Root").transform.Find("UI_ProgressSummary").gameObject.SetActive(true);
                GameObject.Find("Result List").GetComponent<UI_ProgressSummary>().Title.text = "Defeat";
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