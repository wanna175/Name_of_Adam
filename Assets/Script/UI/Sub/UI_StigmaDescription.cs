using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_StigmaDescription : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _name;
    [SerializeField] TextMeshProUGUI _description;
    [SerializeField] Image _image;

    public void SetStigma(BattleUnit unit)
    {
        List<Stigma> st = unit.StigmaList;

        for (int i = 0; i < 3; i++)
        {
            if(st[i] != null)
            {
                _name.text = st[i].Name;
                _description.text = st[i].Description;
                _image.sprite = st[i].Sprite;
            }
        }
    }

    public void SetStigma(DeckUnit unit)
    {
        List<Stigma> st = unit.Data.UniqueStigma;

        for (int i = 0; i < 3; i++)
        {
            if (st[i] != null)
            {
                _name.text = st[i].Name;
                _description.text = st[i].Description;
                _image.sprite = st[i].Sprite;
            }
        }
    }


}
