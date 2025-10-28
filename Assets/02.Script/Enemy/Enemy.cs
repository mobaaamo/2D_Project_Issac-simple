using System.Net.NetworkInformation;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("타겟")]
    private Transform player;

    [Header("공격 세팅")]
    [SerializeField] private EnemyBullet bulletPrefab;    
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireInterval = 1.0f;
    [SerializeField] private int bulletCount = 5;     
    [SerializeField] private float spreadAngle = 45f;
    [Header("공격력")]
    [SerializeField] private int damage = 1;

    private float fireTimer;

    private void Start()
    {
        player = PlayerController.PlayerCachedTransform;
        PoolManager.Instance.CreatPool(bulletPrefab, 20);
    }

    private void Update()
    {

        if (player == null) return;

        fireTimer += Time.deltaTime;

        Vector2 dir = (player.position - transform.position).normalized;
        transform.right = dir; 

        if (fireTimer >= fireInterval)
        {
            FireSpread(dir);
            fireTimer = 0f;
        }
    }

    void FireSpread(Vector2 centerDir)
    {
        float startAngle = -spreadAngle * 0.5f;

        for (int i = 0; i < bulletCount; i++)
        {
            float angleOffset = startAngle + (spreadAngle / (bulletCount - 1)) * i;
            Vector2 shotDir = Quaternion.Euler(0, 0, angleOffset) * centerDir;

            EnemyBullet bullet = PoolManager.Instance.GetFromPool(bulletPrefab);
            bullet.Init(firePoint.position, shotDir);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            if (collision.collider.TryGetComponent<PlayerController>(out var hp))
            {
                hp.TakeDamage(damage);
            }
        }
    }

}
