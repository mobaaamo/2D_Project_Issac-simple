using UnityEngine;

public class EnemyFollower : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 0.5f;
    [SerializeField] private Transform target;

    private Vector2 startPos;
    void Start()
    {
        if(target == null)
        {
            target = GameObject.FindWithTag("Player")?.transform;
        }
        startPos = transform.position;
    }

    void Update()
    {
        Vector2 dir = (target.position - transform.position).normalized;
        transform.Translate(dir * moveSpeed * Time.deltaTime, Space.World);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {

        }
    }
}
