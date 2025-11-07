using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("CurrentPoint Camera")]
    public Transform currentPoint;

    [Header("Camera Move Speed")]
    public float moveSpeed = 3f;

    //카메라 이동
    private void LateUpdate()
    {
        if (currentPoint == null) return;

        Vector3 targetPos = Vector3.Lerp(transform.position, currentPoint.position, Time.deltaTime * moveSpeed);
        transform.position = new Vector3(targetPos.x, targetPos.y, -10f);
    }
    //지정된 카메라 포인트로 이동
    public void ChangeCameraPoint(Transform newPoint)
    {
        currentPoint = newPoint;
    }
}
