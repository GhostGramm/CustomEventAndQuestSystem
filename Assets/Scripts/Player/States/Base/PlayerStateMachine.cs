using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerStateMachine : StateMachine
{
    [field : SerializeField] public PlayerInput Input { get; private set; }
    [field : SerializeField] public CharacterController CharacterController { get; private set; }
    [field : SerializeField] public Animator Animator { get; private set; }

    [field: SerializeField] public float FreeSpeed;
    private void Start()
    {
        Init();

        Invoke(nameof(IncreaseLevel), 3f);
    }

    public void Init()
    {
        Debug.Log("Player Initialized");
        ChangeState(new PlayerTestState(this));
    }

    public void IncreaseLevel()
    {
        GameEventHandler.Instance.OnPlayerLevelChanged?.Invoke(1);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Coin")
        {
            GameEventHandler.Instance.OnCoinCollected?.Invoke();
            Destroy(other.gameObject);
        }
    }
}
