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
    private float lastDash = -1f; //son dash att�g�m�zdan ge�en s�re

    //consts
    private float dashTime = .2f;
    private float dashSpeed = 20f;
    private float distanceBetweenImages = .1f;
    private float dashCooldown = 2f;

    #endregion

    #region Movement

    //vars
    [SerializeField] private bool canMove = true;

    private float xInput;   //girilen inputun y�n�n� belirlemek i�in
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
    private bool wallJumping = false; //tam jump yapt�ktan sonra oyuncunun karakter hakimiyetinin azald��� s�re i�in

    //consts
    private float wallSlideSpeed = 2f;
    private float movementForceInAir = 50f;
    private float airDragMultiplier = .95f; //hava direnci

    private float wallJumpTimer = Mathf.NegativeInfinity;
    private const float wallJumpTimerSet = 0.15f;

    //private float variableJumpHeightMultiplier = .5f;

    //private float wallGrabTimer = .2f; // tutunurken enerji harcama timer'�
    //private float wallGrabTimerMax = .2f;

    //Ledge

    private bool isClimbingLedge = false;

    private Vector2 ledgePos;
    private Vector2 ledgePos1; //duvardan yukar� t�rman�rken
    private Vector2 ledgePos2; //yukar� ��k�nca sa�a do�ru y�r�rken


    private float wallCheckDistance = .3f; //rectangle collider'da da tan�ml�

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
        //y�r�me
        xInput = Input.GetAxisRaw("Horizontal");

        //dash atma
        if (CanDash())
        {
            AttemptToDash();
        }

        //z�plama
        if (canJump && Input.GetKeyDown(KeyCode.C))
        {
            Jump();
        }

        //duvara tutunma
        if (canGrabWall)
        {
            //duvara tutunmaya bas�yor

            wallGrabbing = Input.GetKey(KeyCode.Z);

            //if (Input.GetKey(KeyCode.Z))
            //{
            //    wallGrabbing = true;
            //}

            //duvara tutunmay� b�rakt�
            if (Input.GetKeyUp(KeyCode.Z))
            {
                wallGrabbing = false;
            }

        }

        //yerdeyse
        if (isGrounded)
        {
            //duvara de�miyorsa
            if (!CheckOnWall())
            {
                wallGrabbing = false;
            }

            if ((!IsJumping() && slopeDownAngle <= maxSlopeAngle))
            {
                canJump = true;
                amountOfJumpsLeft = amountOfJumps;
            }

            //d��me h�z� 0'land��� an
            if (rigidbody2d.velocity.y <= 0) // bu olmadan ekstra jump bug� oluyi
            {
                canJumpFromRight = true; //TODO: Bu 2liyi bi metod i�ine al
                canJumpFromLeft = true;
                wallSliding = false;
            }

            //y�r�yor
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

        //yere de�miyorsa
        else if (!isGrounded)
        {

            //sa� duvarda
            if (onRightWall)
            {

                //sa� duvarda a�a�� kay�yor
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
                    //sa� duvardan z�plad�ysa
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

                if (xInput == -1 && rigidbody2d.velocity.y < -2f && !isClimbingLedge && !wallGrabbing) //duvarda sola t�kl�yorsa
                {
                    wallSliding = true;
                }
                else
                {
                    wallSliding = false;
                }


                if (canJumpFromLeft)
                {
                    if (Input.GetKeyDown(KeyCode.C)) //z�plad�ysa
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
        //duvara tutunabilmenin aktive olmas�
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
            transform.position = ledgePos1; //animasyon ba�lar transformun ilk framei
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


        //ilk z�plad���nda
        if (amountOfJumpsLeft == amountOfJumps - 1)
        {
            rigidbody2d.velocity = new Vector2(1.1f * rigidbody2d.velocity.x, jumpForce);
            // rigidbody2d.AddForce(new Vector2(1.1f * rigidbody2d.velocity.x, jumpForce), ForceMode2D.Impulse);
        }

        //birden �ok z�plama hakk�n var. �lk z�plamadan sonra daha az yukar� y�kseliyor.
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
    private void AttemptToDash() //dash at�l�nca
    {
        isDashing = true;
        dashTimeLeft = dashTime;
        lastDash = Time.time;

        PlayerAfterImagePool.Instance.GetFromPool();
        lastImageXpos = transform.position.x;
    }
    public void FinishLedgeClimb() //ledge climb'� tamamlay�nca
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
        //bast���m�z zeminin ortas� tam
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
        //2 zikzakl� da� aras�nda s�k��t��� i�in bunu koydum
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

            slopeNormalPerp = Vector2.Perpendicular(hit.normal).normalized; //yerin e�imi

            slopeDownAngle = Vector2.Angle(hit.normal, Vector2.up);

            if (slopeDownAngle != lastSlopeAngle)
            {
                isOnSlope = true;
            }

            lastSlopeAngle = slopeDownAngle;

            Debug.DrawRay(hit.point, slopeNormalPerp, Color.cyan); //yerin e�imi
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
            //yerde hareket h�z�
            if (isGrounded)
            {
                //z�plam�yorken etkili
                if (!IsJumping())
                {
                    //d�z yolda
                    if (!isOnSlope)
                    {
                        rigidbody2d.velocity = new Vector2(movementSpeed * xInput, rigidbody2d.velocity.y);
                    }
                    //e�imli yolda
                    else if (isOnSlope & canWalkOnSlope)
                    {
                        rigidbody2d.velocity = new Vector2(movementSpeed * slopeNormalPerp.x * -xInput,
                            movementSpeed * slopeNormalPerp.y * -xInput);
                    }
                }
            }

            //yerde de�ilken hareket h�z�
            else if (!isGrounded) // (&& !wallSliding && !wallGrabbing) yazmad�m ��nk� canMove'lar� false oluyo burda
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
                    //havada hareket tu�una basarken
                    if (xInput != 0)
                    {
                        Vector2 forceToAdd = new Vector2(movementForceInAir * xInput, 0f);
                        rigidbody2d.AddForce(forceToAdd);

                        //H�LEMODU: A�a��dakini bir �st if d�ng�s�ne ��kar�rsak z�plama hilesi normale d�ner

                        //havada y�r�me h�z�n� s�n�rlama
                        if (Mathf.Abs(rigidbody2d.velocity.x) > movementSpeed)
                        {
                            rigidbody2d.velocity = new Vector2(movementSpeed * facingDirection, rigidbody2d.velocity.y);

                        }
                    }

                    //havada tu�a basmazken
                    else if (xInput == 0)  //havada durdugunda y�r�me h�z�n� azaltma //jumtaki double jump yap�nca carpandan kaynaklanan hile degistirince duzelir
                    {
                        rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x * airDragMultiplier, rigidbody2d.velocity.y);
                    }
                }

            }

        }

        else if (wallSliding)
        {
            if (rigidbody2d.velocity.y < -wallSlideSpeed) //kayma h�z�n� s�n�rlama
            {
                rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, -wallSlideSpeed);
            }
        }
    }
    private void ApplyJumping()
    {
        //a�a�� do�ru d���yorsa
        if (rigidbody2d.velocity.y < 0)
        {
            //duvarda kaym�yorsa
            if (!wallSliding)
            {
                rigidbody2d.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
            }
            //duvarda kay�yorsa
            else if (wallSliding)
            {
                rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, -wallSlideSpeed);
            }
        }
        //z�plad�ysa ama z�plamaya basmay� b�rakt�ysa h�zl� d��mesi
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
        if (onRightWall || onLeftWall) //TODO: interface'ten �ekilebilir yap
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
