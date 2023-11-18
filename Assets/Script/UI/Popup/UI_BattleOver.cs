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

    private string _result;

    public void SetImage(string result)
    {
        fc.GetComponent<FadeController>().StartFadeIn();

        GetComponent<Canvas>().sortingOrder = 100;
        _result = result;

        if (result == "win") 
        {
            _textImage.sprite = GameManager.Resource.Load<Sprite>($"Arts/UI/Battle_UI/Text/WinText");
            GameManager.Sound.Clear();
            GameManager.Sound.Play("WinLose/WinLoseBGM", Sounds.BGM);
        }
        else if (result == "elite win")
        {
            _textImage.sprite = GameManager.Resource.Load<Sprite>($"Arts/UI/Battle_UI/Text/EliteWinText");
            GameManager.Sound.Clear();
            GameManager.Sound.Play("WinLose/WinLoseBGM", Sounds.BGM);
        }
        else if (result == "lose")
        {
            _textImage.sprite = GameManager.Resource.Load<Sprite>($"Arts/UI/Battle_UI/Text/LoseText");
            GameManager.Sound.Clear();
            GameManager.Sound.Play("WinLose/WinLoseBGM", Sounds.BGM);
        }
    }

    public void OnClick()
    {
        if (_result == "win")
            SceneChanger.SceneChange("StageSelectScene");
        else if (_result == "elite win")
        {
            BattleOverDestroy();

            GameManager.UI.ShowPopup<UI_MyDeck>("UI_MyDeck").HallSaveInit(true, (deckUnit) => {GameManager.OutGameData.AddHallUnit(deckUnit, true);});
        }
        else if (_result == "lose")
        {
            BattleOverDestroy();

            if (GameManager.OutGameData.isTutorialClear())
            {
                GameManager.UI.ShowPopup<UI_MyDeck>("UI_MyDeck").HallSaveInit(false, (deckUnit) => { GameManager.OutGameData.AddHallUnit(deckUnit, false);});
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