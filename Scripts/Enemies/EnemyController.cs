using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour,ICollider
{
    private enum State
    {
        Moving,
        Knockback,
        Dead,
    }
    private State currentState;

    [SerializeField]
    private float
        groundCheckDistance,
        movementSpeed,
        wallCheckDistance,
        knockbackDuration,
        knockbackStartTime;

    [SerializeField]
    private Transform
        groundCheck,
        wallCheck;

    [SerializeField] private LayerMask groundLayerMask;

    [SerializeField] private Vector2 knockbackSpeed;

    [SerializeField]
    private GameObject
       hitParticle,
       deathChunkParticle,
       deathBloodParticle;

    private int
        facingDirection , //sağa bakmak
        damageDirection;

    private Vector2 movement;

    private bool
        groundDetected,
        wallDetected;

    private GameObject alive;
    private Rigidbody2D aliveRigidbody2d;
    private Animator aliveAnim;

    private HealthSystem healthSystem;
    private MinionTypeSO minionType;

    private void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();
    }
    private void Start()
    {
        alive = transform.Find("alive").gameObject;
        aliveRigidbody2d = alive.GetComponent<Rigidbody2D>();
        aliveAnim = alive.GetComponent<Animator>();

        facingDirection = 1;

        healthSystem.OnDamaged += HealthSystem_OnDamaged;
        healthSystem.OnDied += HealthSystem_OnDied;
    }
    private void Update()
    {
        switch (currentState)
        {
            case State.Moving:
                UpdateMovingState();
                break;
            case State.Knockback:
                UpdateKnockbackState();
                break;
            case State.Dead:
                UpdateDeadState();
                break;
        }
    }

    #region Interface Implements
    //Interface objects
    private bool isGrounded;
    private bool onRightWall;
    private bool onLeftWall;
    private bool rightLedgeCheck;
    private bool leftLedgeCheck;

    //Interface Implements
    public void SetBoolIsGrounded(bool isGrounded)
    {
        this.isGrounded = isGrounded;
    }

    public void SetBoolOnRightWall(bool onRightWall)
    {
        this.onRightWall = onRightWall;
    }

    public void SetBoolOnLeftWall(bool onLeftWall)
    {
        this.onLeftWall = onLeftWall;
    }
    public void SetBoolOnRightLedgeCheck(bool rightLedgeCheck)
    {
        this.rightLedgeCheck = rightLedgeCheck;
    }
    public void SetBoolOnLeftLedgeCheck(bool leftLedgeCheck)
    {
        this.leftLedgeCheck = leftLedgeCheck;
    }
    #endregion

    #region Event Implements
    private void HealthSystem_OnDamaged(object sender, System.EventArgs e)
    {
        Instantiate(hitParticle, alive.transform.position, Quaternion.Euler(0.0f, 0.0f, Random.Range(0.0f, 360.0f)));
        SwitchState(State.Knockback);
    }
    private void HealthSystem_OnDied(object sender, System.EventArgs e)
    {
        Instantiate(hitParticle, alive.transform.position, Quaternion.Euler(0.0f, 0.0f, Random.Range(0.0f, 360.0f)));
        SwitchState(State.Dead);
    } 
    #endregion
   

    //--WALKING STATE---------------------------------------------------------------------------------

    private void EnterMovingState()
    {

    }
    private void UpdateMovingState()
    {
        //TODO: interfacele değiştir
        groundDetected = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundLayerMask);
        wallDetected = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance, groundLayerMask);

        if (!groundDetected || wallDetected)
        {
            Flip();
        }
        else
        {
            movement.Set(movementSpeed * facingDirection, aliveRigidbody2d.velocity.y);
            aliveRigidbody2d.velocity = movement;
        }

    }
    private void ExitMovingState()
    {

    }

    //--KNOCKBACK STATE---------------------------------------------------------------------------------

    private void EnterKnockbackState()
    {
        knockbackStartTime = Time.time;
        movement.Set(knockbackSpeed.x * damageDirection, knockbackSpeed.y);
        aliveRigidbody2d.velocity = movement;
        aliveAnim.SetBool("Knockback", true);
    }
    private void UpdateKnockbackState()
    {
        if (Time.time >= knockbackStartTime + knockbackDuration)
        {
            SwitchState(State.Moving);
        }
    }
    private void ExitKnockbackState()
    {
        aliveAnim.SetBool("Knockback", false);
    }

    //--DEAD STATE---------------------------------------------------------------------------------

    private void EnterDeadState()
    {
        Instantiate(deathChunkParticle, alive.transform.position, deathChunkParticle.transform.rotation);
        Instantiate(deathBloodParticle, alive.transform.position, deathBloodParticle.transform.rotation);
        Destroy(gameObject);
    }
    private void UpdateDeadState()
    {

    }
    private void ExitDeadState()
    {

    }

    //--OTHER FUNCTIONS--------------------------------------------------------------------------------

    private void Damage(float[] attackDetails)
    {

        Instantiate(hitParticle, alive.transform.position, Quaternion.Euler(0.0f, 0.0f, Random.Range(0.0f, 360.0f)));

        if (attackDetails[1] > alive.transform.position.x)
        {
            damageDirection = -1;
        }
        else
        {
            damageDirection = 1;
        }

        //Hit particle

        if (healthSystem.GetHealthAmount() > 0.0f)
        {
            SwitchState(State.Knockback);
        }
        else if (healthSystem.GetHealthAmount() <= 0.0f)
        {
            SwitchState(State.Dead);
        }
    }
    private void Flip()
    {
        facingDirection *= -1;
        alive.transform.Rotate(0f, 180f, 0f);
    }

    private void SwitchState(State state)
    {
        switch (currentState)
        {
            case State.Moving:
                ExitMovingState();
                break;
            case State.Knockback:
                ExitKnockbackState();
                break;
            case State.Dead:
                ExitDeadState();
                break;
        }
        switch (state)
        {
            case State.Moving:
                EnterMovingState();
                break;
            case State.Knockback:
                EnterKnockbackState();
                break;
            case State.Dead:
                EnterDeadState();
                break;
        }
        currentState = state;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector2(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector2(wallCheck.position.x + wallCheckDistance, wallCheck.position.y));
    }

   
}
