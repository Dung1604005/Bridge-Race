using System;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Character
{
    [SerializeField] private NavMeshAgent agent;
    private IState currentState;

    public NavMeshAgent Agent => agent;

    public override void OnEnable()
    {
        base.OnEnable();
        EventBus<OnMapLoadComplete>.Subcribe(OnStart);
    }
    public override void OnDisable()
    {
        base.OnDisable();
        EventBus<OnMapLoadComplete>.UnSubcribe(OnStart);
    }

    public override void OnWin(OnWin onWin)
    {
        base.OnWin(onWin);
        agent.enabled = false;
    }



    public override void OnInit()
    {
        base.OnInit();
        agent.enabled = true;
    }

    public override void OnDespawn()
    {
        base.OnDespawn();
        agent.enabled = false;
    }


    
    public void OnStart(OnMapLoadComplete onMapLoadComplete)
    {
        ChangeState(new PatrolState());
    }

    public void ChangeState(IState newState)
    {
        currentState = newState;
        currentState.OnEnter(this);
    }

    public override bool CharacterIsGoingDown()
    {
        if(agent.velocity.z < -0.01f)
        {
            return true;
        }
        return false;
    }

    public bool IsAgentStop()
    {
        if(agent.enabled == false) return true;
        if(agent.pathPending)return false;
        if(agent.remainingDistance <= agent.stoppingDistance )
        {
            return true;
        }
        return false;
    }

    protected override void Update()
    {
        base.Update();
        
        CheckStairForward();
        
        currentState.OnExecute(this);

    }



}
