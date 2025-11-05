using System.Net.NetworkInformation;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Target")]
    private Transform player;

    [Header("Attack Setting")]
    [SerializeField] private EnemyBullet bulletPrefab;    
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireInterval = 1.0f;
    [SerializeField] private int bulletCount = 5;     
    [SerializeField] private float spreadAngle = 45f;
    [Header("Damage")]
    [SerializeField] private int damage = 1;

    [Header("PlayerCheck")]
    [SerializeField] private Transform playerCheck;
    [SerializeField] private Vector2 playerCheckBox = new Vector2(4f, 3f);
    [SerializeField] private LayerMask playerLayer;

    private bool canAttack = false;
    private float fireTimer;

    private void Start()
    {
        player = PlayerController.PlayerTransform;
        PoolManager.Instance.CreatPool(bulletPrefab, 40);
    }

    private void Update()
    {

        if (player == null) return;

        canAttack = Physics2D.OverlapBox(playerCheck.position, playerCheckBox, 0f, playerLayer);
        if(!canAttack) return;

        fireTimer += Time.deltaTime;

        if (fireTimer >= fireInterval)
        {
            AttackPlayer();
            fireTimer = 0f;
        }
    }

    void AttackPlayer()
    {

        Vector2 dir = (player.position - transform.position).normalized;

        float startAngle = -spreadAngle * 0.5f;

        for (int i = 0; i < bulletCount; i++)
        {
            float angleOffset = startAngle + (spreadAngle / (bulletCount - 1)) * i;
            Vector2 shotDir = Quaternion.Euler(0, 0, angleOffset) * dir;

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
    private void OnDrawGizmosSelected()
    {
        if (playerCheck == null) return;

        Gizmos.color = canAttack ? Color.red : Color.gray;
        Gizmos.DrawWireCube(playerCheck.position, playerCheckBox);
    }

}
