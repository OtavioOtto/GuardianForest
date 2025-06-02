using JetBrains.Annotations;
using UnityEngine;

public class CurupiraAnimations : MonoBehaviour
{
    [SerializeField] private PlayerMovement movement;
    [SerializeField] private SpearShooter shooter;
    [SerializeField] private Animator anim;
    [SerializeField] private CapsuleCollider spear;
    [SerializeField] private SpearAddOns addOns;
    private bool jump;
    private void Start()
    {
        jump = false;
    }
    void Update()
    { 

        if (movement.isMoving)
            anim.SetBool("andando", true);

        if (!movement.isMoving)
            anim.SetBool("andando", false);

        if (Input.GetMouseButton(0) && shooter.playerHasSpear && Time.timeScale != 0)
            anim.SetBool("lancou", true);

        if (Input.GetKeyDown(KeyCode.Q))
            anim.SetBool("atacou", true);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            jump = true;
            
        }

        if (movement.isGrounded)
        {
            jump = false;
            if(anim.GetBool("pulou"))
                anim.SetBool("pulou", false);
        }

        if (!movement.isGrounded && jump && !anim.GetBool("pulou"))
        {
            anim.SetBool("pulou", true);
        }

        if (shooter.isReturning)
            anim.SetBool("retornandoLanca", true);
        else
            anim.SetBool("retornandoLanca", false);

        if(shooter.playerHasSpear)
            anim.SetBool("temLanca", true);
        else
            anim.SetBool("temLanca", false);

        transform.position = transform.parent.position;
        transform.rotation = transform.parent.rotation;
    }

    public void ResetAnim() 
    {
        anim.SetBool("lancou", false);
        
    }

    public void NearAttack() 
    {
        movement.NearAttack();
    }

    public void ResetAnimNearAttack() 
    {
        anim.SetBool("atacou", false);
        spear.enabled = false;
        shooter.playerHasSpear = true;
        movement.isNearAttack = false;
        movement.canPlayerMove = true;
        addOns.hasHit = false;
    }

    public void ShootingMethod()
    {
        shooter.ShootingMethod();
    }
}
