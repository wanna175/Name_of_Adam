using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// StageSelectScene에서 노드 사이의 선을 자동으로 연결해주는 클래스
public class StageLine : MonoBehaviour
{
    [SerializeField] List<Sprite> spriteList;

    public void DrawLine(Stage target)
    {
        Vector3 targetPos = target.transform.position;
        float distance = Vector3.Distance(targetPos, transform.position);
        Vector3 vec = targetPos - transform.position;
        float angle = Mathf.Atan2(vec.y, vec.x) * Mathf.Rad2Deg;

        // 선의 폭을 넓히고싶다면 new Vector2의 y를 수정하면 됨
        GetComponent<SpriteRenderer>().size = new Vector2(distance, 2.0f);
        transform.rotation = Quaternion.Euler(0, 0, angle);

        GetComponent<SpriteRenderer>().sprite = spriteList[Random.Range(0, spriteList.Count)];
    }
}
