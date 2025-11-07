using UnityEngine;

public class EnemyHp : MonoBehaviour
{
    [SerializeField] private int maxHp = 10;
    private int currentHp;

    private void Awake()
    {
        currentHp = maxHp;
    }
    public void TakeDamage(int damage)
    {
        currentHp -= damage;
        if (currentHp <= 0)
        { 
            Die(); 
        }
    }
    //BossDie 시 게임 클리어
    void Die()
    {
        if (TryGetComponent<EnemyBoss>(out var boss))
        { 
            if(GameManager.instance != null)
            {
                GameManager.instance.SetState(GameManager.GameState.GameClear);
            }
        }
        Destroy(gameObject);
    }
}
