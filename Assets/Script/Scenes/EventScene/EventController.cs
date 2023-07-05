using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventController : MonoBehaviour
{
    [SerializeField] Image EventImage;
    [SerializeField] Text EventText;
    [SerializeField] GameObject ButtonContainer;
    List<GameObject> Buttons = new List<GameObject>();

    // 외부에서 데이터로 받는 이벤트 내용
    // 지금은 인스펙터를 통해 받는것으로 테스트를 진행함
    [SerializeField] Image image;
    [SerializeField] string text;
    [SerializeField] int ButtonCount;
    [SerializeField] string[] ButtonText;

    bool isEnd;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        isEnd = false;

        if(image != null)
            EventImage = image;
        EventText.text = text;

        CreateButton(ButtonCount, ButtonText);
    }

    private void CreateButton(int count, string[] texts)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject button = GameManager.Resource.Instantiate("UI/Event/EventButton", ButtonContainer.transform);
            button.GetComponent<EventButtonEventTrigger>().Init(this);
            button.transform.GetChild(0).GetComponent<Text>().text = texts[i];
            Buttons.Add(button);
        }
    }

    public void ButtonClick(GameObject button)
    {
        if (!isEnd)
        {
            isEnd = true;

            int index = Buttons.FindIndex(x => x == button) + 1;
            DestroyButton();

            EventText.text = $"{index}번 버튼을 클릭하였습니다.";

            CreateButton(1, new string[1] { "나가기" });
        }
        else
        {
            // 이벤트 종료
            // 스테이지 이동
            SceneChanger.SceneChange("StageSelectScene");
        }
    }

    private void DestroyButton()
    {
        for(int i = Buttons.Count - 1; i >= 0 ; i--)
            Destroy(Buttons[i]);

        Buttons.Clear();
    }
}
