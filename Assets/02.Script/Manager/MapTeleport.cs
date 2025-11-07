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
    private void OnEnable()
    {
        StartCoroutine(ReenableAfterDelay());
    }
    // 반대 포탈로 Player와 카메라 이동
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        collision.transform.position = toObj.transform.position;

        CameraController cam = Camera.main.GetComponent<CameraController>();
        if (cam != null && newCameraPoint != null)
            cam.ChangeCameraPoint(newCameraPoint);

        StartCoroutine(DisableBothForSeconds());
    }
    // 이동 시 양쪽 Collider 잠시 비활성화
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
    // 이동 후 Collider 활성화

    private IEnumerator ReenableAfterDelay()
    {
        yield return null;

        Collider2D otherCol = null;
        if (toObj != null)
            otherCol = toObj.GetComponent<Collider2D>();

        if (myCollider != null && !myCollider.enabled)
            myCollider.enabled = true;

        if (otherCol != null && !otherCol.enabled)
            otherCol.enabled = true;
    }
}
