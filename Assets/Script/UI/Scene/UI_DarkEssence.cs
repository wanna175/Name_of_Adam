using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_DarkEssence : UI_Scene
{
    [SerializeField] private TextMeshProUGUI _darkEssence;
    [SerializeField] private Image cannotLight;
    [SerializeField] private UI_CannotEffect cannotEffect;
    public UI_CannotEffect CannotEffect => cannotEffect;

    public void Init()
    {
        cannotEffect.Init(Vector3.one * 0.5f, Vector3.one * 0.6f, 1.5f);

        Refresh();
    }

    public void Refresh() 
    {
        _darkEssence.text = GameManager.Data.GameData.DarkEssence.ToString();
    }
}
