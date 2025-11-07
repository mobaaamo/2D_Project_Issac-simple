using Unity.VisualScripting;
using UnityEngine;

public class EnemyBoss : MonoBehaviour
{
    [Header("Target")]
    private Transform player;

    [Header("Attack Setting")]
    [SerializeField] private EnemyBullet bulletPrefab;
    [SerializeField] private EnemyFollower EnemyPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireInterval = 1.0f;
    [SerializeField] private int bulletCount = 5;     
    [SerializeField] private float spreadAngle = 45f;

    [Header("Spawn Setting")]
    [SerializeField] private float spawnDelay = 5.0f;

    [Header("Move Speed")]
    [SerializeField] private float moveSpeed = 1.0f;

    [Header("Damage")]
    [SerializeField] private int damage = 1;


    [Header("PlayerCheck")]
    [SerializeField] private Transform playerCheck;
    [SerializeField] private Vector2 playerCheckBox = new Vector2(8f, 5.5f);
    [SerializeField] private LayerMask playerLayer;

    private EnemyHp enemyHp;
    private bool canAttack = false;
    private Vector2 fixCheckBox;

    private Vector2 startPos;
    private float fireTimer;
    private float spawnTimer;
    private void Start()
    {
        player = PlayerController.PlayerTransform;

        startPos = transform.position;

        PoolManager.Instance.CreatPool(bulletPrefab, 50);
        PoolManager.Instance.CreatPool(EnemyPrefab, 5);

        fixCheckBox = playerCheck.position;

        enemyHp = GetComponent<EnemyHp>();
    }

    private void Update()
    {

        if (player == null) return;


        canAttack = Physics2D.OverlapBox(playerCheck.position, playerCheckBox, 0f, playerLayer);
        if (!canAttack) return;

        fireTimer += Time.deltaTime;
        spawnTimer += Time.deltaTime;

        if (fireTimer >= fireInterval)
        {
            AttackPlayer();
            fireTimer = 0f;
        }
        if(spawnTimer >= spawnDelay)
        {
            SpawnFollower();
            spawnTimer = 0f;
        }
        Vector2 playerDir = (player.position - transform.position).normalized;
        transform.Translate(playerDir * moveSpeed * Time.deltaTime, Space.World);

    }
    void LateUpdate()
    {
        playerCheck.position = fixCheckBox;
    }
    void AttackPlayer()
    {
        Vector2 playerDir = (player.position - transform.position).normalized;

        float startAngle = -spreadAngle * 0.5f;

        for (int i = 0; i < bulletCount; i++)
        {
            float angleOffset = startAngle + (spreadAngle / (bulletCount - 1)) * i;
            Vector2 shotDir = Quaternion.Euler(0, 0, angleOffset) * playerDir;

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
    void SpawnFollower()
    {
        EnemyFollower follower = PoolManager.Instance.GetFromPool(EnemyPrefab);
        if (follower != null)
        {

            follower.transform.position = transform.position;
            follower.gameObject.SetActive(true);
        }
    }
    private void OnDrawGizmosSelected()
    {
        if (playerCheck == null) return;

        Gizmos.color = canAttack ? Color.red : Color.gray;
        Gizmos.DrawWireCube(playerCheck.position, playerCheckBox);
    }
}

