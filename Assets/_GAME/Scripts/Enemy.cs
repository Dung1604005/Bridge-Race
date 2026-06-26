using System;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Character
{
    [SerializeField] private NavMeshAgent agent;
    private IState currentState;

    public NavMeshAgent Agent => agent;

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
        if(agent.pathPending)return false;
        if(agent.remainingDistance <= agent.stoppingDistance)
        {
            return true;
        }
        return false;
    }
    
    void Start()
    {
        ChangeState(new PatrolState());
        
    }

    protected override void Update()
    {
        base.Update();
        CheckStairForward();
        
        currentState.OnExecute(this);

    }



}
