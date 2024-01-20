using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_UnitReward : MonoBehaviour
{
    #region 변수
    [SerializeField] private Image unitImage;
    [SerializeField] private TMP_Text unitName;
    [SerializeField] private TMP_Text diff;

    private FadeController fc;
    #endregion

    #region 함수
    public void Init(Sprite _unitImg, string _unitName, int _diff)
    {
        unitImage.sprite = _unitImg;
        unitName.text = _unitName;
        diff.text = _diff.ToString();
        fc = this.GetComponent<FadeController>();
    }
    public bool FadeIn(float time,int count)
    {
        time -= (float)0.5 * time;
        fc.StartFadeIn(time);
        return (count - 1 == time);
    }
    public void EndFadeIn()
    {
        fc.EndFade();
    }

    #endregion
}
