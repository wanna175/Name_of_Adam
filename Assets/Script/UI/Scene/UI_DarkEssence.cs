using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class UI_DarkEssence : UI_Scene, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TextMeshProUGUI _darkEssence;
    [SerializeField] private Image cannotLight;
    [SerializeField] private UI_CannotEffect cannotEffect;
    public UI_CannotEffect CannotEffect => cannotEffect;

    public void Init()
    {
        cannotEffect.Init(Vector3.one * 0.5f, Vector3.one * 0.6f, 1.5f);

        Refresh();
    }

    public void Refresh() 
    {
        _darkEssence.text = GameManager.Data.GameData.DarkEssence.ToString();
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
                    $"{GameManager.Locale.GetLocalizedBattleScene("DarkEssence UI Info")}", Input.mousePosition);
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
