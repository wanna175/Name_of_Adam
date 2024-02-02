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

    public void SetImage(string result,RewardController rc = null)
    {
        fc.GetComponent<FadeController>().StartFadeIn();

        GetComponent<Canvas>().sortingOrder = 100;
        _result = result;

        if (result == "win") 
        {
            rc.RewardSetting(GameManager.Data.GetDeck(), _rewardScene);
            _rewardScene.gameObject.SetActive(true);
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
        if (_result == "win")
        {
            SceneChanger.SceneChange("StageSelectScene");
        }
        else if (_result == "elite win")
        {
            if(GameManager.Data.Map.GetCurrentStage().StageLevel == 20)
            {
                GameManager.Data.GameData.Progress.LeftDarkEssence = GameManager.Data.DarkEssense;
                BattleOverDestroy();
                GameObject.Find("@UI_Root").transform.Find("UI_ProgressSummary").gameObject.SetActive(true);
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

            if (GameManager.OutGameData.isTutorialClear())
            {
                GameObject.Find("@UI_Root").transform.Find("UI_ProgressSummary").gameObject.SetActive(true);
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