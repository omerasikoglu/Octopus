using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State
{
    protected FiniteStateMachine stateMachine;
    protected Entity entity;
    protected string animatorBoolName;

    protected float startTime;

    public State (Entity entity,FiniteStateMachine stateMachine,string animatorBoolName)
    {
        this.entity = entity;
        this.stateMachine = stateMachine;
        this.animatorBoolName = animatorBoolName;
    }
    public virtual void Enter()
    {
        startTime = Time.time;
        entity.animator.SetBool(animatorBoolName, true);
        DoChecks();
    }
    public virtual void Exit()
    {
        entity.animator.SetBool(animatorBoolName, false);
    }
    public virtual void LogicUpdate()
    {

    }
    public virtual void PhysicsUpdate()
    {
        DoChecks();
    }

    public virtual void DoChecks()
    {

    }

}
