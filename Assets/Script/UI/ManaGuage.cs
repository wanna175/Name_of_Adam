using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaGuage : MonoBehaviour
{
    [SerializeField] Image ManaGauge;
    [SerializeField] Text ManaText;

    private void Start()
    {
        GameManager.Instance.DataMNG.InitMana();
        GameManager.Instance.DataMNG.AddMana(4);
    }

    private void Update()
    {
        float MaxManaCost = (float)GameManager.Instance.DataMNG.MaxManaCost;
        float ManaCost = (float)GameManager.Instance.DataMNG.ManaCost;

        ManaGauge.fillAmount = ManaCost / MaxManaCost;
        ManaText.text = ManaCost.ToString();
    }

    public bool UseMana(int value)
    {
        if (ManaCost >= value)
        {
            ManaCost -= value;
            ManaGauge.fillAmount = (float)ManaCost / (float)MaxManaCost;
            ManaText.text = ManaCost.ToString();
        
            return true;
        }
        else
        {
            return false;
        }
    }
}
