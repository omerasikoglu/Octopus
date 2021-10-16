using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatSystem : MonoBehaviour
{
    public static CombatSystem Instance { get; private set; }
    [SerializeField] private LayerMask whatIsDamageable;
    private EnergySystem energySystem;
    private float hitEnergyAmount = 1f;

    [SerializeField] private float attack1Radius;
    private Transform attack1HitBoxPos;
    [SerializeField] private int attack1Damage;
    [SerializeField] private float inputTimer;
    private AttackDetails attackDetails;

    private bool combatEnabled = true;
    private bool gotInput;
    private bool isAttacking;
    private bool isFirstAttack;


    private float lastInputTime = Mathf.NegativeInfinity;

    private Animator animator;

    private void Awake()
    {
        Instance = this;
        energySystem = GetComponent<EnergySystem>();
        attack1HitBoxPos = transform.Find("Attack1HitBoxPos");

    }
    private void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("canAttack", combatEnabled);

       
    }
    private void Update()
    {
        CheckCombatInput();
        CheckAttacks();
    }
    private void CheckCombatInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (combatEnabled && energySystem.SpendEnergy(hitEnergyAmount))
            {
                gotInput = true;
                lastInputTime = Time.time;
            }
        }
    }
    private void CheckAttacks()
    {
        if (gotInput) //attack1 ile saldır
        {
            if (!isAttacking)
            {
                gotInput = false;
                isAttacking = true;
                isFirstAttack = !isFirstAttack;
                animator.SetBool("attack1", true);
                animator.SetBool("firstAttack", isFirstAttack);
                animator.SetBool("isAttacking", isAttacking);
            }
        }
        if (Time.time >= lastInputTime + inputTimer)
        {
            gotInput = false;
        }
    }

   

    private void CheckAttackHitBox() //animatorden ulaşılıyo
    {
        Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(attack1HitBoxPos.position, attack1Radius, whatIsDamageable);

        attackDetails.damageAmount = attack1Damage;
        attackDetails.damageType = null;
        attackDetails.position = attack1HitBoxPos.position;

        foreach (Collider2D collider2d in detectedObjects)
        {
            collider2d.transform.parent.SendMessage("Damage", attackDetails);
            //HealthSystem healthSystem = collider2d.transform.parent.transform.GetComponent<HealthSystem>();
            //if (healthSystem != null)
            //{
            //    healthSystem.Damage(attackDetails);
            //}
        }
    }
    private void FinishAttack1() //animatorden ulaşılıyo
    {
        isAttacking = false;
        animator.SetBool("isAttacking", isAttacking);
        animator.SetBool("attack1", false);
    }

    public int GetAttackDamageAmount()
    {
        return attack1Damage;
    }
    public void IncreaseAttackAmount(int amount)
    {
        attack1Damage += amount;
        StatManager.Instance.IncreaseStatAmount(StatManager.StatType.Damage, amount);
    }
}

//collider2d.transform.parent.GetComponent<HealthSystem>().SendMessage("Metodd");