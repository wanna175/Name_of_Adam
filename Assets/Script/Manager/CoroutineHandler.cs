using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// MonoBehavior가 없는 클래스도 코루틴을 사용할 수 있게 지원하는 클래스
public class CoroutineHandler : MonoBehaviour
{
    IEnumerator enumerator = null;
    bool isActive = true;

    private void Coroutine(IEnumerator coro, float delay)
    {
        enumerator = coro;

        // 전달받은 코루틴과 이 객체를 파괴하기 위한 코루틴을 생성
        StartCoroutine(coro);
        StartCoroutine(StopCoroutine(delay));
    }

    void Update()
    {
        if (enumerator != null)
        {
            if (enumerator.Current == null)
            {
                Destroy(gameObject);
            }
        }
        if (!isActive)
            Destroy(gameObject);
    }

    public void Stop()
    {
        StopCoroutine(enumerator.ToString());
        Destroy(gameObject);
    }

    // 지속시간이 다 되면 이 객체를 파괴할 수 있게 만듦
    IEnumerator StopCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        isActive = false;
    }
    
    // 시작할 코루틴과 그 코루틴의 지속시간을 인자로 전달
    public static CoroutineHandler Start_Coroutine(IEnumerator coro, float delay)
    {
        GameObject obj = new GameObject("CoroutineHandler");
        CoroutineHandler handler = obj.AddComponent<CoroutineHandler>();
        if (handler)
        {
            handler.Coroutine(coro, delay);
        }
        return handler;
    }
}
