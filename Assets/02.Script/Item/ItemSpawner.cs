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
    [SerializeField] private bool isBoosRoom = false;
    [SerializeField] private bool isShopRoom = false;

    [Header("카메라 참조")]
    [SerializeField] private Camera mainCamera;

    private int totalEnemy = 0;
    private int aliveEnemy = 0;
    private bool itemSpawned = false;

    public static ItemSpawner Instance{  get; private set; }

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        if(mainCamera == null)
        {
            mainCamera = Camera.main;
        }
        totalEnemy = GameObject.FindGameObjectsWithTag("Enemy").Length;
        aliveEnemy = totalEnemy;
        Debug.Log($"[ItemSpawner] 전투 시작! 총 적 수: {totalEnemy}");

    }
    public void RegisterEnemy()
    {

        aliveEnemy++;
        itemSpawned = false;
        Debug.Log($"[ItemSpawner] 적 등록 → 현재 적 수: {aliveEnemy}");

    }
    public void UnregisterEnemy()
    {
        if (!gameObject.scene.isLoaded) return; //게임 정지시 아이템 스폰 되는것을 막음
        aliveEnemy = Mathf.Max(0, aliveEnemy - 1);
        Debug.Log($"[ItemSpawner] 적 사망 → 남은 적: {aliveEnemy}");


        if (aliveEnemy == 0 &&!itemSpawned)
        {
            itemSpawned = true;
            if (isBoosRoom)
            {
                return;
            }
            if (isShopRoom) 
            {
                shopManager.OpenShop();
            }
            else
            {
                Spawn();
            }
            itemSpawned = true;

        }
    }

    private void Spawn()
    {
        if (itemPrefab == null || itemPrefab.Count == 0) return;
        int index = Random.Range(0,itemPrefab.Count);
        GameObject prefab = itemPrefab[index];

        Vector3 spawnPos = mainCamera.transform.position;
        spawnPos.z = 0;


        Instantiate(itemPrefab[index], spawnPos, Quaternion.identity);
        Debug.Log($"[ItemSpawner] {prefab.name} 아이템 소환 완료!");


    }
    public void ResetSpawner() // 맵매니저만들때 이거 사용
    {
        totalEnemy = 0;
        aliveEnemy = 0;
        itemSpawned= false;
        shopManager?.ResetShop();
    }
}
