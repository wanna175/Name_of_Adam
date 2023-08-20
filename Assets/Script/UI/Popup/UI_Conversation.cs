using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Conversation : UI_Popup
{
    private float _typingSpeed = 0.05f;
    private Coroutine co_typing = null;
    private List<Script> scripts = new List<Script>();

    //[SerializeField] private Text _conversationText;
    [SerializeField] private Text _nameText;
    [SerializeField] private GameObject _nameObject;
    [SerializeField] private TextMeshProUGUI _conversation;
    
    // autoStart = false면 따로 실행해줘야 함
    public void Init(List<Script> scripts, bool autoStart = true)
    {
        this.scripts = scripts;

        if (autoStart == false)
            return;

        StartCoroutine(PrintScript());
    }

    public IEnumerator PrintScript()
    {
        // 이벤트 스크립트 본문 출력
        foreach (Script script in scripts)
        {
            // 이름 없으면 이름 창 꺼짐
            if (script.name == "")
                _nameObject.SetActive(false);
            else
            {
                _nameObject.SetActive(true);
                _nameText.text = script.name;
            }

            co_typing = StartCoroutine(TypingEffect(script.script));

            yield return new WaitUntil(() => (co_typing == null || GameManager.InputManager.Click));

            if(co_typing != null)
            {
                StopCoroutine(co_typing);
                co_typing = null;
            }
            _conversation.text = script.script;

            yield return new WaitUntil(() => GameManager.InputManager.Click);
        }

        GameManager.UI.ClosePopup(this);
    }

    private IEnumerator TypingEffect(string script)
    {
        TextMeshProUGUI text = _conversation;
        text.text = "";

        foreach (char c in script.ToCharArray())
        {
            text.text += c;
            yield return new WaitForSeconds(_typingSpeed);
        }

        text.text = script;
        co_typing = null;
    }

}
