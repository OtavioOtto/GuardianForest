using System;
using System.Net.Http;
using UnityEngine;

public class SpearAddOns : MonoBehaviour
{
    public int damage;
    [SerializeField] private Rigidbody rb;
    private CapsuleCollider spearCollider;
    public SpearShooter shooter;
    public GameObject trail;
    public GameObject spearPivot;
    public bool isHit;
    public PlayerMovement player;
    public bool hasHit;
    void Start()
    {
        spearCollider = gameObject.GetComponent<CapsuleCollider>();
        isHit = false;
    }

    private void Update()
    {
        if(!shooter.playerHasSpear && !isHit && !shooter.isReturning)
            transform.forward = Vector3.Slerp(transform.forward, rb.linearVelocity.normalized, Time.deltaTime);
        if (shooter.playerHasSpear && !player.isMoving)
        {
            Quaternion targetRotation = Quaternion.Euler(Camera.main.transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        }
        if (shooter.playerHasSpear && player.isMoving)
            transform.rotation = new Quaternion(transform.rotation.x, Camera.main.transform.rotation.y, Camera.main.transform.rotation.z, Camera.main.transform.rotation.w);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") && !player.isNearAttack)
        {
            isHit = true;
            spearCollider.enabled = false;
            rb.isKinematic = true;
            spearPivot.transform.parent = other.transform;
            
            if (other.gameObject.GetComponent<EnemyHealtManager>() != null)
            {
                EnemyHealtManager enemy = other.gameObject.GetComponent<EnemyHealtManager>();
                enemy.TakeDamage(damage);
                trail.GetComponent<TrailRenderer>().enabled = false;
            }

            if (other.gameObject.GetComponent<BaseEnemyAI>() != null)
            {
                BaseEnemyAI enemy = other.gameObject.GetComponent<BaseEnemyAI>();
                enemy.TakeDamage(damage);
                trail.GetComponent<TrailRenderer>().enabled = false;
            }


        }

        if (!other.CompareTag("Player") && player.isNearAttack) 
        {
            hasHit = false;
            if (other.gameObject.GetComponent<EnemyHealtManager>() != null)
            {
                if (!hasHit)
                {
                    hasHit = true;
                    EnemyHealtManager enemy = other.gameObject.GetComponent<EnemyHealtManager>();
                    enemy.TakeDamage(damage);
                    trail.GetComponent<TrailRenderer>().enabled = false;
                }
            }

            if (other.gameObject.GetComponent<BaseEnemyAI>() != null)
            {
                if (!hasHit)
                {
                    hasHit = true;
                    BaseEnemyAI enemy = other.gameObject.GetComponent<BaseEnemyAI>();
                    enemy.TakeDamage(damage);
                    trail.GetComponent<TrailRenderer>().enabled = false;
                }
            }
        }
    }
}
