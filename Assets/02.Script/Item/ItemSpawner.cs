using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [Header("������ ������")]
    [SerializeField] private List<GameObject> itemPrefab = new List<GameObject>();

    [Header("ShopManager")]
    [SerializeField] private ShopManager shopManager;
    
    [Header("MapType")]
    [SerializeField] private bool isBoosRoom = false;
    [SerializeField] private bool isShopRoom = false;

    [Header("ī�޶� ����")]
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
        Debug.Log($"[ItemSpawner] ���� ����! �� �� ��: {totalEnemy}");

    }
    public void RegisterEnemy()
    {

        aliveEnemy++;
        itemSpawned = false;
        Debug.Log($"[ItemSpawner] �� ��� �� ���� �� ��: {aliveEnemy}");

    }
    public void UnregisterEnemy()
    {
        if (!gameObject.scene.isLoaded) return; //���� ������ ������ ���� �Ǵ°��� ����
        aliveEnemy = Mathf.Max(0, aliveEnemy - 1);
        Debug.Log($"[ItemSpawner] �� ��� �� ���� ��: {aliveEnemy}");


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
        Debug.Log($"[ItemSpawner] {prefab.name} ������ ��ȯ �Ϸ�!");


    }
    public void ResetSpawner() // �ʸŴ������鶧 �̰� ���
    {
        totalEnemy = 0;
        aliveEnemy = 0;
        itemSpawned= false;
        shopManager?.ResetShop();
    }
}
