using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    private void OnEnable()
    {
        ItemSpawner.Instance?.RegisterEnemy();
    }

    private void OnDisable()
    {
        ItemSpawner.Instance?.UnregisterEnemy();
    }
}
