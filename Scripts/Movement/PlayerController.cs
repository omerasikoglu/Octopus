//using System;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, ICollider, IShopCustomer
{
    /******** Dependencies ********/
    private Transform pfHitParticle; // for damage then knockback
    public ParticleSystem ps;

    //In GameObject
    private Rigidbody2D rigidbody2d;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    //In GameObject/Script
    private HealthSystem healthSystem;
    private EnergySystem energySystem;
    private CombatSystem combatSystem;
    private MaterialTintColor materialTintColor;

    //Mediator
    private VentsSystem ventsSystem;

    //No mono
    private Inventory inventory;
    private PlayerSkills playerSkills;

    //Data
    //List<PlayerSkills.SkillType> preSkillList;
    private SaveSO saveFile;


    #region Command Pattern
    //Command Pattern
    private Command buttonW;
    private Command buttonC;
    #endregion

    #region Vent
    //vent stuff
    private bool canPressVentButton;
    #endregion

    #region Your Knockback

    private float knockbackSpeedX = 10f;
    //private float knockbackDeathSpeedX = 15f;
    private float knockbackSpeedY = 2f;
    //private float knockbackDeathSpeedY = 4f;
    private float knockbackDuration = .1f;

    private bool isKnockbacking = false;
    private float knockbackStartTime;


    #endregion

    #region Dash

    //variables
    private bool canDash = true;
    private bool isDashing = false;


    private float dashTimeLeft;
    private float lastImageXpos;
    private float lastDash = -1f; //son dash attıgımızdan geçen süre

    //consts
    private float dashTime = .2f;
    private float dashSpeed = 20f;
    private float distanceBetweenImages = .1f;
    private float dashCooldown = 2f;

    #endregion

    #region Movement

    //vars
    private bool canMove = true;

    private float xInput;   //girilen inputun yönünü belirlemek için
    private bool isWalking;
    private bool isFacingRight = true;
    private int facingDirection = 1;
    private bool canFlip = true;

    //consts
    private float movementSpeed = 7f;

    //private float turnTimer;
    //private float turnTimerSet = .3f;

    #endregion

    #region Jump

    //consts
    private const float jumpForce = 6f;
    private const float fallMultiplier = 5f;
    private const float lowJumpMultiplier = 6f;
    private const float wallJumpMultiplier = 6f;



    private int amountOfJumpsLeft;
    private int amountOfJumps = 1;

    private float jumpTimer;
    private float jumpTimerSet = .15f;


    private bool canJump;

    #endregion

    #region EnergyAmounts

    private float doubleJumpEnergy = 1f;
    private float wallJumpEnergy = 1f;
    private float dashEnergy = 3f;
    private float ledgeClimbingEnergy = 2f;

    #endregion

    #region Wall-e

    //vars
    private bool canGrabWall = true;
    private bool wallGrabbing;
    private bool wallSliding;
    private bool canJumpFromRight;
    private bool canJumpFromLeft;
    private bool wallJumping = false; //tam jump yaptıktan sonra oyuncunun karakter hakimiyetinin azaldığı süre için

    //consts
    private float wallSlideSpeed = 2f;
    private float movementForceInAir = 50f;
    private float airDragMultiplier = .95f; //hava direnci

    private float wallJumpTimer = Mathf.NegativeInfinity;
    private const float wallJumpTimerSet = 0.15f;

    //private float variableJumpHeightMultiplier = .5f;

    //private float wallGrabTimer = .2f; // tutunurken enerji harcama timer'ı
    //private float wallGrabTimerMax = .2f;

    //Ledge

    private bool isClimbingLedge = false;

    private Vector2 ledgePos;
    private Vector2 ledgePos1; //duvardan yukarı tırmanırken
    private Vector2 ledgePos2; //yukarı çıkınca sağa doğru yürürken


    private float wallCheckDistance = .3f; //rectangle collider'da da tanımlı

    private float ledgeClimbXOffset1 = 0.3f;
    private float ledgeClimbYOffset1 = 0f;

    private float ledgeClimbXOffset2 = 0.5f;
    private float ledgeClimbYOffset2 = 2f;

    #endregion

    //Injections

    #region Interfaces

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
    public void ItemSold(Item item)
    {
        inventory.AddItem(item);

        //SaveFile işlemleri

        bool itemAlreadyInInventory = false;
        foreach (Item gottenItem in saveFile.gottenItemList)
        {
            if (gottenItem.itemType == item.itemType && item.IsStackable()) //hali hazırda bu item varsa sende
            {
                //gottenItem.amount += 1; 
                //artırmasını inventory'den yapıyor. Senin inventory'n ile birlikte 2sini de artırıyor
                itemAlreadyInInventory = true;
            }
        }
        if (!itemAlreadyInInventory) //ilk defa ekliyorsan
        {
            saveFile.gottenItemList.Add(item); ;
        }


        //Debug.Log("olmalı");
        // inventory.AddItem(new Item { itemType = itemType , amount =1 });
    }
    #endregion

    #region Observer Architecture 
    //observer pattern
    private void EnergySystem_OnNotEnoughEnergy(object sender, System.EventArgs e)
    {
        FlashYellow();
    }
    private void HealthSystem_OnDied(object sender, System.EventArgs e)
    {
        Debug.Log("ölmişsen");

    }
    private void HealthSystem_OnDamaged(object sender, HealthSystem.OnDamagedEventArgs e)
    {
        FlashRed();
        Instantiate(pfHitParticle, transform.position, Quaternion.Euler(0f, 0f, UnityEngine.Random.Range(0f, 360f)));
        Knockback(e.direction);

    }
    #endregion

    #region Dependencies

    public Inventory GetInventory()
    {
        return inventory;
    }
    public PlayerSkills GetPlayerSkills()
    {
        return playerSkills;
    }
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
        //In GameObject/Script
        rigidbody2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        //In Script
        healthSystem = GetComponent<HealthSystem>();
        combatSystem = GetComponent<CombatSystem>(); //enerji sisteminin aşağısına koyma nesnesini oluşturduğu için karakter eciş bücüş kalıyo
        energySystem = GetComponent<EnergySystem>();
        materialTintColor = GetComponent<MaterialTintColor>();

        //Dependency Inversion

        inventory = new Inventory(UseItem, PassiveItem);

        //Datalarımız
        //Data
        pfHitParticle = Resources.Load<Transform>("pfHitParticle");
        SetStatsFirstTime();

        saveFile = Resources.Load<SaveSO>(typeof(SaveSO).Name);
        foreach (Item item in saveFile.gottenItemList)
        {
            inventory.AddItem(item);
        }

        playerSkills = new PlayerSkills(/*saveFile.unlockedSkillList*/); //önceden açık olan skill'leri setler

        foreach (PlayerSkills.SkillType skill in saveFile.unlockedSkillList)
        {
            playerSkills.UnlockSkill(skill);
            IncreaseSkills(skill);
        }

        //UI'daki yıldız sayını total yıldız sayına eşitleme TODO: kalan yıldız sayın yap
        //playerSkills.SetStarAmount(ResourceManager.Instance.GetResourceAmount(ResourceManager.Instance.GetResourceTypeSO(ResourceManager.ResourceType.Star)));

    }
    private void SetStatsFirstTime()
    {
        StatManager.Instance.SetStatAmount(StatManager.StatType.Damage, combatSystem.GetAttackDamageAmount());
        StatManager.Instance.SetStatAmount(StatManager.StatType.Health, healthSystem.GetHealthAmountMax());
        StatManager.Instance.SetStatAmount(StatManager.StatType.Energy, energySystem.GetEnergyAmountMax());
        StatManager.Instance.SetStatAmount(StatManager.StatType.Speed, (int)movementSpeed);
    }
    private void Start()
    {

        amountOfJumpsLeft = amountOfJumps;
        ledgePos = new Vector2(transform.localPosition.x + .4f, transform.localPosition.y);

        //events
        healthSystem.OnDamaged += HealthSystem_OnDamaged;
        healthSystem.OnDied += HealthSystem_OnDied;
        energySystem.OnNotEnoughEnergy += EnergySystem_OnNotEnoughEnergy;
        playerSkills.OnSkillUnlocked += PlayerSkills_OnSkillUnlocked;
        playerSkills.OnResetAllSkills += PlayerSkills_OnResetAllSkills;


        //HİLE and TEST

        //playerSkills.Hile(PlayerSkills.SkillType.Health);
        //buttonW = new VentIt(this);
        //buttonC = new Jump();
    }



    #endregion

    #region Update Functions
    private void Update()
    {

        Timers();
        UpdateAnimations();
        CheckInputs();
        CheckMovementDirection();
        CheckIfCanJump();
        CheckLedgeClimb();
        CheckKnockback();

        /*TEST*/

        if (Input.GetKeyDown(KeyCode.P)) //skiller listeye ekleniyor mu check'i
        {
            playerSkills.GetUnlockedSkills();
        }
    }
    private void Timers()
    {
        //if (turnTimer >= 0)
        //{
        //    turnTimer -= Time.deltaTime;
        //    if (turnTimer <= 0)
        //    {
        //        canMove = true;
        //        canFlip = true;
        //        canJump = true;
        //    }
        //}

        if (jumpTimer > 0)
        {
            jumpTimer -= Time.deltaTime;
            if (jumpTimer <= 0)
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
        if (Input.GetKeyDown(KeyCode.X) && canDash && CanUseDash())
        {
            if (energySystem.SpendEnergy(dashEnergy))
            {
                if (Time.time >= (lastDash + dashCooldown)) AttemptToDash();
            }
            else
            {
                ShakeCamera();
            }
        }

        //zıplama
        if (canJump && Input.GetKeyDown(KeyCode.C))
        {
            Jump();
        }

        if (rigidbody2d.velocity.y >= 0f)
        {
            rigidbody2d.sharedMaterial.friction = 0f;
        }
        else
        {
            rigidbody2d.sharedMaterial.friction = 0.5f;
        }

        //yerdeyse
        if (isGrounded)
        {
            canJump = true;
            amountOfJumpsLeft = amountOfJumps;

            //düşme hızı 0'landığı an
            if (rigidbody2d.velocity.y <= 0) // bu olmadan ekstra jump bugı oluyi
            {
                amountOfJumpsLeft = amountOfJumps;
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

        //yere değmiyorsa
        else if (!isGrounded)
        {

            //sağ duvarda
            if (onRightWall)
            {
                //duvara tutunmaya basıyor
                if (Input.GetKey(KeyCode.Z) && canGrabWall)
                {
                    wallGrabbing = true;
                }

                //duvara tutunmayı bıraktı
                if (Input.GetKeyUp(KeyCode.Z))
                {
                    wallGrabbing = false;
                }

                //sağ duvarda aşağı kayıyor
                if (xInput == 1)
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
                    //sağ duvardan zıpladıysa
                    if (Input.GetKeyDown(KeyCode.C) && energySystem.SpendEnergy(wallJumpEnergy))
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
                if (Input.GetKey(KeyCode.Z) && canGrabWall)
                {
                    wallGrabbing = true;
                }
                if (Input.GetKeyUp(KeyCode.Z))
                {
                    wallGrabbing = false;
                }

                if (xInput == -1 && rigidbody2d.velocity.y < -2f && !isClimbingLedge) //duvarda sola tıklıyorsa
                {
                    wallSliding = true;
                }
                else
                {
                    wallSliding = false;
                }


                if (canJumpFromLeft)
                {
                    if (Input.GetKeyDown(KeyCode.C) && energySystem.SpendEnergy(wallJumpEnergy)) //zıpladıysa
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
                canJump = false;

                if (canJump && Input.GetKeyDown(KeyCode.C)) //zıplıyor
                {
                    Jump();
                }
                //canJumpFromRight = true;
                //canJumpFromLeft = true;
            }
        }

        VentInputs();
    }
    private void VentInputs()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (canPressVentButton)
            {
                if (!IsInVent())
                {
                    ventsSystem.EnterIntoTheVentSystem();
                    VentEntered();
                }
                else
                {
                    VentExited();
                }
            }
            else
            {
                FlashRed();
            }

        }
        if ((Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) && IsInVent())
        {
            ventsSystem.MoveToRightVent();
        }
        if ((Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) && IsInVent())
        {
            ventsSystem.MoveToLeftVent();
        }
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
    private void CheckIfCanJump() //zıplayabiliyor mu
    {
        if (jumpTimer <= 0f && amountOfJumpsLeft > 0)
        {
            canJump = true;
        }
        else
        {
            canJump = false;
        }
    }
    private void CheckLedgeClimb()
    {
        ledgePos = isFacingRight ? new Vector2(transform.localPosition.x + .4f, transform.localPosition.y) : new Vector2(transform.localPosition.x + -.4f, transform.localPosition.y);
        if (CheckTouchingLedge() && !isClimbingLedge && energySystem.SpendEnergy(ledgeClimbingEnergy))
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
            transform.position = ledgePos1; //animasyon başlar transformun ilk framei
        }
    }

    //---------------Update Stipe Methods--------------------------------------------------------
    #region Update Stipe Functions
    private void AttemptToDash() //dash atılınca
    {
        isDashing = true;
        dashTimeLeft = dashTime;
        lastDash = Time.time;

        PlayerAfterImagePool.Instance.GetFromPool();
        lastImageXpos = transform.position.x;
    }
    public void FinishLedgeClimb() //ledge climb'ı tamamlayınca
    {
        isClimbingLedge = false;
        transform.position = ledgePos2;
        canMove = true;
        EnableFlip();
        animator.SetBool("isClimbingLedge", isClimbingLedge);
    }
    #endregion



    #endregion

    #region FixedUpdate Functions
    private void FixedUpdate()
    {
        ps.transform.position = new Vector3(transform.position.x, transform.position.y, 4);

        ApplyMovement();
        ApplyJumping();
        ApplyWallGrabbing();
        ApplyDash();
    }

    private void ApplyMovement()
    {
        if (canMove)
        {
            //yerde hareket hızı
            if (isGrounded && !isKnockbacking)
            {
                rigidbody2d.velocity = new Vector2(movementSpeed * xInput, rigidbody2d.velocity.y);
            }

            //yerde değilken hareket hızı
            else if (!isGrounded && !wallSliding && !isKnockbacking)
            {
                //duvardan atlarken
                if (wallJumping)
                {
                    rigidbody2d.velocity = Vector2.Lerp(rigidbody2d.velocity,
                        new Vector2(xInput * movementSpeed * 2, rigidbody2d.velocity.y), wallJumpTimerSet * Time.deltaTime);
                }

                //havada hareket etmeye çalışırken
                if (xInput != 0 && !wallJumping)
                {
                    Vector2 forceToAdd = new Vector2(movementForceInAir * xInput, 0f);
                    rigidbody2d.AddForce(forceToAdd);

                    //HİLEMODU: Aşağıdakini bir üst if döngüsüne çıkarırsak zıplama hilesi normale döner
                    if (Mathf.Abs(rigidbody2d.velocity.x) > movementSpeed) //havada yürüme hızını sınırlama
                    {
                        rigidbody2d.velocity = new Vector2(movementSpeed * facingDirection, rigidbody2d.velocity.y);
                    }
                }

                //havada tuşa basmazken
                else if (xInput == 0)  //havada durdugunda yürüme hızını azaltma //jumtaki double jump yapınca carpandan kaynaklanan hile degistirince duzelir
                {
                    rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x * airDragMultiplier, rigidbody2d.velocity.y);
                }

            }

        }

        if (wallSliding && !isKnockbacking)
        {
            if (rigidbody2d.velocity.y < -wallSlideSpeed) //kayma hızını sınırlama
            {
                rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, -wallSlideSpeed);
            }
        }
    }
    private void ApplyJumping()
    {
        if (rigidbody2d.velocity.y < 0)      //düşüyorsa
        {
            if (!wallSliding)
            {
                rigidbody2d.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
            }
            else if (wallSliding)
            {
                rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, -wallSlideSpeed);
            }
        }
        else if (rigidbody2d.velocity.y > 0 && !Input.GetKey(KeyCode.C))      //zıpladıysa
        {
            rigidbody2d.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }
    private void ApplyWallGrabbing()
    {
        if (wallGrabbing)
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

    #region Triggers
    private void OnTriggerEnter2D(Collider2D collider)
    {
        ItemWorld itemWorld = collider.GetComponent<ItemWorld>();
        if (itemWorld != null)
        {
            inventory.AddItem(itemWorld.GetItem());
            itemWorld.DestroySelf();
        }

        StarWorld starWorld = collider.GetComponent<StarWorld>();
        if (starWorld != null)
        {
            ResourceManager.Instance.SetStarAchieved(starWorld.GetStarType().resourceType);
            starWorld.DestroySelf();
        }
        Vent vent = collider.GetComponent<Vent>();
        if (vent != null)
        {
            vent.EnableVent(this); //referansları setler , ventsistemi de setlenir
            canPressVentButton = true;
        }

    }
    private void OnTriggerExit2D(Collider2D collider)
    {
        Vent vent = collider.GetComponent<Vent>();
        if (vent != null)
        {
            if (!IsInVent()) canPressVentButton = false; //yürüyerek alandan çıktıysa false yap
        }
    }
    #endregion

    #region Vent Movement Functions
    public void SetVentSystem(VentsSystem ventsSystem)
    {
        this.ventsSystem = ventsSystem;
    }
    public void EnterVent(VentsSystem ventsSystem)
    {
        this.ventsSystem = ventsSystem;

        //animasyonları ekle
        VentEntered();
    }

    public void VentEntered() //animasyonlar bitip vente girince
    {
        EnableDisablePlayer();
        ventsSystem.PlayerInVent();
    }
    public void VentExited()
    {
        EnableDisablePlayer();
        ventsSystem.ExitTheVentSystem();
    }
    public bool IsInVent()
    {
        return rigidbody2d.simulated == false ? true : false;
    }
    private void EnableDisablePlayer()
    {
        Color color = spriteRenderer.color;
        if (IsInVent())
        {
            //pos vent teleport
            color.a = 1;
            spriteRenderer.color = color;
            rigidbody2d.simulated = true;
        }
        else
        {
            color.a = 0;
            spriteRenderer.color = color;
            rigidbody2d.simulated = false;

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
    private void Jump()
    {
        if (true/*energySystem.Instance.SpendEnergy(1f)*/)
        {
            amountOfJumpsLeft--;
            jumpTimer = jumpTimerSet;
            canJump = false;

            if (amountOfJumpsLeft == 0 && amountOfJumps > 1 && energySystem.SpendEnergy(doubleJumpEnergy))
            {
                rigidbody2d.velocity = new Vector2(3f * rigidbody2d.velocity.x, jumpForce);
            }
            else if (amountOfJumpsLeft == 0 && amountOfJumps > 1 && !energySystem.SpendEnergy(doubleJumpEnergy))
            {
                rigidbody2d.velocity = new Vector2(3f * rigidbody2d.velocity.x, jumpForce / 3);
            }
            else
            {
                rigidbody2d.velocity = jumpForce * new Vector2(rigidbody2d.velocity.x, 1);
            }
        }


    }

    #endregion

    #region Knockback
    private void Knockback(int direction) //soldan vurunca düşman direction 1 oluyor
    {
        isKnockbacking = true;
        knockbackStartTime = Time.time;
        rigidbody2d.velocity = new Vector2(direction * knockbackSpeedX, knockbackSpeedY);

    }
    private void CheckKnockback()
    {
        if (Time.time >= knockbackStartTime + knockbackDuration && isKnockbacking)
        {
            isKnockbacking = false;
            rigidbody2d.velocity = new Vector2(0f, rigidbody2d.velocity.y);
        }
    }
    #endregion

    #region Items
    private void UseItem(Item item)
    {
        switch (item.itemType)
        {
            case Item.ItemType.HealthPotion:
                if (!healthSystem.HasFullHealth())
                {
                    FlashGreen();
                    healthSystem.Heal(1);
                    inventory.RemoveItem(new Item { itemType = Item.ItemType.HealthPotion, amount = 1 });
                }
                break;
            case Item.ItemType.Medkit:
                if (!healthSystem.HasFullHealth())
                {
                    FlashGreen();
                    healthSystem.Heal(healthSystem.GetHealthAmountMax());
                    inventory.RemoveItem(new Item { itemType = Item.ItemType.Medkit, amount = 1 });
                }
                break;
            case Item.ItemType.ManaPotion:
                if (!energySystem.HasFullEnergy())
                {
                    FlashBlue();
                    energySystem.HealEnergy(energySystem.GetEnergyAmountMax());
                    inventory.RemoveItem(new Item { itemType = Item.ItemType.ManaPotion, amount = 1 });
                }
                break;

        }
    }
    private void PassiveItem(Item item, bool isAdding) //envantere ekleyip çıkarınca etkisini gösterenlerin etkileri
    {
        switch (item.itemType)
        {
            case Item.ItemType.Sword:
                if (isAdding)
                {
                    combatSystem.IncreaseAttackAmount(1);
                }//takınca
                else
                {
                    combatSystem.IncreaseAttackAmount(-1);
                }//çıkarınca
                break;
            case Item.ItemType.Coin://TODO: Star manager
                if (isAdding)
                {
                    ResourceManager.Instance.AddResource(ResourceManager.ResourceType.Gold, item.amount);

                }
                else
                {
                    ResourceManager.Instance.SpendResources(ResourceManager.ResourceType.Gold, 1);
                }
                break;

            case Item.ItemType.Armor:

                if (isAdding) healthSystem.IncreaseMaxHealthAmount(item.GetChangedHealthPoint());
                else healthSystem.IncreaseMaxHealthAmount(-item.GetChangedHealthPoint());
                break;
            case Item.ItemType.LuckItem:
                if (isAdding)
                {
                    StatManager.Instance.IncreaseStatAmount(StatManager.StatType.Dodge, 5);
                    StatManager.Instance.IncreaseStatAmount(StatManager.StatType.Charisma, 10);
                }
                else
                {
                    StatManager.Instance.IncreaseStatAmount(StatManager.StatType.Dodge, -5);
                    StatManager.Instance.IncreaseStatAmount(StatManager.StatType.Charisma, -10);
                }
                break;
            default:
                break;
        }
    }

    #endregion

    #region Skills
    private void PlayerSkills_OnSkillUnlocked(object sender, PlayerSkills.OnSkillUnlockedEventArgs e)
    {
        IncreaseSkills(e.skillType);
        saveFile.unlockedSkillList.Add(e.skillType);
    }
    private void PlayerSkills_OnResetAllSkills(object sender, EventArgs e)
    {
        foreach (PlayerSkills.SkillType skill in saveFile.unlockedSkillList)
        {
            IncreaseSkills(skill, false);
        }
        saveFile.unlockedSkillList.Clear();
        ResourceManager.Instance.SetRemainedResourceAmount(ResourceManager.ResourceType.Star,
            ResourceManager.Instance.GetMaxResourceAmount(ResourceManager.ResourceType.Star));

    }
    private void IncreaseSkills(PlayerSkills.SkillType skillType, bool isIncreasing = true) //false'ken skill'i resetler
    {
        switch (skillType)
        {
            case PlayerSkills.SkillType.Health:
                if (isIncreasing) healthSystem.IncreaseMaxHealthAmount(1);
                else healthSystem.IncreaseMaxHealthAmount(-1);
                break;
            case PlayerSkills.SkillType.Energy:
                if (isIncreasing) energySystem.IncreaseMaxEnergyAmount(1);
                else energySystem.IncreaseMaxEnergyAmount(-1);
                break;
            case PlayerSkills.SkillType.Speed:
                if (isIncreasing) IncreaseMovementSpeed(3);
                else IncreaseMovementSpeed(-3);
                break;
            case PlayerSkills.SkillType.Damage:
                if (isIncreasing) combatSystem.IncreaseAttackAmount(1);
                else combatSystem.IncreaseAttackAmount(-1);
                break;
            //abilities
            case PlayerSkills.SkillType.Dash:
                CanUseDash();
                break;
            case PlayerSkills.SkillType.DoubleJump:
                if (isIncreasing) amountOfJumps += 1;
                else amountOfJumps -= 1;
                break;
        }
    }

    //Increase Stat
    private void IncreaseMovementSpeed(int amount)
    {
        movementSpeed += amount;
        StatManager.Instance.IncreaseStatAmount(StatManager.StatType.Speed, amount);
    }

    //Check Stat
    private bool CanUseDash()
    {
        return playerSkills.IsSkillUnlocked(PlayerSkills.SkillType.Dash);
    }
    #endregion

    #region Flash Flash Flash (Visuals)
    public void FlashGreen()
    {
        materialTintColor.SetTintColor(new Color(0, 1, 0, 1));
    }
    public void FlashRed()
    {
        materialTintColor.SetTintColor(new Color(1, 0, 0, 1));
    }
    public void FlashBlue()
    {
        materialTintColor.SetTintColor(new Color(0, 0, 1, 1));
    }
    public void FlashYellow()
    {
        materialTintColor.SetTintColor(new Color(255, 255, 65, 1));
    }
    #endregion

    private void ShakeCamera()
    {
        CinemachineShake.Instance.ShakeCamera(2f, .2f);

        //cameraHandler.GetCinemachine().m_Follow = null;

        //cameraHandler.GetCinemachine().transform.DOComplete();
        //cameraHandler.GetCinemachine().transform.DOShakePosition(.2f, .5f, 14, 90, false, true);

        //cameraHandler.GetCinemachine().m_Follow = transform;

        //cam.transform.DOComplete();
        //cam.transform.DOShakePosition(.2f, .5f, 14, 90, false, true);
        //cameraHandler.GetCinemachine().transform.DOShakeRotation(.2f, 30f, 10, 90, true);
    }


}

//public PlayerStats GetPlayerStats()
//{
//    return playerStats;
//}

//if (rigidbody2d.velocity.x < 0 )
//{
//    canMove = false;
//    canFlip = false;
//    canJump = false;
//    turnTimer = turnTimerSet;
//}

//if (rigidbody2d.velocity.x > 0)
//{
//    canMove = false;
//    DisableFlip();
//    canJump = false;
//    turnTimer = turnTimerSet;
//}

//private void SetPreSkillSet(List<PlayerSkills.SkillType> list)
//{
//    PlayerSkills.SkillType[] skills = { PlayerSkills.SkillType.Energy, PlayerSkills.SkillType.Health };
//    list.AddRange(skills);

//    foreach (PlayerSkills.SkillType skillType in list)
//    {
//        IncreaseSkills(skillType);
//        PlayerPrefs.SetInt(skillType.ToString(), 1);
//        if (PlayerPrefs.HasKey(skillType.ToString()))
//        {
//            Debug.Log(skillType);
//        };

//    }
//}