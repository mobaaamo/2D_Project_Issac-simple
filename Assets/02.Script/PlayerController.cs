using UnityEngine;
using UnityEngine.Rendering;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private HeadController headController;

    [Header("이동")]
    [SerializeField] private float moveSpeed = 2.0f;

    [Header("체력")]
    [SerializeField] private int maxHp = 10;

    [Header("총알 프리펩")]
    [SerializeField] private Bullet bulletPrefab;


    private int currentHp;

    private Rigidbody2D rb;
    private Animator playerAnim;
    private Vector2 moveInput;
    private SpriteRenderer spriter;

    private static readonly int moveSpeedXHash = Animator.StringToHash("MoveX");
    private static readonly int moveSpeedYHash = Animator.StringToHash("MoveY");

    public static Transform PlayerCachedTransform { get; private set; } //변수 이름 바꾸자 쉽게, Enemy 스크립트 전부 있으니 바꾸고 걔들도 바꿔
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerAnim = GetComponent<Animator>();
        PlayerCachedTransform = transform;
        currentHp = maxHp;
        spriter = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        moveInput = Vector2.zero;
        if (Input.GetKey(KeyCode.W)) moveInput.y = 1;
        else if(Input.GetKey(KeyCode.S)) moveInput.y = -1;

        if(Input.GetKey(KeyCode.D)) moveInput.x = 1;
        else if (Input.GetKey(KeyCode.A))moveInput.x = -1;

        moveInput = moveInput.normalized;

        playerAnim.SetFloat(moveSpeedXHash, moveInput.x);
        playerAnim.SetFloat(moveSpeedYHash, moveInput.y);

    }
    private void FixedUpdate()
    {
        PlayerMove();
    }
    void PlayerMove()
    {
        Vector2 newPos = rb.position + new Vector2(moveInput.x, moveInput.y) * moveSpeed * Time.deltaTime;
        rb.MovePosition(newPos);
    }
    public void TakeDamage(int damage)
    {
        currentHp -= damage;
        if(currentHp <= 0)
        {
            Die();
        }
    }
    void Die()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {

            OnDamaged(collision.transform.position);
        }
    }
    void OnDamaged(Vector2 targetPos)
    {
        gameObject.layer = 10;
        spriter.color = new Color(1, 0, 0, 0.4f);
        if(headController != null)
        {
            headController.SetColor(new Color(1, 0, 0, 0.4f));
        }
        Invoke("OffDamaged", 0.2f);
    }
    void OffDamaged()
    {
        spriter.color = new Color(1, 1, 1, 1);
        if(headController != null)
        {
            headController.SetColor(Color.white);
        }
        gameObject.layer = 0;
    }
    public void ApplyItemEffect(ItemDataSO item)
    {
        switch (item.type)
        {
            case ItemType.AttackUp:
                headController.AddDamage(item.power); break;


            case ItemType.RangeUp:
                headController.AddRange(item.power); break;

            case ItemType.SpeedUp:
                moveSpeed += item.power;break;

            case ItemType.AttackSpeedUp:
                headController.AddAttackSpeed(item.power); break;

            case ItemType.Heal:
                currentHp = Mathf.Min(maxHp, currentHp +item.power); break;

        }
    }
}
