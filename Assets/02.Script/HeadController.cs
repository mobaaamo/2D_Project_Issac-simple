using System.Reflection;
using UnityEngine;
using UnityEngine.WSA;

public class HeadController : MonoBehaviour
{
    [SerializeField] Vector2 offset = new Vector2(0, 0.45f);

    [Header("Head Sprites")]
    [SerializeField] private Sprite headFront;
    [SerializeField] private Sprite headLeft;
    [SerializeField] private Sprite headRight;
    [SerializeField] private Sprite headBack;

    [Header("Shooting")]
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private float shootInterval = 0.25f;

    private float shootTimer;
    private Vector2 lastDir = Vector2.down;
    private SpriteRenderer sr;

    private int damage = 1;
    private float bulletSpeed = 3f;
    private float bulletRange = 1f;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();

    }
    private void Start()
    {
        PoolManager.Instance.CreatPool(bulletPrefab, 20);
    }

    void LateUpdate()
    {
        transform.localPosition = offset;

        float moveX = 0;
        float moveY = 0;


        if (Input.GetKey(KeyCode.LeftArrow)) moveX = -1; 
        if (Input.GetKey(KeyCode.RightArrow)) moveX = 1; 
        if (Input.GetKey(KeyCode.UpArrow)) moveY = 1; 
        if (Input.GetKey(KeyCode.DownArrow)) moveY = -1; 

        if (moveX != 0 || moveY != 0) 
        {
            lastDir = new Vector2(moveX, moveY);

            shootTimer += Time.deltaTime;
            if (shootTimer >= shootInterval)
            {
                Shot();
                shootTimer = 0f;
            }
        }

        if (moveX < 0) sr.sprite = headLeft;

        else if (moveX > 0) sr.sprite = headRight;

        else if (moveY > 0) sr.sprite = headBack;

        else sr.sprite = headFront;
    }
    void Shot()
    {
        
        Bullet bullet = PoolManager.Instance.GetFromPool(bulletPrefab);
        bullet.SetStats(damage, bulletSpeed, bulletRange);
        bullet.Init(shootPoint.position, lastDir);


    }
    public void AddDamage(int amount)
    {
        damage += amount;
    }
    public void AddAttackSpeed(float amout)
    {
        bulletSpeed+= amout;
    }
    public void AddRange(float amount)
    {
        bulletRange+= amount;
    }
    public void SetColor(Color color)
    {
        if (sr == null)
            sr = GetComponent<SpriteRenderer>();

        sr.color = color;
    }
}