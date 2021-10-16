using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Entity : MonoBehaviour
{

    public D_Entity entityData;
    public FiniteStateMachine stateMachine;
    public int facingDirection { get; private set; }

    public Rigidbody2D rigidbody2d { get; private set; }
    public Animator animator { get; private set; }
    public GameObject aliveGO { get; private set; }
    public BoxCollider2D boxCollider2d { get; private set; }

    //public HealthSystem healthSystem { get; private set; }
    public AnimationToStateMachine animationToStateMachine { get; private set; } //animator bu scrittin çocugunda oi. extra script yazdık

    private Vector2 movementVelocity;
    private int currentHealth;
    private int lastDamageDirection; //TODO: El at bi şuna

    private RaycastHit2D maxAgroRangeRaycast, minAgroRangeRaycast, meleeAttackRangeRaycast;

    [Header("[Checks]")]
    [SerializeField] private Transform wallCheck;
    [SerializeField] private Transform ledgeCheck;
    [SerializeField] private Transform playerCheck;

    public virtual void Start()
    {
        facingDirection = 1;

        aliveGO = transform.Find("Alive").gameObject;
        rigidbody2d = aliveGO.GetComponent<Rigidbody2D>();
        animator = aliveGO.GetComponent<Animator>();
        boxCollider2d = aliveGO.GetComponent<BoxCollider2D>();
        animationToStateMachine = aliveGO.GetComponent<AnimationToStateMachine>();
        //healthSystem = aliveGO.GetComponent<HealthSystem>();

        //healthSystem.SetHealthAmountMax(entityData.healthAmountMax, false);
        currentHealth = entityData.healthAmountMax;

        //healthSystem.OnDied += HealthSystem_OnDied;
        //healthSystem.OnDamaged += HealthSystem_OnDamaged;

        stateMachine = new FiniteStateMachine();
    }

    private void HealthSystem_OnDamaged(object sender, EventArgs e)
    {
        throw new NotImplementedException();
    }

    private void HealthSystem_OnDied(object sender, EventArgs e)
    {
        throw new NotImplementedException();
    }

    public virtual void Update()
    {
        stateMachine.currentState.LogicUpdate();
    }
    public virtual void FixedUpdate()
    {
        stateMachine.currentState.PhysicsUpdate();
    }
    public virtual void SetVelocity(float velocity)
    {
        movementVelocity.Set(facingDirection * velocity, rigidbody2d.velocity.y);
        rigidbody2d.velocity = movementVelocity;
    }
    public virtual void SetVelocity(float velocity,Vector2 angle,int direction)
    {
        angle.Normalize();
    }
    public virtual bool CheckWall()
    {
        //RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider2d.bounds.center, boxCollider2d.bounds.size, 0f, facingDirection * Vector2.right, entityData.wallCheckDistance, entityData.whatIsGround);
        //return raycastHit.collider != null;

        return Physics2D.Raycast(wallCheck.position, aliveGO.transform.right, entityData.wallCheckDistance, entityData.whatIsGround);
    }
    public virtual bool CheckLedge()
    {
        //RaycastHit2D raycastHit = Physics2D.BoxCast(new Vector2(facingDirection*(boxCollider2d.bounds.max.x + boxCollider2d.bounds.extents.x / 2), boxCollider2d.bounds.min.y + .1f), new Vector2(.1f, 0f), 0f, Vector2.down, entityData.ledgeCheckDistance, entityData.whatIsGround);
        //RaycastHit2D raycastHit = Physics2D.BoxCast( new Vector2(facingDirection * boxCollider2d.bounds.extents.x / 2 * 3, boxCollider2d.bounds.center.y),Vector2.one, 0f, Vector2.down, entityData.ledgeCheckDistance, entityData.whatIsGround);
        //return raycastHit.collider!=null;
        return Physics2D.Raycast(ledgeCheck.position, Vector2.down, entityData.ledgeCheckDistance, entityData.whatIsGround);
    }
    public virtual void Flip()
    {
        facingDirection *= -1;
        aliveGO.transform.Rotate(0f, 180f, 0f);

    }
    public virtual bool CheckPlayerInMinAgroRange()
    {
        minAgroRangeRaycast = Physics2D.Raycast(playerCheck.position, aliveGO.transform.right, entityData.minAgroRange, entityData.whatIsPlayer);
        
        return minAgroRangeRaycast.collider != null;
    }
    public virtual bool CheckPlayerInMaxAgroRange()
    {
        maxAgroRangeRaycast = Physics2D.Raycast(playerCheck.position, aliveGO.transform.right, entityData.maxAgroRange, entityData.whatIsPlayer);
        return maxAgroRangeRaycast.collider != null;
    }
    public virtual bool CheckPlayerInCloseAttackRange() //kılıçla melee atak
    {
        meleeAttackRangeRaycast = Physics2D.Raycast(playerCheck.position, aliveGO.transform.right, entityData.meleeAttackRange, entityData.whatIsPlayer);

        //raycastHit = Physics2D.BoxCast(playerCheck.position, Vector2.one / 2, 0f, aliveGO.transform.right, entityData.closeAttackActionDistance, entityData.whatIsPlayer);

        return meleeAttackRangeRaycast.collider != null;

    }
    public virtual void Damage(AttackDetails attackDetails)
    {
        currentHealth -= attackDetails.damageAmount;
        DamageHop(entityData.damageHopSpeed);

        if (attackDetails.position.x > aliveGO.transform.position.x) //oyuncu sağdan vuruyorsa
        {
            lastDamageDirection = -1;
        }
        else
        {
            lastDamageDirection = 1;
        }
    }
    public virtual void DamageHop(float velocity)
    {
        movementVelocity.Set(rigidbody2d.velocity.x, velocity);
        rigidbody2d.velocity = movementVelocity;
    }
    private Color GetColorFromRaycast(RaycastHit2D raycastHit)
    {
        if (raycastHit.collider != null)
        {
            return Color.green;
        }
        else
        {
            return Color.red;
        }
    }
    public virtual void OnDrawGizmos()
    {
        Debug.DrawRay(playerCheck.position,/*                             */facingDirection * (Vector3)Vector2.right * entityData.meleeAttackRange, GetColorFromRaycast(meleeAttackRangeRaycast));
        Debug.DrawRay(playerCheck.position + (Vector3)new Vector2(0f, .1f), facingDirection * (Vector3)Vector2.right * entityData.minAgroRange,/**/ GetColorFromRaycast(minAgroRangeRaycast));
        Debug.DrawRay(playerCheck.position + (Vector3)new Vector2(0f, .2f), facingDirection * (Vector3)Vector2.right * entityData.maxAgroRange,/**/ GetColorFromRaycast(maxAgroRangeRaycast));
    }
   
}




