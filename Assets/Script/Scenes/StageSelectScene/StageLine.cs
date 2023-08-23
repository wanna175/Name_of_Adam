using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageLine : MonoBehaviour
{
    public void DrawLine(Stage target)
    {
        Vector3 targetPos = target.transform.position;
        float distance = Vector3.Distance(targetPos, transform.position);
        Vector3 vec = targetPos - transform.position;
        float angle = Mathf.Atan2(vec.y, vec.x) * Mathf.Rad2Deg;

        GetComponent<SpriteRenderer>().size = new Vector2(distance, 0.25f);
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
