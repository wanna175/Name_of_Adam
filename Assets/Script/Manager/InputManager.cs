using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public bool Click { get { return OneClick(); } }

    private long _lastClickTime = 0;
    public bool OneClick()
    {
        if (Input.GetMouseButtonDown(0) && DateTime.Now.Ticks - _lastClickTime > 2000000)
        {
            _lastClickTime = DateTime.Now.Ticks;
            return true;
        }

        return false;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameManager.Sound.Play("UI/ClickSFX/UI Click1");
            Debug.Log("마우스 좌클릭");
        }
    }
}
