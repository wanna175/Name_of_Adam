using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageCameraController : MonoBehaviour
{
    [SerializeField] float MaxHight;
    [SerializeField] float MinHight;

    private void Update()
    {
        float wheel = Input.GetAxis("Mouse ScrollWheel");
        MoveCamera(wheel);
    }

    void MoveCamera(float num)
    {
        if (num == 0)
            return;

        transform.position += new Vector3(0, num * 5, 0);

        if (transform.position.y < MinHight)
            transform.position = new Vector3(0, MinHight, -10);
        if (transform.position.y > MaxHight)
            transform.position = new Vector3(0, MaxHight, -10);
    }

    public void SetLocate(float y)
    {
        transform.position = new Vector3(0, y, -10);
        if (y < -5)
            transform.position = new Vector3(0, MinHight, -10);
        else if (30 < y)
            transform.position = new Vector3(0, MaxHight, -10);
    }
}
