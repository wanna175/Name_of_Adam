using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Conversation : UI_Popup
{
    private float _typingSpeed = 0.05f;
    private Coroutine co_typing = null;
    private List<Script> scripts = new List<Script>();

    enum Texts
    {
        ConversationText,
        NameText,
    }

    enum Objects
    {
        Name,
    }

    // autoStart = false면 따로 실행해줘야 함
    public void Init(List<Script> scripts, bool autoStart = true)
    {
        Bind<Text>(typeof(Texts));
        Bind<GameObject>(typeof(Objects));

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
                GetObject((int)Objects.Name).SetActive(false);
            else
            {
                GetObject((int)Objects.Name).SetActive(true);
                GetText((int)Texts.NameText).text = script.name;
            }

            co_typing = StartCoroutine(TypingEffect(script.script));

            yield return new WaitUntil(() => GameManager.InputManager.Click);
        }

        GameManager.UI.ClosePopup(this);
    }

    private IEnumerator TypingEffect(string script)
    {
        Text text = GetText((int)Texts.ConversationText);
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
