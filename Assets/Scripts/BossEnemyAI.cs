using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using System.Collections;

public class BossEnemyAI : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform player;
    [SerializeField] private LayerMask whatIsGround, whatIsPlayer;
    [SerializeField] private float health;
    [SerializeField] private float currentHealth;
    [SerializeField] private Slider slider;
    [SerializeField] private PlayerHealthManager playerHp;
    private Animator anim;

    [Header("Patrolling")]
    [SerializeField] private Vector3 walkPoint;
    [SerializeField] private bool walkPointSet;
    [SerializeField] private float walkPointRange;

    [Header("Attacking")]
    [SerializeField] private float timeBetweenAttacks;
    [SerializeField] private bool alreadyAttacked;

    [Header("States")]
    [SerializeField] private float sightRange, attackRange;
    [SerializeField] private bool playerInAttackRange, isDead;
    public bool playerInSightRange;

    [Header("Jump Attack")]
    [SerializeField] private float jumpAttackRange = 15f;
    [SerializeField] private float minJumpDistance = 8f;
    [SerializeField] private float jumpAttackCooldown = 6f; // Reduced from 8f
    [SerializeField] private float jumpHeight = 4f;
    [SerializeField] private float jumpDuration = 1.0f; // Was 1.5f
    [SerializeField] private float aoeRadius = 20f;
    [SerializeField] private float aoeDamage = 30f;
    [SerializeField] private float windupTime = 0.7f; // Reduced from 1f
    [SerializeField] private Color indicatorColor = Color.red;
    [SerializeField] private float fallGravityMultiplier = 2f; // New parameter

    [Header("AOE Indicator")]
    [SerializeField] private float indicatorHeightAboveGround = 0.2f;
    [SerializeField] private float raycastHeight = 10f;
    [SerializeField] private float raycastMaxDistance = 15f;
    private bool isJumpAttacking = false;
    private bool canJumpAttack = true;
    private Vector3 jumpTarget;
    private LineRenderer circleRenderer;

    public bool playerInside;

    private void Awake()
    {
        playerInside = false;
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        currentHealth = health;
        isDead = false;
        transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);

        // Setup line renderer for AOE indicator
        circleRenderer = gameObject.AddComponent<LineRenderer>();
        circleRenderer.startWidth = 0.5f;
        circleRenderer.endWidth = 0.5f;
        circleRenderer.material = new Material(Shader.Find("Unlit/Color")) { color = indicatorColor };
        circleRenderer.enabled = false;
        circleRenderer.loop = true;
    }

    private void Update()
    {
        if (isDead) return;

        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        UpdateHealthBar();

        if (!isJumpAttacking)
        {
            if (!playerInSightRange && !playerInAttackRange)
                Patrolling();

            if (playerInSightRange && !playerInAttackRange)
                ChasePlayer();

            if (playerInSightRange && playerInAttackRange)
                AttackPlayer();

            // Check for jump attack opportunity
            if (canJumpAttack &&
                Vector3.Distance(transform.position, player.position) <= jumpAttackRange &&
                Vector3.Distance(transform.position, player.position) >= minJumpDistance)
            {
                StartJumpAttack();
            }
        }
    }

    void LateUpdate()
    {
        if(!isJumpAttacking)
            transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
    }

    private void UpdateHealthBar()
    {
        slider.value = currentHealth / health;
        if (slider.gameObject.activeSelf)
        {
            Vector3 direction = player.transform.position - slider.transform.position;
            direction.y = 0;
            slider.transform.rotation = Quaternion.LookRotation(direction);
        }
    }

    private void StartJumpAttack()
    {
        isJumpAttacking = true;
        canJumpAttack = false;
        agent.isStopped = true;

        // Set jump target to player's current position
        jumpTarget = player.position;

        // Draw AOE indicator
        DrawCircle(jumpTarget, aoeRadius);

        // Trigger windup animation
        anim.SetBool("run", false);
        anim.SetBool("attack", false);
        anim.SetBool("jumpAttack", true);

        // Face the target during windup
        Vector3 directionToTarget = (jumpTarget - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(new Vector3(directionToTarget.x, 0, directionToTarget.z));

        // Start jump after windup
        Invoke(nameof(PerformJump), windupTime);

        // Reset after cooldown
        Invoke(nameof(ResetJumpAttack), jumpAttackCooldown);
    }

    private void DrawCircle(Vector3 center, float radius)
    {
        circleRenderer.enabled = true;
        circleRenderer.useWorldSpace = true;
        circleRenderer.startWidth = 0.3f;
        circleRenderer.endWidth = 0.3f;

        int segments = Mathf.Clamp((int)(radius * 6), 20, 100); // More segments for larger circles
        circleRenderer.positionCount = segments;

        for (int i = 0; i < segments; i++)
        {
            float angle = (float)i / segments * Mathf.PI * 2;
            Vector3 dir = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;
            Vector3 point = center + dir;

            // Enhanced ground detection
            if (Physics.Raycast(point + Vector3.up * raycastHeight, Vector3.down,
                out RaycastHit hit, raycastMaxDistance, whatIsGround))
            {
                point = hit.point + Vector3.up * indicatorHeightAboveGround;
            }

            circleRenderer.SetPosition(i, point);
        }

        // Close the loop
        if (segments > 0)
            circleRenderer.SetPosition(segments - 1, circleRenderer.GetPosition(0));
    }

    private void PerformJump()
    {
        StartCoroutine(JumpMovement());
    }

    private IEnumerator JumpMovement()
    {
        Vector3 startPos = transform.position;
        float elapsedTime = 0f;

        // Disable collisions and agent
        Collider bossCollider = GetComponent<Collider>();
        bool wasColliderEnabled = bossCollider.enabled;
        bossCollider.enabled = false;
        agent.enabled = false;

        while (elapsedTime < jumpDuration)
        {
            float normalizedTime = elapsedTime / jumpDuration;
            float height = Mathf.Sin(normalizedTime * Mathf.PI) * jumpHeight;

            // Apply faster falling when coming down
            if (normalizedTime > 0.5f) // After peak of jump
            {
                height *= (1 + (fallGravityMultiplier - 1) * (normalizedTime - 0.5f) * 2);
            }

            transform.position = Vector3.Lerp(startPos, jumpTarget, normalizedTime) + Vector3.up * height;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Clean up
        circleRenderer.enabled = false;
        transform.position = jumpTarget;
        bossCollider.enabled = wasColliderEnabled;
        agent.enabled = true;
        PerformAOEAttack();
        isJumpAttacking = false;
        anim.SetBool("jumpAttack", false);
        agent.isStopped = false;
        circleRenderer.enabled = false;
    }

    private void PerformAOEAttack()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, aoeRadius, whatIsPlayer);
        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                playerHp.TakeDamage((int)aoeDamage);
            }
        }

        // You could add impact effects here
    }

    private void ResetJumpAttack()
    {
        canJumpAttack = true;
    }

    private void Patrolling()
    {
        agent.speed = 3;
        if (!walkPointSet)
            SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }

    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }

    private void ChasePlayer()
    {
        agent.speed = 14;
        Vector3 targetPos = new Vector3(player.position.x, transform.position.y, player.position.z);
        agent.SetDestination(targetPos);
    }

    private void AttackPlayer()
    {
        agent.speed = 5;
        agent.SetDestination(player.position);
        transform.LookAt(player);
        if (!alreadyAttacked)
        {
            alreadyAttacked = true;
            anim.SetBool("attack", true);
            Invoke(nameof(ResetAttacks), timeBetweenAttacks);
        }
    }
    public void AttackPlayerAnimation()
    {
        if (playerHp.weaponInside)
        {
            if (playerHp.currentHealth > 25)
                playerHp.TakeDamage(25);

            else if (playerHp.currentHealth <= 25)
                playerHp.currentHealth = 0;
        }
    }
    private void ResetAttacks()
    {
        alreadyAttacked = false;
        anim.SetBool("attack", false);
        agent.speed = 12;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (!slider.gameObject.activeSelf)
            slider.gameObject.SetActive(true);

        if (currentHealth <= 0)
        {
            agent.speed = 0;
            isDead = true;
            currentHealth = 0;
            anim.SetBool("hasDied", true);
            playerInAttackRange = false;
            playerInSightRange = false;
            slider.gameObject.SetActive(false);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, jumpAttackRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, minJumpDistance);
    }
private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
            slider.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            slider.gameObject.SetActive(false);
        }
    }
}
