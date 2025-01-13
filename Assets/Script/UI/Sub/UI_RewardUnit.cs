using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum UnitState
{
    New = 1,
    Dead = 2,
    Default = 3
};

public class UI_RewardUnit : MonoBehaviour
{
    [SerializeField] private Image _unitImage;
    [SerializeField] private TMP_Text _unitName;
    [SerializeField] private TMP_Text _faithDifference;
    [SerializeField] private Image _newImage;
    [SerializeField] private Image _deadImage;

    [SerializeField] private Transform _unitInfoFallGrid;
    [SerializeField] private GameObject _fallGaugePrefab;

    [SerializeField] private FadeController _fadeController;

    public void Init(RewardUnit rewardUnit, int maxFaith, int curFaith, UnitState unitState)
    {
        _newImage.gameObject.SetActive(unitState == UnitState.New);
        _deadImage.gameObject.SetActive(unitState == UnitState.Dead);

        _unitImage.sprite = rewardUnit.Image;
        _unitName.text = rewardUnit.Name;
        _faithDifference.text = (rewardUnit.PreFall - curFaith).ToString();

        for (int i = 0; i < maxFaith - rewardUnit.PreFall; i++)
        {
            UI_FallUnit faithObject = GameObject.Instantiate(_fallGaugePrefab, _unitInfoFallGrid).GetComponent<UI_FallUnit>();

            faithObject.InitFall(Team.Player, 0);
            if (i >= maxFaith - curFaith)
                faithObject.DecreaseGauge(0.0f);
        }
    }

    public void FadeIn()
    {
        _fadeController.StartFadeIn();
    }

    public void EndFadeIn()
    {
        _fadeController.EndFade();
    }
}
