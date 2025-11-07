using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [Header("ItemPrefab")]
    [SerializeField] private List<GameObject> itemPrefab = new List<GameObject>();

    [Header("ShopManager")]
    [SerializeField] private ShopManager shopManager;
    
    [Header("MapType")]
    [SerializeField] private bool isBossRoom = false;
    [SerializeField] private bool isShopRoom = false;

    [Header("Camera")]
    [SerializeField] private Camera mainCamera;

    private bool itemSpawned = false;

    private float checkInterval = 0.5f;
    private float checkTiemr = 0f;    

    private void Start()
    {
        if(mainCamera == null)
        {
            mainCamera = Camera.main;
        }

    }

    private void Update()
    {
        if (itemSpawned) return; 

        checkTiemr += Time.deltaTime;
        if (checkTiemr < checkInterval) return;
        checkTiemr = 0f;

        Vector3 veiw = mainCamera.WorldToViewportPoint(transform.position);
        if (veiw.z < 0 || veiw.x < 0 || veiw.x > 1 || veiw.y < 0 || veiw.y > 1) return;

        int enemyCount = CountEnemiesInCamera();

        if (enemyCount == 0)
        {
            itemSpawned = true;
            if (isBossRoom)
            {
                return;
            }

            if (isShopRoom)
            {
                if (shopManager != null)
                {
                    shopManager.OpenShop();
                }
            }
            else
            {
                Spawn();
            }
        }
    }
    private int CountEnemiesInCamera()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        int count = 0;

        foreach (var enemy in enemies)
        {
            if (enemy == null) continue;

            Vector3 pos = mainCamera.WorldToViewportPoint(enemy.transform.position);
            bool inView = pos.z > 0 && pos.x > 0 && pos.x < 1 && pos.y > 0 && pos.y < 1;
            if (inView) count++;
        }
        return count;
    }
    private void Spawn()
    {
        if (itemPrefab == null || itemPrefab.Count == 0) return;
        int index = Random.Range(0,itemPrefab.Count);
        GameObject prefab = itemPrefab[index];

        Vector3 spawnPos = mainCamera.transform.position;
        spawnPos.z = 0;


        Instantiate(itemPrefab[index], spawnPos, Quaternion.identity);
    }
}
