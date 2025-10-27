using UnityEngine;
using UnityEngine.Rendering;

public class PlayerController : MonoBehaviour
{
    [Header("¿Ãµø")]
    [SerializeField] private float moveSpeed = 2.0f;
    

    private Rigidbody2D rb;
    private Animator playerAnim;
    private Vector2 moveInput;

    private static readonly int moveSpeedXHash = Animator.StringToHash("MoveX");
    private static readonly int moveSpeedYHash = Animator.StringToHash("MoveY");


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerAnim = GetComponent<Animator>();
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

}
