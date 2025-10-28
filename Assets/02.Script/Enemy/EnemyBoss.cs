using Unity.VisualScripting;
using UnityEngine;

public class EnemyBoss : MonoBehaviour
{
    [Header("타겟")]
    private Transform player;

    [Header("공격 세팅")]
    [SerializeField] private EnemyBullet bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireInterval = 1.0f;
    [SerializeField] private int bulletCount = 5;     
    [SerializeField] private float spreadAngle = 45f; 

    [Header("이동속도")]
    [SerializeField] private float moveSpeed = 1.0f;

    [Header("공격력")]
    [SerializeField] private int damage = 1;

    private Vector2 startPos;
    private float fireTimer;

    private void Start()
    {
        player = PlayerController.PlayerCachedTransform;

        startPos = transform.position;

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
        Vector2 playerDir  = (player.position - transform.position).normalized; //변수 이름 좀 바꾸자
        transform.Translate(playerDir * moveSpeed * Time.deltaTime, Space.World);
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
