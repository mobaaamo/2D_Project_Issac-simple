using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [Header("아이템 프리팹")]
    [SerializeField] private List<GameObject> itemPrefab = new List<GameObject>();

    [Header("ShopManager")]
    [SerializeField] private ShopManager shopManager;
    
    [Header("MapType")]
    [SerializeField] private bool isBossRoom = false;
    [SerializeField] private bool isShopRoom = false;

    [Header("카메라 참조")]
    [SerializeField] private Camera mainCamera;

    private bool itemSpawned = false;

    //public static ItemSpawner Instance{  get; private set; }

    //private void Awake()
    //{
    //    Instance = this;
    //}
    private void Start()
    {
        if(mainCamera == null)
        {
            mainCamera = Camera.main;
        }

    }

    private void Update()
    {
        if (itemSpawned) return; // 이미 생성했으면 패스

        //  카메라 화면 안에 있을 때만 작동
        Vector3 v = mainCamera.WorldToViewportPoint(transform.position); //공부
        if (v.z < 0 || v.x < 0 || v.x > 1 || v.y < 0 || v.y > 1) return;

        float dist = Vector2.Distance(mainCamera.transform.position, transform.position);  //공부
        if (dist > 3.0f) return; // 방 간격의 절반 이하로 맞추기 (예: 방 간격이 9면 4.5)



        int enemyCount = CountEnemiesInCamera();

        if (enemyCount == 0)
        {
            itemSpawned = true;
            if (isBossRoom)
            {
                Debug.Log("[ItemSpawner] 보스룸은 아이템 생성 안 함");
                return;
            }

            if (isShopRoom)
            {
                shopManager?.OpenShop();
                Debug.Log("[ItemSpawner] 상점 오픈");
            }
            else
            {
                Spawn();
            }
        }
    }
    //public void RegisterEnemy()
    //{

    //    aliveEnemy++;
    //    itemSpawned = false;

    //}
    //public void UnregisterEnemy()
    //{
    //    if (!gameObject.scene.isLoaded) return; //게임 정지시 아이템 스폰 되는것을 막음
    //    aliveEnemy = Mathf.Max(0, aliveEnemy - 1);


    //    if (aliveEnemy == 0 &&!itemSpawned)
    //    {
    //        itemSpawned = true;
    //        if (isBoosRoom)
    //        {
    //            return;
    //        }
    //        if (isShopRoom) 
    //        {
    //            shopManager.OpenShop();
    //        }
    //        else
    //        {
    //            Spawn();
    //        }
    //        itemSpawned = true;

    //    }
    //}
    private int CountEnemiesInCamera()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        int count = 0;

        foreach (var e in enemies)
        {
            if (e == null) continue;

            Vector3 pos = mainCamera.WorldToViewportPoint(e.transform.position); //공부
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
    public void ResetSpawner() // 맵매니저만들때 이거 사용
    {
        itemSpawned= false;
        shopManager?.ResetShop();
    }
}
