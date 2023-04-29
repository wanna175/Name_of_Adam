using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NextStageSelectArea : EventTrigger
{
    IEnumerator coro;

    float maxHeight = Screen.height;
    float halfSize;

    private void Start()
    {
        halfSize = GetComponent<RectTransform>().sizeDelta.y / 2;
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);

        if(coro != null)   
            StopCoroutine(coro);
        coro = AreaDown();
        StartCoroutine(coro);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);

        if (coro != null)
            StopCoroutine(coro);
        coro = AreaUp();
        StartCoroutine(coro);
    }

    IEnumerator AreaDown()
    {
        Transform trans = GetComponent<RectTransform>().transform;

        while (trans.position.y > maxHeight - halfSize)
        {
            trans.position += new Vector3(0, -5, 0);

            yield return null;
        }

        Vector3 vec = trans.position;
        vec.y = maxHeight - halfSize;
        trans.position = vec;
    }

    IEnumerator AreaUp()
    {
        Transform trans = GetComponent<RectTransform>().transform;

        while (trans.position.y < maxHeight)
        {
            trans.position += new Vector3(0, 5, 0);

            yield return null;
        }

        Vector3 vec = trans.position;
        vec.y = maxHeight;
        trans.position = vec;
    }
}
