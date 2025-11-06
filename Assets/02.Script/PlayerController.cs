using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private HeadController headController;

    [Header("MoveSpeed")]
    [SerializeField] private float moveSpeed = 2.0f;

    [Header("HP")]
    [SerializeField] public int maxHp = 10;

    [Header("BulletPrefab")]
    [SerializeField] private Bullet bulletPrefab;

    public SpriteRenderer playerSr;
    public SpriteRenderer headSr;

    public bool isDead = false;
    public int currentHp;

    private Rigidbody2D rb;
    private Animator playerAnim;
    private Vector2 moveInput;
    private SpriteRenderer spriter;

    private static readonly int moveSpeedXHash = Animator.StringToHash("MoveX");
    private static readonly int moveSpeedYHash = Animator.StringToHash("MoveY");

    public static Transform PlayerTransform { get; private set; } //변수 이름 바꾸자 쉽게, Enemy 스크립트 전부 있으니 바꾸고 걔들도 바꿔

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerAnim = GetComponent<Animator>();
        PlayerTransform = transform;
        currentHp = maxHp;
        spriter = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        moveInput = Vector2.zero;
        if (Input.GetKey(KeyCode.W)) moveInput.y = 1;
        else if (Input.GetKey(KeyCode.S)) moveInput.y = -1;

        if (Input.GetKey(KeyCode.D)) moveInput.x = 1;
        else if (Input.GetKey(KeyCode.A)) moveInput.x = -1;

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
        if (GameManager.instance.globalGameState == GameManager.GameState.GameClear)
            return;

        currentHp -= damage;
        if (currentHp <= 0)
        {
            isDead = true;
            StartCoroutine(DieDelay());
            return;
        }
        OnDamaged(transform.position);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        

        if (collision.collider.CompareTag("Enemy"))
        {
            gameObject.layer = 10;
            SoundManager.instance.playerHit.Play();
            OnDamaged(collision.transform.position);
        }
    }
    void OnDamaged(Vector2 targetPos)
    {
        spriter.color = new Color(1, 0, 0, 0.4f);
        if (headController != null)
        {
            headController.SetColor(new Color(1, 0, 0, 0.4f));
        }
        Invoke("OffDamaged", 0.5f);
    }
    void OffDamaged()
    {
        spriter.color = new Color(1, 1, 1, 1);
        if (headController != null)
        {
            headController.SetColor(Color.white);
        }
        gameObject.layer = 8;
    }
    public void ApplyItemEffect(ItemDataSO item)
    {
        switch (item.type)
        {
            case ItemType.AttackUp:
                headController.AddDamage((int)item.power); break;


            case ItemType.RangeUp:
                headController.AddRange(item.power); break;

            case ItemType.SpeedUp:
                moveSpeed += item.power; break;

            case ItemType.AttackSpeedUp:
                headController.AddAttackSpeed(item.power); break;

            case ItemType.Heal:
                currentHp = Mathf.Min(maxHp, currentHp + (int)item.power); break;

        }
    }
    private IEnumerator DieDelay()
    {
        SoundManager.instance.playerDie.Play();

        yield return null;
        GameManager.instance.SetState(GameManager.GameState.GameOver);

        playerSr.enabled = false;
        headSr.enabled = false;

        yield return new WaitForSecondsRealtime(0.1f);
        gameObject.SetActive(false);


    }
}
