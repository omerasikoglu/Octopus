using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DummyController : MonoBehaviour
{
    [SerializeField] private MinionTypeSO minionType;

    private PlayerController playerController;
    private HealthSystem healthSystem;

    private GameObject aliveGO, brokenTopGO, brokenBotGO;
    private Rigidbody2D rigidbodyAlive, rigidbodyBrokenTop, rigidbodyBrokenBot;

    private Animator aliveAnimator;

    private int playerFacingDirection;
    private bool playerOnRight;

    //knockback
    [SerializeField] private float knockbackSpeedX;
    [SerializeField] private float knockbackDeathSpeedX;
    [SerializeField] private float knockbackSpeedY;
    [SerializeField] private float knockbackDeathSpeedY;
    private float knockbackDuration = .1f;
    [SerializeField] private float deathTorque;

    private bool isKnockbacking = false;
    private float knockbackStart;

    private Transform pfHitParticle;


    private void Awake()
    {
        playerController = GameObject.Find("Mike").GetComponent<PlayerController>();

        aliveGO = transform.Find("Alive").gameObject;
        brokenTopGO = transform.Find("BrokenTop").gameObject;
        brokenBotGO = transform.Find("BrokenBottom").gameObject;

        aliveAnimator = aliveGO.GetComponent<Animator>();

        rigidbodyAlive = aliveGO.GetComponent<Rigidbody2D>();
        rigidbodyBrokenTop = brokenTopGO.GetComponent<Rigidbody2D>();
        rigidbodyBrokenBot = brokenBotGO.GetComponent<Rigidbody2D>();

        aliveGO.SetActive(true);
        brokenTopGO.SetActive(false);
        brokenBotGO.SetActive(false);

        healthSystem = GetComponent<HealthSystem>();
        pfHitParticle = Resources.Load<Transform>("pfHitParticle");
    }
    private void Start()
    {
        healthSystem.SetHealthAmountMax(minionType.healthAmountMax, false);
        healthSystem.OnDied += HealthSystem_OnDied;
        healthSystem.OnDamaged += HealthSystem_OnDamaged;
    }
    private void Update()
    {
        CheckKnockback();


    }



    #region Event Implements

    private void HealthSystem_OnDamaged(object sender, System.EventArgs e)
    {

        Instantiate(pfHitParticle, aliveGO.transform.position, Quaternion.Euler(0f, 0f, UnityEngine.Random.Range(0f, 360f)));
        // facing Direction

        playerFacingDirection = playerController.GetFacingDirection();
        if (playerFacingDirection == 1)
        {
            playerOnRight = false;
        }
        else
        {
            playerOnRight = true;
        }
        aliveAnimator.SetBool("PlayerOnRight", playerOnRight);
        aliveAnimator.SetTrigger("Damage");

        //knockback
        if (minionType.knockbackable)
        {
            Knockback();
        }

    }

    private void HealthSystem_OnDied(object sender, System.EventArgs e)
    {
        //ölünce
        aliveGO.SetActive(false);
        brokenTopGO.SetActive(true);
        brokenBotGO.SetActive(true);

        brokenTopGO.transform.position = aliveGO.transform.position;
        brokenBotGO.transform.position = aliveGO.transform.position;

        rigidbodyBrokenTop.velocity = new Vector2(playerFacingDirection * knockbackSpeedX, knockbackDeathSpeedY);
        rigidbodyBrokenTop.AddTorque(deathTorque * -playerFacingDirection, ForceMode2D.Impulse);
        rigidbodyBrokenBot.velocity = new Vector2(playerFacingDirection * knockbackDeathSpeedX, knockbackSpeedY);


    }
    #endregion


    private void Knockback()
    {
        isKnockbacking = true;
        knockbackStart = Time.time;
        rigidbodyAlive.velocity = new Vector2(playerFacingDirection * knockbackSpeedX, knockbackSpeedY);

    }
    private void CheckKnockback() //süresi bitti mi diye check ederiz
    {
        if (Time.time >= knockbackStart + knockbackDuration && isKnockbacking)
        {
            isKnockbacking = false;
            rigidbodyAlive.velocity = new Vector2(0f, rigidbodyAlive.velocity.y);
        }
    }

}
