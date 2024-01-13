using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlphaClicker : MonoBehaviour
{
    private const float DISABLE_THRESHOLD = 0f;
    private const float ENABLE_THRESHOLD = 0.1f;

    [SerializeField]
    private bool isEnable;

    // Start is called before the first frame update
    void Start()
    {
        SetEnable(isEnable);
    }

    public void SetEnable(bool isEnable)
    {
        GetComponent<Image>().alphaHitTestMinimumThreshold = (isEnable) ? ENABLE_THRESHOLD : DISABLE_THRESHOLD;
        this.isEnable = isEnable;
    }
}
