using System;
using System.Net.Http;
using UnityEngine;

public class SpearAddOns : MonoBehaviour
{
    public int damage;
    private Rigidbody rb;
    private CapsuleCollider spearCollider;
    private SpearShooter shooter;
    public GameObject trail;
    public bool isHit;
    private PlayerMovement player;
    public bool hasHit;
    private GameObject playerObj;
    void Start()
    {
        if(GetComponent<Rigidbody>() != null)
            rb = GetComponent<Rigidbody>();
        spearCollider = gameObject.GetComponent<CapsuleCollider>();
        isHit = false;
        playerObj = GameObject.Find("Player");
    }

    private void Update()
    {
        if(player == null)
            player = playerObj.GetComponent<PlayerMovement>();
        if(shooter == null)
            shooter = playerObj.GetComponent<SpearShooter>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") && !player.isNearAttack)
        {
            isHit = true;
            spearCollider.enabled = false;
            rb.isKinematic = true;
            transform.parent = other.transform;
            if (other.gameObject.GetComponent<BossEnemyAI>() != null)
            {
                BossEnemyAI enemy = other.gameObject.GetComponent<BossEnemyAI>();
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
            if (other.gameObject.GetComponent<BossEnemyAI>() != null)
            {
                if (!hasHit)
                {
                    hasHit = true;
                    BossEnemyAI enemy = other.gameObject.GetComponent<BossEnemyAI>();
                    enemy.TakeDamage(damage);
                }
            }

            if (other.gameObject.GetComponent<BaseEnemyAI>() != null)
            {
                if (!hasHit)
                {
                    hasHit = true;
                    BaseEnemyAI enemy = other.gameObject.GetComponent<BaseEnemyAI>();
                    enemy.TakeDamage(damage);
                }
            }
        }
    }
}
