using UnityEngine;

public class ItemObject : MonoBehaviour
{
    [SerializeField] private ItemDataSO itemData;
    private ShopManager shopManager;
    private bool pickUp = false;

    //아이템 획득 시 효과 적용 및 사운드 재생
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
    public void SetItemData(ItemDataSO data)
    {
        itemData = data;
    }
    public void SetShopManager(ShopManager manager)
    {
        shopManager = manager;
    }
}
