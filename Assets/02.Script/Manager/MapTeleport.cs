using System.Collections;
using UnityEngine;

public class MapTeleport : MonoBehaviour
{
    [Header("Teleport")]
    public GameObject toObj;              
    public Transform newCameraPoint;      
    public float cooldown = 1f;           

    private Collider2D myCollider;       

    private void Awake()
    {
        myCollider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        // 플레이어 이동
        collision.transform.position = toObj.transform.position;

        // 카메라 이동
        CameraController cam = Camera.main.GetComponent<CameraController>();
        if (cam != null && newCameraPoint != null)
            cam.ChangeCameraPoint(newCameraPoint);

        //  두 문 모두 2초 동안 비활성화
        StartCoroutine(DisableBothForSeconds());
    }

    private IEnumerator DisableBothForSeconds()
    {
        Collider2D otherCol = null;
        if (toObj != null)
            otherCol = toObj.GetComponent<Collider2D>();

        if (myCollider != null) myCollider.enabled = false;
        if (otherCol != null) otherCol.enabled = false;

        yield return new WaitForSeconds(cooldown);

        if (myCollider != null) myCollider.enabled = true;
        if (otherCol != null) otherCol.enabled = true;
    }
}
