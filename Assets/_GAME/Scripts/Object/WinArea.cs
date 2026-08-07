using System.Collections.Generic;
using UnityEngine;

public class WinArea : MonoBehaviour
{
    [SerializeField] private List<Transform> rankPositions = new List<Transform>();

    [SerializeField] private List<ParticleSystem> particleSystems = new List<ParticleSystem>();

    public Transform TF;


    public void OnInit()
    {
        foreach (ParticleSystem particle in particleSystems)
        {
            particle.Stop();
        }
    }

    public void OnDespawn()
    {
        foreach (ParticleSystem particle in particleSystems)
        {
            particle.Stop();
        }
    }

    public void LoadData(TransformData data)
    {
        Helper.LoadTransformData(TF, data);
    }

    public void OnWin()
    {
        LevelManager.Instance.OnWin();
        for (int i = 0; i < 4; i++)
        {
            LevelManager.Instance.RankManager.GetCharacterRank(i + 1).SetSpawn(rankPositions[i].position);
        }
        foreach (ParticleSystem particleSystem in particleSystems)
        {
            particleSystem.Play();
        }
        int expectStar = LevelManager.Instance.RankManager.CaculateStarPlayer();

        if (expectStar > 0)
        {
            SoundManager.Instance.PlayMusicSound(AudioClipType.BGM_WIN);
        }
        else
        {
             SoundManager.Instance.PlayMusicSound(AudioClipType.BGM_LOSE);
        }
    }

    public void OnCollisionCharacter(Collider collider)
    {
        Character character = ColliderCache<Character>.GetComponent(collider);
        EventBus<OnCharacterUpStage>.Raise(new OnCharacterUpStage
        {
            Character = character,
            Stage = 10
        });
        GameManager.Instance.ChangeGameState(GameState.VICTORY);

        if (!LevelManager.Instance.RankManager.IsPlayerRankFirst())
        {
            UIManager.Instance.OpenUI<CanvasTimeUp>();
        }
        Invoke(nameof(OnWin), 3f);
        
    }


    public void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag(GameConfig.CHARACTER_TAG))
        {
            OnCollisionCharacter(collider);

        }
    }

    void Awake()
    {
        TF = transform;
    }


}
