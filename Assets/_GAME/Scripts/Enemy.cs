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

    void Update()
    {
        currentState.OnExecute(this);

    }



}
