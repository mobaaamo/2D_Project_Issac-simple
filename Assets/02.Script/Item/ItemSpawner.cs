using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [Header("������ ������")]
    [SerializeField] private List<GameObject> itemPrefab = new List<GameObject>();

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
    }
    public void RegisterEnemy()
    {
        aliveEnemy++;
        itemSpawned = false;
    }
    public void UnregisterEnemy()
    {
        aliveEnemy = Mathf.Max(0, aliveEnemy - 1);

        if (aliveEnemy == 0 &&!itemSpawned)
        {

            Spawn();
            itemSpawned = true;
        }
    }

    private void Spawn()
    {
        int index = Random.Range(0,itemPrefab.Count);
        GameObject randItem = itemPrefab[index];

        Vector3 spawnPos = mainCamera.transform.position;
        spawnPos.z = 0;


        Instantiate(randItem, spawnPos, Quaternion.identity);


    }
    public void ResetSpawner() // �ʸŴ������鶧 �̰� ���
    {
        aliveEnemy = 0;
        itemSpawned= false;
    }
}
