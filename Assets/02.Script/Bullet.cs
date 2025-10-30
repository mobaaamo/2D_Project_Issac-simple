using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float attackspeed = 3.0f;
    public float bulletDistance = 1.0f;
    public int damage = 1;


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
        rb.velocity = dir.normalized * attackspeed;

        //Debug.Log($"[Bullet] 현재 데미지: {damage}");
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
            if(collision.TryGetComponent<EnemyHp>(out var hp))
            {
                hp.TakeDamage(damage);
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
    //void OnEnable()
    //{
    //    damage = 1;
    //    bulletDistance = 1.0f;
    //    attackspeed = 3.0f;
    //}
    public void SetStats(int dmg, float speed, float range)
    {
        damage = dmg;
        attackspeed = speed;
        bulletDistance = range;
    }

}

