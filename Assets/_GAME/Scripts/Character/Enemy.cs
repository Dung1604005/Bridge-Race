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
        agent.enabled = false;
        ChangeState(new IdleState());
        base.OnWin(onWin);


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
        if (agent.velocity.z < -0.01f)
        {
            return true;
        }
        return false;
    }

    public bool IsAgentValid()
    {
        if (agent.enabled == false) return false;

        if (agent.isOnNavMesh == false) return false;

        return true;
    }

    public bool IsAgentStop()
    {

        if (IsAgentValid() == false) return true;
        if (agent.pathPending) return false;
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            return true;
        }
        return false;
    }

    public override void Knockback()
    {
        rb.isKinematic = false;
        agent.enabled = false;
        base.Knockback();
    }

    public override void StandUp()
    {

        base.StandUp();
        TurnOnAgent();
        ChangeState(new PatrolState());
    }

    public void TurnOnAgent()
    {
        rb.isKinematic = true;
        
        agent.enabled = true;
    }

    protected override void Update()
    {
        if (CharacterIsFalling())
        {
            ChangeState(new IdleState());
            OnDespawn();
            Invoke(nameof(ReSpawn), 0.5f);
            return;
        }
        if (!IsAgentValid()) return;

        base.Update();

        CheckForward();

        currentState.OnExecute(this);

    }



}
