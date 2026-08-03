using System;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Character
{
    [SerializeField] private NavMeshAgent agent;

    private int stairId = -1;
    private IState currentState;

    public override void OnStart()
    {
        base.OnStart();
        agent.enabled = true;
        ChangeState(new PatrolState());
    }
    public override void OnWin()
    {
        agent.enabled = false;
        ChangeState(new IdleState());
        base.OnWin();
    }

    public override void OnDespawn()
    {
        base.OnDespawn();
        agent.enabled = false;
    }

    public override void OnPause()
    {
        base.OnPause();
       agent.enabled = false;

    }
    public override void OnContinue()
    {
        base.OnContinue();
       agent.enabled = true;
    }

    public override void SetSpeed(float speed)
    {
        base.SetSpeed(speed);
        agent.speed = speed;
    }

    public void SetStairId(int stairId)
    {
        this.stairId = stairId;
    }

    public int GetStairId()
    {
        return stairId;
    }

    public void ChangeState(IState newState)
    {
        currentState?.OnExit(this);
        currentState = newState;
        currentState?.OnEnter(this);
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

    public override void Knockback(Vector3 dir)
    {
        rb.isKinematic = false;
        agent.enabled = false;
        base.Knockback(dir);
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

    public void SetDestination(Vector3 destination)
    {
        agent.SetDestination(destination);
    }
    public void CaculateDestination()
    {
        if(currentState is BuildState)
        {
            BuildState buildState = currentState as BuildState;
            buildState.CaculateDestination(this);
        }
    }

    protected override void Update()
    {
        base.Update();
        if (!characterState.GetIsOnGround())
        {
            EventBus<OnCharacterInActive>.Raise(new OnCharacterInActive
            {
                CharacterId = CharacterId
            });
            ChangeState(new IdleState());
            //OnDespawn();
            Invoke(nameof(ReSpawn), 0.5f);

            
            return;
        }
        if (!IsAgentValid()) return;

        

        CharacterChecker.CheckForward();
            
        if(currentState != null)
        {
            currentState.OnExecute(this);
        }

    }



}
