using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
    [Header("CloserDoor")]
    [SerializeField] private List<GameObject> closerDoor = new List<GameObject>();
    [Header("OpeenDoor")]
    [SerializeField] private List<GameObject> openDoor = new List<GameObject>();

    [Header("Camera")]
    [SerializeField] private Camera mainCamera;

    private void Start()
    {
        if (mainCamera == null) mainCamera = Camera.main;
        UpdateDoor(false);
    }

    private void Update()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        int count = 0;

        foreach (var e in enemies)
        {
            if (e == null) continue;

            Vector3 pos = mainCamera.WorldToViewportPoint(e.transform.position);
            if (pos.z > 0 && pos.x > 0 && pos.x < 1 && pos.y > 0 && pos.y < 1)
                count++;
        }

        UpdateDoor(count == 0);
    }

    private void UpdateDoor(bool isOpen)
    {
        if(closerDoor != null)
        {
            foreach(var door in closerDoor)
                if(door !=null) door.SetActive(!isOpen);
        }
        if(openDoor != null)
        {
            foreach(var door in openDoor)
                if(door !=null) door.SetActive(isOpen);
        }
    }
}
