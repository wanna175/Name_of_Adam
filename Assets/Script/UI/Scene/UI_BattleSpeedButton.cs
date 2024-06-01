using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_BattleSpeedButton : UI_Scene
{
    [SerializeField] private TextMeshProUGUI _speedText;
    private float _currentBattleSpeed = 1f;

    private void TimeScaleChange()
    {
        _speedText.text = "¡¿" + _currentBattleSpeed.ToString();
        Time.timeScale = _currentBattleSpeed;
    }

    public void SetBattleSpeed(float speed)
    { 
        _currentBattleSpeed = speed;
        TimeScaleChange();
    }

    public void OnButtonClick()
    {
        GameManager.Sound.Play("UI/ButtonSFX/UIButtonClickSFX");

        if (_currentBattleSpeed == 2f)
        {
            _currentBattleSpeed = 1f;
        }
        else
        {
            _currentBattleSpeed += 0.5f;
        }

        GameManager.OutGameData.SetBattleSpeed(_currentBattleSpeed);
        TimeScaleChange();
    }
}