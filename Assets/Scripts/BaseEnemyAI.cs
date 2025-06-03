using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class BaseEnemyAI : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform player;
    [SerializeField] private LayerMask whatIsGround, whatIsPlayer;
    [SerializeField] private float health;
    [SerializeField] private float currentHealth;
    [SerializeField] private Slider slider;
    [SerializeField] private GameObject spear;
    [SerializeField] private BoxCollider axe;
    [SerializeField] private PlayerHealthManager playerHp;
    [SerializeField] private SpearShooter shooter;
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

    [Header("Vision")]
    [SerializeField] private LayerMask layersToIgnore;
    [SerializeField] private float viewAngle = 160f;
    [SerializeField] private float eyeHeight = 1.7f;
    private bool playerIsVisible = false;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        currentHealth = health;
        isDead = false;
        transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
    }

    private void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        playerIsVisible = false;
        if (playerInSightRange)
        {
            playerIsVisible = CanSeePlayer();
        }

        slider.value = currentHealth / health;
        if (slider.gameObject.activeSelf)
        {
            Vector3 direction = player.transform.position - slider.transform.position;
            direction.y = 0;
            slider.transform.rotation = Quaternion.LookRotation(direction);
        }

        if (!playerIsVisible && !playerInSightRange &&!playerInAttackRange && !isDead)
            Patrolling();

        if (playerIsVisible && playerInSightRange && !playerInAttackRange && !isDead)
            ChasePlayer();

        if (playerIsVisible && playerInAttackRange && !isDead)
            AttackPlayer();
    }

    private bool CanSeePlayer()
    {
        Vector3 eyePosition = transform.position + Vector3.up * eyeHeight;
        Vector3 playerEyePosition = player.position + Vector3.up * eyeHeight;
        Vector3 directionToPlayer = (playerEyePosition - eyePosition).normalized;
        float distanceToPlayer = Vector3.Distance(eyePosition, playerEyePosition);

        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);
        if (angleToPlayer > viewAngle / 2f)
        {
            return false;
        }

        if (Physics.Raycast(eyePosition, directionToPlayer, out RaycastHit hit, distanceToPlayer, ~layersToIgnore))
        {
            if (hit.collider.CompareTag("Player"))
            {
                return true;
            }
            return false;
        }

        return false;
    }

    void LateUpdate()
    {
        transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
    }


    private void Patrolling()
    {
        agent.speed = 2;
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
        agent.speed = 12;
        Vector3 targetPos = new Vector3(player.position.x, transform.position.y, player.position.z);
        agent.SetDestination(targetPos);
    }

    public void AttackPlayerAnimation() 
    {
        if (playerHp.weaponInside)
        {
            if (playerHp.currentHealth > 15)
                playerHp.TakeDamage(15);

            else if (playerHp.currentHealth <= 15)
                playerHp.currentHealth = 0;
        }
    }
    private void AttackPlayer()
    {
        agent.speed = 3;
        agent.SetDestination(player.position);
        transform.LookAt(player);
        if (!alreadyAttacked)
        {
            alreadyAttacked = true;
            anim.SetBool("attack", true);
            Invoke(nameof(ResetAttacks), timeBetweenAttacks);
            
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
        if(!slider.gameObject.activeSelf)
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

    public void DestroyEnemy()
    {
        if (!shooter.playerHasSpear)
        {
            spear.transform.parent = null;
            spear.GetComponent<Rigidbody>().isKinematic = false;
            spear.GetComponent<CapsuleCollider>().enabled = true;
        }
        Destroy(gameObject);
    }
}