using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSceneController : MonoBehaviour
{
    [SerializeField] GameObject Tutorial;
    public void Start()
    {
        if(GameManager.Instance.Tutorial_Trigger == false)
        {
            Tutorial.gameObject.SetActive(false);
        }
        else
        {
            GameManager.Instance.Tutorial_Trigger = false;
        }
    }

}
