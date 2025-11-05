using UnityEngine;

public class MapTeleport : MonoBehaviour
{
    //public GameObject toObj;             
    //public Transform newCameraPoint;


    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Player"))
    //    {
    //        // 플레이어 이동
    //        collision.transform.position = toObj.transform.position;

    //        // 카메라 고정 지점 변경
    //        CameraController cam = Camera.main.GetComponent<CameraController>();
    //        cam.ChangeCameraPoint(newCameraPoint);
    //    }

    //}


    [Header("Teleport")]
    public GameObject toObj;              // 반대편 문
    public Transform newCameraPoint;      // 이동 후 카메라 포인트

    private bool playerInside = false;    // 플레이어가 이 문 안에 있는가?
    private bool teleporting = false;     // 텔레포트 중인가?

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        // 이미 텔레포트 중이라면 중복 실행 방지
        if (teleporting) return;

        if (!playerInside)
        {
            StartCoroutine(TeleportPlayer(collision));
        }
    }

    private System.Collections.IEnumerator TeleportPlayer(Collider2D collision)
    {
        teleporting = true;
        playerInside = true;

        // 플레이어 이동
        collision.transform.position = toObj.transform.position;

        // 카메라 이동
        CameraController cam = Camera.main.GetComponent<CameraController>();
        cam.ChangeCameraPoint(newCameraPoint);

        // 0.1초 후 텔레포트 중 해제
        yield return new WaitForSeconds(0.1f);
        teleporting = false;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // 플레이어가 이 문 트리거 밖으로 나갔을 때 다시 사용 가능
            playerInside = false;
        }
    }

}
