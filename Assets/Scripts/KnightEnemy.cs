using UnityEngine;

public class KnightEnemy : EnemyBase
{
    [Header("Knight Specific")]
    [SerializeField] private GameObject weapon;
    [SerializeField] private float patrolPointRadius = 0.5f;

    private float waitTimer = 0f;
    private bool isWaiting = false;

    protected override void InitializeState()
    {
        if (patrolPoints.Length > 0)
        {
            agent.SetDestination(patrolPoints[currentPatrolIndex].position);
        }
    }

    protected override void UpdatePatrolState()
    {
        // Check for player
        if (CheckLineOfSight() && hasLineOfSight)
        {
            ChangeState(EnemyState.Chase);
            return;
        }

        // Patrol logic
        if (isWaiting)
        {
            waitTimer += Time.deltaTime;
            if (waitTimer >= waitTimeAtPoint)
            {
                isWaiting = false;
                waitTimer = 0f;
                GoToNextPatrolPoint();
            }
        }
        else if (agent.remainingDistance <= agent.stoppingDistance)
        {
            isWaiting = true;
        }
    }

    protected override void UpdateChaseState()
    {
        if (player == null)
        {
            ChangeState(EnemyState.ReturnToBase);
            return;
        }

        // Update destination to player
        agent.SetDestination(player.position);

        // Check if player is too far
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer > losePlayerRange)
        {
            ChangeState(EnemyState.ReturnToBase);
            return;
        }

        // Check if we can attack
        if (distanceToPlayer <= attackRange && CheckLineOfSight())
        {
            ChangeState(EnemyState.Attack);
            return;
        }

        // Check if we lost line of sight for too long
        if (!CheckLineOfSight())
        {
            stateTimer += Time.deltaTime;
            if (stateTimer > 3f) // Chase for 3 seconds without LOS
            {
                ChangeState(EnemyState.ReturnToBase);
            }
        }
        else
        {
            stateTimer = 0f;
        }
    }

    protected override void UpdateAttackState()
    {
        if (player == null || !player.gameObject.activeSelf)
        {
            ChangeState(EnemyState.ReturnToBase);
            return;
        }

        // Face the player
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        directionToPlayer.y = 0;
        if (directionToPlayer.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        // Attack logic
        attackTimer += Time.deltaTime;
        if (attackTimer >= attackCooldown)
        {
            if (player.gameObject.activeSelf)
            {
                PerformAttack();
                attackTimer = 0f;
            }
            ChangeState(EnemyState.ReturnToBase);
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer > losePlayerRange)
        {
            ChangeState(EnemyState.ReturnToBase);
        } else if (distanceToPlayer > attackRange * 1.2f)
        {
            ChangeState(EnemyState.Chase);
        }
    }

    protected override void UpdateReturnState()
    {
        // Check if we reached base
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            ChangeState(EnemyState.Patrol);
            return;
        }

        // Check if player appears again
        if (CheckLineOfSight() && hasLineOfSight)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            if (distanceToPlayer <= detectionRange)
            {
                ChangeState(EnemyState.Chase);
            }
        }
    }

    private void GoToNextPatrolPoint()
    {
        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        agent.SetDestination(patrolPoints[currentPatrolIndex].position);
    }

    private void PerformAttack()
    {
        // Trigger attack animation
        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }
    }
    public void OnAttackHit()
    {
        // Check if attack hits
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            if (distanceToPlayer <= attackRange)
            {
                // Raycast to ensure we have line of sight
                RaycastHit hit;
                Vector3 attackDirection = (player.position - transform.position).normalized;
                if (Physics.Raycast(transform.position + Vector3.up, attackDirection, out hit, attackRange))
                {
                    if (hit.transform.CompareTag("Player"))
                    {
                        // Damage player
                        PlayerHealth playerHealth = hit.transform.GetComponent<PlayerHealth>();
                        if (playerHealth != null)
                        {
                            playerHealth.TakeDamage(attackDamage);
                        }
                    }
                }
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        // Draw detection ranges
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, losePlayerRange);

        // Draw patrol points
        if (patrolPoints != null)
        {
            Gizmos.color = Color.green;
            foreach (Transform point in patrolPoints)
            {
                if (point != null)
                {
                    Gizmos.DrawSphere(point.position, patrolPointRadius);
                    Gizmos.DrawLine(transform.position, point.position);
                }
            }
        }

        // Draw line of sight if chasing
        if (currentState == EnemyState.Chase || currentState == EnemyState.Attack)
        {
            Gizmos.color = hasLineOfSight ? Color.green : Color.red;
            if (player != null)
            {
                Gizmos.DrawLine(transform.position + Vector3.up, player.position + Vector3.up);
            }
        }
    }
}