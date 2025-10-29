using UnityEngine;

public class ItemObject : MonoBehaviour
{
    [SerializeField] private ItemDataSO itemData;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            if (player != null)
            {
                player.ApplyItemEffect(itemData);
            }
            gameObject.SetActive(false);
        }
    }
    public void SetItemData(ItemDataSO data)
    {
        itemData = data;
    }
}
