using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyBase : MonoBehaviour
{
    [Header("State Settings")]
    [SerializeField] protected EnemyState currentState = EnemyState.Patrol;

    [Header("Detection Settings")]
    [SerializeField] protected float detectionRange = 10f;
    [SerializeField] protected float attackRange = 2f;
    [SerializeField] protected float losePlayerRange = 15f;
    [SerializeField] protected LayerMask detectionLayer = Physics.DefaultRaycastLayers;
    [SerializeField] protected LayerMask obstacleLayer;

    [Header("Patrol Settings")]
    [SerializeField] protected Transform[] patrolPoints;
    [SerializeField] protected float waitTimeAtPoint = 2f;
    [SerializeField] protected float patrolSpeed = 2f;
    [SerializeField] protected Vector3 defaultPointA;
    [SerializeField] protected Vector3 defaultPointB;

    [Header("Chase Settings")]
    [SerializeField] protected float chaseSpeed = 5f;
    [SerializeField] protected float rotationSpeed = 10f;

    [Header("Attack Settings")]
    [SerializeField] protected float attackDamage = 10f;
    [SerializeField] protected float attackCooldown = 1.5f;

    protected NavMeshAgent agent;
    protected Transform player;
    protected Animator animator;
    protected Vector3 basePosition;
    protected int currentPatrolIndex = 0;
    protected float stateTimer = 0f;
    protected float attackTimer = 0f;
    protected bool hasLineOfSight = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        basePosition = transform.position;

        if (patrolPoints.Length == 0)
        {
            CreateDefaultPatrolPoints();
        }

        InitializeState();
    }

    void Update()
    {
        UpdateState();
        UpdateAnimator();
    }

    protected virtual void UpdateState()
    {
        switch (currentState)
        {
            case EnemyState.Patrol:
                UpdatePatrolState();
                break;
            case EnemyState.Chase:
                UpdateChaseState();
                break;
            case EnemyState.Attack:
                UpdateAttackState();
                break;
            case EnemyState.ReturnToBase:
                UpdateReturnState();
                break;
        }
    }

    protected virtual void ChangeState(EnemyState newState)
    {
        OnStateExit(currentState);
        currentState = newState;
        OnStateEnter(newState);
    }

    protected virtual void OnStateEnter(EnemyState state)
    {
        switch (state)
        {
            case EnemyState.Patrol:
                agent.speed = patrolSpeed;
                agent.stoppingDistance = 0.1f;
                break;
            case EnemyState.Chase:
                agent.speed = chaseSpeed;
                agent.stoppingDistance = attackRange * 0.8f;
                break;
            case EnemyState.Attack:
                agent.isStopped = true;
                attackTimer = 0f;
                break;
            case EnemyState.ReturnToBase:
                agent.speed = patrolSpeed;
                agent.stoppingDistance = 0.5f;
                agent.SetDestination(basePosition);
                break;
        }
    }

    protected virtual void OnStateExit(EnemyState state)
    {
        if (state == EnemyState.Attack)
        {
            agent.isStopped = false;
        }
    }

    protected virtual void UpdateAnimator()
    {
        if (animator == null) return;

        animator.SetFloat("Speed", agent.velocity.magnitude);
        animator.SetBool("IsAttacking", currentState == EnemyState.Attack);
    }

    protected bool CheckLineOfSight()
    {
        if (player == null) return false;

        Vector3 directionToPlayer = (player.position - transform.position);
        float distanceToPlayer = directionToPlayer.magnitude;

        if (distanceToPlayer > detectionRange) return false;

        // Height offset for raycast (eye level)
        Vector3 rayOrigin = transform.position + Vector3.up * 1.5f;
        Vector3 playerTarget = player.position + Vector3.up * 1f;

        RaycastHit hit;
        if (Physics.Raycast(rayOrigin, (playerTarget - rayOrigin).normalized, out hit, detectionRange, detectionLayer))
        {
            if (hit.transform.CompareTag("Player"))
            {
                hasLineOfSight = true;
                return true;
            }
        }

        hasLineOfSight = false;
        return false;
    }

    protected virtual void CreateDefaultPatrolPoints()
    {
        patrolPoints = new Transform[2];

        GameObject pointA = new GameObject("PatrolPoint_A");
        pointA.transform.position = transform.position + defaultPointA;
        pointA.transform.parent = transform.parent;
        patrolPoints[0] = pointA.transform;

        GameObject pointB = new GameObject("PatrolPoint_B");
        pointB.transform.position = transform.position + defaultPointB;
        pointB.transform.parent = transform.parent;
        patrolPoints[1] = pointB.transform;
    }
    #region Abstract Methods
    protected virtual void InitializeState() { }
    protected virtual void UpdatePatrolState() { }
    protected virtual void UpdateChaseState() { }
    protected virtual void UpdateAttackState() { }
    protected virtual void UpdateReturnState() { }
    #endregion
}
