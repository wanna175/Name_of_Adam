using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StageCameraController : MonoBehaviour
{
    [SerializeField] Transform MapTransform;
    private float _cameraSpeed = 4.0f; // ������ �� ī�޶� �̵� �ӵ� ������
    private Vector3 _topPosition = new(0, 25, -10);
    private Vector3 _bottomPosition = new(0, -5, -10);

    const int _wheelSpeed = 200;

    private void Start()
    {
        StartCoroutine(MapScanMove());
    }

    private void Update()
    {
        float wheel = Input.GetAxis("Mouse ScrollWheel");
        MoveCamera(wheel);

        float mousePosition = Input.mousePosition.y;

        if (mousePosition > Screen.height * 0.99f)
        {
            MoveCamera(0.03f);
        }
        else if (mousePosition < Screen.height * 0.01f)
        {
            MoveCamera(-0.03f);
        }
    }

    private Vector3 _velocity = Vector3.zero; // �ʱ� �ӵ���

    void MoveCamera(float num)
    {
        if ((_velocity.y == 0 && num == 0) || EventSystem.current.IsPointerOverGameObject())
            return;

        Vector3 desiredMove = new(0, transform.position.y + num * _wheelSpeed, -10);
        Vector3 movePosition = Vector3.SmoothDamp(transform.position, desiredMove, ref _velocity, 0.3f);

        movePosition.y = Mathf.Clamp(movePosition.y, _bottomPosition.y, _topPosition.y);
        transform.position = movePosition;
    }


    public void SetLocate(float y)
    {
        transform.localPosition = new Vector3(0, y, -10);
    }

    IEnumerator MapScanMove()
    {
        if (GameManager.Data.Map.CurrentTileID != 0)
        {
            yield break;
        }

        float elapsedTime = 0;
        while (elapsedTime < _cameraSpeed)
        {
            float t = elapsedTime / _cameraSpeed;
            t = Mathf.Sin((t * Mathf.PI) / 2);

            transform.position = Vector3.Lerp(_topPosition, _bottomPosition, t);
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        transform.position = _bottomPosition;
        yield return null;
    }
}
