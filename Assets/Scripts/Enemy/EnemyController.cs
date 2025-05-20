using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Enemy Settings")]
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float moveTime = 3f;
    [SerializeField] private float waitTime = 1f;
    [SerializeField] private bool isFrogEnemy;

    private bool isMovingRight;
    private Rigidbody2D rb;
    private Animator animator;
    private float moveCount;
    private float waitCount;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        DeparentPoints();
        isMovingRight = true;
        moveCount = moveTime;
        waitCount = waitTime;
    }

    void Update()
    {
        // handle move and wait logic
        if (moveCount > 0)
        {
            moveCount -= Time.deltaTime;
            MoveEnemy();

            if (moveCount <= 0)
            {
                waitCount = Random.Range(waitTime * 0.75f, waitTime * 1.25f); // Randomize wait time
            }
        }
        else if (waitCount > 0)
        {
            waitCount -= Time.deltaTime;

            if (waitCount <= 0)
            {
                moveCount = Random.Range(moveTime * 0.75f, moveTime * 1.75f); // Randomize move time
            }
        }

        // handle animation if the enemy is frog
        if (isFrogEnemy)
        {
            animator.SetFloat("moveSpeed", moveCount);
        }
    }

    private void MoveEnemy()
    {
        if (isMovingRight)
        {
            rb.MovePosition(Vector2.MoveTowards(rb.position, pointB.position, moveSpeed * Time.deltaTime));
            spriteRenderer.flipX = false; // Face right

            if (Vector2.Distance(rb.position, pointB.position) < 0.4f)
            {
                isMovingRight = false;
            }
        }
        else
        {
            rb.MovePosition(Vector2.MoveTowards(rb.position, pointA.position, moveSpeed * Time.deltaTime));
            spriteRenderer.flipX = true; // Face left

            if (Vector2.Distance(rb.position, pointA.position) < 0.4f)
            {
                isMovingRight = true;
            }
        }
    }

    private void DeparentPoints()
    {
        pointA.parent = null;
        pointB.parent = null;
    }
}
