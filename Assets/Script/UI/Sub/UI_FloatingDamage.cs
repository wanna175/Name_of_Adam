using UnityEngine;
using TMPro;
using System.Collections;

public class UI_FloatingDamage : UI_Base
{
    [SerializeField] private TextMeshProUGUI _damageNumber;

    readonly private float _sizeUpTime = 0.3f;
    readonly private float _displayTime = 0.3f;
    readonly private float _fadeOutTime = 0.3f;
    //1.2
    readonly private int _startSize = 35;
    readonly private int _endSize = 40;

    private float _accumTime;

    private void Awake()
    {
        _damageNumber.alpha = 0f;
    }

    public void Init(int damage, bool direction)
    {
        if (direction)
        {
            _damageNumber.alignment = TextAlignmentOptions.BottomRight;
        }
        else
        {
            _damageNumber.alignment = TextAlignmentOptions.BottomLeft;
        }

        if (damage <= 0)
        {
            _damageNumber.color = Color.red;
            damage *= -1;
        }
        else
        {
            _damageNumber.color = Color.green;
        }

        _damageNumber.text = damage.ToString();
        _damageNumber.alpha = 1f;
        StartCoroutine(TextSizeUp());
    }

    //크기 키우기
    public IEnumerator TextSizeUp()
    {
        _accumTime = 0f;
        while (_accumTime < _sizeUpTime)
        {
            _damageNumber.fontSize = Mathf.Lerp(_startSize, _endSize, _accumTime / _sizeUpTime);
            _damageNumber.alpha = Mathf.Lerp(0f, 1f, _accumTime / _sizeUpTime);

            _damageNumber.transform.position += new Vector3(0f, 0.003f, 0f);

            _accumTime += Time.deltaTime;
            yield return null;
        }

        _damageNumber.fontSize = _endSize;
        _damageNumber.alpha = 1f;

        StartCoroutine(TextDisplay());
    }

    public IEnumerator TextDisplay()
    {
        _accumTime = 0f;
        while (_accumTime < _displayTime)
        {
            _damageNumber.transform.position += new Vector3(0f, 0.001f, 0f);

            _accumTime += Time.deltaTime;
            yield return null;
        }

        StartCoroutine(TextFadeOut());
    }

    //사라지기
    public IEnumerator TextFadeOut()
    {
        _accumTime = 0f;

        while (_accumTime < _fadeOutTime)
        {
            _damageNumber.alpha = Mathf.Lerp(1f, 0f, _accumTime / _fadeOutTime);

            _damageNumber.transform.position += new Vector3(0f, 0.001f, 0f);

            _accumTime += Time.deltaTime;
            yield return null;
        }

        Destroy(this.gameObject);
    }
}
