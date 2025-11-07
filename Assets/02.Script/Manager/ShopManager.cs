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
