using UnityEngine;
using TMPro;
using System.Collections;

public class UI_FloatingDamage : UI_Base
{
    [SerializeField] private TextMeshProUGUI _damageNumber;

    readonly private float _sizeUpTime = 0.3f;
    readonly private float _displayTime = 0.5f;
    readonly private float _fadeOutTime = 0.4f;
    //1.2
    readonly private int _startSize = 80;
    readonly private int _endSize = 130;

    private float _accumTime;

    private bool _direction = false;

    private void Awake()
    {
        _damageNumber.alpha = 0f;
    }

    public void DirectionChange(bool direction)
    {
        _direction = direction;
    }

    public void Init(int damage)
    {
        if (_direction)
        {
            _damageNumber.alignment = TextAlignmentOptions.BottomRight;
        }
        else
        {
            _damageNumber.alignment = TextAlignmentOptions.BottomLeft;
        }

        if (damage < 0)
        {
            _damageNumber.color = Color.red;
            damage *= -1;
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

            _damageNumber.transform.position += new Vector3(0f, 0.001f, 0f);

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

        _damageNumber.alpha = 0f;
        _damageNumber.transform.localPosition = new Vector3(0,0,0);
    }
}
