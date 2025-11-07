using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private HeadController headController;

    [Header("MoveSpeed")]
    [SerializeField] private float moveSpeed = 2.0f;

    [Header("HP")]
    [SerializeField] public int maxHp = 10;

    [Header("BulletPrefab")]
    [SerializeField] private Bullet bulletPrefab;

    [Header("PlayerHit Sprite")]
    [SerializeField] private Sprite playerHit;

    private Sprite originalSprite;

    public SpriteRenderer playerSr;
    public SpriteRenderer headSr;

    public bool isDead = false;
    public int currentHp;

    private Rigidbody2D rb;
    private Animator playerAnim;
    private Vector2 moveInput;
    private SpriteRenderer sprite;

    private static readonly int moveSpeedXHash = Animator.StringToHash("MoveX");
    private static readonly int moveSpeedYHash = Animator.StringToHash("MoveY");

    public static Transform PlayerTransform { get; private set; } 

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerAnim = GetComponent<Animator>();
        PlayerTransform = transform;
        currentHp = maxHp;
        sprite = GetComponent<SpriteRenderer>();

        originalSprite = sprite.sprite;

    }
    //이동
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
    //공격받음
    public void TakeDamage(int damage)
    {
        SoundManager.instance.playerHit.Play();

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
    //Enemy닿으면 무적
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            gameObject.layer = 10;
            OnDamaged(collision.transform.position);
        }
    }
    void OnDamaged(Vector2 targetPos)
    {
        if (playerAnim != null)
        {
            playerAnim.enabled = false;
        }
        sprite.color = new Color(1, 0, 0, 0.4f);
        if (playerHit != null)
        { 
            sprite.sprite = playerHit; 
        }
        if (headSr != null)
        {
            headSr.enabled = false;
        }
        if (headController != null)
        {
            headController.SetColor(new Color(1, 0, 0, 0.4f));
        }
        Invoke("OffDamaged", 0.5f);
    }
    void OffDamaged()
    {
        sprite.color = new Color(1, 1, 1, 1);
        sprite.sprite = originalSprite;
        if (headSr != null)
        {
            headSr.enabled = true;
        }
        if (headController != null)
        {
            headController.SetColor(Color.white);
        }
        if (playerAnim != null)
        {
            playerAnim.enabled = true;
        }
        gameObject.layer = 8;
    }
    //아이템 효과적용
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
    //Die 애니메이션 재생
    private IEnumerator DieDelay()
    {
        SoundManager.instance.playerDie.Play();
        playerAnim.updateMode = AnimatorUpdateMode.UnscaledTime;

        playerAnim.SetTrigger("Die");
        headSr.enabled = false;

        yield return new WaitForSecondsRealtime(0.1f);

        GameManager.instance.SetState(GameManager.GameState.GameOver);

        yield return new WaitForSecondsRealtime(1.5f);
        gameObject.SetActive(false);
        playerAnim.updateMode = AnimatorUpdateMode.Normal;

    }

}
