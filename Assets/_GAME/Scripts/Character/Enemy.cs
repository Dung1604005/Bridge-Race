using System;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Character
{
    [SerializeField] private NavMeshAgent agent;

    private int stairId = -1;

    private int numbTargetBrick = 0;

    private Bridge bestBridge;

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

    public void SetNumbTargetBrick(int numbTargetBrick)
    {
        this.numbTargetBrick = numbTargetBrick;
    }

    public int GetNumbTargetBrick()
    {
        return numbTargetBrick;
    }

    public void SetBestBridge(Bridge bridge)
    {
        this.bestBridge = bridge;
    }

    public Bridge GetBestBridge()
    {
        return bestBridge;
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
        if (currentState is BuildState)
        {
            BuildState buildState = currentState as BuildState;
            buildState.CaculateDestination(this);
        }
    }
    public void OnEnemyFalling()
    {
        EventBus<OnCharacterInActive>.Raise(new OnCharacterInActive
        {
            CharacterId = CharacterId
        });
        ChangeState(new IdleState());
        OnDespawn();
        Invoke(nameof(ReSpawn), 0.5f);
    }

    public void UpdateEnemy()
    {
        if (!characterState.GetIsOnGround())
        {
            OnEnemyFalling();
            return;
        }
        if (!IsAgentValid()) return;
        CharacterChecker.CheckForward();
        currentState?.OnExecute(this);
    }

    protected override void Update()
    {
        base.Update();
        UpdateEnemy();
    }



}
