using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
    public float attackspeed = 3.0f;
    public float bulletDistance = 0.5f;
    public int damage = 1;

    private float gravityScale = 1.0f;
    private float spawnTime;
    private Rigidbody2D rb;
    private Animator anim;
    private bool gravityOn = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }
    public void Init(Vector2 spawnPos, Vector2 dir)
    {
        transform.position = spawnPos;
        spawnTime = Time.time;
        gravityOn = false;

        rb.gravityScale = 0f;
        rb.velocity = dir.normalized * attackspeed;
    }

    private void Update()
    {
        float elapsed = Time.time - spawnTime;

        if (!gravityOn && elapsed >= bulletDistance * 0.87f)
        {
            rb.gravityScale = gravityScale;
            gravityOn = true;
        }
        if(elapsed >= bulletDistance)
        {
            StartCoroutine(ExplodeAndReturn());
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
            StartCoroutine(ExplodeAndReturn());
        }
        else if (collision.CompareTag("Wall"))
        {
            StartCoroutine(ExplodeAndReturn());
        }
    }

    void ReturnPool()
    {
        rb.velocity = Vector2.zero;
        rb.gravityScale = 0f;
        PoolManager.Instance.ReturnPool(this);
    }
    public void SetStats(int dmg, float speed, float range)
    {
        damage = dmg;
        attackspeed = speed;
        bulletDistance = range;
    }

    private IEnumerator ExplodeAndReturn()
    {
        rb.velocity = Vector2.zero;
        rb.gravityScale = 0f;

        anim.Play("TearsHit");
        yield return null;

        AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);
        float animTime = state.length;

        yield return new WaitForSeconds(animTime);

        ReturnPool();
    }
}

