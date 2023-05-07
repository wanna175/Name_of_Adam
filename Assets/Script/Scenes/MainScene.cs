using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScene : MonoBehaviour
{
    private void Awake()
    {
        GameManager.Sound.Play("BattleBGMB", Sounds.BGM);
    }
}
