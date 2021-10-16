using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerTwo : MonoBehaviour, ICollider
{

    //In GameObject
    private Rigidbody2D rigidbody2d;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private BoxCollider2D boxCollider2d;

    [SerializeField] private LayerMask whatIsGround;

    #region Slope

    [SerializeField] private PhysicsMaterial2D noFriction;
    [SerializeField] private PhysicsMaterial2D fullFriction;

    [SerializeField] private float slopeCheckDistance;
    [SerializeField] private float maxSlopeAngle;

    private float slopeDownAngle;
    private float slopeSideAngle;
    private float lastSlopeAngle;

    private bool isOnSlope;
    private bool canWalkOnSlope;

    private Vector2 boxCollderSize;
    private Vector2 slopeNormalPerp;

    #endregion

    #region Dash

    //variables
    private bool isDashing = false;


    private float dashTimeLeft;
    private float lastImageXpos;
    private float lastDash = -1f; //son dash attýgýmýzdan geçen süre

    //consts
    private float dashTime = .2f;
    private float dashSpeed = 20f;
    private float distanceBetweenImages = .1f;
    private float dashCooldown = 2f;

    #endregion

    #region Movement

    //vars
    [SerializeField] private bool canMove = true;

    private float xInput;   //girilen inputun yönünü belirlemek için
    private bool isWalking;
    private bool isFacingRight = true;
    private int facingDirection = 1;
    private bool canFlip = true;

    //consts
    [SerializeField] private float movementSpeed = 7f;

    //private float turnTimer;
    //private float turnTimerSet = .3f;

    #endregion

    #region Jump

    //consts
    private const float jumpForce = 6f;
    private const float fallMultiplier = 5f;
    private const float lowJumpMultiplier = 6f;
    private const float wallJumpMultiplier = 6f;



    [SerializeField] private int amountOfJumpsLeft;
    private int amountOfJumps = 2;

    [SerializeField] private float jumpTimer;
    private float jumpTimerSet = .3f;//.15f;


    [SerializeField] private bool canJump;

    #endregion

    #region Wall-e

    //vars
    [SerializeField] private bool canGrabWall = true;
    private bool wallGrabbing;
    private bool wallSliding;
    private bool canJumpFromRight;
    private bool canJumpFromLeft;
    private bool wallJumping = false; //tam jump yaptýktan sonra oyuncunun karakter hakimiyetinin azaldýðý süre için

    //consts
    private float wallSlideSpeed = 2f;
    private float movementForceInAir = 50f;
    private float airDragMultiplier = .95f; //hava direnci

    private float wallJumpTimer = Mathf.NegativeInfinity;
    private const float wallJumpTimerSet = 0.15f;

    //private float variableJumpHeightMultiplier = .5f;

    //private float wallGrabTimer = .2f; // tutunurken enerji harcama timer'ý
    //private float wallGrabTimerMax = .2f;

    //Ledge

    private bool isClimbingLedge = false;

    private Vector2 ledgePos;
    private Vector2 ledgePos1; //duvardan yukarý týrmanýrken
    private Vector2 ledgePos2; //yukarý çýkýnca saða doðru yürürken


    private float wallCheckDistance = .3f; //rectangle collider'da da tanýmlý

    private float ledgeClimbXOffset1 = 0.3f;
    private float ledgeClimbYOffset1 = 0f;

    private float ledgeClimbXOffset2 = 0.5f;
    private float ledgeClimbYOffset2 = 2f;

    #endregion

    //Injections

    #region Interfaces

    //Interface objects
    [SerializeField] private bool isGrounded;
    [SerializeField] private bool onRightWall;
    [SerializeField] private bool onLeftWall;
    [SerializeField] private bool rightLedgeCheck;
    [SerializeField] private bool leftLedgeCheck;

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

    #region Dependencies
    public Vector3 GetPosition()
    {
        return transform.position;
    }
    public void SetPosition(Vector3 position)
    {
        this.transform.position = position;
    }


    #endregion

    //Functions

    #region Awake and Start Functions

    private void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        boxCollider2d = GetComponent<BoxCollider2D>();
        boxCollderSize = boxCollider2d.size;


    }

    private void Start()
    {

        amountOfJumpsLeft = amountOfJumps;
        ledgePos = new Vector2(transform.localPosition.x + .4f, transform.localPosition.y);


    }



    #endregion

    #region Update Functions
    private void Update()
    {
        Timers();
        UpdateAnimations();
        CheckInputs();
        CheckMovementDirection();
        CheckIfCanGrab();
        CheckLedgeClimb();

        if (Input.GetKeyDown(KeyCode.P))
        {
            rigidbody2d.AddForce(new Vector2(0, 10), ForceMode2D.Impulse);
        }

    }



    private void Timers()
    {
        if (jumpTimer > 0)
        {
            jumpTimer -= Time.deltaTime;
            if (jumpTimer <= 0 && amountOfJumpsLeft > 0)
            {
                canJump = true;
            }
        }
        if (wallJumpTimer > 0)
        {
            wallJumpTimer -= Time.deltaTime;
            if (wallJumpTimer <= 0)
            {
                wallJumping = false;
            }
        }

    }
    private void UpdateAnimations()
    {
        animator.SetBool("isWalking", isWalking);
        animator.SetBool("isGrounded", isGrounded);
        animator.SetFloat("yVelocity", rigidbody2d.velocity.y);
        animator.SetBool("isWallSliding", wallSliding);
        animator.SetBool("isGrabbing", wallGrabbing);
    }
    //**********************************
    private void CheckInputs()
    {
        //yürüme
        xInput = Input.GetAxisRaw("Horizontal");

        //dash atma
        if (CanDash())
        {
            AttemptToDash();
        }

        //zýplama
        if (canJump && Input.GetKeyDown(KeyCode.C))
        {
            Jump();
        }

        //duvara tutunma
        if (canGrabWall)
        {
            //duvara tutunmaya basýyor

            wallGrabbing = Input.GetKey(KeyCode.Z);

            //if (Input.GetKey(KeyCode.Z))
            //{
            //    wallGrabbing = true;
            //}

            //duvara tutunmayý býraktý
            if (Input.GetKeyUp(KeyCode.Z))
            {
                wallGrabbing = false;
            }

        }

        //yerdeyse
        if (isGrounded)
        {
            //duvara deðmiyorsa
            if (!CheckOnWall())
            {
                wallGrabbing = false;
            }

            if ((!IsJumping() && slopeDownAngle <= maxSlopeAngle))
            {
                canJump = true;
                amountOfJumpsLeft = amountOfJumps;
            }

            //düþme hýzý 0'landýðý an
            if (rigidbody2d.velocity.y <= 0) // bu olmadan ekstra jump bugý oluyi
            {
                canJumpFromRight = true; //TODO: Bu 2liyi bi metod içine al
                canJumpFromLeft = true;
                wallSliding = false;
            }

            //yürüyor
            if (Mathf.Abs(rigidbody2d.velocity.x) >= .01f && xInput != 0)
            {
                isWalking = true;
            }

            //duruyor
            else if (xInput == 0)
            {
                isWalking = false;
            }


        }

        //yere deðmiyorsa
        else if (!isGrounded)
        {

            //sað duvarda
            if (onRightWall)
            {

                //sað duvarda aþaðý kayýyor
                if (xInput == 1 && !wallGrabbing)
                {
                    if (rigidbody2d.velocity.y < -2f && !isClimbingLedge)
                    {
                        wallSliding = true;
                    }
                }
                else
                {
                    wallSliding = false;
                }

                if (canJumpFromRight)
                {
                    //sað duvardan zýpladýysa
                    if (Input.GetKeyDown(KeyCode.C))
                    {
                        rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, wallJumpMultiplier);

                        canJumpFromRight = false;
                        canJumpFromLeft = true;

                        wallJumping = true;
                        wallJumpTimer = wallJumpTimerSet;
                    }
                }
            }

            //sol duvarda
            else if (onLeftWall)
            {

                if (xInput == -1 && rigidbody2d.velocity.y < -2f && !isClimbingLedge && !wallGrabbing) //duvarda sola týklýyorsa
                {
                    wallSliding = true;
                }
                else
                {
                    wallSliding = false;
                }


                if (canJumpFromLeft)
                {
                    if (Input.GetKeyDown(KeyCode.C)) //zýpladýysa
                    {
                        rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, wallJumpMultiplier);
                        canJumpFromRight = true;
                        canJumpFromLeft = false;

                        wallJumping = true;
                        wallJumpTimer = wallJumpTimerSet;
                    }

                }
            }



            //havada
            else if (!CheckOnWall()) //havadaysa
            {
                wallSliding = false;
                if (amountOfJumpsLeft == 0) canJump = false;
            }
        }

    }

    private bool CanDash()
    {
        return Input.GetKeyDown(KeyCode.X) && Time.time >= (lastDash + dashCooldown);
    }



    //**********************************
    private void CheckMovementDirection()
    {
        if (isFacingRight && xInput < 0 && canFlip)
        {
            Flip();
        }
        else if (!isFacingRight && xInput > 0 && canFlip)
        {
            Flip();
        }
    }
    private void CheckIfCanGrab()
    {
        //duvara tutunabilmenin aktive olmasý
        if (CheckOnWall())
        {
            canGrabWall = true;
        }
        else
        {
            canGrabWall = false;
        }
    }

    private void CheckLedgeClimb()
    {
        ledgePos = isFacingRight ? new Vector2(transform.localPosition.x + .4f, transform.localPosition.y) : new Vector2(transform.localPosition.x + -.4f, transform.localPosition.y);
        if (CheckTouchingLedge() && !isClimbingLedge)
        {
            if (rightLedgeCheck && isFacingRight)
            {
                isClimbingLedge = true;
                ledgePos1 = new Vector2(Mathf.Floor(ledgePos.x + wallCheckDistance) - ledgeClimbXOffset1, Mathf.Floor(ledgePos.y) + ledgeClimbYOffset1);
                ledgePos2 = new Vector2(Mathf.Floor(ledgePos.x + wallCheckDistance) + ledgeClimbXOffset2, Mathf.Floor(ledgePos.y) + ledgeClimbYOffset2);
            }
            else if (leftLedgeCheck && !isFacingRight)
            {
                isClimbingLedge = true;
                ledgePos1 = new Vector2(Mathf.Ceil(ledgePos.x - wallCheckDistance) + ledgeClimbXOffset1, Mathf.Floor(ledgePos.y) + ledgeClimbYOffset1);
                ledgePos2 = new Vector2(Mathf.Ceil(ledgePos.x - wallCheckDistance) - ledgeClimbXOffset2, Mathf.Floor(ledgePos.y) + ledgeClimbYOffset2);
            }



            animator.SetBool("isClimbingLedge", isClimbingLedge);
        }
        if (isClimbingLedge)
        {
            canMove = false;
            DisableFlip();
            transform.position = ledgePos1; //animasyon baþlar transformun ilk framei
        }
    }
    private void Jump()
    {
        if (amountOfJumpsLeft == 0)
        {
            canJump = false;
        }
        else
        {
            amountOfJumpsLeft--;
            jumpTimer = jumpTimerSet;
        }


        //ilk zýpladýðýnda
        if (amountOfJumpsLeft == amountOfJumps - 1)
        {
            rigidbody2d.velocity = new Vector2(1.1f * rigidbody2d.velocity.x, jumpForce);
            // rigidbody2d.AddForce(new Vector2(1.1f * rigidbody2d.velocity.x, jumpForce), ForceMode2D.Impulse);
        }

        //birden çok zýplama hakkýn var. Ýlk zýplamadan sonra daha az yukarý yükseliyor.
        else if (amountOfJumpsLeft < amountOfJumps - 1 && amountOfJumps > 1)
        {
            rigidbody2d.velocity = new Vector2(1.1f * rigidbody2d.velocity.x, jumpForce / 1.2f);
        }

        else
        {
            rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, jumpForce);
        }



    }

    //---------------Update Stipe Methods--------------------------------------------------------
    private void AttemptToDash() //dash atýlýnca
    {
        isDashing = true;
        dashTimeLeft = dashTime;
        lastDash = Time.time;

        PlayerAfterImagePool.Instance.GetFromPool();
        lastImageXpos = transform.position.x;
    }
    public void FinishLedgeClimb() //ledge climb'ý tamamlayýnca
    {
        isClimbingLedge = false;
        transform.position = ledgePos2;
        canMove = true;
        EnableFlip();
        animator.SetBool("isClimbingLedge", isClimbingLedge);
    }

    #endregion

    #region FixedUpdate Functions
    private void FixedUpdate()
    {
        ApplyMovement();
        ApplyJumping();
        ApplyWallGrabbing();
        ApplyDash();
        SlopeCheck();
    }



    private void SlopeCheck()
    {
        //bastýðýmýz zeminin ortasý tam
        Vector2 checkPos = transform.position - (Vector3)(new Vector2(0.0f, boxCollderSize.y / 2));

        SlopeCheckHorizontal(checkPos);
        SlopeCheckVertical(checkPos);
    }

    private void SlopeCheckHorizontal(Vector2 checkPos)
    {
        RaycastHit2D slopeHitFront = Physics2D.Raycast(checkPos, transform.right, slopeCheckDistance, whatIsGround);
        RaycastHit2D slopeHitBack = Physics2D.Raycast(checkPos, -transform.right, slopeCheckDistance, whatIsGround);

        if (slopeHitFront && !slopeHitBack)
        {
            isOnSlope = true;

            slopeSideAngle = Vector2.Angle(slopeHitFront.normal, Vector2.up);

        }
        else if (slopeHitBack && !slopeHitFront)
        {
            isOnSlope = true;

            slopeSideAngle = Vector2.Angle(slopeHitBack.normal, Vector2.up);
        }
        //2 zikzaklý dað arasýnda sýkýþtýðý için bunu koydum
        else if (slopeHitBack && slopeHitFront)
        {
            slopeSideAngle = 0.0f;
            isOnSlope = false;

        }
        else
        {
            slopeSideAngle = 0.0f;
            isOnSlope = false;
        }

    }

    private void SlopeCheckVertical(Vector2 checkPos)
    {
        RaycastHit2D hit = Physics2D.Raycast(checkPos, Vector2.down, slopeCheckDistance, whatIsGround);

        if (hit)
        {

            slopeNormalPerp = Vector2.Perpendicular(hit.normal).normalized; //yerin eðimi

            slopeDownAngle = Vector2.Angle(hit.normal, Vector2.up);

            if (slopeDownAngle != lastSlopeAngle)
            {
                isOnSlope = true;
            }

            lastSlopeAngle = slopeDownAngle;

            Debug.DrawRay(hit.point, slopeNormalPerp, Color.cyan); //yerin eðimi
            Debug.DrawRay(hit.point, hit.normal, Color.white); //zemine dik

        }

        if (slopeDownAngle > maxSlopeAngle || slopeSideAngle > maxSlopeAngle)
        {
            canWalkOnSlope = false;
        }
        else
        {
            canWalkOnSlope = true;
        }

        if (isOnSlope && canWalkOnSlope && xInput == 0.0f)
        {
            rigidbody2d.sharedMaterial = fullFriction;
        }
        else
        {
            rigidbody2d.sharedMaterial = noFriction;
        }
    }

    private void ApplyMovement()
    {
        //hareket edebiliyo
        if (canMove)
        {
            //yerde hareket hýzý
            if (isGrounded)
            {
                //zýplamýyorken etkili
                if (!IsJumping())
                {
                    //düz yolda
                    if (!isOnSlope)
                    {
                        rigidbody2d.velocity = new Vector2(movementSpeed * xInput, rigidbody2d.velocity.y);
                    }
                    //eðimli yolda
                    else if (isOnSlope & canWalkOnSlope)
                    {
                        rigidbody2d.velocity = new Vector2(movementSpeed * slopeNormalPerp.x * -xInput,
                            movementSpeed * slopeNormalPerp.y * -xInput);
                    }
                }
            }

            //yerde deðilken hareket hýzý
            else if (!isGrounded) // (&& !wallSliding && !wallGrabbing) yazmadým çünkü canMove'larý false oluyo burda
            {
                //duvardan atlarken
                if (wallJumping)
                {
                    rigidbody2d.velocity = Vector2.Lerp(rigidbody2d.velocity,
                        new Vector2(xInput * movementSpeed * 2, rigidbody2d.velocity.y), wallJumpTimerSet * Time.deltaTime);
                }

                //duvardan atlamazken
                else
                {
                    //havada hareket tuþuna basarken
                    if (xInput != 0)
                    {
                        Vector2 forceToAdd = new Vector2(movementForceInAir * xInput, 0f);
                        rigidbody2d.AddForce(forceToAdd);

                        //HÝLEMODU: Aþaðýdakini bir üst if döngüsüne çýkarýrsak zýplama hilesi normale döner

                        //havada yürüme hýzýný sýnýrlama
                        if (Mathf.Abs(rigidbody2d.velocity.x) > movementSpeed)
                        {
                            rigidbody2d.velocity = new Vector2(movementSpeed * facingDirection, rigidbody2d.velocity.y);

                        }
                    }

                    //havada tuþa basmazken
                    else if (xInput == 0)  //havada durdugunda yürüme hýzýný azaltma //jumtaki double jump yapýnca carpandan kaynaklanan hile degistirince duzelir
                    {
                        rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x * airDragMultiplier, rigidbody2d.velocity.y);
                    }
                }

            }

        }

        else if (wallSliding)
        {
            if (rigidbody2d.velocity.y < -wallSlideSpeed) //kayma hýzýný sýnýrlama
            {
                rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, -wallSlideSpeed);
            }
        }
    }
    private void ApplyJumping()
    {
        //aþaðý doðru düþüyorsa
        if (rigidbody2d.velocity.y < 0)
        {
            //duvarda kaymýyorsa
            if (!wallSliding)
            {
                rigidbody2d.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
            }
            //duvarda kayýyorsa
            else if (wallSliding)
            {
                rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, -wallSlideSpeed);
            }
        }
        //zýpladýysa ama zýplamaya basmayý býraktýysa hýzlý düþmesi
        else if (rigidbody2d.velocity.y > 0 && !Input.GetKey(KeyCode.C))
        {
            rigidbody2d.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }

    }
    private void ApplyWallGrabbing()
    {
        if (wallGrabbing && canGrabWall)
        {
            rigidbody2d.gravityScale = 0;
            canMove = false;
            canFlip = false;
            rigidbody2d.velocity = new Vector2(0, 0);
        }
        else
        {
            canMove = true;
            canFlip = true;
            rigidbody2d.gravityScale = 1;
        }
    }
    private void ApplyDash()
    {
        if (isDashing)
        {
            if (dashTimeLeft > 0)
            {
                canMove = false;
                canFlip = false;

                rigidbody2d.velocity = new Vector2(dashSpeed * facingDirection, 0);
                dashTimeLeft -= Time.deltaTime;

                if (Mathf.Abs(transform.position.x - lastImageXpos) > distanceBetweenImages)
                {
                    PlayerAfterImagePool.Instance.GetFromPool();
                    lastImageXpos = transform.position.x;
                }
            }
            if (dashTimeLeft <= 0)
            {
                isDashing = false;
                canMove = true;
                canFlip = true;
            }

        }
    }


    #endregion



    #region Check Functions

    private void Flip()
    {
        if (!wallSliding && canFlip && !isClimbingLedge)
        {
            isFacingRight = !isFacingRight;
            transform.Rotate(0f, 180f, 0f);
            facingDirection = isFacingRight ? 1 : -1;
        }

    }
    public void EnableFlip()
    {
        canFlip = true;
    }
    public void DisableFlip()
    {
        canFlip = false;
    }
    public int GetFacingDirection()
    {
        return (int)facingDirection;
    }
    private bool CheckOnWall()
    {
        if (onRightWall || onLeftWall) //TODO: interface'ten çekilebilir yap
        {
            return true;
        }
        else return false;
    }
    private bool CheckTouchingLedge()
    {
        if ((leftLedgeCheck || rightLedgeCheck)) { return true; }
        else return false;
    }


    private bool IsJumping()
    {
        if (jumpTimer <= 0)
        {
            return false;
        }
        else return true;
    }

    #endregion




}
