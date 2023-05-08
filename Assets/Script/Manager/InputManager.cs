using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameManager.Sound.Play("UI/ClickSFX/UI Click1");
            Debug.Log("마우스 좌클릭");
        }
    }
}
