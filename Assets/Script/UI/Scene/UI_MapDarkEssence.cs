using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_MapDarkEssence : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;

    private void Update()
    {
        text.text = GameManager.Data.GameData.DarkEssence.ToString();
    }
}
