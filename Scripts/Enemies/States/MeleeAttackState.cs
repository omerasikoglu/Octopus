using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackState : AttackState
{
    protected D_MeleeAttackState stateData;
    protected AttackDetails attackDetails;
    public MeleeAttackState(Entity entity, FiniteStateMachine stateMachine, string animatorBoolName, Transform attackPosition, D_MeleeAttackState stateData)
        : base(entity, stateMachine, animatorBoolName, attackPosition)
    {
        this.stateData = stateData;
    }

    public override void DoChecks()
    {
        base.DoChecks();
        
    }

    public override void Enter()
    {
        base.Enter();

        attackDetails.damageAmount = stateData.attackDamage;
        attackDetails.position = entity.aliveGO.transform.position;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FinishAttack()
    {
        base.FinishAttack();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        entity.CheckPlayerInCloseAttackRange(); //raycast için
    }

    public override void TriggerAttack()
    {
        base.TriggerAttack();

        Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(attackPosition.position, stateData.attackRadius,stateData.whatIsPlayer);

        foreach (Collider2D collider2d in detectedObjects)
        {
            HealthSystem healthSystem = collider2d.GetComponent<HealthSystem>();
            if (healthSystem!=null)
            {
                healthSystem.Damage(attackDetails);
            }
            //collider2d.transform.parent.SendMessage("Damage",attackDetails);
        }

    }
}
