using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 3.0f;
    [SerializeField] private float bulletDistance = 1.0f;
    [SerializeField] private int dameage = 1;


    private float spawnTime;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public void Init(Vector2 spawnPos, Vector2 dir)
    {
        transform.position = spawnPos;
        spawnTime = Time.time;
        rb.velocity = dir.normalized * speed;
    }
    private void Update()
    {
        if(Time.time - spawnTime >= bulletDistance) 
        {
            ReturnPool();
        }
    }


    void OnTriggerEnter2D(Collider2D collision)  
    {
        if (collision.CompareTag("Enemy"))
        {
            if(collision.TryGetComponent<EnemyHp>(out var health))
            {
                health.TakeDamage(dameage);
            }
            ReturnPool();
        }
        else if (collision.CompareTag("Wall"))
        {
            ReturnPool();
        }
    }

    void ReturnPool()
    {
        rb.velocity = Vector2.zero;
        PoolManager.Instance.ReturnPool(this);
    }

}

