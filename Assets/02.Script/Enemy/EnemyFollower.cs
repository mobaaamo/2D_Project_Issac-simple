using UnityEngine;

public class EnemyFollower : MonoBehaviour
{
    [Header("Ÿ��")]
    private Transform player;
    [Header("�̵��ӵ�")]
    [SerializeField] private float moveSpeed = 0.5f;
    [Header("���ݷ�")]
    [SerializeField] private int damage = 1;

    private Vector2 startPos;
    void Start()
    {
        player = PlayerController.PlayerCachedTransform;
        startPos = transform.position;
    }

    void Update()
    {
        if (player == null) return;
        Vector2 dir = (player.position - transform.position).normalized;
        transform.Translate(dir * moveSpeed * Time.deltaTime, Space.World);
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
