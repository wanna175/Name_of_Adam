using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageCameraController : MonoBehaviour
{
    [SerializeField] Transform MapTransform;
    Vector3 lastMousePosition;

    private void Update()
    {
        MouseInputs();
    }

    void MouseInputs()
    {
        if (Input.GetMouseButtonDown(0))
        {
            lastMousePosition = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0))
        {
            Vector3 delta = Input.mousePosition - lastMousePosition;

            MoveCamera(delta.y);

            lastMousePosition = Input.mousePosition;
        }
    }

    void MoveCamera(float num)
    {
        if (num == 0)
            return;

        transform.position += new Vector3(0, num * -0.01f , 0);

        if (GameManager.Data.StageAct == 0)
        {
            if (transform.position.y < -50)
                transform.position = new Vector3(0, -50, -10);
            if (transform.position.y > -40 && transform.position.y < -20)
                transform.position = new Vector3(0, -40, -10);
        }
        else
        {
            if (transform.position.y < -5 && transform.position.y > -20)
                transform.position = new Vector3(0, -5, -10);
            if (transform.position.y > 25)
                transform.position = new Vector3(0, 25, -10);
        }
    }

    public void SetLocate(float y)
    {
        transform.localPosition = new Vector3(0, y, -10);
    }
}
