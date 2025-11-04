using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("현재 카메라 고정 지점")]
    public Transform currentPoint;

    [Header("카메라 이동 속도")]
    public float moveSpeed = 3f;

    private void LateUpdate()
    {
        if (currentPoint == null) return;

        Vector3 targetPos = Vector3.Lerp(transform.position, currentPoint.position, Time.deltaTime * moveSpeed);
        transform.position = new Vector3(targetPos.x, targetPos.y, -10f);
    }

    public void ChangeCameraPoint(Transform newPoint)
    {
        currentPoint = newPoint;
    }
}
