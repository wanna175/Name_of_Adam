using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class UI_CancelCard : UI_Base, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private GameObject _highlight;
    [SerializeField] private GameObject _selectHighlight;

    private Action<DeckUnit> _onSelectAction;

    private void Start()
    {
        _highlight.SetActive(false);
    }

    public void Init(Action<DeckUnit> onSelectAction)
    {
        _onSelectAction = onSelectAction;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _highlight.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _highlight.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        GameManager.Sound.Play("UI/UISFX/UISelectSFX");
        _onSelectAction(null);
    }
}
