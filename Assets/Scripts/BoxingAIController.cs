using UnityEngine;

public class BoxingAIController : MonoBehaviour
{
    public enum AIState { Idle, Approach, Retreat, Attack, Defend } // States of the AI
    private AIState currentState = AIState.Idle;

    [Header("References")]
    public Transform player;          // Player's position
    public Animator animator;         // Animator for controlling animations

    [Header("Movement Settings")]
    public float moveSpeed = 2f;      // Movement speed
    public float rotationSpeed = 5f; // Rotation speed towards the player
    public float approachDistance = 3f; // Distance to maintain when approaching
    public float retreatDistance = 1.5f; // Distance to maintain when retreating

    [Header("Attack Settings")]
    public float attackInterval = 2f; // Time between attacks
    public float punchRange = 2f;     // Maximum range for attacks
    public float hookChance = 0.3f;   // Chance to perform a hook instead of a punch

    [Header("Defense Settings")]
    public float blockChance = 0.5f;  // Chance to block an attack
    public float blockCooldown = 3f; // Time between blocks

    private float stateTimer = 0f;    // Timer to control state changes
    private float attackTimer = 0f;
    private float blockTimer = 0f;

    void Update()
    {
        stateTimer += Time.deltaTime;
        attackTimer += Time.deltaTime;
        blockTimer += Time.deltaTime;

        RotateTowardsPlayer(); // Always face the player

        switch (currentState)
        {
            case AIState.Idle:
                IdleBehavior();
                break;
            case AIState.Approach:
                ApproachBehavior();
                break;
            case AIState.Retreat:
                RetreatBehavior();
                break;
            case AIState.Attack:
                AttackBehavior();
                break;
            case AIState.Defend:
                DefendBehavior();
                break;
        }

        EvaluateStateTransition(); // Decide when to switch states
    }

    void RotateTowardsPlayer()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    void IdleBehavior()
    {
        animator.SetFloat("Speed", 0); // Play idle animation
    }

    void ApproachBehavior()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer > approachDistance)
        {
            // Move towards the player
            Vector3 moveDirection = (player.position - transform.position).normalized;
            transform.position += moveDirection * moveSpeed * Time.deltaTime;
            animator.SetFloat("Speed", moveSpeed);
        }
        else
        {
            animator.SetFloat("Speed", 0);
        }
    }

    void RetreatBehavior()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer < retreatDistance)
        {
            // Move away from the player
            Vector3 moveDirection = (transform.position - player.position).normalized;
            transform.position += moveDirection * moveSpeed * Time.deltaTime;
            animator.SetFloat("Speed", -moveSpeed);
        }
        else
        {
            animator.SetFloat("Speed", 0);
        }
    }

    void AttackBehavior()
    {
        if (attackTimer >= attackInterval)
        {
            // Choose between punches and hooks
            string attack = Random.value > hookChance ? "PunchRight" : "HookLeft";
            animator.SetTrigger(attack);
            attackTimer = 0f;
        }
    }

    void DefendBehavior()
    {
        if (blockTimer >= blockCooldown)
        {
            // Choose a block direction
            string block = Random.value > 0.5f ? "BlockLeft" : "BlockRight";
            animator.SetTrigger(block);
            blockTimer = 0f;
        }
    }

    void EvaluateStateTransition()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer > approachDistance)
        {
            currentState = AIState.Approach; // Approach if too far
        }
        else if (distanceToPlayer < retreatDistance)
        {
            currentState = AIState.Retreat; // Retreat if too close
        }
        else if (Random.value < 0.5f)
        {
            currentState = AIState.Attack; // Attack with a 50% chance
        }
        else if (Random.value < blockChance)
        {
            currentState = AIState.Defend; // Defend with a set chance
        }
        else
        {
            currentState = AIState.Idle; // Default to idle
        }
    }
}
