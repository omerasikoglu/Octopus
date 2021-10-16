using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunState : State
{
    D_StunState stateData;

    protected bool isStunTimeOver;
    public StunState(Entity entity, FiniteStateMachine stateMachine, string animatorBoolName, D_StunState stateData)
        : base(entity, stateMachine, animatorBoolName)
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

        isStunTimeOver = false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (Time.time >= startTime + stateData.stunTime)
        {
            isStunTimeOver = true;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
