using UnityEngine;

public class ItemObject : MonoBehaviour
{
    [SerializeField] private ItemDataSO itemData;
    private ShopManager shopManager;
    private bool pickUp = false;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (pickUp) return;
        if (collision.CompareTag("Player"))
        {
            SoundManager.instance.playerPowerUp.Play();

            PlayerController player = collision.GetComponent<PlayerController>();
            if (player != null)
            {
                player.ApplyItemEffect(itemData);
            }
 
            pickUp = true;
            shopManager?.OnItemChosen(gameObject);
            gameObject.SetActive(false);
        }
    }
    public void SetItemData(ItemDataSO data) //°øºÎ
    {
        itemData = data;
    }
    public void SetShopManager(ShopManager manager)
    {
        shopManager = manager;
    }
}
