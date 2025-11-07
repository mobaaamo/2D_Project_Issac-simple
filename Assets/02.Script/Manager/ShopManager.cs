using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [Header("ItemPrefab")]
    [SerializeField] private List<GameObject> itemPrefab = new List<GameObject>();

    [Header("ItemSpawnPoint")]
    [SerializeField] private Transform[] spawnPoints;

    private List<GameObject> spawnedItems = new List<GameObject>();
    private bool itemChosen = false;

    public void OpenShop()
    {
        itemChosen = false;
        SpawnItems();
    }
    //아이템 3개 랜덤 생성
    private void SpawnItems()
    {
        for (int i = 0; i < 3; i++)
        {
            int rand = Random.Range(0, itemPrefab.Count);
            GameObject prefab = itemPrefab[rand];
            

            GameObject itemObj = Instantiate(prefab, spawnPoints[i].position, Quaternion.identity);
            if(itemObj.TryGetComponent<ItemObject>(out var item))
            {
                item.SetShopManager(this);
            }

            spawnedItems.Add(itemObj);
        }
    }
    // 1개의 아이템 획득시 나머지 삭제
    public void OnItemChosen(GameObject chosenItem)
    {
        if(itemChosen) return;

        itemChosen = true;

        foreach(var item in spawnedItems)
        {
            if(item != null && item !=chosenItem)
            {
                Destroy(item);
            }
        }
        spawnedItems.Clear();
    }

}
