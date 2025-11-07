using UnityEngine;

public class EnemyFollower : MonoBehaviour
{
    [Header("Target")]
    private Transform player;
    [Header("Move Speed")]
    [SerializeField] private float moveSpeed = 0.5f;
    [Header("Damage")]
    [SerializeField] private int damage = 1;

    [Header("PlayerCheck")]
    [SerializeField] private Transform playerCheck;
    [SerializeField] private Vector2 playerCheckBox = new Vector2(8.0f, 5.5f);
    [SerializeField] private LayerMask playerLayer;

    private bool canAttack = false;
    private Vector2 fixCheckBox;

    private Vector2 startPos;
    void Start()
    {
        player = PlayerController.PlayerTransform;
        startPos = transform.position;
        fixCheckBox = playerCheck.position;

    }
    //PlayerCheck
    void Update()
    {
        if (player == null) return;

        canAttack = Physics2D.OverlapBox(playerCheck.position, playerCheckBox, 0f, playerLayer);
        if(!canAttack) return;

        Vector2 dir = (player.position - transform.position).normalized;
        transform.Translate(dir * moveSpeed * Time.deltaTime, Space.World);
    }
    void LateUpdate()
    {
        playerCheck.position = fixCheckBox;
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
