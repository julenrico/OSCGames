using UnityEngine;

public class BoxingAIController : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Animator animator;

    [Header("Movement Settings")]
    public float moveSpeed = 2f;
    public float rotationSpeed = 5f;
    public float approachDistance = 3f;
    public float retreatDistance = 1.5f;
    public float movementInterval = 4f;

    [Header("Attack Settings")]
    public float attackInterval = 3f;
    public float punchRange = 2f;

    private float movementTimer = 0f;
    private float attackTimer = 0f;
    private bool isApproaching = true;

    void Update()
    {
        movementTimer += Time.deltaTime;
        attackTimer += Time.deltaTime;

        RotateTowardsPlayer();

        if (movementTimer >= movementInterval)
        {
            ToggleMovement();
            movementTimer = 0f;
        }

        Move();

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= punchRange && attackTimer >= attackInterval)
        {
            PerformAttack();
            attackTimer = 0f;
        }
    }

    void RotateTowardsPlayer()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    void ToggleMovement()
    {
        isApproaching = !isApproaching;
    }

    void Move()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (isApproaching && distanceToPlayer > approachDistance)
        {
            Vector3 moveDirection = (player.position - transform.position).normalized;
            transform.position += moveDirection * moveSpeed * Time.deltaTime;
            animator.SetFloat("Speed", moveSpeed);
        }
        else if (!isApproaching && distanceToPlayer < retreatDistance)
        {
            Vector3 moveDirection = (transform.position - player.position).normalized;
            transform.position += moveDirection * moveSpeed * Time.deltaTime;
            animator.SetFloat("Speed", -moveSpeed);
        }
        else
        {
            animator.SetFloat("Speed", Mathf.MoveTowards(animator.GetFloat("Speed"), 0, Time.deltaTime * moveSpeed));
        }
    }

    void PerformAttack()
    {
        string punch = Random.value > 0.5f ? "PunchRight" : "PunchLeft";
        animator.SetTrigger(punch);

        Debug.Log($"Opponent performs {punch}");
    }
}
