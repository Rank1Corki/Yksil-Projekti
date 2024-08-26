using UnityEngine;
using UnityEngine.AI;

public class EnemyAi : MonoBehaviour
{
    public Transform player;
    public float visionConeAngle = 60f;
    public float visionDistance = 15f;
    public float attackDistance = 10f;
    public float wanderRadius = 5f;
    public float wanderInterval = 5f;
    public float attackRate = 1.5f;
    public float projectileForce = 500f;
    public GameObject enemyProjectilePrefab;
    public Transform projectileSpawnPoint;

    private NavMeshAgent agent;
    private Animator animator;
    private float nextAttackTime;
    private float nextWanderTime;
    private Vector3 wanderDestination;
    private Vector3 lastKnownPlayerPosition;
    private bool isPlayerInSight = false;
    private bool isDead = false;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();  // Reference to the Animator component
        nextWanderTime = Time.time;
        SetWanderDestination();
    }

    private void Update()
    {
        if (isDead) return;

        float speed = agent.velocity.magnitude;
        animator.SetFloat("Speed", speed);
        animator.SetBool("IsChasing", isPlayerInSight);

        if (IsPlayerInVisionCone())
        {
            isPlayerInSight = true;
            lastKnownPlayerPosition = player.position;

            if (Vector3.Distance(transform.position, player.position) <= attackDistance)
            {
                ShootProjectile();
            }
            else
            {
                ChasePlayer();
            }
        }
        else
        {
            if (isPlayerInSight)
            {
                ChasePlayerToLastKnownPosition();
            }
            else if (Time.time >= nextWanderTime)
            {
                Wander();
            }
        }
    }

    private bool IsPlayerInVisionCone()
    {
        Vector3 directionToPlayer = player.position - transform.position;
        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

        if (angleToPlayer < visionConeAngle / 2f && directionToPlayer.magnitude <= visionDistance)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, directionToPlayer, out hit, visionDistance))
            {
                if (hit.transform == player)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    private void ChasePlayerToLastKnownPosition()
    {
        if (Vector3.Distance(transform.position, lastKnownPlayerPosition) < 1f)
        {
            isPlayerInSight = false;
            nextWanderTime = Time.time + wanderInterval;
        }
        else
        {
            agent.SetDestination(lastKnownPlayerPosition);
        }
    }

    private void ShootProjectile()
    {
        if (Time.time >= nextAttackTime)
        {
            if (projectileSpawnPoint == null) return;

            GameObject projectile = Instantiate(enemyProjectilePrefab, projectileSpawnPoint.position, Quaternion.identity);
            Vector3 directionToPlayer = (player.position - projectileSpawnPoint.position).normalized;
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            rb.AddForce(directionToPlayer * projectileForce);
            projectile.transform.LookAt(player.position);

            nextAttackTime = Time.time + attackRate;
        }
    }

    private void Wander()
    {
        SetWanderDestination();
        agent.SetDestination(wanderDestination);
        nextWanderTime = Time.time + wanderInterval;
    }

    private void SetWanderDestination()
    {
        Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
        randomDirection += transform.position;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randomDirection, out navHit, wanderRadius, -1);
        wanderDestination = navHit.position;
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        // Logic to reduce health
        // if health <= 0, then:
        isDead = true;
        agent.isStopped = true;  // Stop moving
        animator.SetTrigger("Die");  // Trigger death animation

        // Optionally destroy or disable the enemy after the death animation
        Destroy(gameObject, 5f);  // Destroy after 5 seconds (adjust timing based on animation length)
    }
}
