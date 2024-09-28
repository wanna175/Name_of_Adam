using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_WaitingPlayer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject _darkProphetImage;
    [SerializeField] private GameObject _heirofViceImage;
    [SerializeField] private GameObject _saintessOfSinsImage;

    private void Start()
    {
        _darkProphetImage.SetActive(GameManager.Data.GameData.Incarna.Name == "Dark Prophet");
        _heirofViceImage.SetActive(GameManager.Data.GameData.Incarna.Name == "Heir Of Vice");
        _saintessOfSinsImage.SetActive(GameManager.Data.GameData.Incarna.Name == "Saintess Of Sins");
    }

    public void SetAnimatorBool(string name, bool on)
    {
        GetComponent<Animator>().SetBool(name, on);
    }

    public void RemoveAnimationEnd()
    {
        this.gameObject.SetActive(false);
    }

    private bool _isHover = false;
    private bool _isHoverMessegeOn = false;

    public void OnPointerEnter(PointerEventData eventData)
    {
        _isHover = true;
        GameManager.Instance.PlayAfterCoroutine(() => {
            if (_isHover && !_isHoverMessegeOn)
            {
                _isHoverMessegeOn = true;
                GameManager.UI.ShowHover<UI_TextHover>().SetText(
                    $"{GameManager.Locale.GetLocalizedBattleScene("WaitingPlayer UI Info")}", Input.mousePosition);
            }
        }, 0.5f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _isHover = false;

        if (_isHoverMessegeOn)
        {
            _isHoverMessegeOn = false;
            GameManager.UI.CloseHover();
        }
    }
}
