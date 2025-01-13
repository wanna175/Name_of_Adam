using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    [SerializeField] private float _fadeTime = 1f;

    private void Awake()
    {
        GetComponent<Animator>().speed = 1f / _fadeTime;
    }

    public void EndFade()
    {
        this.gameObject.SetActive(false);
    }
}
