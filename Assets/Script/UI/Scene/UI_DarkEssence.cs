using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_DarkEssence : UI_Scene
{
    [SerializeField] private TextMeshProUGUI _darkEssence;

    private void Awake()
    {
        Refresh();
    }

    public void Refresh() 
    {
        _darkEssence.text = GameManager.Data.GameData.DarkEssence.ToString();
    }
}
