using UnityEngine;
[CreateAssetMenu(fileName = "Item_", menuName = "Game/ItemData")]

public class ItemDataSO : ScriptableObject
{
    public int ID;
    public string itemName;
    public ItemType type;
    public float power;
    public string description;
}
