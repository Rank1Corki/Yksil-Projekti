using UnityEngine;
using UnityEngine.AI;

public class EnemyAi : MonoBehaviour
{
    public Transform player;
    public float visionRange = 10f;
    public float visionAngle = 45f;
    public float attackRange = 2f;
    public float wanderRange = 10f;
    public float wanderTime = 5f;
    public float timeBetweenShots = 1f;
    public GameObject projectilePrefab;
    public Transform projectileSpawnPoint;
    public float projectileSpeed = 10f;

    private NavMeshAgent agent;
    private float wanderTimer;
    private bool playerInSight;
    private bool isAttacking;
    private float timeSinceLastShot;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        wanderTimer = wanderTime;
        timeSinceLastShot = timeBetweenShots;
    }

    void Update()
    {
        playerInSight = IsPlayerInSight();

        if (playerInSight)
        {
            Debug.Log("Player in sight. Chasing...");
            ChasePlayer();
        }
        else
        {
            Debug.Log("Player not in sight. Wandering...");
            Wander();
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= attackRange)
        {
            Debug.Log("Player in attack range. Attacking...");
            AttackPlayer();
        }

        timeSinceLastShot += Time.deltaTime;
    }

    private bool IsPlayerInSight()
    {
        Vector3 directionToPlayer = player.position - transform.position;
        float angle = Vector3.Angle(transform.forward, directionToPlayer);

        if (directionToPlayer.magnitude <= visionRange && angle <= visionAngle)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position + Vector3.up, directionToPlayer.normalized, out hit, visionRange))
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

    private void Wander()
    {
        if (agent.remainingDistance < 1f || !agent.hasPath)
        {
            wanderTimer -= Time.deltaTime;
            if (wanderTimer <= 0f)
            {
                SetNewWanderPoint();
                wanderTimer = wanderTime;
            }
        }
    }

    private void SetNewWanderPoint()
    {
        Vector3 randomDirection = Random.insideUnitSphere * wanderRange;
        randomDirection += transform.position;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, wanderRange, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }

    private void AttackPlayer()
    {
        if (timeSinceLastShot >= timeBetweenShots)
        {
            Shoot();
            timeSinceLastShot = 0f;
        }
    }

    private void Shoot()
    {
        GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = (player.position - projectileSpawnPoint.position).normalized * projectileSpeed;
        }
        else
        {
            Debug.LogError("Projectile prefab does not have a Rigidbody component!");
        }
    }

    public void TakeDamage(int damageAmount)
    {
        // Implement damage logic here (e.g., reduce health)
        Debug.Log("Enemy took damage: " + damageAmount);
    }
}
