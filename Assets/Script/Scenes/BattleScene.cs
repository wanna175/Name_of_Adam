using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleScene : MonoBehaviour
{
    private void Awake()
    {
        GameManager.Sound.Play("BattleBGMA", Sounds.BGM);
    }

}
