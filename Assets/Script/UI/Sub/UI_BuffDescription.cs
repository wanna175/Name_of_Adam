using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_BuffDescription : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _description;

    public void SetBuff(Buff buff)
    {
        string text = $"<style={buff.Sprite.name}></style>{buff.GetDescription(2)}";
        _description.SetText(text);
    }
}