//Gizmos.DrawLine(wallCheck.position, wallCheck.position + (Vector3)(Vector2.right * facingDirection * entityData.wallCheckDistance));
//Gizmos.DrawLine(wallCheck.position, wallCheck.position + (Vector3)(Vector2.right * facingDirection * entityData.wallCheckDistance));

//Gizmos.DrawWireSphere(playerCheck.position + (Vector3)(aliveGO.transform.right * entityData.closeAttackActionDistance), 0.2f);
//Gizmos.DrawWireSphere(playerCheck.position + (Vector3)(aliveGO.transform.right * entityData.minAgroRange), 0.2f);
//Gizmos.DrawWireSphere(playerCheck.position + (Vector3)(aliveGO.transform.right * entityData.maxAgroRange), 0.2f);


//Color rayColor;
//if (raycastHit.collider != null) rayColor = Color.cyan;
//else rayColor = Color.red;
//Debug.DrawRay(boxCollider2d.bounds.center + new Vector3(0, boxCollider2d.bounds.extents.y), facingDirection * Vector2.right * (boxCollider2d.bounds.extents.x + entityData.wallCheckDistance), rayColor);
//Debug.DrawRay(boxCollider2d.bounds.center - new Vector3(0, (boxCollider2d.bounds.extents.y) / 2), facingDirection * Vector2.right * (boxCollider2d.bounds.extents.x + entityData.wallCheckDistance), rayColor);
//Debug.DrawRay(boxCollider2d.bounds.center + new Vector3((facingDirection * (boxCollider2d.bounds.extents.x + entityData.wallCheckDistance)), boxCollider2d.bounds.extents.y), Vector2.down * ((3 * boxCollider2d.bounds.extents.y) / 2), rayColor);




//RaycastHit2D raycastHitUp = Physics2D.BoxCast(boxCollider2d.bounds.center + new Vector3(0, (boxCollider2d.bounds.extents.y) / 4 * 3, 0), new Vector3(facingDirection * boxCollider2d.bounds.extents.x, .1f, 0), 0f, facingDirection * Vector2.right, entityData.wallCheckDistance, entityData.whatIsGround);
//RaycastHit2D raycastHitDown = Physics2D.BoxCast(boxCollider2d.bounds.center + new Vector3(0, (boxCollider2d.bounds.extents.y) / 4 * 2, 0), new Vector3(facingDirection * boxCollider2d.bounds.extents.x, .1f, 0), 0f, facingDirection * Vector2.right, entityData.wallCheckDistance, entityData.whatIsGround);

//if (raycastHitDown.collider != null && raycastHitUp.collider == null) //kafası collidera değmiyor, vücudu duvarda ama
//{
//    return true;
//}
//else return false;